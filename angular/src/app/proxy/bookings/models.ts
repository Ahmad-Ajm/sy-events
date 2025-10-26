// تعليق: نماذج البيانات للحجوزات
import type { EntityDto } from '@abp/ng.core';

export interface BookingDto extends EntityDto<string> {
  eventId?: string;
  userId?: string;
  status?: number;
  createdTime?: string;
}












