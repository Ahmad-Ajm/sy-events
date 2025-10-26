import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { UploadFilesResultDto } from '../events/dtos/models';
import type { IFormFile } from '../microsoft/asp-net-core/http/models';

@Injectable({
  providedIn: 'root',
})
export class EventFileService {
  apiName = 'Default';
  

  uploadMultipleByEventIdAndFiles = (eventId: string, files: IFormFile[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, UploadFilesResultDto>({
      method: 'POST',
      url: `/api/app/event/${eventId}/files/upload-multiple`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
