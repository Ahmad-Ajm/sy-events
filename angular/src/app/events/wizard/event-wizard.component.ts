import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { EventService } from '../../proxy/event.service';

@Component({
  selector: 'app-event-wizard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <!-- تعليق: معالج إضافة فعالية بـ 3 خطوات -->
    <div class="container py-4">
      <!-- تعليق: العنوان والتقدم -->
      <div class="card mb-4">
        <div class="card-body">
          <h2 class="mb-3">
            <i class="fas fa-plus-circle me-2"></i>
            إنشاء فعالية جديدة
          </h2>
          
          <!-- تعليق: شريط التقدم -->
          <div class="wizard-steps mb-4">
            <div class="step" [class.active]="step >= 1" [class.completed]="step > 1">
              <div class="step-number">1</div>
              <div class="step-label">المعلومات الأساسية</div>
            </div>
            <div class="step-divider" [class.active]="step > 1"></div>
            <div class="step" [class.active]="step >= 2" [class.completed]="step > 2">
              <div class="step-number">2</div>
              <div class="step-label">التواريخ والموقع</div>
            </div>
            <div class="step-divider" [class.active]="step > 2"></div>
            <div class="step" [class.active]="step >= 3">
              <div class="step-number">3</div>
              <div class="step-label">المراجعة والإرسال</div>
            </div>
          </div>
        </div>
      </div>

      <!-- تعليق: رسالة تحويل لمنظم -->
      @if (showOrganizerMessage) {
        <div class="alert alert-warning alert-dismissible fade show" role="alert">
          <h5 class="alert-heading">
            <i class="fas fa-exclamation-triangle me-2"></i>
            تحويل إلى منظم
          </h5>
          <p class="mb-3">
            بإضافة فعالية، سيتم تحويل حسابك من <strong>متابع</strong> إلى <strong>منظم</strong>.
            ستتمكن من إدارة فعالياتك وتتبع المشاركين.
          </p>
          <div class="d-flex gap-2">
            <button class="btn btn-warning" (click)="confirmOrganizer()">
              <i class="fas fa-check me-2"></i>
              موافق، تابع
            </button>
            <button class="btn btn-outline-secondary" (click)="cancelOrganizer()">
              <i class="fas fa-times me-2"></i>
              إلغاء
            </button>
          </div>
        </div>
      }

      <!-- تعليق: نموذج الإضافة -->
      @if (!showOrganizerMessage) {
        <div class="card">
          <div class="card-body">
            <form [formGroup]="form" (ngSubmit)="next()">
              <!-- تعليق: الخطوة 1 - المعلومات الأساسية -->
              <div *ngIf="step === 1" class="step-content">
                <h4 class="mb-4">
                  <i class="fas fa-info-circle me-2"></i>
                  المعلومات الأساسية
                </h4>
                <div class="row g-3">
                  <div class="col-md-6">
                    <label class="form-label">عنوان الفعالية <span class="text-danger">*</span></label>
                    <input class="form-control" formControlName="title" placeholder="مثال: مؤتمر التقنية السنوي 2025" />
                    @if (form.controls.title.invalid && form.controls.title.touched) {
                      <div class="text-danger small mt-1">العنوان مطلوب (حد أقصى 300 حرف)</div>
                    }
                  </div>
                  <div class="col-md-6">
                    <label class="form-label">الفئة</label>
                    <select class="form-select" formControlName="categoryId">
                      <option value="">-- اختر الفئة --</option>
                      <option value="cat1">مؤتمرات</option>
                      <option value="cat2">ورش عمل</option>
                      <option value="cat3">معارض</option>
                      <option value="cat4">حفلات</option>
                    </select>
                  </div>
                  <div class="col-12">
                    <label class="form-label">الوصف <span class="text-danger">*</span></label>
                    <textarea rows="4" class="form-control" formControlName="description" 
                      placeholder="اكتب وصفاً شاملاً للفعالية..."></textarea>
                    @if (form.controls.description.invalid && form.controls.description.touched) {
                      <div class="text-danger small mt-1">الوصف مطلوب</div>
                    }
                  </div>
                </div>
              </div>

              <!-- تعليق: الخطوة 2 - التواريخ والموقع -->
              <div *ngIf="step === 2" class="step-content">
                <h4 class="mb-4">
                  <i class="fas fa-map-marker-alt me-2"></i>
                  التواريخ والموقع
                </h4>
                <div class="row g-3">
                  <div class="col-md-6">
                    <label class="form-label">تاريخ البداية <span class="text-danger">*</span></label>
                    <input type="datetime-local" class="form-control" formControlName="startDate" />
                    @if (form.controls.startDate.invalid && form.controls.startDate.touched) {
                      <div class="text-danger small mt-1">تاريخ البداية مطلوب</div>
                    }
                  </div>
                  <div class="col-md-6">
                    <label class="form-label">تاريخ النهاية <span class="text-danger">*</span></label>
                    <input type="datetime-local" class="form-control" formControlName="endDate" />
                    @if (form.controls.endDate.invalid && form.controls.endDate.touched) {
                      <div class="text-danger small mt-1">تاريخ النهاية مطلوب</div>
                    }
                  </div>
                  <div class="col-md-6">
                    <label class="form-label">الموقع <span class="text-danger">*</span></label>
                    <input class="form-control" formControlName="location" placeholder="مثال: فندق الشام - دمشق" />
                    @if (form.controls.location.invalid && form.controls.location.touched) {
                      <div class="text-danger small mt-1">الموقع مطلوب (حد أقصى 400 حرف)</div>
                    }
                  </div>
                  <div class="col-md-6">
                    <label class="form-label">المدينة</label>
                    <select class="form-select" formControlName="cityId">
                      <option value="">-- اختر المدينة --</option>
                      <option value="city1">دمشق</option>
                      <option value="city2">حلب</option>
                      <option value="city3">حمص</option>
                      <option value="city4">اللاذقية</option>
                    </select>
                  </div>
                  <div class="col-md-6">
                    <label class="form-label">الحد الأقصى للمشاركين</label>
                    <input type="number" class="form-control" formControlName="maxCapacity" placeholder="0 = غير محدود" />
                  </div>
                </div>
              </div>

              <!-- تعليق: الخطوة 3 - المراجعة -->
              <div *ngIf="step === 3" class="step-content">
                <h4 class="mb-4">
                  <i class="fas fa-check-circle me-2"></i>
                  مراجعة البيانات
                </h4>
                <div class="alert alert-info mb-4">
                  <i class="fas fa-info-circle me-2"></i>
                  <strong>ملاحظة مهمة:</strong> سيتم إرسال الفعالية للمدير للموافقة عليها قبل نشرها.
                </div>
                
                <div class="review-section">
                  <div class="row g-3">
                    <div class="col-md-6">
                      <div class="review-item">
                        <label class="text-muted small">العنوان</label>
                        <p class="fw-bold mb-0">{{ form.value.title || '-' }}</p>
                      </div>
                    </div>
                    <div class="col-md-6">
                      <div class="review-item">
                        <label class="text-muted small">الموقع</label>
                        <p class="fw-bold mb-0">{{ form.value.location || '-' }}</p>
                      </div>
                    </div>
                    <div class="col-md-6">
                      <div class="review-item">
                        <label class="text-muted small">تاريخ البداية</label>
                        <p class="fw-bold mb-0">{{ form.value.startDate || '-' }}</p>
                      </div>
                    </div>
                    <div class="col-md-6">
                      <div class="review-item">
                        <label class="text-muted small">تاريخ النهاية</label>
                        <p class="fw-bold mb-0">{{ form.value.endDate || '-' }}</p>
                      </div>
                    </div>
                    <div class="col-12">
                      <div class="review-item">
                        <label class="text-muted small">الوصف</label>
                        <p class="mb-0">{{ form.value.description || '-' }}</p>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <!-- تعليق: أزرار التنقل -->
              <div class="mt-4 d-flex justify-content-between align-items-center">
                <button type="button" class="btn btn-outline-secondary" (click)="prev()" [disabled]="step === 1">
                  <i class="fas fa-arrow-right me-2"></i>
                  السابق
                </button>
                <div class="text-muted small">
                  خطوة {{ step }} من 3
                </div>
                <button type="submit" class="btn btn-primary" [disabled]="saving">
                  @if (step === 3) {
                    <i class="fas fa-paper-plane me-2"></i>
                    إرسال للموافقة
                  } @else {
                    التالي
                    <i class="fas fa-arrow-left ms-2"></i>
                  }
                </button>
              </div>
            </form>
          </div>
        </div>
      }
    </div>
    
    <!-- تعليق: CSS مضمن -->
    <style>
      .wizard-steps {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 1rem 0;
      }
      
      .step {
        display: flex;
        flex-direction: column;
        align-items: center;
        flex: 1;
        opacity: 0.5;
        transition: all 0.3s ease;
      }
      
      .step.active {
        opacity: 1;
      }
      
      .step.completed .step-number {
        background-color: #28a745;
        border-color: #28a745;
      }
      
      .step-number {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        background-color: #e9ecef;
        border: 2px solid #dee2e6;
        display: flex;
        align-items: center;
        justify-content: center;
        font-weight: bold;
        margin-bottom: 0.5rem;
        transition: all 0.3s ease;
      }
      
      .step.active .step-number {
        background-color: #007bff;
        border-color: #007bff;
        color: white;
      }
      
      .step-label {
        font-size: 0.875rem;
        text-align: center;
      }
      
      .step-divider {
        flex: 1;
        height: 2px;
        background-color: #dee2e6;
        margin: 0 1rem;
        margin-bottom: 2rem;
        transition: all 0.3s ease;
      }
      
      .step-divider.active {
        background-color: #28a745;
      }
      
      .step-content {
        animation: fadeInUp 0.5s ease-out;
      }
      
      @keyframes fadeInUp {
        from {
          opacity: 0;
          transform: translateY(20px);
        }
        to {
          opacity: 1;
          transform: translateY(0);
        }
      }
      
      .review-section {
        background-color: #f8f9fa;
        border-radius: 8px;
        padding: 1.5rem;
      }
      
      .review-item {
        background-color: white;
        padding: 1rem;
        border-radius: 6px;
        border: 1px solid #e9ecef;
      }
    </style>
  `,
})
export class EventWizardComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly events = inject(EventService);

  // تعليق: حالة الخطوة الحالية
  step = 1;
  
  // تعليق: عرض رسالة تحويل المستخدم لمنظم
  showOrganizerMessage = true;
  
  // تعليق: حالة الحفظ
  saving = false;
  
  // تعليق: نموذج البيانات
  form = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(300)]],
    description: ['', Validators.required],
    startDate: ['', Validators.required],
    endDate: ['', Validators.required],
    location: ['', [Validators.required, Validators.maxLength(400)]],
    categoryId: [''],
    cityId: [''],
    maxCapacity: [null],
  });

  ngOnInit(): void {
    // تعليق: فحص إذا كان المستخدم منظم بالفعل
    // TODO: ربط مع CurrentUser service للتحقق من الدور
    // this.showOrganizerMessage = !this.currentUser.hasRole('Organizer');
  }

  // تعليق: الموافقة على تحويل الحساب لمنظم
  confirmOrganizer(): void {
    this.showOrganizerMessage = false;
    // TODO: تحديث دور المستخدم في Backend
  }

  // تعليق: إلغاء وإرجاع للصفحة السابقة
  cancelOrganizer(): void {
    this.router.navigate(['/events']);
  }

  // تعليق: الانتقال للخطوة السابقة
  prev(): void { 
    if (this.step > 1) this.step--; 
  }

  // تعليق: الانتقال للخطوة التالية أو الحفظ
  next(): void {
    // التحقق من الحقول المطلوبة حسب الخطوة
    if (this.step === 1) {
      if (this.form.controls.title.invalid || this.form.controls.description.invalid) {
        this.form.controls.title.markAsTouched();
        this.form.controls.description.markAsTouched();
        return;
      }
      this.step++;
      return;
    }
    
    if (this.step === 2) {
      if (this.form.controls.startDate.invalid || this.form.controls.endDate.invalid || this.form.controls.location.invalid) {
        this.form.controls.startDate.markAsTouched();
        this.form.controls.endDate.markAsTouched();
        this.form.controls.location.markAsTouched();
        return;
      }
      this.step++;
      return;
    }
    
    // الخطوة 3 - الحفظ
    if (this.form.invalid) return;
    
    this.saving = true;
    const v = this.form.getRawValue() as any;
    
    // تعليق: إنشاء الفعالية وإرسالها للموافقة
    this.events.create(v).subscribe({
      next: (ev) => {
        this.saving = false;
        alert('✅ تم إرسال الفعالية للموافقة! سيتم إشعارك عند الموافقة عليها.');
        this.router.navigate(['/events']);
      },
      error: (err) => {
        this.saving = false;
        console.error('خطأ في إنشاء الفعالية:', err);
        alert('❌ حدث خطأ أثناء إنشاء الفعالية. يرجى المحاولة مرة أخرى.');
      }
    });
  }
}


