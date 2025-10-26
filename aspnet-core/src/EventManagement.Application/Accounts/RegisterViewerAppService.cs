using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using EventManagement.Users;
using EventManagement.Enums;

namespace EventManagement.Accounts
{
    /// <summary>
    /// خدمة تطبيقية لتسجيل المستخدمين كمشاهدين
    /// </summary>
    public class RegisterViewerAppService : ApplicationService, IRegisterViewerAppService
    {
        private readonly IRepository<User, Guid> _userRepository;
        public RegisterViewerAppService(IRepository<User, Guid> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> RegisterAsync(RegisterViewerInput input)
        {
            // TODO: استخدم تشفير/تجزئة كلمة المرور الحقيقي (Identity)
            if (string.IsNullOrWhiteSpace(input.Email)) throw new BusinessException("EmailRequired");
            var exists = await _userRepository.FirstOrDefaultAsync(u => u.Email == input.Email);
            if (exists != null) throw new BusinessException("UserExists");

            var entity = new User(GuidGenerator.Create(), input.Email, input.Name, input.Password, UserRole.Viewer);
            await _userRepository.InsertAsync(entity);
            return entity.Id;
        }
    }
}


