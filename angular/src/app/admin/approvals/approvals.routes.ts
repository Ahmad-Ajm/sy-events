import { Routes } from '@angular/router';

export const approvalsRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./approvals.component').then(m => m.ApprovalsComponent),
    data: { title: 'موافقات الفعاليات' },
  },
];


