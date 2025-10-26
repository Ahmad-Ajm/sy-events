import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'login-social-buttons',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="d-flex align-items-center gap-2 flex-wrap">
      <button class="btn btn-outline-danger" (click)="go('Google')">
        <i class="fab fa-google me-2"></i> تسجيل عبر Google
      </button>
      <button class="btn btn-outline-primary" (click)="go('Facebook')">
        <i class="fab fa-facebook me-2"></i> تسجيل عبر Facebook
      </button>
    </div>
  `
})
export class LoginSocialButtonsComponent {
  @Input() issuer = '';
  @Input() returnUrl = '/';

  go(provider: 'Google'|'Facebook') {
    const url = `${this.issuer}Account/ExternalLogin?provider=${encodeURIComponent(provider)}&returnUrl=${encodeURIComponent(this.returnUrl)}`;
    window.location.href = url;
  }
}



