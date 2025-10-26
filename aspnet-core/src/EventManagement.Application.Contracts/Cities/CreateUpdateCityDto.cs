using System.ComponentModel.DataAnnotations;

namespace EventManagement.Cities
{
    /// <summary>
    /// DTO لإنشاء أو تعديل مدينة
    /// </summary>
    public class CreateUpdateCityDto
    {
        /// <summary>
        /// اسم المدينة بالعربية
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// اسم المدينة بالإنجليزية
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string NameEn { get; set; } = string.Empty;
    }
}


