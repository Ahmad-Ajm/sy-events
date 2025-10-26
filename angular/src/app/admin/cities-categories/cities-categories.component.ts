// تعليق: مكون إدارة المدن والتصنيفات
import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CityService } from '../../proxy/cities/city.service';
import { CityDto, CreateUpdateCityDto } from '../../proxy/cities/models';

@Component({
  selector: 'app-cities-categories',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <!-- تعليق: صفحة إدارة المدن والتصنيفات -->
    <div class="container py-4">
      <!-- تعليق: قسم المدن -->
      <div class="card mb-4">
        <div class="card-header d-flex justify-content-between align-items-center">
          <h4 class="mb-0">
            <i class="fas fa-city me-2"></i>
            إدارة المدن
          </h4>
          <button class="btn btn-primary btn-sm" (click)="showCityModal = true; editingCity = null; resetCityForm()">
            <i class="fas fa-plus me-2"></i>
            إضافة مدينة
          </button>
        </div>
        <div class="card-body">
          <div class="table-responsive">
            <table class="table table-hover">
              <thead>
                <tr>
                  <th>الاسم بالعربية</th>
                  <th>الاسم بالإنجليزية</th>
                  <th>تاريخ الإنشاء</th>
                  <th class="text-end">الإجراءات</th>
                </tr>
              </thead>
              <tbody>
                <tr *ngFor="let city of cities()">
                  <td>{{ city.name }}</td>
                  <td>{{ city.nameEn }}</td>
                  <td>{{ city.creationTime | date:'short' }}</td>
                  <td class="text-end">
                    <button class="btn btn-sm btn-outline-primary me-2" (click)="editCity(city)">
                      <i class="fas fa-edit"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger" (click)="deleteCity(city.id)">
                      <i class="fas fa-trash"></i>
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
            <div class="text-center text-muted py-4" *ngIf="cities().length === 0">
              <i class="fas fa-city fa-3x mb-3 opacity-25"></i>
              <p>لا توجد مدن بعد</p>
            </div>
          </div>
        </div>
      </div>

      <!-- تعليق: قسم التصنيفات -->
      <div class="card">
        <div class="card-header d-flex justify-content-between align-items-center">
          <h4 class="mb-0">
            <i class="fas fa-tags me-2"></i>
            إدارة التصنيفات
          </h4>
          <button class="btn btn-primary btn-sm" (click)="showCategoryModal = true; editingCategory = null; resetCategoryForm()">
            <i class="fas fa-plus me-2"></i>
            إضافة تصنيف
          </button>
        </div>
        <div class="card-body">
          <div class="table-responsive">
            <table class="table table-hover">
              <thead>
                <tr>
                  <th>الأيقونة</th>
                  <th>الاسم بالعربية</th>
                  <th>الاسم بالإنجليزية</th>
                  <th>الوصف</th>
                  <th class="text-end">الإجراءات</th>
                </tr>
              </thead>
              <tbody>
                <tr *ngFor="let cat of categories()">
                  <td>
                    <i [class]="'fas ' + (cat.icon || 'fa-tag')"></i>
                  </td>
                  <td>{{ cat.name }}</td>
                  <td>{{ cat.nameEn }}</td>
                  <td>{{ cat.description || '-' }}</td>
                  <td class="text-end">
                    <button class="btn btn-sm btn-outline-primary me-2" (click)="editCategory(cat)">
                      <i class="fas fa-edit"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger" (click)="deleteCategory(cat.id)">
                      <i class="fas fa-trash"></i>
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
            <div class="text-center text-muted py-4" *ngIf="categories().length === 0">
              <i class="fas fa-tags fa-3x mb-3 opacity-25"></i>
              <p>لا توجد تصنيفات بعد</p>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- تعليق: Modal إضافة/تعديل مدينة -->
    <div class="modal fade" [class.show]="showCityModal" [style.display]="showCityModal ? 'block' : 'none'" *ngIf="showCityModal">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">
              {{ editingCity ? 'تعديل مدينة' : 'إضافة مدينة جديدة' }}
            </h5>
            <button type="button" class="btn-close" (click)="showCityModal = false"></button>
          </div>
          <form [formGroup]="cityForm" (ngSubmit)="saveCity()">
            <div class="modal-body">
              <div class="mb-3">
                <label class="form-label">الاسم بالعربية <span class="text-danger">*</span></label>
                <input type="text" class="form-control" formControlName="name" placeholder="دمشق">
                <div class="text-danger small mt-1" *ngIf="cityForm.get('name')?.invalid && cityForm.get('name')?.touched">
                  الاسم مطلوب (حد أقصى 150 حرف)
                </div>
              </div>
              <div class="mb-3">
                <label class="form-label">الاسم بالإنجليزية <span class="text-danger">*</span></label>
                <input type="text" class="form-control" formControlName="nameEn" placeholder="Damascus">
                <div class="text-danger small mt-1" *ngIf="cityForm.get('nameEn')?.invalid && cityForm.get('nameEn')?.touched">
                  الاسم بالإنجليزية مطلوب (حد أقصى 150 حرف)
                </div>
              </div>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" (click)="showCityModal = false">إلغاء</button>
              <button type="submit" class="btn btn-primary" [disabled]="cityForm.invalid || savingCity()">
                <i class="fas fa-save me-2"></i>
                {{ savingCity() ? 'جاري الحفظ...' : 'حفظ' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>

    <!-- تعليق: Modal إضافة/تعديل تصنيف -->
    <div class="modal fade" [class.show]="showCategoryModal" [style.display]="showCategoryModal ? 'block' : 'none'" *ngIf="showCategoryModal">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">
              {{ editingCategory ? 'تعديل تصنيف' : 'إضافة تصنيف جديد' }}
            </h5>
            <button type="button" class="btn-close" (click)="showCategoryModal = false"></button>
          </div>
          <form [formGroup]="categoryForm" (ngSubmit)="saveCategory()">
            <div class="modal-body">
              <div class="mb-3">
                <label class="form-label">الاسم بالعربية <span class="text-danger">*</span></label>
                <input type="text" class="form-control" formControlName="name" placeholder="تقني">
              </div>
              <div class="mb-3">
                <label class="form-label">الاسم بالإنجليزية <span class="text-danger">*</span></label>
                <input type="text" class="form-control" formControlName="nameEn" placeholder="Technology">
              </div>
              <div class="mb-3">
                <label class="form-label">الوصف بالعربية</label>
                <textarea class="form-control" rows="2" formControlName="description" placeholder="وصف التصنيف..."></textarea>
              </div>
              <div class="mb-3">
                <label class="form-label">الوصف بالإنجليزية</label>
                <textarea class="form-control" rows="2" formControlName="descriptionEn" placeholder="Category description..."></textarea>
              </div>
              <div class="mb-3">
                <label class="form-label">رمز الأيقونة (FontAwesome)</label>
                <input type="text" class="form-control" formControlName="icon" placeholder="fa-laptop-code">
                <small class="text-muted">مثال: fa-laptop-code, fa-stethoscope, fa-briefcase</small>
              </div>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" (click)="showCategoryModal = false">إلغاء</button>
              <button type="submit" class="btn btn-primary" [disabled]="categoryForm.invalid || savingCategory()">
                <i class="fas fa-save me-2"></i>
                {{ savingCategory() ? 'جاري الحفظ...' : 'حفظ' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>

    <!-- تعليق: Backdrop للـ Modal -->
    <div class="modal-backdrop fade show" *ngIf="showCityModal || showCategoryModal"></div>

    <style>
      .modal.show {
        display: block !important;
      }
    </style>
  `,
})
export class CitiesCategoriesComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly cityService = inject(CityService);

  // تعليق: حالات المكون
  cities = signal<any[]>([]);
  categories = signal<any[]>([]);
  showCityModal = false;
  showCategoryModal = false;
  editingCity: any = null;
  editingCategory: any = null;
  savingCity = signal(false);
  savingCategory = signal(false);

  // تعليق: نماذج الإدخال
  cityForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    nameEn: ['', [Validators.required, Validators.maxLength(150)]]
  });

  categoryForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    nameEn: ['', [Validators.required, Validators.maxLength(150)]],
    description: [''],
    descriptionEn: [''],
    icon: ['']
  });

  ngOnInit(): void {
    this.loadCities();
    this.loadCategories();
  }

  // تعليق: تحميل المدن
  loadCities(): void {
    this.cityService.getList({ maxResultCount: 100 }).subscribe({
      next: (result) => this.cities.set(result.items || []),
      error: (err) => console.error('Error loading cities:', err)
    });
  }

  // تعليق: تحميل التصنيفات
  loadCategories(): void {
    // TODO: استبدال بـ CategoryService عند الحاجة
    // مؤقتاً نستخدم بيانات وهمية
    this.categories.set([
      { id: '1', name: 'تقني', nameEn: 'Technology', description: 'فعاليات تقنية', icon: 'fa-laptop-code' },
      { id: '2', name: 'طبي', nameEn: 'Medical', description: 'مؤتمرات طبية', icon: 'fa-stethoscope' }
    ]);
  }

  // تعليق: تعديل مدينة
  editCity(city: any): void {
    this.editingCity = city;
    this.cityForm.patchValue({
      name: city.name,
      nameEn: city.nameEn
    });
    this.showCityModal = true;
  }

  // تعليق: حفظ مدينة
  saveCity(): void {
    if (this.cityForm.invalid) return;

    this.savingCity.set(true);
    const data = this.cityForm.value as CreateUpdateCityDto;

    const operation = this.editingCity
      ? this.cityService.update(this.editingCity.id, data)
      : this.cityService.create(data);

    operation.subscribe({
      next: () => {
        this.savingCity.set(false);
        this.showCityModal = false;
        this.loadCities();
        alert(this.editingCity ? 'تم تحديث المدينة بنجاح!' : 'تمت إضافة المدينة بنجاح!');
      },
      error: (err) => {
        this.savingCity.set(false);
        alert('حدث خطأ: ' + (err.error?.error?.message || err.message));
      }
    });
  }

  // تعليق: حذف مدينة
  deleteCity(id: string): void {
    if (!confirm('هل أنت متأكد من حذف هذه المدينة؟')) return;

    this.cityService.delete(id).subscribe({
      next: () => {
        this.loadCities();
        alert('تم حذف المدينة بنجاح!');
      },
      error: (err) => alert('حدث خطأ: ' + (err.error?.error?.message || err.message))
    });
  }

  // تعليق: تعديل تصنيف
  editCategory(category: any): void {
    this.editingCategory = category;
    this.categoryForm.patchValue({
      name: category.name,
      nameEn: category.nameEn,
      description: category.description,
      descriptionEn: category.descriptionEn,
      icon: category.icon
    });
    this.showCategoryModal = true;
  }

  // تعليق: حفظ تصنيف
  saveCategory(): void {
    if (this.categoryForm.invalid) return;

    this.savingCategory.set(true);
    const data = this.categoryForm.value;

    // TODO: استبدال بـ CategoryService API call
    setTimeout(() => {
      this.savingCategory.set(false);
      this.showCategoryModal = false;
      alert(this.editingCategory ? 'تم تحديث التصنيف بنجاح!' : 'تمت إضافة التصنيف بنجاح!');
    }, 500);
  }

  // تعليق: حذف تصنيف
  deleteCategory(id: string): void {
    if (!confirm('هل أنت متأكد من حذف هذا التصنيف؟')) return;
    // TODO: استبدال بـ CategoryService API call
    alert('تم حذف التصنيف بنجاح!');
  }

  // تعليق: إعادة تعيين نموذج المدينة
  resetCityForm(): void {
    this.cityForm.reset();
    this.editingCity = null;
  }

  // تعليق: إعادة تعيين نموذج التصنيف
  resetCategoryForm(): void {
    this.categoryForm.reset();
    this.editingCategory = null;
  }
}

