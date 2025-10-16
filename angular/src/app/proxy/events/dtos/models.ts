import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { EventStatus } from '../../enums/event-status.enum';

export interface CreateUpdateEventDto {
  title: string;
  titleEn?: string;
  description: string;
  descriptionEn?: string;
  startDate: string;
  endDate: string;
  location: string;
  locationEn?: string;
  maxCapacity?: number;
  categoryId: string;
  cityId: string;
  imageUrl?: string;
  thumbnailUrl?: string;
}

export interface EventDto extends FullAuditedEntityDto<string> {
  title?: string;
  titleEn?: string;
  description?: string;
  descriptionEn?: string;
  startDate?: string;
  endDate?: string;
  location?: string;
  locationEn?: string;
  maxCapacity?: number;
  isApproved: boolean;
  status?: EventStatus;
  imageUrl?: string;
  thumbnailUrl?: string;
  categoryName?: string;
  categoryNameEn?: string;
  cityName?: string;
  cityNameEn?: string;
  organizerName?: string;
  bookingsCount: number;
  availableCapacity?: number;
}

export interface EventStatisticsDto {
  eventId?: string;
  totalBookings: number;
  confirmedBookings: number;
  attendedCount: number;
  cancelledCount: number;
  availableCapacity?: number;
}

export interface GetEventsInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  categoryId?: string;
  cityId?: string;
  status?: EventStatus;
  startDate?: string;
  endDate?: string;
  organizerId?: string;
  isUpcoming?: boolean;
  minAttendees?: number;
}
