import { Routes } from '@angular/router';
import { HomeComponent } from './home/home/home.component';

export const appRoutes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: HomeComponent,
  },
  {
    path: 'events',
    loadChildren: () => import('./events/events.routes').then(m => m.eventsRoutes),
  },
  {
    path: 'admin/home-slider',
    loadChildren: () => import('./admin/home-slider/home-slider.routes').then(m => m.adminHomeSliderRoutes),
  },
  {
    path: 'admin/featured-boxes',
    loadChildren: () => import('./admin/featured-boxes/featured-boxes.routes').then(m => m.adminFeaturedBoxesRoutes),
  },
  {
    path: 'admin-approvals',
    loadChildren: () => import('./admin/approvals/approvals.routes').then(m => m.approvalsRoutes),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(m => m.createRoutes()),
  },
  {
    path: 'register-viewer',
    loadComponent: () => import('./account/register-viewer/register-viewer.component').then(m => m.RegisterViewerComponent),
  },
  {
    path: 'calendar',
    loadChildren: () => import('./calendar/calendar.routes').then(m => m.calendarRoutes),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(m => m.createRoutes()),
  },
  {
    path: 'tenant-management',
    loadChildren: () =>
      import('@abp/ng.tenant-management').then(m => m.createRoutes()),
  },
  {
    path: 'setting-management',
    loadChildren: () =>
      import('@abp/ng.setting-management').then(m => m.createRoutes()),
  },
  {
    path: 'profile',
    loadChildren: () => import('./profile/profile.routes').then(m => m.profileRoutes),
  },
  {
    path: 'legal',
    loadChildren: () => import('./legal/legal.routes').then(m => m.legalRoutes),
  },
  {
    path: 'meetings',
    loadChildren: () => import('./meetings/meetings.routes').then(m => m.meetingsRoutes),
  },
  {
    path: '**',
    redirectTo: '',
  },
];
