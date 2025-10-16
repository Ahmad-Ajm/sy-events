import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormsModule } from '@angular/forms';
import { ListService, PagedResultDto, CoreModule, AuthService } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { EventService } from '../../proxy/event.service';
import { EventImageService } from '../../proxy/event-image.service';
import { EventDto, GetEventsInput, CreateUpdateEventDto } from '../../proxy/events/dtos/models';
import { EventStatus } from '../../proxy/enums/event-status.enum';
import { Router } from '@angular/router';
import { FileUploadComponent } from '../file-upload/file-upload.component';

@Component({
  selector: 'app-event-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, CoreModule, ThemeSharedModule, NgxDatatableModule, FileUploadComponent],
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.scss'],
  providers: [ListService],
})
export class EventListComponent implements OnInit {
  events = { items: [], totalCount: 0 } as PagedResultDto<EventDto>;
  isModalOpen = false;
  selectedEvent = {} as EventDto;
  form: FormGroup;
  EventStatus = EventStatus;

  // تعليق: فلاتر متقدمة
  filters: GetEventsInput = {
    filter: '',
    categoryId: null,
    cityId: null,
    status: null,
    startDate: null,
    endDate: null,
    organizerId: null,
    isUpcoming: null,
    minAttendees: null,
    skipCount: 0,
    maxResultCount: 10
  } as any;

  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  constructor(
    public readonly list: ListService,
    private eventService: EventService,
    private imageService: EventImageService,
    private confirmation: ConfirmationService,
    private fb: FormBuilder
  ) {
    this.form = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', [Validators.required]],
      startDate: ['', [Validators.required]],
      endDate: ['', [Validators.required]],
      location: ['', [Validators.required, Validators.maxLength(200)]],
      categoryId: ['', [Validators.required]],
      cityId: ['', [Validators.required]],
      maxCapacity: [null],
    });
  }

  ngOnInit(): void {
    const eventsStreamCreator = (query: GetEventsInput) => {
      // تعليق: دمج الفلاتر المخصصة مع query parameters من ListService
      const mergedQuery = { ...this.filters, ...query };
      return this.eventService.getList(mergedQuery);
    };

    this.list.hookToQuery(eventsStreamCreator).subscribe((response) => {
      this.events = response;
    });
  }

  // تعليق: تطبيق الفلاتر عند التغيير
  applyFilters(): void {
    this.list.get();
  }

  // تعليق: مسح جميع الفلاتر
  clearFilters(): void {
    this.filters = {
      filter: '',
      categoryId: null,
      cityId: null,
      status: null,
      startDate: null,
      endDate: null,
      organizerId: null,
      isUpcoming: null,
      minAttendees: null,
      skipCount: 0,
      maxResultCount: 10
    } as any;
    this.list.get();
  }

  createEvent() {
    this.selectedEvent = {} as EventDto;
    this.form.reset();
    this.isModalOpen = true;
  }

  editEvent(id: string) {
    this.eventService.get(id).subscribe((event) => {
      this.selectedEvent = event;
      this.form.patchValue(event);
      this.isModalOpen = true;
    });
  }

  save() {
    if (this.form.invalid) return;

    const request = this.selectedEvent.id
      ? this.eventService.update(this.selectedEvent.id, this.form.value)
      : this.eventService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

  deleteEvent(id: string) {
    this.confirmation.warn('::AreYouSure', '::AreYouSureToDelete').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.eventService.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  approveEvent(id: string) {
    this.eventService.approve(id).subscribe(() => {
      this.list.get();
    });
  }

  rejectEvent(id: string) {
    this.confirmation.warn('::AreYouSure', '::AreYouSureToReject').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.eventService.reject(id).subscribe(() => this.list.get());
      }
    });
  }

  onUpload(event: Event, eventId: string) {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) return;
    const file = input.files[0];
    this.imageService.upload(eventId, file).subscribe(() => this.list.get());
    input.value = '';
  }

  followSelected(): void {
    // NOTE: Placeholder: follow first row if exists; extend to selected row later
    const first = this.events?.items?.[0];
    if (!first) { return; }
    const returnUrl = this.router.url;
    if (!this.auth.isAuthenticated) {
      this.auth.navigateToLogin({ returnUrl });
      return;
    }
    // If authenticated, navigate to event detail page (to be implemented)
    this.router.navigate(['/events', first.id]);
  }
}
