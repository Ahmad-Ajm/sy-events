import type { CreateUpdateEventDto, EventDto, EventStatisticsDto, GetEventsInput } from './events/dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EventService {
  apiName = 'Default';
  

  approve = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventDto>({
      method: 'POST',
      url: `/api/app/event/${id}/approve`,
    },
    { apiName: this.apiName,...config });
  

  bulkApprove = (ids: string[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/event/bulk-approve',
      body: ids,
    },
    { apiName: this.apiName,...config });
  

  create = (input: CreateUpdateEventDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventDto>({
      method: 'POST',
      url: '/api/app/event',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/event/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventDto>({
      method: 'GET',
      url: `/api/app/event/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetEventsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<EventDto>>({
      method: 'GET',
      url: '/api/app/event',
      params: { filter: input.filter, categoryId: input.categoryId, cityId: input.cityId, status: input.status, startDate: input.startDate, endDate: input.endDate, organizerId: input.organizerId, organizerFilter: (input as any).organizerFilter, isUpcoming: input.isUpcoming, minAttendees: input.minAttendees, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getPending = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventDto[]>({
      method: 'GET',
      url: '/api/app/event/pending',
    },
    { apiName: this.apiName,...config });
  

  getPopularEvents = (count: number = 10, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventDto[]>({
      method: 'GET',
      url: '/api/app/event/popular-events',
      params: { count },
    },
    { apiName: this.apiName,...config });
  

  getStatistics = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventStatisticsDto>({
      method: 'GET',
      url: `/api/app/event/${id}/statistics`,
    },
    { apiName: this.apiName,...config });
  

  getUpcomingEvents = (count: number = 10, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventDto[]>({
      method: 'GET',
      url: '/api/app/event/upcoming-events',
      params: { count },
    },
    { apiName: this.apiName,...config });
  

  hide = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventDto>({
      method: 'POST',
      url: `/api/app/event/${id}/hide`,
    },
    { apiName: this.apiName,...config });
  

  publish = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventDto>({
      method: 'POST',
      url: `/api/app/event/${id}/publish`,
    },
    { apiName: this.apiName,...config });
  

  reject = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventDto>({
      method: 'POST',
      url: `/api/app/event/${id}/reject`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateEventDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventDto>({
      method: 'PUT',
      url: `/api/app/event/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
