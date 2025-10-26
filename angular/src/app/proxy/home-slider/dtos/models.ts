import type { SliderItemType } from '../slider-item-type.enum';
import type { FullAuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdateHomeSliderItemDto {
  displayOrder: number;
  type: SliderItemType;
  customEventId?: string;
  isActive: boolean;
  title?: string;
  titleEn?: string;
  imageUrl?: string;
}

export interface HomeSliderItemDto extends FullAuditedEntityDto<string> {
  displayOrder: number;
  type?: SliderItemType;
  customEventId?: string;
  isActive: boolean;
  title?: string;
  titleEn?: string;
  imageUrl?: string;
  eventTitle?: string;
  eventTitleEn?: string;
  eventStartDate?: string;
  eventImageUrl?: string;
}
