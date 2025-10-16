// تعليق: خدمة التقويم - جلب وتحويل بيانات الفعاليات لعرضها في FullCalendar
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { EventService } from '../proxy/event.service';
import { RestService } from '@abp/ng.core';
import { EventDto } from '../proxy/events/dtos/models';

// تعليق: نوع الحدث في FullCalendar
export interface CalendarEventItem {
  id: string;
  title: string;
  start: Date;
  end: Date;
  backgroundColor: string;
  borderColor: string;
  extendedProps: {
    location: string;
    description: string;
    status: 'attended' | 'noShow' | 'pastNotFollowed' | 'upcomingNotFollowed' | 'upcomingFollowed';
    bookingId?: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class CalendarService {
  // تعليق: استخدام Constructor Injection للتوافق
  constructor(
    private eventService: EventService,
    private rest: RestService
  ) {}

  // تعليق: ألوان الفعاليات حسب الحالة
  private readonly colorMap = {
    attended: '#28a745',
    noShow: '#dc3545',
    pastNotFollowed: '#ffc107',
    upcomingNotFollowed: '#007bff',
    upcomingFollowed: '#6f42c1'
  };

  // تعليق: جلب فعاليات المستخدم مع الحالة اللونية
  getUserEventsWithStatus(): Observable<CalendarEventItem[]> {
    // استدعاء API الفعلي المتاح: /api/app/calendar/my-events
    return this.rest.request<any, CalendarEventItem[]>({
      method: 'GET',
      url: '/api/app/calendar/my-events'
    }, { apiName: 'Default' });
  }

  // تعليق: جلب فعاليات بفترة زمنية محددة
  getEventsByDateRange(start: Date, end: Date): Observable<CalendarEventItem[]> {
    return this.rest.request<any, CalendarEventItem[]>({
      method: 'GET',
      url: '/api/app/calendar/events-by-range',
      params: { start: start.toISOString(), end: end.toISOString() }
    }, { apiName: 'Default' });
  }

  // تعليق: تحويل EventDto إلى CalendarEventItem
  private convertToCalendarEvents(events: EventDto[]): CalendarEventItem[] {
    const now = new Date();
    
    return events.map(event => {
      const startDate = new Date(event.startDate);
      const endDate = new Date(event.endDate);
      const isPast = endDate < now;
      
      // تعليق: تحديد الحالة والحجز (وهمي - سيتم ربطه بـ API الحجوزات)
      const status = isPast ? 'pastNotFollowed' : 'upcomingNotFollowed';
      
      return {
        id: event.id,
        title: event.title,
        start: startDate,
        end: endDate,
        backgroundColor: this.colorMap[status],
        borderColor: this.colorMap[status],
        extendedProps: {
          location: event.location,
          description: event.description,
          status: status
        }
      };
    });
  }

  // تعليق: بيانات وهمية للتطوير
  private getDummyEvents(): Observable<CalendarEventItem[]> {
    const today = new Date();
    
    const events: CalendarEventItem[] = [
      {
        id: '1',
        title: 'مؤتمر التقنية السنوي',
        start: new Date(today.getTime() - 30 * 24 * 60 * 60 * 1000),
        end: new Date(today.getTime() - 28 * 24 * 60 * 60 * 1000),
        backgroundColor: this.colorMap.attended,
        borderColor: this.colorMap.attended,
        extendedProps: {
          location: 'فندق الشام - دمشق',
          description: 'مؤتمر تقني سنوي',
          status: 'attended'
        }
      },
      {
        id: '2',
        title: 'ورشة عمل تطوير الويب',
        start: new Date(today.getTime() - 15 * 24 * 60 * 60 * 1000),
        end: new Date(today.getTime() - 13 * 24 * 60 * 60 * 1000),
        backgroundColor: this.colorMap.noShow,
        borderColor: this.colorMap.noShow,
        extendedProps: {
          location: 'مركز التدريب - حلب',
          description: 'ورشة Web Development',
          status: 'noShow'
        }
      },
      {
        id: '3',
        title: 'معرض الفنون التشكيلية',
        start: new Date(today.getTime() - 7 * 24 * 60 * 60 * 1000),
        end: new Date(today.getTime() - 5 * 24 * 60 * 60 * 1000),
        backgroundColor: this.colorMap.pastNotFollowed,
        borderColor: this.colorMap.pastNotFollowed,
        extendedProps: {
          location: 'المركز الثقافي - دمشق',
          description: 'معرض فنون',
          status: 'pastNotFollowed'
        }
      },
      {
        id: '4',
        title: 'حفل موسيقي كلاسيكي',
        start: new Date(today.getTime() + 7 * 24 * 60 * 60 * 1000),
        end: new Date(today.getTime() + 7 * 24 * 60 * 60 * 1000 + 3 * 60 * 60 * 1000),
        backgroundColor: this.colorMap.upcomingFollowed,
        borderColor: this.colorMap.upcomingFollowed,
        extendedProps: {
          location: 'دار الأوبرا - دمشق',
          description: 'أمسية موسيقية',
          status: 'upcomingFollowed'
        }
      },
      {
        id: '5',
        title: 'دورة البرمجة المتقدمة',
        start: new Date(today.getTime() + 14 * 24 * 60 * 60 * 1000),
        end: new Date(today.getTime() + 16 * 24 * 60 * 60 * 1000),
        backgroundColor: this.colorMap.upcomingFollowed,
        borderColor: this.colorMap.upcomingFollowed,
        extendedProps: {
          location: 'أكاديمية البرمجة - حمص',
          description: 'دورة Python',
          status: 'upcomingFollowed'
        }
      },
      {
        id: '6',
        title: 'مهرجان الطعام السوري',
        start: new Date(today.getTime() + 21 * 24 * 60 * 60 * 1000),
        end: new Date(today.getTime() + 23 * 24 * 60 * 60 * 1000),
        backgroundColor: this.colorMap.upcomingNotFollowed,
        borderColor: this.colorMap.upcomingNotFollowed,
        extendedProps: {
          location: 'حديقة تشرين - دمشق',
          description: 'مهرجان طعام',
          status: 'upcomingNotFollowed'
        }
      }
    ];

    return new Observable(observer => {
      observer.next(events);
      observer.complete();
    });
  }
}

