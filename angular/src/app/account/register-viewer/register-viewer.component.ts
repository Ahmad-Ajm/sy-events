import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-register-viewer',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="container py-4">
      <h2 class="mb-3">تسجيل كمتابع فقط</h2>
      <form [formGroup]="form" (ngSubmit)="submit()" class="card p-3">
        <div class="mb-3">
          <label class="form-label">الاسم</label>
          <input class="form-control" formControlName="name" />
        </div>
        <div class="mb-3">
          <label class="form-label">البريد الإلكتروني</label>
          <input type="email" class="form-control" formControlName="email" />
        </div>
        <div class="mb-3">
          <label class="form-label">كلمة المرور</label>
          <input type="password" class="form-control" formControlName="password" />
        </div>
        <button class="btn btn-primary" type="submit" [disabled]="form.invalid">تسجيل</button>
      </form>
    </div>
  `,
})
export class RegisterViewerComponent {
  form = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  submit(): void {
    if (this.form.invalid) return;
    this.http.post<string>(`${environment.apis.default.url}/api/app/accounts/register-viewer`, this.form.value)
      .subscribe(() => alert('تم التسجيل كمتابع بنجاح'));
  }
}


