using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using EventManagement.Accounts;

namespace EventManagement.Controllers
{
    /// <summary>
    /// API Controller لتسجيل المستخدمين كمشاهدين
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/app/accounts")] 
    public class RegisterViewerController : AbpController
    {
        private readonly IRegisterViewerAppService _service;
        
        public RegisterViewerController(IRegisterViewerAppService service) 
        { 
            _service = service; 
        }

        /// <summary>
        /// تسجيل مستخدم جديد كمشاهد
        /// </summary>
        [HttpPost("register-viewer")] 
        public Task<Guid> RegisterViewer([FromBody] RegisterViewerInput input) => _service.RegisterAsync(input);
    }
}


