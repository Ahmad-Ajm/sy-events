import { Component, OnInit } from '@angular/core';
import { environment } from '../../../environments/environment';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HomeSliderService } from '../../proxy/home-slider/home-slider.service';
import { HomeSliderItemDto } from '../../proxy/home-slider/dtos/models';
import { EventService } from '../../proxy/event.service';
import { EventDto } from '../../proxy/events/dtos/models';
import { FeaturedBoxesComponent } from '../featured-boxes/featured-boxes.component';
import { ThemeService } from '../../shared/theme.service';
import { LoginSocialButtonsComponent } from '../../shared/login-social-buttons.component';

// تعليق: مكون الصفحة الرئيسية العامة - يعرض السلايدر والفعاليات
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule, CommonModule, FeaturedBoxesComponent, LoginSocialButtonsComponent],
  template: `
    <!-- الصفحة الرئيسية المخصصة لمنصة إدارة الفعاليات -->
    <div class="home-page">
      <!-- Slider Section -->
      <section class="hero-slider" *ngIf="sliderItems.length > 0 && !loading">
        <div id="homeSlider" class="carousel slide carousel-fade" data-bs-ride="carousel" data-bs-interval="5000">
          <!-- Indicators -->
          <div class="carousel-indicators">
            <button 
              *ngFor="let item of sliderItems; let i = index"
              type="button" 
              [attr.data-bs-target]="'#homeSlider'" 
              [attr.data-bs-slide-to]="i" 
              [class.active]="i === 0"
              [attr.aria-current]="i === 0 ? 'true' : null"
              [attr.aria-label]="'Slide ' + (i + 1)">
            </button>
          </div>
          
          <!-- Carousel Items -->
          <div class="carousel-inner">
            <div 
              *ngFor="let item of sliderItems; let i = index" 
              class="carousel-item" 
              [class.active]="i === 0">
              <!-- عرض صورة الفعالية أو الصورة المخصصة -->
              <img 
                [src]="resolveImageUrl(item.eventImageUrl || item.imageUrl)" 
                (error)="onImgError($event)"
                class="d-block w-100 slider-image" 
                [alt]="item.eventTitle || item.title">
              
              <!-- Caption -->
              <div class="carousel-caption d-none d-md-block">
                <div class="caption-content">
                  <h2 class="display-4 fw-bold text-white mb-3">{{ item.eventTitle || item.title }}</h2>
                  <p *ngIf="item.eventStartDate" class="event-date text-white-50 mb-4">
                    <i class="fas fa-calendar-alt me-2"></i>
                    {{ item.eventStartDate | date:'dd/MM/yyyy' }}
                  </p>
                  <a 
                    *ngIf="item.customEventId"
                    [routerLink]="['/events', item.customEventId]" 
                    class="btn btn-primary btn-lg mt-3 me-3">
                    <i class="fas fa-eye me-2"></i>
                    عرض التفاصيل
                  </a>
                  <button *ngIf="!item.customEventId" class="btn btn-outline-light btn-lg mt-3" (click)="followEvent()">
                    <i class="fas fa-heart me-2"></i>
                    متابعة الفعالية
                  </button>
                </div>
              </div>
            </div>
          </div>
          
          <!-- Controls -->
          <button class="carousel-control-prev" type="button" data-bs-target="#homeSlider" data-bs-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            <span class="visually-hidden">السابق</span>
          </button>
          <button class="carousel-control-next" type="button" data-bs-target="#homeSlider" data-bs-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="visually-hidden">التالي</span>
          </button>
        </div>
      </section>

      <!-- تعليق: المربعات الثلاث المميزة تحت السلايدر -->
      <app-featured-boxes></app-featured-boxes>

      <!-- Loading Spinner -->
      <div *ngIf="loading" class="text-center p-5">
        <div class="spinner-border text-primary" role="status" style="width: 3rem; height: 3rem;">
          <span class="visually-hidden">جاري التحميل...</span>
        </div>
        <p class="mt-3 text-muted">جاري تحميل الفعاليات...</p>
      </div>

      <!-- Empty State -->
      <div *ngIf="!loading && sliderItems.length === 0" class="text-center p-5">
        <i class="fas fa-images fa-4x text-muted mb-3"></i>
        <h3 class="text-muted">لا توجد فعاليات معروضة حالياً</h3>
        <p class="text-muted">يرجى المحاولة لاحقاً</p>
      </div>

      <!-- Featured Boxes: ديناميكية (الأحدث / الأكثر شعبية / مخصصة) -->
      <section class="container my-5">
        <div class="row g-4">
          <div class="col-md-4">
            <div class="card h-100 shadow-sm">
              <div class="card-body text-center">
                <i class="fas fa-clock fa-3x text-primary mb-3"></i>
                <h5 class="card-title">الأحدث</h5>
                <p class="card-text text-muted">اكتشف أحدث الفعاليات والأحداث في سوريا</p>
                <div class="list-group list-group-flush small text-start my-3" *ngIf="latestEvents.length">
                  <a *ngFor="let e of latestEvents" [routerLink]="['/events', e.id]" class="list-group-item list-group-item-action">
                    <i class="fa fa-calendar me-2"></i>{{ e.title }}
                  </a>
                </div>
                <a class="btn btn-outline-primary" [routerLink]="['/events']">
                  <i class="fas fa-arrow-left me-2"></i>
                  عرض المزيد
                </a>
              </div>
            </div>
          </div>
          <div class="col-md-4">
            <div class="card h-100 shadow-sm">
              <div class="card-body text-center">
                <i class="fas fa-fire fa-3x text-danger mb-3"></i>
                <h5 class="card-title">الأكثر شعبية</h5>
                <p class="card-text text-muted">الفعاليات الأكثر شعبية ومتابعة</p>
                <div class="list-group list-group-flush small text-start my-3" *ngIf="popularEvents.length">
                  <a *ngFor="let e of popularEvents" [routerLink]="['/events', e.id]" class="list-group-item list-group-item-action">
                    <i class="fa fa-fire me-2"></i>{{ e.title }}
                  </a>
                </div>
                <a class="btn btn-outline-primary" [routerLink]="['/events']">
                  <i class="fas fa-arrow-left me-2"></i>
                  عرض المزيد
                </a>
              </div>
            </div>
          </div>
          <div class="col-md-4">
            <div class="card h-100 shadow-sm">
              <div class="card-body text-center">
                <i class="fas fa-star fa-3x text-warning mb-3"></i>
                <h5 class="card-title">مخصصة</h5>
                <p class="card-text text-muted">قائمة مخصصة من الفعاليات المختارة</p>
                <div class="list-group list-group-flush small text-start my-3" *ngIf="customEvents.length">
                  <a *ngFor="let e of customEvents" [routerLink]="['/events', e.id]" class="list-group-item list-group-item-action">
                    <i class="fa fa-star me-2"></i>{{ e.title }}
                  </a>
                </div>
                <a class="btn btn-outline-primary" [routerLink]="['/events']">
                  <i class="fas fa-arrow-left me-2"></i>
                  عرض المزيد
                </a>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Welcome Section -->
      <section class="container my-5">
        <div class="row align-items-center">
          <div class="col-lg-6">
            <h2 class="display-5 fw-bold text-primary mb-4">مرحباً بك في منصة إدارة الفعاليات</h2>
            <p class="lead text-muted mb-4">
              منصة شاملة لإدارة الفعاليات والأحداث في سوريا. اكتشف الفعاليات القادمة، 
              سجل حضورك، وأنشئ فعالياتك الخاصة.
            </p>
            <div class="d-flex gap-3">
              <a class="btn btn-primary btn-lg" [routerLink]="['/events']">
                <i class="fas fa-calendar-alt me-2"></i>
                تصفح الفعاليات
              </a>
              <a class="btn btn-outline-primary btn-lg" href="/account/register">
                <i class="fas fa-user-plus me-2"></i>
                انضم إلينا
              </a>
              <button class="btn btn-outline-secondary btn-lg" (click)="toggleTheme()">
                <i class="fas" [class.fa-moon]="!isDark" [class.fa-sun]="isDark"></i>
                تبديل الثيم
              </button>
            </div>
          </div>
          <div class="col-lg-6">
            <img src="https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=600&h=400&fit=crop" 
                 class="img-fluid rounded shadow" 
                 alt="فعاليات سوريا">
          </div>
        </div>
      </section>
      <section class="container my-3">
        <login-social-buttons [issuer]="issuer" [returnUrl]="'/events'"></login-social-buttons>
      </section>
    </div>
  `,
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  // تعليق: قائمة عناصر السلايدر
  sliderItems: HomeSliderItemDto[] = [];
  // تعليق: حالة التحميل
  loading = false;
  // تعليق: قوائم صناديق الميزات
  latestEvents: EventDto[] = [];
  popularEvents: EventDto[] = [];
  customEvents: EventDto[] = [];
  isDark = false;
  issuer = environment.oAuthConfig.issuer;
  private readonly fallbackImage = 'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=1200&h=500&fit=crop';

  constructor(private sliderService: HomeSliderService, private eventService: EventService, private theme: ThemeService) {}

  ngOnInit() {
    this.theme.applyCurrent();
    this.isDark = this.theme.isDark();
    this.loadSliderItems();
    this.loadFeaturedBoxes();
  }

  // تعليق: تحميل عناصر السلايدر النشطة من الـ API
  loadSliderItems() {
    this.loading = true;
    this.sliderService.getActiveSliderItems().subscribe({
      next: (items) => {
        this.sliderItems = items;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading slider items:', error);
        this.loading = false;
      }
    });
  }

  // تعليق: تحميل صناديق الميزات ديناميكياً
  loadFeaturedBoxes() {
    // الأحدث: طلب الصفحة الأولى مع ترتيب تنازلي حسب التاريخ
    this.eventService.getList({ skipCount: 0, maxResultCount: 5, sorting: 'startDate DESC' } as any)
      .subscribe(res => this.latestEvents = res.items || []);

    // الأكثر شعبية: واجهة جاهزة
    this.eventService.getPopularEvents(5)
      .subscribe(list => this.popularEvents = list || []);

    // مخصصة: سنستخدم upcoming كتمهيد
    this.eventService.getUpcomingEvents(5)
      .subscribe(list => this.customEvents = list || []);
  }

  // تعليق: زر متابعة الفعالية للزائر — إعادة توجيه إلى صفحة الدخول مع returnUrl عام لقائمة الفعاليات
  followEvent() {
    const returnUrl = '/events';
    window.location.href = '/account/login?returnUrl=' + encodeURIComponent(returnUrl);
  }

  // تعليق: تبديل الثيم وتحديث الحالة
  toggleTheme(): void {
    this.theme.toggle();
    this.isDark = this.theme.isDark();
  }

  // تعليق: معالجة مسار صور السلايدر
  resolveImageUrl(url?: string | null): string {
    if (!url) {
      return this.fallbackImage;
    }
    const trimmed = url.trim();
    if (/\/images\/events\/default/i.test(trimmed)) {
      return this.fallbackImage;
    }
    if (trimmed.startsWith('http://') || trimmed.startsWith('https://') || trimmed.startsWith('data:') || trimmed.startsWith('/assets/')) {
      return trimmed;
    }
    if (trimmed.startsWith('/images/')) {
      return `${environment.apis.default.url}${trimmed}`;
    }
    return trimmed || this.fallbackImage;
  }

  // تعليق: فfallback عند فشل التحميل
  onImgError(event: Event): void {
    const img = event.target as HTMLImageElement;
    if (img && img.src !== this.fallbackImage) {
      img.src = this.fallbackImage;
    }
  }
}
