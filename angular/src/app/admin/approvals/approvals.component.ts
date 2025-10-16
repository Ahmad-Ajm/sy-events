import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { EventService } from '../../proxy/event.service';
import { EventDto } from '../../proxy/events/dtos/models';

@Component({
  selector: 'app-approvals',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container py-3">
      <h2 class="mb-3">موافقات الفعاليات</h2>
      <div class="mb-3">
        <button class="btn btn-success me-2" (click)="bulkApprove()" [disabled]="pending().length === 0">الموافقة على الجميع</button>
      </div>
      <div class="table-responsive">
        <table class="table table-striped align-middle">
          <thead>
            <tr>
              <th>العنوان</th>
              <th>التاريخ</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let ev of pending()">
              <td>{{ ev.title }}</td>
              <td>{{ ev.startDate | date:'mediumDate' }}</td>
              <td class="text-end">
                <button class="btn btn-sm btn-outline-primary me-2" (click)="approve(ev.id)">موافقة</button>
                <button class="btn btn-sm btn-outline-danger" (click)="reject(ev.id)">رفض</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  `,
})
export class ApprovalsComponent implements OnInit {
  private readonly events = inject(EventService);
  pending = signal<EventDto[]>([]);

  ngOnInit(): void {
    this.loadPending();
  }

  loadPending(): void {
    this.events.getPending().subscribe(items => this.pending.set(items));
  }

  approve(id: string): void {
    this.events.approve(id).subscribe(() => this.loadPending());
  }

  reject(id: string): void {
    this.events.reject(id).subscribe(() => this.loadPending());
  }

  bulkApprove(): void {
    const list = this.pending();
    if (list.length === 0) return;
    const ids = list.map(e => e.id);
    this.events.bulkApprove(ids).subscribe(() => this.loadPending());
  }
}


