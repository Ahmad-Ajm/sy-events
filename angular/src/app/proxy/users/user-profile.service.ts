import type { UpdateUserProfileDto, UserProfileDto } from './dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UserProfileService {
  apiName = 'Default';
  

  getMyProfile = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserProfileDto>({
      method: 'GET',
      url: '/api/app/user-profile/my-profile',
    },
    { apiName: this.apiName,...config });
  

  getPublicProfile = (userId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserProfileDto>({
      method: 'GET',
      url: `/api/app/user-profile/public-profile/${userId}`,
    },
    { apiName: this.apiName,...config });
  

  updateMyProfile = (input: UpdateUserProfileDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserProfileDto>({
      method: 'PUT',
      url: '/api/app/user-profile/my-profile',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  uploadProfileImage = (userId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'POST',
      responseType: 'text',
      url: `/api/app/user-profile/upload-profile-image/${userId}`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
