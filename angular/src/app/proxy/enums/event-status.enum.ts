import { mapEnumToOptions } from '@abp/ng.core';

export enum EventStatus {
  Draft = 1,
  Pending = 2,
  Approved = 3,
  Rejected = 4,
  Hidden = 5,
}

export const eventStatusOptions = mapEnumToOptions(EventStatus);
