using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace EventManagement.Accounts
{
    /// <summary>
    /// واجهة خدمة تسجيل المستخدمين كمشاهدين
    /// </summary>
    public interface IRegisterViewerAppService : IApplicationService
    {
        /// <summary>
        /// تسجيل مستخدم جديد كمشاهد
        /// </summary>
        Task<Guid> RegisterAsync(RegisterViewerInput input);
    }
}


