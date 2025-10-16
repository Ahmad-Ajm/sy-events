import { Component, Input, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { environment } from '../../../environments/environment';

interface UploadedFile {
  id?: string;
  file: File;
  preview?: string;
  type: 'image' | 'pdf' | 'text';
  progress: number;
  uploaded: boolean;
  error?: string;
}

@Component({
  selector: 'app-file-upload',
  standalone: true,
  imports: [CommonModule],
  template: `
    <!-- تعليق: مكون رفع ملفات متعددة -->
    <div class="file-upload-container">
      <div class="card">
        <div class="card-header">
          <h5 class="mb-0">
            <i class="fas fa-cloud-upload-alt me-2"></i>
            رفع الملفات
          </h5>
        </div>
        <div class="card-body">
          <!-- تعليق: قسم اختيار الملفات -->
          <div class="upload-section mb-4">
            <div class="row g-3">
              <!-- الصور -->
              <div class="col-md-4">
                <label class="upload-box">
                  <input type="file" multiple accept="image/jpeg,image/jpg,image/png,image/webp" (change)="onImageSelect($event)" [disabled]="images().length >= 3">
                  <div class="upload-box-content">
                    <i class="fas fa-images fa-3x mb-2 text-primary"></i>
                    <p class="mb-1"><strong>صور الفعالية</strong></p>
                    <p class="text-muted small">JPG, PNG, WebP (حتى 3 صور)</p>
                    <p class="text-muted small">الحد الأقصى: 5MB لكل صورة</p>
                    <span class="badge bg-info">{{ images().length }}/3</span>
                  </div>
                </label>
              </div>
              <!-- PDF -->
              <div class="col-md-4">
                <label class="upload-box">
                  <input type="file" accept="application/pdf" (change)="onPdfSelect($event)" [disabled]="pdfs().length >= 1">
                  <div class="upload-box-content">
                    <i class="fas fa-file-pdf fa-3x mb-2 text-danger"></i>
                    <p class="mb-1"><strong>ملف PDF</strong></p>
                    <p class="text-muted small">مستند PDF (ملف واحد)</p>
                    <p class="text-muted small">الحد الأقصى: 10MB</p>
                    <span class="badge bg-info">{{ pdfs().length }}/1</span>
                  </div>
                </label>
              </div>
              <!-- ملف نصي -->
              <div class="col-md-4">
                <label class="upload-box">
                  <input type="file" accept="text/plain,text/markdown" (change)="onTextSelect($event)" [disabled]="texts().length >= 1">
                  <div class="upload-box-content">
                    <i class="fas fa-file-alt fa-3x mb-2 text-success"></i>
                    <p class="mb-1"><strong>ملف نصي</strong></p>
                    <p class="text-muted small">TXT, MD (ملف واحد)</p>
                    <p class="text-muted small">الحد الأقصى: 2MB</p>
                    <span class="badge bg-info">{{ texts().length }}/1</span>
                  </div>
                </label>
              </div>
            </div>
          </div>

          <!-- تعليق: قائمة الملفات المحددة -->
          <ng-container *ngIf="allFiles().length > 0">
            <div class="selected-files">
              <h6 class="mb-3">الملفات المحددة ({{ allFiles().length }})</h6>
              <div class="file-item card mb-2" *ngFor="let file of allFiles(); trackBy: trackByFileName">
                <div class="card-body p-3">
                  <div class="row align-items-center">
                    <!-- Preview / Icon -->
                    <div class="col-auto">
                      <ng-container *ngIf="file.preview; else noPreview">
                        <img [src]="file.preview" class="file-preview">
                      </ng-container>
                      <ng-template #noPreview>
                        <i class="fas" [ngClass]="{
                          'fa-file-pdf fa-2x text-danger': file.type === 'pdf',
                          'fa-file-alt fa-2x text-success': file.type !== 'pdf'
                        }"></i>
                      </ng-template>
                    </div>
                    <!-- معلومات الملف -->
                    <div class="col">
                      <h6 class="mb-1">{{ file.file.name }}</h6>
                      <small class="text-muted">
                        {{ formatFileSize(file.file.size) }}
                        <ng-container *ngIf="file.type === 'image'"> - صورة</ng-container>
                        <ng-container *ngIf="file.type === 'pdf'"> - PDF</ng-container>
                        <ng-container *ngIf="file.type === 'text'"> - ملف نصي</ng-container>
                      </small>
                      <!-- شريط التقدم -->
                      <div class="progress mt-2" style="height: 5px;" *ngIf="file.progress > 0 && file.progress < 100">
                        <div class="progress-bar" [style.width.%]="file.progress"></div>
                      </div>
                      <!-- حالة الرفع -->
                      <span class="badge bg-success mt-1" *ngIf="file.uploaded">
                        <i class="fas fa-check me-1"></i>
                        تم الرفع
                      </span>
                      <span class="badge bg-danger mt-1" *ngIf="file.error">
                        <i class="fas fa-times me-1"></i>
                        {{ file.error }}
                      </span>
                    </div>
                    <!-- زر الحذف -->
                    <div class="col-auto">
                      <button class="btn btn-sm btn-danger" (click)="removeFile(file)" [disabled]="file.progress > 0 && file.progress < 100">
                        <i class="fas fa-trash"></i>
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <!-- تعليق: زر الرفع -->
            <div class="mt-3 d-flex gap-2">
              <button class="btn btn-primary" (click)="uploadAll()" [disabled]="isBulkUploadDisabled()">
                <i class="fas fa-cloud-upload-alt me-2"></i>
                <ng-container *ngIf="uploading(); else notUploading">جاري الرفع...</ng-container>
                <ng-template #notUploading>رفع جميع الملفات ({{ pendingUploadCount() }})</ng-template>
              </button>
              <button class="btn btn-outline-secondary" (click)="clearAll()" [disabled]="uploading()">
                <i class="fas fa-times me-2"></i>
                مسح الكل
              </button>
            </div>
          </ng-container>

          <!-- تعليق: رسالة فارغة -->
          <ng-container *ngIf="allFiles().length === 0">
            <div class="text-center text-muted py-4">
              <i class="fas fa-cloud-upload-alt fa-4x mb-3 opacity-25"></i>
              <p>لم يتم تحديد أي ملفات بعد</p>
              <p class="small">اختر الملفات من الأعلى</p>
            </div>
          </ng-container>
        </div>
      </div>
    </div>
  `,
  styles: [`
    /* تعليق: تنسيقات مكون رفع الملفات */
    
    .upload-box {
      display: block;
      border: 2px dashed #dee2e6;
      border-radius: 8px;
      padding: 2rem 1rem;
      cursor: pointer;
      transition: all 0.3s ease;
      background-color: #f8f9fa;
      text-align: center;
      height: 100%;
    }
    
    .upload-box:hover {
      border-color: #007bff;
      background-color: #e7f3ff;
    }
    
    .upload-box input[type="file"] {
      display: none;
    }
    
    .upload-box[disabled] {
      opacity: 0.5;
      cursor: not-allowed;
    }
    
    .upload-box-content {
      pointer-events: none;
    }
    
    .file-preview {
      width: 60px;
      height: 60px;
      object-fit: cover;
      border-radius: 4px;
      border: 1px solid #dee2e6;
    }
    
    .file-item {
      transition: all 0.2s ease;
    }
    
    .file-item:hover {
      box-shadow: 0 4px 12px rgba(0,0,0,0.1);
    }
  `]
})
export class FileUploadComponent implements OnInit {
  @Input() eventId: string = '';
  
  // تعليق: قوائم الملفات المحددة
  images = signal<UploadedFile[]>([]);
  pdfs = signal<UploadedFile[]>([]);
  texts = signal<UploadedFile[]>([]);
  allFiles = signal<UploadedFile[]>([]);
  uploading = signal(false);

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    // تعليق: تحميل الملفات الموجودة (إن وجدت)
    if (this.eventId) {
      this.loadExistingFiles();
    }
  }

  // تعليق: معالجة اختيار الصور
  onImageSelect(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files) return;

    const newImages: UploadedFile[] = [];
    for (let i = 0; i < Math.min(input.files.length, 3 - this.images().length); i++) {
      const file = input.files[i];
      const uploadFile: UploadedFile = {
        file,
        type: 'image',
        progress: 0,
        uploaded: false
      };
      
      // معاينة الصورة
      const reader = new FileReader();
      reader.onload = (e) => {
        uploadFile.preview = e.target?.result as string;
        this.updateSignals();
      };
      reader.readAsDataURL(file);
      
      newImages.push(uploadFile);
    }
    
    this.images.set([...this.images(), ...newImages]);
    this.updateAllFiles();
    input.value = '';
  }

  // تعليق: معالجة اختيار PDF
  onPdfSelect(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) return;

    const file = input.files[0];
    this.pdfs.set([{
      file,
      type: 'pdf',
      progress: 0,
      uploaded: false
    }]);
    
    this.updateAllFiles();
    input.value = '';
  }

  // تعليق: معالجة اختيار ملف نصي
  onTextSelect(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) return;

    const file = input.files[0];
    this.texts.set([{
      file,
      type: 'text',
      progress: 0,
      uploaded: false
    }]);
    
    this.updateAllFiles();
    input.value = '';
  }

  // تعليق: حذف ملف من القائمة
  removeFile(fileToRemove: UploadedFile): void {
    if (fileToRemove.type === 'image') {
      this.images.set(this.images().filter(f => f !== fileToRemove));
    } else if (fileToRemove.type === 'pdf') {
      this.pdfs.set([]);
    } else {
      this.texts.set([]);
    }
    this.updateAllFiles();
  }

  // تعليق: مسح جميع الملفات
  clearAll(): void {
    this.images.set([]);
    this.pdfs.set([]);
    this.texts.set([]);
    this.updateAllFiles();
  }

  // تعليق: رفع جميع الملفات
  async uploadAll(): Promise<void> {
    if (!this.eventId) {
      alert('Event ID غير محدد');
      return;
    }

    this.uploading.set(true);
    const formData = new FormData();

    // إضافة جميع الملفات
    this.allFiles().forEach(f => {
      if (!f.uploaded) {
        formData.append('files', f.file);
      }
    });

    try {
      // رفع الملفات
      this.http.post<any>(
        `${environment.apis.default.url}/api/app/event/${this.eventId}/files/upload-multiple`,
        formData,
        {
          reportProgress: true,
          observe: 'events'
        }
      ).subscribe({
        next: (event) => {
          if (event.type === HttpEventType.UploadProgress && event.total) {
            const progress = Math.round(100 * event.loaded / event.total);
            // تحديث progress لجميع الملفات
            this.allFiles().forEach(f => {
              if (!f.uploaded) f.progress = progress;
            });
            this.updateSignals();
          } else if (event.type === HttpEventType.Response) {
            // نجح الرفع
            this.allFiles().forEach(f => {
              f.uploaded = true;
              f.progress = 100;
            });
            this.updateSignals();
            this.uploading.set(false);
            alert('✅ تم رفع الملفات بنجاح!');
          }
        },
        error: (err) => {
          this.uploading.set(false);
          alert('❌ حدث خطأ أثناء رفع الملفات');
          console.error(err);
        }
      });
    } catch (error) {
      this.uploading.set(false);
      console.error('Upload error:', error);
    }
  }

  // تعليق: تحميل الملفات الموجودة
  private loadExistingFiles(): void {
    // TODO: استدعاء GET /api/app/event/{id}/files
  }

  // تعليق: تحديث قائمة allFiles
  private updateAllFiles(): void {
    const all = [
      ...this.images(),
      ...this.pdfs(),
      ...this.texts()
    ];
    this.allFiles.set(all);
  }

  // تعليق: تحديث الـ signals
  private updateSignals(): void {
    this.images.set([...this.images()]);
    this.allFiles.set([...this.allFiles()]);
  }

  // تعليق: trackBy لتحسين الأداء
  trackByFileName = (_: number, item: UploadedFile) => item.file?.name;

  // تعليق: حساب عدد الملفات غير المرفوعة
  pendingUploadCount(): number {
    return this.allFiles().filter(f => !f.uploaded).length;
  }

  // تعليق: تعطيل زر الرفع الجماعي عند عدم وجود ملفات أو أثناء الرفع
  isBulkUploadDisabled(): boolean {
    return this.uploading() || this.allFiles().every(f => f.uploaded);
  }

  // تعليق: تنسيق حجم الملف
  formatFileSize(bytes: number): string {
    if (bytes < 1024) return bytes + ' B';
    if (bytes < 1024 * 1024) return (bytes / 1024).toFixed(1) + ' KB';
    return (bytes / (1024 * 1024)).toFixed(1) + ' MB';
  }
}

