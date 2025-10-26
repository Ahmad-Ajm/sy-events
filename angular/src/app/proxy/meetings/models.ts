import type { MeetingStatus } from './meeting-status.enum';

export interface AttendeeMeetingDto {
  id?: string;
  eventId?: string;
  requesterId?: string;
  requesterName?: string;
  requestedId?: string;
  requestedName?: string;
  meetingTime?: string;
  location?: string;
  status?: MeetingStatus;
  notes?: string;
  rejectionReason?: string;
}

export interface CreateAttendeeMeetingDto {
  eventId?: string;
  requestedId?: string;
  meetingTime?: string;
  location?: string;
  notes?: string;
}
