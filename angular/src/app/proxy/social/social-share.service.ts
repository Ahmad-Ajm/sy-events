import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SocialShareService {
  apiName = 'Default';
  

  getFacebookLink = (eventId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'GET',
      responseType: 'text',
      url: `/api/app/social-share/facebook-link/${eventId}`,
    },
    { apiName: this.apiName,...config });
  

  getWhatsAppLink = (eventId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'GET',
      responseType: 'text',
      url: `/api/app/social-share/whats-app-link/${eventId}`,
    },
    { apiName: this.apiName,...config });
  

  shareToTelegram = (eventId: string, chatId: string, botToken: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/api/app/social-share/share-to-telegram',
      params: { eventId, chatId, botToken },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
