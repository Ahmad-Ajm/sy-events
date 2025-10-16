// تعليق: مكون جدولة الاجتماعات - طلب وإدارة اللقاءات بين الحضور
import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

interface Meeting {
  id: string;
  eventId: string;
  requesterId: string;
  requesterName: string;
  requestedId: string;
  requestedName: string;
  meetingTime: string;
  location: string;
  status: number;
  notes: string;
  rejectionReason?: string;
}

@Component({
  selector: 'app-meetings',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <!-- تعليق: صفحة جدولة الاجتماعات -->
    <div class="container py-4">
      <h2 class="mb-4">
        <i class="fas fa-handshake me-2"></i>
        جدولة الاجتماعات
      </h2>

      <!-- تبويبات -->
      <ul class="nav nav-tabs mb-4">
        <li class="nav-item">
          <a class="nav-link" [class.active]="activeTab() === 'incoming'" (click)="activeTab.set('incoming')">
            <i class="fas fa-inbox me-2"></i>
            الطلبات الواردة ({{ incomingRequests().length }})
          </a>
        </li>
        <li class="nav-item">
          <a class="nav-link" [class.active]="activeTab() === 'outgoing'" (click)="activeTab.set('outgoing')">
            <i class="fas fa-paper-plane me-2"></i>
            الطلبات الصادرة ({{ outgoingRequests().length }})
          </a>
        </li>
        <li class="nav-item">
          <a class="nav-link" [class.active]="activeTab() === 'accepted'" (click)="activeTab.set('accepted')">
            <i class="fas fa-calendar-check me-2"></i>
            اجتماعات مؤكدة ({{ acceptedMeetings().length }})
          </a>
        </li>
      </ul>

      <!-- محتوى التبويبات -->
      @if (activeTab() === 'incoming') {
        <div class="row">
          @for (meeting of incomingRequests(); track meeting.id) {
            <div class="col-md-6 mb-3">
              <div class="card">
                <div class="card-body">
                  <div class="d-flex justify-content-between align-items-start mb-3">
                    <div>
                      <h5 class="mb-1">{{ meeting.requesterName }}</h5>
                      <small class="text-muted">طلب لقاء معك</small>
                    </div>
                    <span class="badge bg-warning">معلق</span>
                  </div>
                  
                  <div class="mb-3">
                    <p class="mb-2">
                      <i class="fas fa-clock me-2"></i>
                      {{ meeting.meetingTime | date:'medium' }}
                    </p>
                    <p class="mb-2">
                      <i class="fas fa-map-marker-alt me-2"></i>
                      {{ meeting.location }}
                    </p>
                    @if (meeting.notes) {
                      <p class="mb-0 text-muted">
                        <i class="fas fa-sticky-note me-2"></i>
                        {{ meeting.notes }}
                      </p>
                    }
                  </div>

                  <div class="d-flex gap-2">
                    <button class="btn btn-success btn-sm" (click)="acceptMeeting(meeting.id)">
                      <i class="fas fa-check me-1"></i>
                      قبول
                    </button>
                    <button class="btn btn-danger btn-sm" (click)="rejectMeeting(meeting.id)">
                      <i class="fas fa-times me-1"></i>
                      رفض
                    </button>
                  </div>
                </div>
              </div>
            </div>
          } @empty {
            <div class="col-12">
              <div class="alert alert-info">
                <i class="fas fa-info-circle me-2"></i>
                لا توجد طلبات واردة
              </div>
            </div>
          }
        </div>
      }

      @if (activeTab() === 'outgoing') {
        <div class="row">
          @for (meeting of outgoingRequests(); track meeting.id) {
            <div class="col-md-6 mb-3">
              <div class="card">
                <div class="card-body">
                  <div class="d-flex justify-content-between align-items-start mb-3">
                    <div>
                      <h5 class="mb-1">{{ meeting.requestedName }}</h5>
                      <small class="text-muted">طلبت لقاءً</small>
                    </div>
                    @if (meeting.status === 1) {
                      <span class="badge bg-warning">معلق</span>
                    }
                    @if (meeting.status === 2) {
                      <span class="badge bg-success">مقبول</span>
                    }
                    @if (meeting.status === 3) {
                      <span class="badge bg-danger">مرفوض</span>
                    }
                  </div>
                  
                  <p class="mb-2">
                    <i class="fas fa-clock me-2"></i>
                    {{ meeting.meetingTime | date:'medium' }}
                  </p>
                  <p class="mb-2">
                    <i class="fas fa-map-marker-alt me-2"></i>
                    {{ meeting.location }}
                  </p>
                  
                  @if (meeting.status === 3 && meeting.rejectionReason) {
                    <div class="alert alert-danger mb-0">
                      <small><strong>سبب الرفض:</strong> {{ meeting.rejectionReason }}</small>
                    </div>
                  }
                  
                  @if (meeting.status === 1) {
                    <button class="btn btn-outline-danger btn-sm mt-2" (click)="cancelMeeting(meeting.id)">
                      <i class="fas fa-ban me-1"></i>
                      إلغاء الطلب
                    </button>
                  }
                </div>
              </div>
            </div>
          } @empty {
            <div class="col-12">
              <div class="alert alert-info">
                <i class="fas fa-info-circle me-2"></i>
                لم ترسل أي طلبات بعد
              </div>
            </div>
          }
        </div>
      }

      @if (activeTab() === 'accepted') {
        <div class="row">
          @for (meeting of acceptedMeetings(); track meeting.id) {
            <div class="col-md-6 mb-3">
              <div class="card border-success">
                <div class="card-body">
                  <div class="d-flex justify-content-between align-items-start mb-3">
                    <div>
                      <h5 class="mb-1">
                        {{ meeting.requesterId === currentUserId ? meeting.requestedName : meeting.requesterName }}
                      </h5>
                      <small class="text-muted">اجتماع مؤكد</small>
                    </div>
                    <span class="badge bg-success">مقبول</span>
                  </div>
                  
                  <p class="mb-2">
                    <i class="fas fa-clock me-2 text-success"></i>
                    <strong>{{ meeting.meetingTime | date:'medium' }}</strong>
                  </p>
                  <p class="mb-2">
                    <i class="fas fa-map-marker-alt me-2 text-success"></i>
                    {{ meeting.location }}
                  </p>
                  
                  @if (meeting.notes) {
                    <p class="mb-3 text-muted small">
                      {{ meeting.notes }}
                    </p>
                  }

                  <button class="btn btn-outline-danger btn-sm" (click)="cancelMeeting(meeting.id)">
                    <i class="fas fa-ban me-1"></i>
                    إلغاء الاجتماع
                  </button>
                </div>
              </div>
            </div>
          } @empty {
            <div class="col-12">
              <div class="alert alert-info">
                <i class="fas fa-info-circle me-2"></i>
                لا توجد اجتماعات مؤكدة حالياً
              </div>
            </div>
          }
        </div>
      }
    </div>
  `,
  styles: [`
    .nav-tabs .nav-link {
      cursor: pointer;
    }
    
    .card {
      transition: all 0.2s ease;
    }
    
    .card:hover {
      box-shadow: 0 4px 12px rgba(0,0,0,0.1);
    }
  `]
})
export class MeetingsComponent implements OnInit {
  private readonly http: HttpClient;
  
  activeTab = signal<'incoming' | 'outgoing' | 'accepted'>('incoming');
  incomingRequests = signal<Meeting[]>([]);
  outgoingRequests = signal<Meeting[]>([]);
  acceptedMeetings = signal<Meeting[]>([]);
  currentUserId = ''; // TODO: get from CurrentUser service

  constructor(http: HttpClient) {
    this.http = http;
  }

  ngOnInit(): void {
    this.loadMeetings();
  }

  loadMeetings(): void {
    // تعليق: جلب الطلبات الواردة
    this.http.get<Meeting[]>('/api/app/attendee-meeting/incoming-requests').subscribe({
      next: (data) => this.incomingRequests.set(data),
      error: (err) => console.error('فشل تحميل الطلبات الواردة', err)
    });

    // تعليق: جلب الطلبات الصادرة
    this.http.get<Meeting[]>('/api/app/attendee-meeting/outgoing-requests').subscribe({
      next: (data) => this.outgoingRequests.set(data),
      error: (err) => console.error('فشل تحميل الطلبات الصادرة', err)
    });

    // تعليق: جلب الاجتماعات المؤكدة
    this.http.get<Meeting[]>('/api/app/attendee-meeting/my-meetings').subscribe({
      next: (data) => this.acceptedMeetings.set(data),
      error: (err) => console.error('فشل تحميل الاجتماعات', err)
    });
  }

  acceptMeeting(id: string): void {
    this.http.post(`/api/app/attendee-meeting/${id}/accept`, {}).subscribe({
      next: () => {
        alert('✅ تم قبول طلب الاجتماع!');
        this.loadMeetings();
      },
      error: (err) => {
        console.error('فشل قبول الطلب', err);
        alert('❌ حدث خطأ');
      }
    });
  }

  rejectMeeting(id: string): void {
    const reason = prompt('الرجاء إدخال سبب الرفض (اختياري):');
    if (reason === null) return;

    this.http.post(`/api/app/attendee-meeting/${id}/reject`, { reason }).subscribe({
      next: () => {
        alert('تم رفض طلب الاجتماع');
        this.loadMeetings();
      },
      error: (err) => {
        console.error('فشل رفض الطلب', err);
        alert('❌ حدث خطأ');
      }
    });
  }

  cancelMeeting(id: string): void {
    if (!confirm('هل أنت متأكد من إلغاء هذا الاجتماع؟')) return;

    this.http.delete(`/api/app/attendee-meeting/${id}`).subscribe({
      next: () => {
        alert('تم إلغاء الاجتماع');
        this.loadMeetings();
      },
      error: (err) => {
        console.error('فشل إلغاء الاجتماع', err);
        alert('❌ حدث خطأ');
      }
    });
  }
}

