import { mapEnumToOptions } from '@abp/ng.core';

export enum FeaturedBoxType {
  Latest = 1,
  Popular = 2,
  Custom = 3,
  Upcoming = 4,
}

export const featuredBoxTypeOptions = mapEnumToOptions(FeaturedBoxType);
