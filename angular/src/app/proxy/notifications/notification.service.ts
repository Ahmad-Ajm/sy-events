import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  apiName = 'Default';
  

  scheduleReminder = (bookingId: string, hoursBeforeEvent: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/notification/schedule-reminder/${bookingId}`,
      params: { hoursBeforeEvent },
    },
    { apiName: this.apiName,...config });
  

  sendEmailReminder = (bookingId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/notification/send-email-reminder/${bookingId}`,
    },
    { apiName: this.apiName,...config });
  

  sendSmsReminder = (bookingId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/notification/send-sms-reminder/${bookingId}`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
