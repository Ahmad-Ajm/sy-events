// تعليق: مكون التقويم الكامل - عرض الفعاليات مثل Google Calendar
import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FullCalendarModule } from '@fullcalendar/angular';
import { CalendarOptions, EventClickArg } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import listPlugin from '@fullcalendar/list';
import interactionPlugin from '@fullcalendar/interaction';
import { CalendarService, CalendarEventItem } from '../services/calendar.service';

type ColorKey = 'attended' | 'noShow' | 'pastNotFollowed' | 'upcomingNotFollowed' | 'upcomingFollowed';

@Component({
  selector: 'app-calendar',
  standalone: true,
  imports: [CommonModule, FullCalendarModule],
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {
  events = signal<CalendarEventItem[]>([]);

  // تعليق: استخدام Constructor Injection بدلاً من inject() لتجنب مشاكل FullCalendar
  constructor(
    private calendarService: CalendarService,
    private router: Router
  ) {}

  // تعليق: ألوان الفعاليات حسب الحالة
  colorLegend = {
    attended: { color: '#28a745', label: 'حضرها', description: 'الفعاليات التي حضرتها' },
    noShow: { color: '#dc3545', label: 'تغيب عنها', description: 'الفعاليات التي تابعتها ولم تحضرها' },
    pastNotFollowed: { color: '#ffc107', label: 'انقضت ولم تتابعها', description: 'فعاليات انتهت ولم تسجل متابعة' },
    upcomingNotFollowed: { color: '#007bff', label: 'قادمة ولم تتابعها', description: 'فعاليات قادمة لم تسجل لها' },
    upcomingFollowed: { color: '#6f42c1', label: 'قادمة ومتابعة', description: 'فعاليات قادمة سجلت لها' }
  };

  // تعليق: إعدادات FullCalendar
  calendarOptions: CalendarOptions = {
    plugins: [dayGridPlugin, timeGridPlugin, listPlugin, interactionPlugin],
    initialView: 'dayGridMonth',
    headerToolbar: {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
    },
    locale: 'ar',
    direction: 'rtl',
    buttonText: {
      today: 'اليوم',
      month: 'شهر',
      week: 'أسبوع',
      day: 'يوم',
      list: 'قائمة'
    },
    events: [],
    eventClick: this.handleEventClick.bind(this),
    dateClick: this.handleDateClick.bind(this),
    height: 'auto',
    weekends: true,
    editable: false,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: true,
    navLinks: true
  };

  ngOnInit(): void {
    // تعليق: جلب فعاليات المستخدم من الخدمة
    this.calendarService.getUserEventsWithStatus().subscribe(events => {
      this.events.set(events);
      this.calendarOptions.events = events;
    });
  }

  // تعليق: معالجة النقر على فعالية - الانتقال لصفحة التفاصيل
  handleEventClick(clickInfo: EventClickArg): void {
    const eventId = clickInfo.event.id;
    if (eventId) {
      this.router.navigate(['/events', eventId]);
    }
  }

  // تعليق: معالجة النقر على تاريخ - يمكن استخدامه لإضافة فعالية
  handleDateClick(dateClickInfo: any): void {
    console.log('Date clicked:', dateClickInfo.dateStr);
    // يمكن إضافة منطق هنا لإضافة فعالية جديدة في هذا التاريخ
  }

  // تعليق: الحصول على لون الفعالية (للـ Legend)
  getEventColor(colorKey: ColorKey): string {
    return this.colorLegend[colorKey].color;
  }
}


