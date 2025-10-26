import type { EventFileDto } from './dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EventFileService {
  apiName = 'Default';
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/event-file/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventFileDto>({
      method: 'GET',
      url: `/api/app/event-file/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getEventFiles = (eventId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EventFileDto[]>({
      method: 'GET',
      url: `/api/app/event-file/event-files/${eventId}`,
    },
    { apiName: this.apiName,...config });
  

  updateDisplayOrder = (id: string, order: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/event-file/${id}/display-order`,
      params: { order },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
