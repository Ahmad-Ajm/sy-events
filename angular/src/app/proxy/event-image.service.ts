import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class EventImageService {
  private readonly baseUrl = environment.apis.default.url + '/api/app/event-image';

  constructor(private http: HttpClient) {}

  upload(eventId: string, file: File) {
    const form = new FormData();
    form.append('file', file);
    return this.http.post(`${this.baseUrl}/${eventId}`, form);
  }

  get(eventId: string) {
    return this.http.get(`${this.baseUrl}/${eventId}`, { responseType: 'blob' });
  }

  delete(eventId: string) {
    return this.http.delete(`${this.baseUrl}/${eventId}`);
  }
}


