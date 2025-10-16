// تعليق: مكون ملف تعريف المستخدم - عرض وتعديل الملف الشخصي
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

interface UserProfile {
  userId: string;
  userName: string;
  email: string;
  bio: string;
  profileImageUrl: string;
  jobTitle: string;
  company: string;
  website: string;
  linkedInUrl?: string;
  twitterHandle?: string;
  facebookUrl?: string;
  interests: string[];
  skills: string[];
  eventsAttendedCount: number;
  eventsOrganizedCount: number;
}

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <!-- تعليق: صفحة الملف الشخصي -->
    <div class="container py-4">
      <!-- Cover Image -->
      <div class="profile-cover mb-4">
        <div class="profile-cover-overlay">
          <button class="btn btn-sm btn-light">
            <i class="fas fa-camera me-2"></i>
            تغيير الغلاف
          </button>
        </div>
      </div>

      <div class="row">
        <!-- الصورة الشخصية -->
        <div class="col-md-4">
          <div class="card text-center">
            <div class="card-body">
              <div class="profile-image-container mb-3">
                <img [src]="profile?.profileImageUrl || '/assets/default-avatar.png'" 
                     class="profile-image"
                     alt="Profile">
                <button class="btn btn-sm btn-primary profile-image-btn">
                  <i class="fas fa-camera"></i>
                </button>
              </div>
              
              <h4 class="mb-1">{{ profile?.userName }}</h4>
              <p class="text-muted mb-3">{{ profile?.jobTitle || 'مستخدم' }}</p>
              
              <!-- الإحصائيات -->
              <div class="row text-center mb-3">
                <div class="col-6">
                  <div class="stat-box">
                    <h3 class="mb-0">{{ profile?.eventsAttendedCount || 0 }}</h3>
                    <small class="text-muted">فعالية حضرتها</small>
                  </div>
                </div>
                <div class="col-6">
                  <div class="stat-box">
                    <h3 class="mb-0">{{ profile?.eventsOrganizedCount || 0 }}</h3>
                    <small class="text-muted">فعالية نظمتها</small>
                  </div>
                </div>
              </div>

              @if (!editMode) {
                <button class="btn btn-primary w-100" (click)="toggleEditMode()">
                  <i class="fas fa-edit me-2"></i>
                  تعديل الملف الشخصي
                </button>
              }
            </div>
          </div>

          <!-- وسائل التواصل -->
          <div class="card mt-3">
            <div class="card-body">
              <h6 class="mb-3">
                <i class="fas fa-share-alt me-2"></i>
                وسائل التواصل
              </h6>
              @if (profile?.linkedInUrl) {
                <a [href]="profile.linkedInUrl" target="_blank" class="btn btn-sm btn-outline-primary w-100 mb-2">
                  <i class="fab fa-linkedin me-2"></i>
                  LinkedIn
                </a>
              }
              @if (profile?.twitterHandle) {
                <a [href]="'https://twitter.com/' + profile.twitterHandle" target="_blank" class="btn btn-sm btn-outline-info w-100 mb-2">
                  <i class="fab fa-twitter me-2"></i>
                  Twitter
                </a>
              }
              @if (profile?.facebookUrl) {
                <a [href]="profile.facebookUrl" target="_blank" class="btn btn-sm btn-outline-primary w-100">
                  <i class="fab fa-facebook me-2"></i>
                  Facebook
                </a>
              }
            </div>
          </div>
        </div>

        <!-- معلومات الملف الشخصي -->
        <div class="col-md-8">
          <div class="card">
            <div class="card-body">
              @if (editMode) {
                <!-- وضع التعديل -->
                <form [formGroup]="profileForm" (ngSubmit)="saveProfile()">
                  <h5 class="mb-4">تعديل الملف الشخصي</h5>
                  
                  <div class="mb-3">
                    <label class="form-label">النبذة التعريفية</label>
                    <textarea class="form-control" formControlName="bio" rows="4" 
                              placeholder="اكتب نبذة عنك..." maxlength="500"></textarea>
                    <small class="text-muted">{{ profileForm.value.bio?.length || 0 }}/500</small>
                  </div>

                  <div class="row">
                    <div class="col-md-6 mb-3">
                      <label class="form-label">المسمى الوظيفي</label>
                      <input type="text" class="form-control" formControlName="jobTitle" 
                             placeholder="مثال: مطور برمجيات">
                    </div>
                    <div class="col-md-6 mb-3">
                      <label class="form-label">الشركة</label>
                      <input type="text" class="form-control" formControlName="company" 
                             placeholder="مثال: شركة التقنية">
                    </div>
                  </div>

                  <div class="mb-3">
                    <label class="form-label">الموقع الإلكتروني</label>
                    <input type="url" class="form-control" formControlName="website" 
                           placeholder="https://example.com">
                  </div>

                  <hr class="my-4">

                  <h6 class="mb-3">إعدادات الخصوصية</h6>
                  <div class="form-check mb-2">
                    <input class="form-check-input" type="checkbox" formControlName="isPublic" id="isPublic">
                    <label class="form-check-label" for="isPublic">
                      اجعل ملفي الشخصي عاماً
                    </label>
                  </div>
                  <div class="form-check mb-2">
                    <input class="form-check-input" type="checkbox" formControlName="showEmail" id="showEmail">
                    <label class="form-check-label" for="showEmail">
                      عرض البريد الإلكتروني
                    </label>
                  </div>

                  <div class="mt-4 d-flex gap-2">
                    <button type="submit" class="btn btn-primary" [disabled]="saving">
                      <i class="fas fa-save me-2"></i>
                      @if (saving) { جاري الحفظ... } @else { حفظ التغييرات }
                    </button>
                    <button type="button" class="btn btn-outline-secondary" (click)="cancelEdit()">
                      <i class="fas fa-times me-2"></i>
                      إلغاء
                    </button>
                  </div>
                </form>
              } @else {
                <!-- وضع العرض -->
                <h5 class="mb-4">عن {{ profile?.userName }}</h5>
                
                @if (profile?.bio) {
                  <p class="text-muted">{{ profile.bio }}</p>
                } @else {
                  <p class="text-muted fst-italic">لم يتم إضافة نبذة تعريفية بعد.</p>
                }

                <hr class="my-4">

                <div class="row">
                  @if (profile?.jobTitle) {
                    <div class="col-md-6 mb-3">
                      <h6 class="text-muted mb-1">المسمى الوظيفي</h6>
                      <p class="mb-0">{{ profile.jobTitle }}</p>
                    </div>
                  }
                  @if (profile?.company) {
                    <div class="col-md-6 mb-3">
                      <h6 class="text-muted mb-1">الشركة</h6>
                      <p class="mb-0">{{ profile.company }}</p>
                    </div>
                  }
                  @if (profile?.website) {
                    <div class="col-12 mb-3">
                      <h6 class="text-muted mb-1">الموقع الإلكتروني</h6>
                      <a [href]="profile.website" target="_blank" class="text-primary">
                        {{ profile.website }}
                      </a>
                    </div>
                  }
                </div>

                @if (profile?.interests && profile.interests.length > 0) {
                  <hr class="my-4">
                  <h6 class="mb-3">الاهتمامات</h6>
                  <div class="d-flex flex-wrap gap-2">
                    @for (interest of profile.interests; track interest) {
                      <span class="badge bg-primary">{{ interest }}</span>
                    }
                  </div>
                }
              }
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .profile-cover {
      height: 200px;
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      border-radius: 8px;
      position: relative;
    }
    
    .profile-cover-overlay {
      position: absolute;
      top: 1rem;
      right: 1rem;
    }
    
    .profile-image-container {
      position: relative;
      display: inline-block;
    }
    
    .profile-image {
      width: 150px;
      height: 150px;
      border-radius: 50%;
      border: 5px solid white;
      margin-top: -75px;
      object-fit: cover;
    }
    
    .profile-image-btn {
      position: absolute;
      bottom: 10px;
      right: 10px;
      border-radius: 50%;
      width: 35px;
      height: 35px;
      padding: 0;
    }
    
    .stat-box {
      padding: 1rem;
      border-radius: 8px;
      background-color: #f8f9fa;
    }
  `]
})
export class ProfileComponent implements OnInit {
  profile: UserProfile | null = null;
  editMode = false;
  saving = false;

  profileForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient
  ) {
    this.profileForm = this.fb.group({
      bio: ['', [Validators.maxLength(500)]],
      jobTitle: [''],
      company: [''],
      website: [''],
      linkedInUrl: [''],
      twitterHandle: [''],
      facebookUrl: [''],
      isPublic: [true],
      showEmail: [false],
      interests: [[]]
    });
  }

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.http.get<UserProfile>('/api/app/user-profile/my-profile').subscribe({
      next: (profile) => {
        this.profile = profile;
        this.profileForm.patchValue(profile);
      },
      error: (err) => console.error('فشل تحميل الملف الشخصي', err)
    });
  }

  toggleEditMode(): void {
    this.editMode = !this.editMode;
  }

  cancelEdit(): void {
    this.editMode = false;
    if (this.profile) {
      this.profileForm.patchValue(this.profile);
    }
  }

  saveProfile(): void {
    if (this.profileForm.invalid) return;

    this.saving = true;
    this.http.put('/api/app/user-profile/my-profile', this.profileForm.value).subscribe({
      next: () => {
        this.saving = false;
        this.editMode = false;
        this.loadProfile();
        alert('✅ تم حفظ التغييرات بنجاح!');
      },
      error: (err) => {
        this.saving = false;
        console.error('فشل حفظ الملف الشخصي', err);
        alert('❌ حدث خطأ أثناء الحفظ');
      }
    });
  }
}

