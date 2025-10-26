import type { CreateEventDiscussionDto, EventDiscussionDto } from './dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EventDiscussionService {
  apiName = 'Default';
  

  create = (input: CreateEventDiscussionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventDiscussionDto>({
      method: 'POST',
      url: '/api/app/event-discussion',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/event-discussion/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getEventDiscussions = (eventId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventDiscussionDto[]>({
      method: 'GET',
      url: `/api/app/event-discussion/event-discussions/${eventId}`,
    },
    { apiName: this.apiName,...config });
  

  hide = (id: string, reason: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/event-discussion/${id}/hide`,
      params: { reason },
    },
    { apiName: this.apiName,...config });
  

  show = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/event-discussion/${id}/show`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
