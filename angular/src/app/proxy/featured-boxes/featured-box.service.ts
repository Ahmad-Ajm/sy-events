import type { CreateUpdateFeaturedBoxDto, FeaturedBoxDto } from './dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class FeaturedBoxService {
  apiName = 'Default';
  

  create = (input: CreateUpdateFeaturedBoxDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FeaturedBoxDto>({
      method: 'POST',
      url: '/api/app/featured-box',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/featured-box/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FeaturedBoxDto>({
      method: 'GET',
      url: `/api/app/featured-box/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getActiveFeaturedBoxes = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, FeaturedBoxDto[]>({
      method: 'GET',
      url: '/api/app/featured-box/active-featured-boxes',
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<FeaturedBoxDto>>({
      method: 'GET',
      url: '/api/app/featured-box',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  reorder = (orderedIds: string[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/featured-box/reorder',
      body: orderedIds,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateFeaturedBoxDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FeaturedBoxDto>({
      method: 'PUT',
      url: `/api/app/featured-box/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
