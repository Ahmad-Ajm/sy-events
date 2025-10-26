using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventManagement.Bookings;
using EventManagement.Email;
using EventManagement.Events;
using EventManagement.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace EventManagement.BackgroundJobs
{
    /// <summary>
    /// Periodic background worker that sends reminder emails to users who have confirmed bookings
    /// for events starting soon, based on user's selected ReminderTime.
    /// Runs every 5 minutes by default.
    /// </summary>
    public class UpcomingEventReminderWorker : AsyncPeriodicBackgroundWorkerBase, ITransientDependency
    {
        public UpcomingEventReminderWorker(
            AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
        {
            // Run every 5 minutes
            Timer.Period = 5 * 60 * 1000;
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            var logger = workerContext.ServiceProvider.GetRequiredService<ILogger<UpcomingEventReminderWorker>>();
            var clock = workerContext.ServiceProvider.GetRequiredService<IClock>();
            var eventRepository = workerContext.ServiceProvider.GetRequiredService<IRepository<Event, Guid>>();
            var bookingRepository = workerContext.ServiceProvider.GetRequiredService<IRepository<Booking, Guid>>();
            var emailService = workerContext.ServiceProvider.GetRequiredService<IEmailService>();

            var now = clock.Now;
            var horizon = now.AddHours(24); // Look ahead 24h window

            // Fetch upcoming approved events within horizon
            var upcomingEvents = await eventRepository.GetListAsync(e => e.IsApproved && e.StartDate >= now && e.StartDate <= horizon);
            if (!upcomingEvents.Any())
            {
                logger.LogDebug("No upcoming events within next 24h.");
                return;
            }

            foreach (var ev in upcomingEvents)
            {
                var bookings = await bookingRepository.GetListAsync(b => b.EventId == ev.Id && b.Status == BookingStatus.Confirmed);
                foreach (var booking in bookings)
                {
                    if (!booking.ReminderTime.HasValue)
                    {
                        continue;
                    }

                    if (!booking.ShouldSendReminder(ev.StartDate))
                    {
                        continue;
                    }

                    // Attempt to send email using booking.User navigation if available
                    var to = booking.User?.Email;
                    var name = booking.User?.Name ?? "مستخدم";

                    if (string.IsNullOrWhiteSpace(to))
                    {
                        logger.LogWarning("Skipping reminder for booking {BookingId} because user email is empty.", booking.Id);
                        continue;
                    }

                    try
                    {
                        await emailService.SendEventReminderAsync(to, name, ev.Title, ev.StartDate, ev.Location);
                        logger.LogInformation("Reminder email sent for Booking {BookingId} Event {EventId}.", booking.Id, ev.Id);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed sending reminder for Booking {BookingId} Event {EventId}.", booking.Id, ev.Id);
                    }
                }
            }
        }
    }
}


