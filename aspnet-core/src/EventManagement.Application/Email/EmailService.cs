// تعليق: خدمة إرسال الإيميلات عبر SMTP
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace EventManagement.Email
{
    /// <summary>
    /// تطبيق خدمة الإيميل باستخدام SMTP
    /// </summary>
    public class EmailService : IEmailService, ITransientDependency
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        
        public EmailService(
            IConfiguration configuration,
            ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        
        /// <summary>
        /// تعليق: إرسال إيميل بسيط
        /// </summary>
        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                // تعليق: قراءة إعدادات SMTP من appsettings.json
                var smtpHost = _configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
                var smtpUserName = _configuration["Email:SmtpUserName"];
                var smtpPassword = _configuration["Email:SmtpPassword"];
                var smtpEnableSsl = bool.Parse(_configuration["Email:SmtpEnableSsl"] ?? "true");
                var fromAddress = _configuration["Email:DefaultFromAddress"] ?? "noreply@events-syria.com";
                var fromName = _configuration["Email:DefaultFromName"] ?? "منصة إدارة الفعاليات";
                
                // تعليق: التحقق من وجود الإعدادات
                if (string.IsNullOrEmpty(smtpUserName) || string.IsNullOrEmpty(smtpPassword))
                {
                    _logger.LogWarning("SMTP credentials not configured. Email not sent.");
                    return;
                }
                
                // تعليق: إنشاء رسالة الإيميل
                using var message = new MailMessage
                {
                    From = new MailAddress(fromAddress, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };
                
                message.To.Add(to);
                
                // تعليق: إعداد SMTP Client
                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUserName, smtpPassword),
                    EnableSsl = smtpEnableSsl
                };
                
                // تعليق: إرسال الإيميل
                await client.SendMailAsync(message);
                
                _logger.LogInformation($"Email sent successfully to {to}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {to}");
                throw;
            }
        }
        
        /// <summary>
        /// تعليق: إرسال تذكير بالفعالية
        /// </summary>
        public async Task SendEventReminderAsync(
            string to, 
            string userName, 
            string eventTitle, 
            DateTime eventDate,
            string eventLocation)
        {
            var subject = $"تذكير: فعالية {eventTitle}";
            
            var body = $@"
                <div dir='rtl' style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                    <div style='background: white; padding: 30px; border-radius: 10px; max-width: 600px; margin: 0 auto;'>
                        <h2 style='color: #007bff; text-align: center;'>
                            <span style='font-size: 2em;'>🔔</span><br/>
                            تذكير بالفعالية
                        </h2>
                        
                        <p style='font-size: 16px; color: #333;'>
                            مرحباً <strong>{userName}</strong>،
                        </p>
                        
                        <p style='font-size: 16px; color: #333;'>
                            هذا تذكير بأن الفعالية التالية ستبدأ قريباً:
                        </p>
                        
                        <div style='background: #f8f9fa; padding: 20px; border-right: 4px solid #007bff; margin: 20px 0;'>
                            <h3 style='color: #007bff; margin-top: 0;'>{eventTitle}</h3>
                            <p style='margin: 10px 0;'>
                                <strong>📅 التاريخ:</strong> {eventDate:dd/MM/yyyy} في تمام الساعة {eventDate:HH:mm}
                            </p>
                            <p style='margin: 10px 0;'>
                                <strong>📍 المكان:</strong> {eventLocation}
                            </p>
                        </div>
                        
                        <p style='font-size: 16px; color: #333;'>
                            نتطلع لرؤيتك في الفعالية!
                        </p>
                        
                        <div style='text-align: center; margin-top: 30px;'>
                            <p style='color: #666; font-size: 14px;'>
                                مع أطيب التحيات،<br/>
                                فريق منصة إدارة الفعاليات
                            </p>
                        </div>
                    </div>
                </div>
            ";
            
            await SendEmailAsync(to, subject, body);
        }
        
        /// <summary>
        /// تعليق: إرسال إشعار بموافقة على فعالية
        /// </summary>
        public async Task SendEventApprovedNotificationAsync(
            string to, 
            string organizerName, 
            string eventTitle)
        {
            var subject = $"✅ تمت الموافقة على فعاليتك: {eventTitle}";
            
            var body = $@"
                <div dir='rtl' style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                    <div style='background: white; padding: 30px; border-radius: 10px; max-width: 600px; margin: 0 auto;'>
                        <h2 style='color: #28a745; text-align: center;'>
                            <span style='font-size: 2em;'>✅</span><br/>
                            تمت الموافقة على فعاليتك!
                        </h2>
                        
                        <p style='font-size: 16px; color: #333;'>
                            مرحباً <strong>{organizerName}</strong>،
                        </p>
                        
                        <p style='font-size: 16px; color: #333;'>
                            يسرنا إبلاغك بأنه تمت الموافقة على فعاليتك:
                        </p>
                        
                        <div style='background: #d4edda; padding: 20px; border-right: 4px solid #28a745; margin: 20px 0;'>
                            <h3 style='color: #28a745; margin-top: 0;'>{eventTitle}</h3>
                            <p style='color: #155724;'>
                                فعاليتك أصبحت الآن مرئية للجميع على المنصة!
                            </p>
                        </div>
                        
                        <p style='font-size: 16px; color: #333;'>
                            نتمنى لك فعالية ناجحة ومثمرة.
                        </p>
                        
                        <div style='text-align: center; margin-top: 30px;'>
                            <p style='color: #666; font-size: 14px;'>
                                مع أطيب التحيات،<br/>
                                فريق منصة إدارة الفعاليات
                            </p>
                        </div>
                    </div>
                </div>
            ";
            
            await SendEmailAsync(to, subject, body);
        }
        
        /// <summary>
        /// تعليق: إرسال إشعار برفض فعالية
        /// </summary>
        public async Task SendEventRejectedNotificationAsync(
            string to, 
            string organizerName, 
            string eventTitle,
            string reason)
        {
            var subject = $"❌ تم رفض فعاليتك: {eventTitle}";
            
            var body = $@"
                <div dir='rtl' style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                    <div style='background: white; padding: 30px; border-radius: 10px; max-width: 600px; margin: 0 auto;'>
                        <h2 style='color: #dc3545; text-align: center;'>
                            <span style='font-size: 2em;'>❌</span><br/>
                            تم رفض فعاليتك
                        </h2>
                        
                        <p style='font-size: 16px; color: #333;'>
                            مرحباً <strong>{organizerName}</strong>،
                        </p>
                        
                        <p style='font-size: 16px; color: #333;'>
                            نأسف لإبلاغك بأنه تم رفض فعاليتك:
                        </p>
                        
                        <div style='background: #f8d7da; padding: 20px; border-right: 4px solid #dc3545; margin: 20px 0;'>
                            <h3 style='color: #dc3545; margin-top: 0;'>{eventTitle}</h3>
                            <p style='color: #721c24;'>
                                <strong>السبب:</strong> {reason}
                            </p>
                        </div>
                        
                        <p style='font-size: 16px; color: #333;'>
                            يمكنك تعديل البيانات وإعادة المحاولة مرة أخرى.
                        </p>
                        
                        <div style='text-align: center; margin-top: 30px;'>
                            <p style='color: #666; font-size: 14px;'>
                                مع أطيب التحيات،<br/>
                                فريق منصة إدارة الفعاليات
                            </p>
                        </div>
                    </div>
                </div>
            ";
            
            await SendEmailAsync(to, subject, body);
        }
        
        /// <summary>
        /// تعليق: إرسال إشعار بفعالية جديدة
        /// </summary>
        public async Task SendNewEventNotificationAsync(
            string to, 
            string userName, 
            string eventTitle,
            DateTime eventDate)
        {
            var subject = $"🎉 فعالية جديدة: {eventTitle}";
            
            var body = $@"
                <div dir='rtl' style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                    <div style='background: white; padding: 30px; border-radius: 10px; max-width: 600px; margin: 0 auto;'>
                        <h2 style='color: #007bff; text-align: center;'>
                            <span style='font-size: 2em;'>🎉</span><br/>
                            فعالية جديدة قد تهمك!
                        </h2>
                        
                        <p style='font-size: 16px; color: #333;'>
                            مرحباً <strong>{userName}</strong>،
                        </p>
                        
                        <p style='font-size: 16px; color: #333;'>
                            تمت إضافة فعالية جديدة على المنصة:
                        </p>
                        
                        <div style='background: #e7f3ff; padding: 20px; border-right: 4px solid #007bff; margin: 20px 0;'>
                            <h3 style='color: #007bff; margin-top: 0;'>{eventTitle}</h3>
                            <p style='color: #004085;'>
                                <strong>📅 التاريخ:</strong> {eventDate:dd/MM/yyyy}
                            </p>
                        </div>
                        
                        <p style='font-size: 16px; color: #333;'>
                            سارع بالتسجيل قبل اكتمال العدد!
                        </p>
                        
                        <div style='text-align: center; margin-top: 30px;'>
                            <p style='color: #666; font-size: 14px;'>
                                مع أطيب التحيات،<br/>
                                فريق منصة إدارة الفعاليات
                            </p>
                        </div>
                    </div>
                </div>
            ";
            
            await SendEmailAsync(to, subject, body);
        }
    }
}

