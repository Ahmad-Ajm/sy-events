import { Routes } from '@angular/router';
import { authGuard } from '@abp/ng.core';

// تعليق: التقويم متاح فقط للمستخدمين المسجلين
export const calendarRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./calendar.component').then(m => m.CalendarComponent),
    canActivate: [authGuard], // تعليق: حماية المسار - يتطلب تسجيل دخول
  },
];


