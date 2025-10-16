// تعليق: خدمة المشاركة عبر وسائل التواصل الاجتماعي
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using EventManagement.Events;

namespace EventManagement.Social
{
    public class SocialShareAppService : ApplicationService
    {
        private readonly IRepository<Event, Guid> _eventRepository;

        public SocialShareAppService(IRepository<Event, Guid> eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // تعليق: توليد رابط WhatsApp للمشاركة
        public async Task<string> GetWhatsAppLinkAsync(Guid eventId)
        {
            var eventEntity = await _eventRepository.GetAsync(eventId);
            
            var text = $"🎉 {eventEntity.Title}\n" +
                      $"📅 {eventEntity.StartDate:dd/MM/yyyy}\n" +
                      $"📍 {eventEntity.Location}\n" +
                      $"🔗 http://localhost:4200/events/{eventId}";
            
            var encodedText = Uri.EscapeDataString(text);
            return $"https://wa.me/?text={encodedText}";
        }

        // تعليق: توليد رابط Facebook للمشاركة
        public Task<string> GetFacebookLinkAsync(Guid eventId)
        {
            var url = $"http://localhost:4200/events/{eventId}";
            return Task.FromResult($"https://www.facebook.com/sharer/sharer.php?u={Uri.EscapeDataString(url)}");
        }

        // تعليق: مشاركة عبر Telegram Bot
        public async Task<bool> ShareToTelegramAsync(Guid eventId, string chatId, string botToken)
        {
            var eventEntity = await _eventRepository.GetAsync(eventId);
            
            var message = $"🎉 *{eventEntity.Title}*\n\n" +
                         $"📅 {eventEntity.StartDate:dd/MM/yyyy HH:mm}\n" +
                         $"📍 {eventEntity.Location}\n\n" +
                         $"{eventEntity.Description}\n\n" +
                         $"🔗 [سجل الآن](http://localhost:4200/events/{eventId})";
            
            // TODO: إرسال عبر Telegram Bot API
            // await HttpClient.PostAsync($"https://api.telegram.org/bot{botToken}/sendMessage", ...);
            
            return await Task.FromResult(true);
        }
    }
}

