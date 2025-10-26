import type { FeaturedBoxType } from '../../enums/featured-box-type.enum';
import type { EntityDto } from '@abp/ng.core';

export interface CreateUpdateFeaturedBoxDto {
  displayOrder: number;
  type?: FeaturedBoxType;
  customEventId?: string;
  isActive: boolean;
  title?: string;
  titleEn?: string;
  description?: string;
  descriptionEn?: string;
  imageUrl?: string;
  customLink?: string;
}

export interface FeaturedBoxDto extends EntityDto<string> {
  displayOrder: number;
  type?: FeaturedBoxType;
  customEventId?: string;
  isActive: boolean;
  title?: string;
  titleEn?: string;
  description?: string;
  descriptionEn?: string;
  imageUrl?: string;
  customLink?: string;
  eventTitle?: string;
  eventTitleEn?: string;
  eventStartDate?: string;
  eventImageUrl?: string;
  eventLocation?: string;
}
