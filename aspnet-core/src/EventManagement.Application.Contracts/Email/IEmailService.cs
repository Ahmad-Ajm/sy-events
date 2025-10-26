// تعليق: Interface لخدمة الإيميل
using System;
using System.Threading.Tasks;

namespace EventManagement.Email
{
    /// <summary>
    /// خدمة إرسال الإيميلات
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// إرسال إيميل بسيط
        /// </summary>
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        
        /// <summary>
        /// إرسال تذكير بالفعالية
        /// </summary>
        Task SendEventReminderAsync(string to, string userName, string eventTitle, DateTime eventDate, string eventLocation);
        
        /// <summary>
        /// إرسال إشعار بموافقة على فعالية
        /// </summary>
        Task SendEventApprovedNotificationAsync(string to, string organizerName, string eventTitle);
        
        /// <summary>
        /// إرسال إشعار برفض فعالية
        /// </summary>
        Task SendEventRejectedNotificationAsync(string to, string organizerName, string eventTitle, string reason);
        
        /// <summary>
        /// إرسال إشعار بفعالية جديدة
        /// </summary>
        Task SendNewEventNotificationAsync(string to, string userName, string eventTitle, DateTime eventDate);
    }
}

