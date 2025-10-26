import { mapEnumToOptions } from '@abp/ng.core';

export enum MeetingStatus {
  Pending = 1,
  Accepted = 2,
  Rejected = 3,
  Cancelled = 4,
}

export const meetingStatusOptions = mapEnumToOptions(MeetingStatus);
