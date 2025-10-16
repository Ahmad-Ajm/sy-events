import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EventService } from '../../proxy/event.service';
import { EventDto } from '../../proxy/events/dtos/models';

@Component({
  selector: 'app-event-detail',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container py-3" *ngIf="event">
      <h2 class="mb-2">{{ event.title }}</h2>
      <p class="text-muted">{{ event.location }} — {{ event.startDate | date:'medium' }}</p>
      <div class="card p-3">
        <p>{{ event.description }}</p>
      </div>
    </div>
  `,
})
export class EventDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly events = inject(EventService);
  event?: EventDto;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.events.get(id).subscribe(e => this.event = e);
    }
  }
}


