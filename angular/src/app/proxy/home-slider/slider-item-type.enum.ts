import { mapEnumToOptions } from '@abp/ng.core';

export enum SliderItemType {
  Latest = 1,
  Popular = 2,
  Custom = 3,
}

export const sliderItemTypeOptions = mapEnumToOptions(SliderItemType);
