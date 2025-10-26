
export interface AttendeeDemographicsDto {
  eventId?: string;
  totalAttendees: number;
}

export interface EngagementMetricsDto {
  eventId?: string;
  discussionsCount: number;
  meetingsScheduledCount: number;
  averageDiscussionsPerUser: number;
}

export interface EventAnalyticsDto {
  eventId?: string;
  eventTitle?: string;
  totalRegistrations: number;
  confirmedCount: number;
  attendedCount: number;
  cancelledCount: number;
  noShowCount: number;
  attendanceRate: number;
  cancellationRate: number;
}
