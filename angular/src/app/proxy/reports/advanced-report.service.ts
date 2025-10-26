import type { AttendeeDemographicsDto, EngagementMetricsDto, EventAnalyticsDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AdvancedReportService {
  apiName = 'Default';
  

  exportToCsv = (eventId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, number[]>({
      method: 'POST',
      url: `/api/app/advanced-report/export-to-csv/${eventId}`,
    },
    { apiName: this.apiName,...config });
  

  getAttendeeDemographics = (eventId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AttendeeDemographicsDto>({
      method: 'GET',
      url: `/api/app/advanced-report/attendee-demographics/${eventId}`,
    },
    { apiName: this.apiName,...config });
  

  getEngagementMetrics = (eventId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EngagementMetricsDto>({
      method: 'GET',
      url: `/api/app/advanced-report/engagement-metrics/${eventId}`,
    },
    { apiName: this.apiName,...config });
  

  getEventAnalytics = (eventId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventAnalyticsDto>({
      method: 'GET',
      url: `/api/app/advanced-report/event-analytics/${eventId}`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
