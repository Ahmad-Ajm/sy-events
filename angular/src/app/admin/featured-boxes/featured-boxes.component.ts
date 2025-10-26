import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FeaturedBoxService } from '../../proxy/featured-boxes/featured-box.service';
import { FeaturedBoxDto, CreateUpdateFeaturedBoxDto } from '../../proxy/featured-boxes/dtos/models';

@Component({
  selector: 'app-admin-featured-boxes',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  template: `
    <div class="container py-3">
      <div class="d-flex justify-content-between align-items-center mb-3">
        <h2 class="mb-0">إدارة المربعات المميزة</h2>
        <button class="btn btn-primary" (click)="openCreate()"><i class="fa fa-plus me-2"></i> جديد</button>
      </div>

      <div class="table-responsive">
        <table class="table table-striped align-middle">
          <thead>
            <tr>
              <th>العنوان</th>
              <th>النوع</th>
              <th>الترتيب</th>
              <th>رابط مخصص</th>
              <th>فعالية مخصصة</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let b of list()">
              <td>{{ b.title }}</td>
              <td>{{ b.type }}</td>
              <td>{{ b.order }}</td>
              <td>{{ b.customLink || '-' }}</td>
              <td>{{ b.customEventId || '-' }}</td>
              <td class="text-end">
                <button class="btn btn-sm btn-outline-primary me-2" (click)="edit(b)"><i class="fa fa-edit"></i></button>
                <button class="btn btn-sm btn-outline-danger" (click)="remove(b.id)"><i class="fa fa-trash"></i></button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="card mt-4" *ngIf="isOpen()">
        <div class="card-header"><strong>{{ editingId() ? 'تعديل' : 'إنشاء' }}</strong></div>
        <div class="card-body">
          <form [formGroup]="form" (ngSubmit)="save()">
            <div class="row g-3">
              <div class="col-md-6">
                <label class="form-label">العنوان</label>
                <input class="form-control" formControlName="title" />
              </div>
              <div class="col-md-6">
                <label class="form-label">النوع</label>
                <select class="form-select" formControlName="type">
                  <option value="0">رئيسي</option>
                  <option value="1">ثانوي</option>
                </select>
              </div>
              <div class="col-md-4">
                <label class="form-label">الترتيب</label>
                <input type="number" class="form-control" formControlName="order" />
              </div>
              <div class="col-md-4">
                <label class="form-label">رابط مخصص</label>
                <input class="form-control" formControlName="customLink" />
              </div>
              <div class="col-md-4">
                <label class="form-label">ID فعالية مخصصة</label>
                <input class="form-control" formControlName="customEventId" />
              </div>
            </div>
            <div class="mt-3 d-flex gap-2">
              <button class="btn btn-primary" type="submit" [disabled]="form.invalid"><i class="fa fa-check me-2"></i>حفظ</button>
              <button class="btn btn-secondary" type="button" (click)="close()">إلغاء</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  `,
})
export class AdminFeaturedBoxesComponent implements OnInit {
  private readonly api = inject(FeaturedBoxService);
  private readonly fb = inject(FormBuilder);

  list = signal<FeaturedBoxDto[]>([]);
  isOpen = signal(false);
  editingId = signal<string | null>(null);

  form: FormGroup = this.fb.group({
    title: ['', Validators.required],
    type: [0, Validators.required],
    order: [0, Validators.required],
    customLink: [''],
    customEventId: [''],
  });

  ngOnInit(): void {
    this.reload();
  }

  reload(): void {
    this.api.getList({ skipCount: 0, maxResultCount: 100 } as any).subscribe(r => this.list.set(r.items));
  }

  openCreate(): void {
    this.editingId.set(null);
    this.form.reset({ title: '', type: 0, order: 0, customLink: '', customEventId: '' });
    this.isOpen.set(true);
  }

  edit(b: FeaturedBoxDto): void {
    this.editingId.set(b.id);
    this.form.patchValue(b as any);
    this.isOpen.set(true);
  }

  close(): void { this.isOpen.set(false); }

  save(): void {
    const dto = this.form.value as CreateUpdateFeaturedBoxDto;
    const id = this.editingId();
    const req = id ? this.api.update(id, dto) : this.api.create(dto);
    req.subscribe(() => { this.isOpen.set(false); this.reload(); });
  }

  remove(id?: string): void {
    if (!id) return;
    this.api.delete(id).subscribe(() => this.reload());
  }
}


