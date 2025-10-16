import { Routes } from '@angular/router';

export const adminHomeSliderRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./slider-management/slider-management.component').then(m => m.SliderManagementComponent),
  },
];


