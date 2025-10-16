import { Injectable } from '@angular/core';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Observable } from 'rxjs';
import type { HomeSliderItemDto, CreateUpdateHomeSliderItemDto, AppSettingsDto, UpdateAppSettingsDto } from './models';

@Injectable({
  providedIn: 'root',
})
export class HomeSliderService {
  apiName = 'Default';

  constructor(private restService: RestService) {}

  create(input: CreateUpdateHomeSliderItemDto): Observable<HomeSliderItemDto> {
    return this.restService.request<any, HomeSliderItemDto>({
      method: 'POST',
      url: '/api/app/home-slider',
      body: input,
    },
    { apiName: this.apiName });
  }

  delete(id: string): Observable<void> {
    return this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/home-slider/${id}`,
    },
    { apiName: this.apiName });
  }

  get(id: string): Observable<HomeSliderItemDto> {
    return this.restService.request<any, HomeSliderItemDto>({
      method: 'GET',
      url: `/api/app/home-slider/${id}`,
    },
    { apiName: this.apiName });
  }

  getList(input: PagedAndSortedResultRequestDto): Observable<PagedResultDto<HomeSliderItemDto>> {
    return this.restService.request<any, PagedResultDto<HomeSliderItemDto>>({
      method: 'GET',
      url: '/api/app/home-slider',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName });
  }

  update(id: string, input: CreateUpdateHomeSliderItemDto): Observable<HomeSliderItemDto> {
    return this.restService.request<any, HomeSliderItemDto>({
      method: 'PUT',
      url: `/api/app/home-slider/${id}`,
      body: input,
    },
    { apiName: this.apiName });
  }

  getActiveSliderItems(): Observable<HomeSliderItemDto[]> {
    return this.restService.request<any, HomeSliderItemDto[]>({
      method: 'GET',
      url: '/api/app/home-slider/active-slider-items',
    },
    { apiName: this.apiName });
  }

  reorder(orderedIds: string[]): Observable<void> {
    return this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/home-slider/reorder',
      body: orderedIds,
    },
    { apiName: this.apiName });
  }

  getSettings(): Observable<AppSettingsDto> {
    return this.restService.request<any, AppSettingsDto>({
      method: 'GET',
      url: '/api/app/home-slider/settings',
    },
    { apiName: this.apiName });
  }

  updateSettings(input: UpdateAppSettingsDto): Observable<void> {
    return this.restService.request<any, void>({
      method: 'PUT',
      url: '/api/app/home-slider/settings',
      body: input,
    },
    { apiName: this.apiName });
  }
}

