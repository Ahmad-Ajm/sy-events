import type { AttendeeMeetingDto, CreateAttendeeMeetingDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AttendeeMeetingService {
  apiName = 'Default';
  

  acceptMeeting = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AttendeeMeetingDto>({
      method: 'POST',
      url: `/api/app/attendee-meeting/${id}/accept-meeting`,
    },
    { apiName: this.apiName,...config });
  

  cancelMeeting = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/attendee-meeting/${id}/cancel-meeting`,
    },
    { apiName: this.apiName,...config });
  

  getIncomingRequests = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, AttendeeMeetingDto[]>({
      method: 'GET',
      url: '/api/app/attendee-meeting/incoming-requests',
    },
    { apiName: this.apiName,...config });
  

  getMyMeetings = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, AttendeeMeetingDto[]>({
      method: 'GET',
      url: '/api/app/attendee-meeting/my-meetings',
    },
    { apiName: this.apiName,...config });
  

  getOutgoingRequests = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, AttendeeMeetingDto[]>({
      method: 'GET',
      url: '/api/app/attendee-meeting/outgoing-requests',
    },
    { apiName: this.apiName,...config });
  

  rejectMeeting = (id: string, reason: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AttendeeMeetingDto>({
      method: 'POST',
      url: `/api/app/attendee-meeting/${id}/reject-meeting`,
      params: { reason },
    },
    { apiName: this.apiName,...config });
  

  requestMeeting = (input: CreateAttendeeMeetingDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AttendeeMeetingDto>({
      method: 'POST',
      url: '/api/app/attendee-meeting/request-meeting',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
