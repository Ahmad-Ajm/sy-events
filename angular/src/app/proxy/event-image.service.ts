import type { IFormFile } from './microsoft/asp-net-core/http/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EventImageService {
  apiName = 'Default';
  

  delete = (eventId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/app/event-image',
      params: { eventId },
    },
    { apiName: this.apiName,...config });
  

  get = (eventId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, number[]>({
      method: 'GET',
      url: '/api/app/event-image',
      params: { eventId },
    },
    { apiName: this.apiName,...config });
  

  upload = (eventId: string, file: IFormFile, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/event-image/upload/${eventId}`,
      body: file,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
