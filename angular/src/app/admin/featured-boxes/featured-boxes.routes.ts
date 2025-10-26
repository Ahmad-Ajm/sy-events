import { Routes } from '@angular/router';

export const adminFeaturedBoxesRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./featured-boxes.component').then(m => m.AdminFeaturedBoxesComponent),
    data: { title: 'إدارة المربعات المميزة' },
  },
];


