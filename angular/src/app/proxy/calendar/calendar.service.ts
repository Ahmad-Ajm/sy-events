import type { CalendarEventItemDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CalendarService {
  apiName = 'Default';
  

  getEventsByRange = (start: string, end: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CalendarEventItemDto[]>({
      method: 'GET',
      url: '/api/app/calendar/events-by-range',
      params: { start, end },
    },
    { apiName: this.apiName,...config });
  

  getMyEvents = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, CalendarEventItemDto[]>({
      method: 'GET',
      url: '/api/app/calendar/my-events',
    },
    { apiName: this.apiName,...config });
  

  getUserEvents = (userId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CalendarEventItemDto[]>({
      method: 'GET',
      url: `/api/app/calendar/user-events/${userId}`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
