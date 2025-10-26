import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { EventStatus } from '../../enums/event-status.enum';

export interface EventFileDto extends FullAuditedEntityDto<string> {
  eventId?: string;
  fileName?: string;
  originalFileName?: string;
  filePath?: string;
  fileType?: string;
  mimeType?: string;
  fileSize: number;
  displayOrder: number;
  thumbnailPath?: string;
  width?: number;
  height?: number;
  downloadUrl?: string;
  thumbnailUrl?: string;
  fileSizeFormatted?: string;
}

export interface UploadFilesResultDto {
  successCount: number;
  failedCount: number;
  errors: string[];
  uploadedFiles: EventFileDto[];
}

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
  organizerFilter?: string;
  isUpcoming?: boolean;
  minAttendees?: number;
}

export interface CreateEventDiscussionDto {
  eventId?: string;
  message?: string;
  parentId?: string;
}

export interface EventDiscussionDto extends FullAuditedEntityDto<string> {
  eventId?: string;
  userId?: string;
  userName?: string;
  userProfileImage?: string;
  message?: string;
  parentId?: string;
  isHidden: boolean;
  hiddenReason?: string;
  replies: EventDiscussionDto[];
  repliesCount: number;
}
