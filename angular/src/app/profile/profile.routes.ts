// تعليق: مسارات ملفات التعريف
import { Routes } from '@angular/router';

export const profileRoutes: Routes = [
  {
    path: 'me',
    loadComponent: () => import('./profile.component').then(m => m.ProfileComponent),
  },
  {
    path: ':userId',
    loadComponent: () => import('./profile.component').then(m => m.ProfileComponent),
  }
];

