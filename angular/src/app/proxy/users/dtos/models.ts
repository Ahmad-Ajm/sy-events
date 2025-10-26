import type { FullAuditedEntityDto } from '@abp/ng.core';

export interface UpdateUserProfileDto {
  bio?: string;
  profileImageUrl?: string;
  coverImageUrl?: string;
  jobTitle?: string;
  company?: string;
  website?: string;
  linkedInUrl?: string;
  twitterHandle?: string;
  facebookUrl?: string;
  isPublic: boolean;
  showEmail: boolean;
  showPhone: boolean;
  interests: string[];
  skills: string[];
}

export interface UserProfileDto extends FullAuditedEntityDto<string> {
  userId?: string;
  userName?: string;
  email?: string;
  bio?: string;
  profileImageUrl?: string;
  coverImageUrl?: string;
  jobTitle?: string;
  company?: string;
  website?: string;
  linkedInUrl?: string;
  twitterHandle?: string;
  facebookUrl?: string;
  isPublic: boolean;
  showEmail: boolean;
  showPhone: boolean;
  eventsAttendedCount: number;
  eventsOrganizedCount: number;
  interests: string[];
  skills: string[];
}
