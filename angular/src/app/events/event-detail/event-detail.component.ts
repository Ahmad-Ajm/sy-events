import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EventService } from '../../proxy/event.service';
import { EventDto } from '../../proxy/events/dtos/models';
import { BookingService } from '../../proxy/bookings/booking.service';
import { AuthService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
// Leaflet lazy import to avoid SSR issues
declare const L: any;

@Component({
  selector: 'app-event-detail',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container py-4" *ngIf="event">
      <!-- تعليق: رأس الفعالية مع الصورة -->
      <div class="card mb-4 overflow-hidden">
        <img 
          [src]="getImageUrl(event.imageUrl)" 
          (error)="onImageError($event)"
          class="card-img-top" 
          style="height: 300px; object-fit: cover;"
          [alt]="event.title">
        <div class="card-body">
          <div class="d-flex justify-content-between align-items-start mb-3">
            <div>
              <h2 class="mb-2">{{ event.title }}</h2>
              <p class="text-muted mb-0">
                <i class="fas fa-map-marker-alt me-2"></i>{{ event.location }}
              </p>
              <p class="text-muted mb-0">
                <i class="fas fa-calendar me-2"></i>{{ event.startDate | date:'dd/MM/yyyy HH:mm' }}
              </p>
              <p class="text-muted" *ngIf="event.availableCapacity !== null">
                <i class="fas fa-users me-2"></i>السعة المتاحة: {{ event.availableCapacity }} / {{ event.maxCapacity }}
              </p>
            </div>
            
            <!-- تعليق: زر متابعة الفعالية -->
            <div>
              <button 
                *ngIf="!isFollowing && !loading"
                class="btn btn-primary btn-lg"
                (click)="followEvent()"
                [disabled]="isSubmitting">
                <i class="fas fa-heart me-2"></i>
                {{ isSubmitting ? 'جاري المتابعة...' : 'متابعة الفعالية' }}
              </button>
              
              <button 
                *ngIf="isFollowing && !loading"
                class="btn btn-outline-danger btn-lg"
                (click)="unfollowEvent()"
                [disabled]="isSubmitting">
                <i class="fas fa-heart-broken me-2"></i>
                {{ isSubmitting ? 'جاري الإلغاء...' : 'إلغاء المتابعة' }}
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- تعليق: الوصف -->
      <div class="card mb-4">
        <div class="card-header">
          <h5 class="mb-0">
            <i class="fas fa-info-circle me-2"></i>
            وصف الفعالية
          </h5>
        </div>
        <div class="card-body">
          <p class="mb-0">{{ event.description }}</p>
        </div>
      </div>

      <!-- تعليق: خريطة تفاعلية (Leaflet) -->
      <div class="card">
        <div class="card-header">
          <h5 class="mb-0">
            <i class="fas fa-map me-2"></i>
            الموقع
          </h5>
        </div>
        <div class="card-body p-0">
          <div id="map" style="height: 400px;"></div>
        </div>
        <div class="card-footer">
          <a class="btn btn-outline-primary btn-sm" [href]="googleMapsUrl()" target="_blank" rel="noopener">
            <i class="fas fa-external-link-alt me-2"></i>
            فتح في خرائط Google
          </a>
        </div>
      </div>
    </div>
    
    <!-- تعليق: Loading State -->
    <div class="container py-5 text-center" *ngIf="loading">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">جاري التحميل...</span>
      </div>
      <p class="mt-3 text-muted">جاري تحميل تفاصيل الفعالية...</p>
    </div>
  `,
})
export class EventDetailComponent implements OnInit, AfterViewInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly events = inject(EventService);
  private readonly bookingService = inject(BookingService);
  private readonly auth = inject(AuthService);
  private readonly toaster = inject(ToasterService);
  
  event?: EventDto;
  private mapInited = false;
  loading = true;
  isSubmitting = false;
  isFollowing = false;
  fallbackImage = 'https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=800&h=400&fit=crop';

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loading = true;
      this.events.get(id).subscribe({
        next: (e) => {
          this.event = e;
          this.loading = false;
          this.checkFollowingStatus(id);
        },
        error: (err) => {
          console.error('Error loading event:', err);
          this.toaster.error('فشل تحميل تفاصيل الفعالية');
          this.loading = false;
        }
      });
    }
  }
  
  // تعليق: التحقق من حالة المتابعة
  private checkFollowingStatus(eventId: string): void {
    if (!this.auth.isAuthenticated) {
      this.isFollowing = false;
      return;
    }
    
    // تعليق: استدعاء API للتحقق من المتابعة
    this.bookingService.isFollowingEvent(eventId).subscribe({
      next: (result) => {
        this.isFollowing = result;
      },
      error: (err) => {
        console.error('Error checking follow status:', err);
        this.isFollowing = false;
      }
    });
  }
  
  // تعليق: متابعة الفعالية
  followEvent(): void {
    if (!this.auth.isAuthenticated) {
      this.toaster.warn('يجب تسجيل الدخول لمتابعة الفعالية');
      this.router.navigate(['/account/login'], {
        queryParams: { returnUrl: this.router.url }
      });
      return;
    }
    
    if (!this.event) return;
    
    this.isSubmitting = true;
    
    // تعليق: استدعاء API لمتابعة الفعالية
    this.bookingService.followEvent(this.event.id).subscribe({
      next: () => {
        this.isFollowing = true;
        this.toaster.success('تمت متابعة الفعالية بنجاح!');
      },
      error: (err) => {
        console.error('Error following event:', err);
        const message = err?.error?.error?.message || 'فشل في متابعة الفعالية';
        this.toaster.error(message);
      },
      complete: () => {
        this.isSubmitting = false;
      }
    });
  }
  
  // تعليق: إلغاء متابعة الفعالية
  unfollowEvent(): void {
    if (!this.event) return;
    
    this.isSubmitting = true;
    
    // تعليق: استدعاء API لإلغاء المتابعة
    this.bookingService.unfollowEvent(this.event.id).subscribe({
      next: () => {
        this.isFollowing = false;
        this.toaster.info('تم إلغاء المتابعة');
      },
      error: (err) => {
        console.error('Error unfollowing event:', err);
        const message = err?.error?.error?.message || 'فشل في إلغاء المتابعة';
        this.toaster.error(message);
      },
      complete: () => {
        this.isSubmitting = false;
      }
    });
  }
  
  // تعليق: الحصول على مسار الصورة الصحيح
  getImageUrl(imageUrl: string | null | undefined): string {
    if (!imageUrl) {
      return this.fallbackImage;
    }
    // تعليق: إذا كانت الصورة تبدأ بـ /images فهي من Backend
    if (imageUrl.startsWith('/images/')) {
      return `https://localhost:44388${imageUrl}`;
    }
    return imageUrl || this.fallbackImage;
  }
  
  // تعليق: معالجة خطأ الصورة
  onImageError(event: Event): void {
    const img = event.target as HTMLImageElement;
    img.src = this.fallbackImage;
  }

  async ngAfterViewInit(): Promise<void> {
    // تحميل Leaflet ديناميكياً
    if (!(window as any).L) {
      await this.loadLeafletAssets();
    }
    this.initMapWhenReady();
  }

  private initMapWhenReady(): void {
    const wait = setInterval(() => {
      if (this.event && (window as any).L && !this.mapInited) {
        this.mapInited = true;
        const coords = this.getCoordinates();
        const map = L.map('map').setView([coords.lat, coords.lng], 13);
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
          maxZoom: 19,
          attribution: '© OpenStreetMap'
        }).addTo(map);
        L.marker([coords.lat, coords.lng]).addTo(map).bindPopup(this.event?.title || 'فعالية');
        clearInterval(wait);
      }
    }, 300);
  }

  private getCoordinates(): { lat: number; lng: number } {
    // مبدئي: إذا توفرت إحداثيات ضمن الوصف بصيغة lat,lng استخدمها، وإلا fallback لدمشق
    const fallback = { lat: 33.5138, lng: 36.2765 }; // Damascus
    const desc = (this.event?.description || '') + ' ' + (this.event?.location || '');
    const match = /(-?\d{1,2}\.\d+)[,\s]+(-?\d{1,3}\.\d+)/.exec(desc);
    if (match) {
      return { lat: parseFloat(match[1]), lng: parseFloat(match[2]) };
    }
    return fallback;
  }

  googleMapsUrl(): string {
    const c = this.getCoordinates();
    return `https://www.google.com/maps/search/?api=1&query=${c.lat},${c.lng}`;
  }

  private loadLeafletAssets(): Promise<void> {
    return new Promise(resolve => {
      const css = document.createElement('link');
      css.rel = 'stylesheet';
      css.href = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.css';
      document.head.appendChild(css);

      const script = document.createElement('script');
      script.src = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.js';
      script.onload = () => resolve();
      document.body.appendChild(script);
    });
  }
}


