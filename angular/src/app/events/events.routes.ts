import { Routes } from '@angular/router';

export const eventsRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./event-list/event-list.component').then(m => m.EventListComponent),
  },
  {
    path: 'create-wizard',
    loadComponent: () => import('./wizard/event-wizard.component').then(m => m.EventWizardComponent),
  },
  {
    path: ':id',
    loadComponent: () => import('./event-detail/event-detail.component').then(m => m.EventDetailComponent),
  },
];

