// تعليق: مكون المربعات الثلاث المميزة في الصفحة الرئيسية
import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Router } from '@angular/router';
import { FeaturedBoxService } from '../../proxy/featured-boxes/featured-box.service';
import { FeaturedBoxDto } from '../../proxy/featured-boxes/dtos/models';

@Component({
  selector: 'app-featured-boxes',
  standalone: true,
  imports: [CommonModule],
  template: `
    <!-- تعليق: المربعات الثلاث المميزة -->
    <div class="featured-boxes-section py-5 bg-light">
      <div class="container">
        <div class="row g-4">
          @for (box of boxes(); track box.id) {
            <div class="col-md-4">
              <div class="featured-box card h-100 shadow-sm hover-shadow" (click)="navigateToBox(box)">
                <!-- تعليق: صورة المربع -->
                <div class="featured-box-image">
                  <img 
                    [src]="resolveImageUrl(box.imageUrl || box.eventImageUrl)"
                    (error)="onImgError($event)"
                    [alt]="box.title || box.eventTitle"
                    class="card-img-top"
                    style="height: 200px; object-fit: cover;"
                  />
                  <!-- Badge للنوع -->
                  <div class="position-absolute top-0 end-0 m-2">
                    <span class="badge" [class.bg-primary]="box.type === 1" 
                          [class.bg-success]="box.type === 2"
                          [class.bg-info]="box.type === 3"
                          [class.bg-warning]="box.type === 4">
                      {{ getTypeLabel(box.type) }}
                    </span>
                  </div>
                </div>
                
                <!-- تعليق: محتوى المربع -->
                <div class="card-body d-flex flex-column">
                  <h5 class="card-title text-primary mb-2">
                    <i class="fas fa-star me-2"></i>
                    {{ box.title || box.eventTitle || 'فعالية' }}
                  </h5>
                  
                  <p class="card-text text-muted small mb-3">
                    {{ box.description || 'اكتشف المزيد من الفعاليات المميزة' }}
                  </p>
                  
                  @if (box.eventStartDate) {
                    <div class="event-meta mb-2">
                      <small class="text-muted">
                        <i class="fas fa-calendar me-1"></i>
                        {{ formatDate(box.eventStartDate) }}
                      </small>
                    </div>
                  }
                  
                  @if (box.eventLocation) {
                    <div class="event-meta mb-3">
                      <small class="text-muted">
                        <i class="fas fa-map-marker-alt me-1"></i>
                        {{ box.eventLocation }}
                      </small>
                    </div>
                  }
                  
                  <div class="mt-auto">
                    <button class="btn btn-outline-primary btn-sm w-100">
                      <i class="fas fa-arrow-left me-2"></i>
                      عرض التفاصيل
                    </button>
                  </div>
                </div>
              </div>
            </div>
          }
          
          <!-- تعليق: رسالة في حالة عدم وجود مربعات -->
          @if (boxes().length === 0 && !loading()) {
            <div class="col-12 text-center py-5">
              <i class="fas fa-box-open fa-3x text-muted mb-3"></i>
              <p class="text-muted">لا توجد مربعات مميزة حالياً</p>
            </div>
          }
          
          <!-- تعليق: مؤشر التحميل -->
          @if (loading()) {
            <div class="col-12 text-center py-5">
              <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">جاري التحميل...</span>
              </div>
            </div>
          }
        </div>
      </div>
    </div>
    
    <!-- تعليق: CSS مضمن للتحسينات -->
    <style>
      .featured-box {
        cursor: pointer;
        transition: all 0.3s ease;
        border: none;
      }
      
      .featured-box:hover {
        transform: translateY(-5px);
      }
      
      .hover-shadow {
        box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
      }
      
      .hover-shadow:hover {
        box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
      }
      
      .featured-box-image {
        position: relative;
        overflow: hidden;
      }
      
      .featured-box-image img {
        transition: transform 0.3s ease;
      }
      
      .featured-box:hover .featured-box-image img {
        transform: scale(1.05);
      }
      
      .event-meta {
        display: flex;
        align-items: center;
        gap: 0.5rem;
      }
    </style>
  `,
})
export class FeaturedBoxesComponent implements OnInit {
  private readonly featuredBoxService = inject(FeaturedBoxService);
  private readonly router = inject(Router);
  
  // تعليق: حالة البيانات باستخدام signals
  boxes = signal<FeaturedBoxDto[]>([]);
  loading = signal(true);
  // تعليق: صورة احتياطية في حال تعذّر تحميل الصورة الأساسية
  private readonly fallbackImage = 'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=800&h=400&fit=crop';
  
  ngOnInit(): void {
    this.loadFeaturedBoxes();
  }
  
  // تعليق: جلب المربعات من الـ API
  private loadFeaturedBoxes(): void {
    this.featuredBoxService.getActiveFeaturedBoxes().subscribe({
      next: (data) => {
        this.boxes.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error loading featured boxes:', err);
        this.loading.set(false);
      }
    });
  }
  
  // تعليق: التنقل عند الضغط على المربع
  navigateToBox(box: FeaturedBoxDto): void {
    if (box.customEventId) {
      this.router.navigate(['/events', box.customEventId]);
      return;
    }
    if (box.customLink) {
      this.router.navigateByUrl(box.customLink);
    } else if (box.eventTitle) {
      // fallback للذهاب لصفحة الفعاليات
      this.router.navigate(['/events']);
    } else {
      this.router.navigate(['/events']);
    }
  }
  
  // تعليق: الحصول على تسمية النوع
  getTypeLabel(type: number): string {
    switch (type) {
      case 1: return 'أحدث';
      case 2: return 'الأكثر شعبية';
      case 3: return 'مخصص';
      case 4: return 'قادم قريباً';
      default: return '';
    }
  }
  
  // تعليق: تنسيق التاريخ
  formatDate(date: Date | string | null): string {
    if (!date) return '';
    const d = new Date(date);
    return d.toLocaleDateString('ar-EG', { 
      year: 'numeric', 
      month: 'long', 
      day: 'numeric',
      weekday: 'long'
    });
  }

  // تعليق: معالجة مسار الصورة ليعمل على بيئة التطوير والإنتاج
  resolveImageUrl(url?: string | null): string {
    if (!url) {
      return this.fallbackImage;
    }
    const trimmed = url.trim();
    // منع طلب place-holderات غير الموجودة افتراضياً
    if (/\/images\/events\/default/i.test(trimmed)) {
      return this.fallbackImage;
    }
    if (trimmed.startsWith('http://') || trimmed.startsWith('https://') || trimmed.startsWith('data:')) {
      return trimmed;
    }
    if (trimmed.startsWith('/assets/')) {
      return trimmed;
    }
    // إذا كان المسار يبدأ بـ /images/ فنوجهه إلى الخادم الخلفي (44388)
    if (trimmed.startsWith('/images/')) {
      return `${environment.apis.default.url}${trimmed}`;
    }
    // عودة المسار كما هو وإلا استخدام صورة بديلة
    return trimmed || this.fallbackImage;
  }

  // تعليق: فallback عند فشل تحميل الصورة
  onImgError(event: Event): void {
    const img = event.target as HTMLImageElement;
    if (img && img.src !== this.fallbackImage) {
      img.src = this.fallbackImage;
    }
  }
}

