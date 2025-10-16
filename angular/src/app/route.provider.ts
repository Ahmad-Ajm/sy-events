import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application, // استخدام layout عادي مع قائمة التنقل
      },
      {
        path: '/events',
        name: '::Menu:Events',
        iconClass: 'fas fa-calendar-alt',
        order: 2,
        layout: eLayoutType.application,
      },
      {
        path: '/bookings',
        name: '::Menu:Bookings',
        iconClass: 'fas fa-ticket-alt',
        order: 3,
        layout: eLayoutType.application,
      },
      {
        path: '/admin/categories',
        name: '::Menu:Categories',
        parentName: '::Menu:Administration',
        iconClass: 'fas fa-tags',
        order: 101,
        layout: eLayoutType.application,
        requiredPolicy: 'EventManagement.Categories',
      },
      {
        path: '/admin/cities',
        name: '::Menu:Cities',
        parentName: '::Menu:Administration',
        iconClass: 'fas fa-map-marker-alt',
        order: 102,
        layout: eLayoutType.application,
        requiredPolicy: 'EventManagement.Cities',
      },
      {
        path: '/admin/users',
        name: '::Menu:Users',
        parentName: '::Menu:Administration',
        iconClass: 'fas fa-users',
        order: 103,
        layout: eLayoutType.application,
        requiredPolicy: 'EventManagement.Admin.Users',
      },
      {
        path: '/admin/home-slider',
        name: '::Menu:HomeSlider',
        parentName: '::Menu:Administration',
        iconClass: 'fas fa-images',
        order: 104,
        layout: eLayoutType.application,
        requiredPolicy: 'EventManagement.Admin.Settings',
      },
      {
        path: '/admin-approvals',
        name: '::Menu:Approvals',
        parentName: '::Menu:Administration',
        iconClass: 'fas fa-check-double',
        order: 105,
        layout: eLayoutType.application,
        requiredPolicy: 'EventManagement.Events.Approve',
      },
      {
        path: '/calendar',
        name: '::Menu:MyCalendar',
        iconClass: 'fas fa-calendar',
        order: 50,
        layout: eLayoutType.application,
        requiredPolicy: 'AbpIdentity.Users', // تعليق: يتطلب تسجيل دخول (أي مستخدم مسجل)
      },
    ]);
  };
}
