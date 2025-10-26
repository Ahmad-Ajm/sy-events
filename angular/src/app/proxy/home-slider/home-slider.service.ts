import type { CreateUpdateHomeSliderItemDto, HomeSliderItemDto } from './dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { AppSettingsDto, UpdateAppSettingsDto } from '../settings/dtos/models';

@Injectable({
  providedIn: 'root',
})
export class HomeSliderService {
  apiName = 'Default';
  

  create = (input: CreateUpdateHomeSliderItemDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HomeSliderItemDto>({
      method: 'POST',
      url: '/api/app/home-slider',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/home-slider/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HomeSliderItemDto>({
      method: 'GET',
      url: `/api/app/home-slider/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getActiveSliderItems = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, HomeSliderItemDto[]>({
      method: 'GET',
      url: '/api/app/home-slider/active-slider-items',
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<HomeSliderItemDto>>({
      method: 'GET',
      url: '/api/app/home-slider',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getSettings = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, AppSettingsDto>({
      method: 'GET',
      url: '/api/app/home-slider/settings',
    },
    { apiName: this.apiName,...config });
  

  reorder = (orderedIds: string[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/home-slider/reorder',
      body: orderedIds,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateHomeSliderItemDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HomeSliderItemDto>({
      method: 'PUT',
      url: `/api/app/home-slider/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  updateSettings = (input: UpdateAppSettingsDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: '/api/app/home-slider/settings',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
