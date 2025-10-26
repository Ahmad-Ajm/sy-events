import type { EntityDto } from '@abp/ng.core';

export interface AppSettingsDto extends EntityDto<string> {
  sliderItemsCount: number;
  autoApproveEvents: boolean;
}

export interface UpdateAppSettingsDto {
  sliderItemsCount: number;
  autoApproveEvents: boolean;
}
