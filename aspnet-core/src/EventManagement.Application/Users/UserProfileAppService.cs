// تعليق: خدمة ملفات تعريف المستخدمين
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Volo.Abp.ObjectExtending; // for SetProperty/GetProperty on ExtraProperties
using EventManagement.Users;
using EventManagement.Users.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace EventManagement.Users
{
    [Authorize]
    public class UserProfileAppService : ApplicationService, IUserProfileAppService
    {
        private readonly IRepository<UserProfile, Guid> _profileRepository;
        private readonly ICurrentUser _currentUser;

        public UserProfileAppService(
            IRepository<UserProfile, Guid> profileRepository,
            ICurrentUser currentUser)
        {
            _profileRepository = profileRepository;
            _currentUser = currentUser;
        }

        // تعليق: جلب ملفي الشخصي
        public async Task<UserProfileDto> GetMyProfileAsync()
        {
            var userId = _currentUser.GetId();
            var profile = await _profileRepository.FirstOrDefaultAsync(x => x.UserId == userId);
            
            if (profile == null)
            {
                // إنشاء ملف جديد إذا لم يكن موجوداً
                profile = new UserProfile(GuidGenerator.Create(), userId);
                await _profileRepository.InsertAsync(profile);
            }
            
            var dto = ObjectMapper.Map<UserProfile, UserProfileDto>(profile);
            dto.UserName = _currentUser.UserName;
            dto.Email = _currentUser.Email;
            
            return dto;
        }

        // تعليق: جلب ملف عام لمستخدم آخر
        [AllowAnonymous]
        public async Task<UserProfileDto> GetPublicProfileAsync(Guid userId)
        {
            var profile = await _profileRepository.FirstOrDefaultAsync(x => x.UserId == userId);
            
            if (profile == null || !profile.IsPublic)
            {
                throw new BusinessException("الملف الشخصي غير متاح أو خاص");
            }
            
            var dto = ObjectMapper.Map<UserProfile, UserProfileDto>(profile);
            
            // إخفاء البريد والهاتف إذا كانت الخصوصية مفعلة
            if (!profile.ShowEmail)
            {
                dto.Email = null;
            }
            
            return dto;
        }

        // تعليق: تحديث ملفي الشخصي
        public async Task<UserProfileDto> UpdateMyProfileAsync(UpdateUserProfileDto input)
        {
            var userId = _currentUser.GetId();
            var profile = await _profileRepository.FirstOrDefaultAsync(x => x.UserId == userId);
            
            if (profile == null)
            {
                profile = new UserProfile(GuidGenerator.Create(), userId);
            }
            
            // تحديث البيانات
            profile.UpdateBio(input.Bio);
            profile.UpdateProfileImage(input.ProfileImageUrl);
            profile.CoverImageUrl = input.CoverImageUrl;
            profile.JobTitle = input.JobTitle;
            profile.Company = input.Company;
            profile.Website = input.Website;
            profile.LinkedInUrl = input.LinkedInUrl;
            profile.TwitterHandle = input.TwitterHandle;
            profile.FacebookUrl = input.FacebookUrl;
            profile.UpdatePrivacySettings(input.IsPublic, input.ShowEmail, input.ShowPhone);
            
            // حفظ الاهتمامات والمهارات مباشرة
            profile.Interests = input.Interests ?? Array.Empty<string>();
            profile.Skills = input.Skills ?? Array.Empty<string>();
            
            if (profile.Id == Guid.Empty)
            {
                await _profileRepository.InsertAsync(profile);
            }
            else
            {
                await _profileRepository.UpdateAsync(profile);
            }
            
            return await GetMyProfileAsync();
        }

        // تعليق: رفع صورة شخصية (Placeholder)
        public async Task<string> UploadProfileImageAsync(Guid userId)
        {
            // TODO: تنفيذ رفع الصورة عبر BlobStoring
            return await Task.FromResult("/assets/default-avatar.png");
        }
    }
}

