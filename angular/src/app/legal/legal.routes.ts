// تعليق: مسارات الصفحات القانونية
import { Routes } from '@angular/router';

export const legalRoutes: Routes = [
  {
    path: 'privacy',
    loadComponent: () => import('./privacy-policy.component').then(m => m.PrivacyPolicyComponent),
  },
  {
    path: 'terms',
    loadComponent: () => import('./terms-conditions.component').then(m => m.TermsConditionsComponent),
  }
];

