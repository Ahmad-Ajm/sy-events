// تعليق: واجهة خدمة ملفات تعريف المستخدمين
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using EventManagement.Users.Dtos;

namespace EventManagement.Users
{
    public interface IUserProfileAppService : IApplicationService
    {
        // تعليق: جلب ملف تعريف المستخدم الحالي
        Task<UserProfileDto> GetMyProfileAsync();
        
        // تعليق: جلب ملف تعريف مستخدم آخر (عام)
        Task<UserProfileDto> GetPublicProfileAsync(Guid userId);
        
        // تعليق: تحديث ملفي الشخصي
        Task<UserProfileDto> UpdateMyProfileAsync(UpdateUserProfileDto input);
        
        // تعليق: رفع صورة شخصية
        Task<string> UploadProfileImageAsync(Guid userId);
    }
}

