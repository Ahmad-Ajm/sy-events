// تعليق: مكون إعدادات الثيم - تخصيص الألوان والوضع الداكن/الفاتح
import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ThemeService } from '../../shared/theme.service';

interface ThemeColor {
  name: string;
  nameAr: string;
  cssVar: string;
  value: string;
}

@Component({
  selector: 'app-theme-settings',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <!-- تعليق: صفحة إعدادات الثيم -->
    <div class="container py-4">
      <div class="card">
        <div class="card-header">
          <h3 class="mb-0">
            <i class="fas fa-palette me-2"></i>
            إعدادات الثيم والألوان
          </h3>
        </div>
        <div class="card-body">
          <!-- تعليق: زر تبديل الوضع الداكن/الفاتح -->
          <div class="mb-4">
            <h5 class="mb-3">الوضع العام</h5>
            <div class="form-check form-switch">
              <input 
                class="form-check-input" 
                type="checkbox" 
                id="darkModeSwitch" 
                [checked]="isDarkMode()"
                (change)="toggleDarkMode()"
              >
              <label class="form-check-label" for="darkModeSwitch">
                <i [class]="isDarkMode() ? 'fas fa-moon' : 'fas fa-sun'"></i>
                {{ isDarkMode() ? 'الوضع الداكن' : 'الوضع الفاتح' }}
              </label>
            </div>
          </div>

          <hr>

          <!-- تعليق: تخصيص الألوان الأساسية -->
          <div class="mb-4">
            <h5 class="mb-3">الألوان الأساسية</h5>
            <form [formGroup]="colorForm" (ngSubmit)="saveColors()">
              <div class="row g-3">
                <!-- تعليق: حقل لكل لون أساسي -->
                <div class="col-md-6" *ngFor="let color of themeColors">
                  <div class="color-picker-field">
                    <label class="form-label">
                      {{ color.nameAr }}
                      <span class="text-muted small">({{ color.name }})</span>
                    </label>
                    <div class="input-group">
                      <input 
                        type="color" 
                        class="form-control form-control-color" 
                        [formControlName]="color.cssVar"
                        title="اختر اللون"
                      >
                      <input 
                        type="text" 
                        class="form-control" 
                        [formControlName]="color.cssVar + '-text'"
                        placeholder="#000000"
                        maxlength="7"
                      >
                      <button 
                        class="btn btn-outline-secondary" 
                        type="button"
                        (click)="resetColor(color)"
                        title="إعادة تعيين"
                      >
                        <i class="fas fa-undo"></i>
                      </button>
                    </div>
                    <!-- تعليق: معاينة اللون -->
                    <div class="color-preview mt-2" [style.backgroundColor]="colorForm.get(color.cssVar)?.value">
                      <small class="text-white">معاينة</small>
                    </div>
                  </div>
                </div>
              </div>

              <!-- تعليق: أزرار الحفظ والإعادة -->
              <div class="mt-4 d-flex gap-2">
                <button type="submit" class="btn btn-primary" [disabled]="saving()">
                  <i class="fas fa-save me-2"></i>
                  {{ saving() ? 'جاري الحفظ...' : 'حفظ التغييرات' }}
                </button>
                <button type="button" class="btn btn-outline-secondary" (click)="resetAllColors()">
                  <i class="fas fa-undo me-2"></i>
                  إعادة تعيين الكل
                </button>
                <button type="button" class="btn btn-outline-info" (click)="previewChanges()">
                  <i class="fas fa-eye me-2"></i>
                  معاينة
                </button>
              </div>
            </form>
          </div>

          <hr>

          <!-- تعليق: قوالب ألوان جاهزة -->
          <div class="mb-4">
            <h5 class="mb-3">قوالب ألوان جاهزة</h5>
            <div class="row g-3">
              <div class="col-md-4" *ngFor="let preset of colorPresets">
                <div 
                  class="theme-preset-card" 
                  (click)="applyPreset(preset)"
                  [class.active]="currentPreset() === preset.id"
                >
                  <div class="preset-name mb-2">{{ preset.name }}</div>
                  <div class="preset-colors d-flex gap-1">
                    <div 
                      *ngFor="let color of preset.colors" 
                      class="preset-color-sample"
                      [style.backgroundColor]="color"
                    ></div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- تعليق: إشعار النجاح -->
          <div class="alert alert-success mt-3" *ngIf="showSuccess()">
            <i class="fas fa-check-circle me-2"></i>
            تم حفظ إعدادات الثيم بنجاح!
          </div>
        </div>
      </div>

      <!-- تعليق: بطاقة المعاينة -->
      <div class="card mt-4">
        <div class="card-header">
          <h5 class="mb-0">
            <i class="fas fa-desktop me-2"></i>
            معاينة التصميم
          </h5>
        </div>
        <div class="card-body">
          <p class="text-muted mb-3">مثال على كيفية ظهور الألوان في الموقع:</p>
          <div class="preview-demo">
            <button class="btn btn-primary me-2">زر أساسي</button>
            <button class="btn btn-secondary me-2">زر ثانوي</button>
            <button class="btn btn-success me-2">نجاح</button>
            <button class="btn btn-danger me-2">خطر</button>
            <button class="btn btn-warning me-2">تحذير</button>
            <button class="btn btn-info">معلومات</button>
          </div>
          <div class="preview-cards mt-3">
            <div class="card mb-2">
              <div class="card-header bg-primary text-white">عنوان البطاقة</div>
              <div class="card-body">محتوى البطاقة مع النص العادي</div>
            </div>
            <div class="alert alert-primary">تنبيه أساسي</div>
            <div class="alert alert-success">تنبيه نجاح</div>
          </div>
        </div>
      </div>
    </div>

    <!-- تعليق: CSS مضمن -->
    <style>
      .color-picker-field {
        background: #f8f9fa;
        padding: 1rem;
        border-radius: 8px;
        border: 1px solid #dee2e6;
      }
      
      .form-control-color {
        width: 60px;
        height: 38px;
        cursor: pointer;
      }
      
      .color-preview {
        height: 40px;
        border-radius: 4px;
        display: flex;
        align-items: center;
        justify-content: center;
        border: 1px solid #dee2e6;
      }
      
      .theme-preset-card {
        padding: 1rem;
        border: 2px solid #dee2e6;
        border-radius: 8px;
        cursor: pointer;
        transition: all 0.3s ease;
        background: white;
      }
      
      .theme-preset-card:hover {
        border-color: #007bff;
        box-shadow: 0 4px 12px rgba(0,0,0,0.1);
      }
      
      .theme-preset-card.active {
        border-color: #28a745;
        background: #f0fff4;
      }
      
      .preset-name {
        font-weight: bold;
        text-align: center;
      }
      
      .preset-color-sample {
        height: 30px;
        flex: 1;
        border-radius: 4px;
        border: 1px solid rgba(0,0,0,0.1);
      }
      
      .preview-demo {
        padding: 1rem;
        background: #f8f9fa;
        border-radius: 8px;
      }
      
      /* تعليق: تنسيقات الوضع الداكن */
      :host-context(body.theme-dark) .color-picker-field {
        background: #2d3748;
        border-color: #4a5568;
      }
      
      :host-context(body.theme-dark) .theme-preset-card {
        background: #2d3748;
        border-color: #4a5568;
      }
      
      :host-context(body.theme-dark) .theme-preset-card.active {
        background: #1a4d2e;
      }
    </style>
  `,
})
export class ThemeSettingsComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly themeService = inject(ThemeService);

  // تعليق: حالات المكون
  isDarkMode = signal(false);
  saving = signal(false);
  showSuccess = signal(false);
  currentPreset = signal<string | null>(null);

  // تعليق: الألوان الأساسية للثيم
  themeColors: ThemeColor[] = [
    { name: 'Primary', nameAr: 'اللون الأساسي', cssVar: 'primary', value: '#007bff' },
    { name: 'Secondary', nameAr: 'اللون الثانوي', cssVar: 'secondary', value: '#6c757d' },
    { name: 'Success', nameAr: 'لون النجاح', cssVar: 'success', value: '#28a745' },
    { name: 'Danger', nameAr: 'لون الخطر', cssVar: 'danger', value: '#dc3545' },
    { name: 'Warning', nameAr: 'لون التحذير', cssVar: 'warning', value: '#ffc107' },
    { name: 'Info', nameAr: 'لون المعلومات', cssVar: 'info', value: '#17a2b8' },
  ];

  // تعليق: قوالب ألوان جاهزة
  colorPresets = [
    {
      id: 'default',
      name: 'الافتراضي',
      colors: ['#007bff', '#6c757d', '#28a745', '#dc3545', '#ffc107', '#17a2b8']
    },
    {
      id: 'ocean',
      name: 'المحيط',
      colors: ['#0077be', '#4a5568', '#38b2ac', '#e53e3e', '#ed8936', '#4299e1']
    },
    {
      id: 'forest',
      name: 'الغابة',
      colors: ['#2d7738', '#4a5568', '#48bb78', '#f56565', '#ed8936', '#38b2ac']
    },
    {
      id: 'sunset',
      name: 'الغروب',
      colors: ['#d64545', '#4a5568', '#48bb78', '#e53e3e', '#ed8936', '#f56565']
    },
    {
      id: 'royal',
      name: 'ملكي',
      colors: ['#5a67d8', '#4a5568', '#48bb78', '#f56565', '#ed8936', '#667eea']
    },
    {
      id: 'modern',
      name: 'عصري',
      colors: ['#000000', '#718096', '#48bb78', '#fc8181', '#f6ad55', '#63b3ed']
    }
  ];

  // تعليق: نموذج الألوان
  colorForm = this.fb.group({});

  ngOnInit(): void {
    // تعليق: تحميل الوضع الحالي
    this.isDarkMode.set(this.themeService.isDark());
    this.themeService.applyCurrent();

    // تعليق: تهيئة حقول النموذج
    this.themeColors.forEach(color => {
      const savedColor = localStorage.getItem(`theme-color-${color.cssVar}`) || color.value;
      this.colorForm.addControl(color.cssVar, this.fb.control(savedColor, Validators.required));
      this.colorForm.addControl(color.cssVar + '-text', this.fb.control(savedColor, [
        Validators.required,
        Validators.pattern(/^#[0-9A-Fa-f]{6}$/)
      ]));

      // تعليق: مزامنة حقل اللون مع حقل النص
      this.colorForm.get(color.cssVar)?.valueChanges.subscribe(value => {
        this.colorForm.get(color.cssVar + '-text')?.setValue(value, { emitEvent: false });
      });

      this.colorForm.get(color.cssVar + '-text')?.valueChanges.subscribe(value => {
        if (/^#[0-9A-Fa-f]{6}$/.test(value)) {
          this.colorForm.get(color.cssVar)?.setValue(value, { emitEvent: false });
        }
      });
    });

    // تعليق: تحميل القالب المحفوظ
    const savedPreset = localStorage.getItem('theme-preset');
    if (savedPreset) {
      this.currentPreset.set(savedPreset);
    }
  }

  // تعليق: تبديل الوضع الداكن/الفاتح
  toggleDarkMode(): void {
    this.themeService.toggle();
    this.isDarkMode.set(this.themeService.isDark());
  }

  // تعليق: حفظ الألوان
  saveColors(): void {
    if (this.colorForm.invalid) return;

    this.saving.set(true);

    // تعليق: حفظ الألوان في localStorage
    this.themeColors.forEach(color => {
      const value = this.colorForm.get(color.cssVar)?.value;
      if (value) {
        localStorage.setItem(`theme-color-${color.cssVar}`, value);
        // تطبيق اللون على CSS Variables
        document.documentElement.style.setProperty(`--color-${color.cssVar}`, value);
      }
    });

    setTimeout(() => {
      this.saving.set(false);
      this.showSuccess.set(true);
      setTimeout(() => this.showSuccess.set(false), 3000);
    }, 500);
  }

  // تعليق: إعادة تعيين لون واحد
  resetColor(color: ThemeColor): void {
    this.colorForm.get(color.cssVar)?.setValue(color.value);
    this.colorForm.get(color.cssVar + '-text')?.setValue(color.value);
    localStorage.removeItem(`theme-color-${color.cssVar}`);
    document.documentElement.style.setProperty(`--color-${color.cssVar}`, color.value);
  }

  // تعليق: إعادة تعيين جميع الألوان
  resetAllColors(): void {
    if (!confirm('هل تريد إعادة تعيين جميع الألوان إلى القيم الافتراضية؟')) return;

    this.themeColors.forEach(color => {
      this.resetColor(color);
    });
    this.currentPreset.set(null);
    localStorage.removeItem('theme-preset');
    alert('تم إعادة تعيين جميع الألوان بنجاح!');
  }

  // تعليق: تطبيق قالب ألوان جاهز
  applyPreset(preset: any): void {
    preset.colors.forEach((color: string, index: number) => {
      if (index < this.themeColors.length) {
        const themeColor = this.themeColors[index];
        this.colorForm.get(themeColor.cssVar)?.setValue(color);
        this.colorForm.get(themeColor.cssVar + '-text')?.setValue(color);
      }
    });
    this.currentPreset.set(preset.id);
    localStorage.setItem('theme-preset', preset.id);
    this.saveColors();
  }

  // تعليق: معاينة التغييرات مؤقتاً
  previewChanges(): void {
    this.themeColors.forEach(color => {
      const value = this.colorForm.get(color.cssVar)?.value;
      if (value) {
        document.documentElement.style.setProperty(`--color-${color.cssVar}`, value);
      }
    });
    alert('تم تطبيق المعاينة! اضغط "حفظ التغييرات" لحفظها بشكل دائم.');
  }
}

