import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, computed, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HomeSliderService } from '../../../proxy/home-slider/home-slider.service';
import { AppSettingsDto, UpdateAppSettingsDto } from '../../../proxy/settings/dtos/models';
import { CreateUpdateHomeSliderItemDto, HomeSliderItemDto } from '../../../proxy/home-slider/dtos/models';
import { SliderItemType } from '../../../proxy/home-slider/slider-item-type.enum';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-slider-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './slider-management.component.html',
  styleUrls: ['./slider-management.component.scss']
})
export class SliderManagementComponent implements OnInit {
  private readonly sliderService = inject(HomeSliderService);
  private readonly fb = inject(FormBuilder);

  // قائمة العناصر
  items = signal<HomeSliderItemDto[]>([]);
  loadingList = signal<boolean>(false);
  savingItem = signal<boolean>(false);
  savingSettings = signal<boolean>(false);

  // إعدادات السلايدر
  settings = signal<AppSettingsDto | null>(null);

  // نموذج العنصر (إضافة/تعديل)
  itemForm = this.fb.group({
    id: this.fb.control<string | null>(null),
    displayOrder: this.fb.control<number>(1, { nonNullable: true, validators: [Validators.required, Validators.min(1)] }),
    type: this.fb.control<SliderItemType>(SliderItemType.Latest, { nonNullable: true, validators: [Validators.required] }),
    customEventId: this.fb.control<string | null>(null),
    isActive: this.fb.control<boolean>(true, { nonNullable: true }),
    title: this.fb.control<string>('', { nonNullable: true, validators: [Validators.required, Validators.maxLength(200)] }),
    titleEn: this.fb.control<string>('', { nonNullable: true, validators: [Validators.maxLength(200)] }),
    imageUrl: this.fb.control<string>('', { nonNullable: true, validators: [Validators.required] }),
  });

  // نموذج الإعدادات
  settingsForm = this.fb.group({
    sliderItemsCount: this.fb.control<number>(3, { nonNullable: true, validators: [Validators.required, Validators.min(2), Validators.max(6)] }),
    autoApproveEvents: this.fb.control<boolean>(false, { nonNullable: true }),
  });

  SliderItemType = SliderItemType;

  ngOnInit(): void {
    this.loadSettings();
    this.loadItems();
  }

  loadItems(): void {
    this.loadingList.set(true);
    this.sliderService
      .getList({ skipCount: 0, maxResultCount: 100, sorting: 'displayOrder' })
      .pipe(finalize(() => this.loadingList.set(false)))
      .subscribe(res => this.items.set(res.items));
  }

  loadSettings(): void {
    this.sliderService.getSettings().subscribe(s => {
      this.settings.set(s);
      this.settingsForm.patchValue({
        sliderItemsCount: s.sliderItemsCount,
        autoApproveEvents: s.autoApproveEvents,
      });
    });
  }

  editItem(item: HomeSliderItemDto): void {
    this.itemForm.reset({
      id: item.id,
      displayOrder: item.displayOrder,
      type: item.type,
      customEventId: item.customEventId ?? null,
      isActive: item.isActive,
      title: item.title,
      titleEn: item.titleEn,
      imageUrl: item.imageUrl,
    });
  }

  newItem(): void {
    this.itemForm.reset({
      id: null,
      displayOrder: (this.items().length || 0) + 1,
      type: SliderItemType.Latest,
      customEventId: null,
      isActive: true,
      title: '',
      titleEn: '',
      imageUrl: '',
    });
  }

  deleteItem(id: string): void {
    if (!confirm('هل تريد حذف هذا العنصر؟')) return;
    this.sliderService.delete(id).subscribe(() => this.loadItems());
  }

  saveItem(): void {
    if (this.itemForm.invalid) return;
    const value = this.itemForm.getRawValue();
    const dto: CreateUpdateHomeSliderItemDto = {
      displayOrder: value.displayOrder!,
      type: value.type!,
      customEventId: value.customEventId ?? undefined,
      isActive: value.isActive!,
      title: value.title!,
      titleEn: value.titleEn!,
      imageUrl: value.imageUrl!,
    };
    this.savingItem.set(true);
    const obs = value.id
      ? this.sliderService.update(value.id, dto)
      : this.sliderService.create(dto);
    obs.pipe(finalize(() => this.savingItem.set(false)))
      .subscribe(() => {
        this.newItem();
        this.loadItems();
      });
  }

  saveSettings(): void {
    if (this.settingsForm.invalid) return;
    const s: UpdateAppSettingsDto = this.settingsForm.getRawValue();
    this.savingSettings.set(true);
    this.sliderService.updateSettings(s)
      .pipe(finalize(() => this.savingSettings.set(false)))
      .subscribe(() => this.loadSettings());
  }
}


