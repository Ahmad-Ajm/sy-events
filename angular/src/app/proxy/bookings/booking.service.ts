// تعليق: خدمة الحجوزات - متابعة الفعاليات
import type { BookingDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BookingService {
  apiName = 'Default';

  // تعليق: متابعة فعالية
  followEvent = (eventId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BookingDto>({
      method: 'POST',
      url: '/api/app/booking/follow-event',
      params: { eventId },
    },
    { apiName: this.apiName, ...config });

  // تعليق: إلغاء متابعة فعالية
  unfollowEvent = (eventId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/booking/unfollow-event',
      params: { eventId },
    },
    { apiName: this.apiName, ...config });

  // تعليق: التحقق من متابعة فعالية
  isFollowingEvent = (eventId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'GET',
      url: '/api/app/booking/is-following-event',
      params: { eventId },
    },
    { apiName: this.apiName, ...config });

  // تعليق: تأكيد الحضور
  confirmAttendance = (bookingId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/booking/confirm-attendance',
      params: { bookingId },
    },
    { apiName: this.apiName, ...config });

  constructor(private restService: RestService) {}
}

