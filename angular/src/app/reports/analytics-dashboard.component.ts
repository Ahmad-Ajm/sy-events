// تعليق: لوحة التحليلات والتقارير المتقدمة
import { Component, Input, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

interface EventAnalytics {
  eventId: string;
  eventTitle: string;
  totalRegistrations: number;
  confirmedCount: number;
  attendedCount: number;
  cancelledCount: number;
  noShowCount: number;
  attendanceRate: number;
  cancellationRate: number;
}

@Component({
  selector: 'app-analytics-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <!-- تعليق: لوحة التحليلات والتقارير -->
    <div class="analytics-dashboard">
      <div class="card">
        <div class="card-header">
          <h5 class="mb-0">
            <i class="fas fa-chart-line me-2"></i>
            تحليلات الفعالية
          </h5>
        </div>
        <div class="card-body">
          
          @if (analytics()) {
            <!-- الإحصائيات الرئيسية -->
            <div class="row mb-4">
              <div class="col-md-3">
                <div class="stat-card text-center p-3 bg-primary text-white rounded">
                  <h3 class="mb-1">{{ analytics()!.totalRegistrations }}</h3>
                  <small>إجمالي التسجيلات</small>
                </div>
              </div>
              <div class="col-md-3">
                <div class="stat-card text-center p-3 bg-success text-white rounded">
                  <h3 class="mb-1">{{ analytics()!.attendedCount }}</h3>
                  <small>الحضور</small>
                </div>
              </div>
              <div class="col-md-3">
                <div class="stat-card text-center p-3 bg-warning text-dark rounded">
                  <h3 class="mb-1">{{ analytics()!.confirmedCount }}</h3>
                  <small>المؤكدين</small>
                </div>
              </div>
              <div class="col-md-3">
                <div class="stat-card text-center p-3 bg-danger text-white rounded">
                  <h3 class="mb-1">{{ analytics()!.cancelledCount }}</h3>
                  <small>الملغيات</small>
                </div>
              </div>
            </div>

            <!-- النسب والمعدلات -->
            <div class="row mb-4">
              <div class="col-md-6">
                <div class="card">
                  <div class="card-body">
                    <h6 class="mb-3">معدل الحضور</h6>
                    <div class="progress" style="height: 30px;">
                      <div class="progress-bar bg-success" 
                           [style.width.%]="analytics()!.attendanceRate">
                        {{ analytics()!.attendanceRate | number:'1.1-1' }}%
                      </div>
                    </div>
                  </div>
                </div>
              </div>
              <div class="col-md-6">
                <div class="card">
                  <div class="card-body">
                    <h6 class="mb-3">معدل الإلغاء</h6>
                    <div class="progress" style="height: 30px;">
                      <div class="progress-bar bg-danger" 
                           [style.width.%]="analytics()!.cancellationRate">
                        {{ analytics()!.cancellationRate | number:'1.1-1' }}%
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- التفاصيل -->
            <div class="row">
              <div class="col-md-12">
                <div class="card">
                  <div class="card-body">
                    <h6 class="mb-3">تفاصيل الحالة</h6>
                    <table class="table table-striped">
                      <thead>
                        <tr>
                          <th>الحالة</th>
                          <th>العدد</th>
                          <th>النسبة</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td><span class="badge bg-success">حضر</span></td>
                          <td>{{ analytics()!.attendedCount }}</td>
                          <td>{{ analytics()!.attendanceRate | number:'1.1-1' }}%</td>
                        </tr>
                        <tr>
                          <td><span class="badge bg-warning">مؤكد</span></td>
                          <td>{{ analytics()!.confirmedCount }}</td>
                          <td>{{ (analytics()!.confirmedCount / analytics()!.totalRegistrations * 100) | number:'1.1-1' }}%</td>
                        </tr>
                        <tr>
                          <td><span class="badge bg-danger">ملغي</span></td>
                          <td>{{ analytics()!.cancelledCount }}</td>
                          <td>{{ analytics()!.cancellationRate | number:'1.1-1' }}%</td>
                        </tr>
                        <tr>
                          <td><span class="badge bg-secondary">لم يحضر</span></td>
                          <td>{{ analytics()!.noShowCount }}</td>
                          <td>{{ (analytics()!.noShowCount / analytics()!.totalRegistrations * 100) | number:'1.1-1' }}%</td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
            </div>

            <!-- أزرار التصدير -->
            <div class="mt-4 text-center">
              <button class="btn btn-success me-2" (click)="exportCSV()">
                <i class="fas fa-file-csv me-2"></i>
                تصدير CSV
              </button>
              <button class="btn btn-primary" (click)="exportPDF()">
                <i class="fas fa-file-pdf me-2"></i>
                تصدير PDF
              </button>
            </div>
          } @else if (loading()) {
            <div class="text-center py-5">
              <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">جاري التحميل...</span>
              </div>
            </div>
          } @else {
            <div class="alert alert-info">
              <i class="fas fa-info-circle me-2"></i>
              لا توجد بيانات تحليلية متاحة
            </div>
          }
        </div>
      </div>
    </div>
  `,
  styles: [`
    .stat-card {
      transition: transform 0.2s ease;
    }
    
    .stat-card:hover {
      transform: translateY(-5px);
    }
    
    .progress {
      font-weight: bold;
    }
  `]
})
export class AnalyticsDashboardComponent implements OnInit {
  @Input() eventId: string = '';
  
  analytics = signal<EventAnalytics | null>(null);
  loading = signal(false);

  constructor(private readonly http: HttpClient) {}

  ngOnInit(): void {
    if (this.eventId) {
      this.loadAnalytics();
    }
  }

  loadAnalytics(): void {
    this.loading.set(true);
    this.http.get<EventAnalytics>(`/api/app/report/event-analytics/${this.eventId}`).subscribe({
      next: (data) => {
        this.analytics.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('فشل تحميل التحليلات', err);
        this.loading.set(false);
      }
    });
  }

  exportCSV(): void {
    window.open(`/api/app/report/export-csv/${this.eventId}`, '_blank');
  }

  exportPDF(): void {
    alert('تصدير PDF قيد التطوير');
  }
}

