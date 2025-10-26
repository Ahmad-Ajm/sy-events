using System.ComponentModel.DataAnnotations;

namespace EventManagement.Accounts
{
    /// <summary>
    /// DTO لتسجيل مستخدم جديد كمشاهد
    /// </summary>
    public class RegisterViewerInput
    {
        /// <summary>
        /// البريد الإلكتروني
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// الاسم الكامل
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// كلمة المرور
        /// </summary>
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}


