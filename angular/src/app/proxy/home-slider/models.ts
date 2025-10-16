import type { FullAuditedEntityDto } from '@abp/ng.core';

export interface HomeSliderItemDto extends FullAuditedEntityDto<string> {
  displayOrder: number;
  type: SliderItemType;
  customEventId?: string;
  isActive: boolean;
  title: string;
  titleEn: string;
  imageUrl: string;
  eventTitle: string;
  eventTitleEn: string;
  eventStartDate?: string;
  eventImageUrl: string;
}

export interface CreateUpdateHomeSliderItemDto {
  displayOrder: number;
  type: SliderItemType;
  customEventId?: string;
  isActive: boolean;
  title: string;
  titleEn: string;
  imageUrl: string;
}

export interface AppSettingsDto {
  id: string;
  sliderItemsCount: number;
  autoApproveEvents: boolean;
}

export interface UpdateAppSettingsDto {
  sliderItemsCount: number;
  autoApproveEvents: boolean;
}

export enum SliderItemType {
  Latest = 1,
  Popular = 2,
  Custom = 3,
}

