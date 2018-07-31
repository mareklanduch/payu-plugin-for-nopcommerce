using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.PayU.Models;
using Nop.Services.Configuration;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Payments.PayU.Controllers
{
    public class PaymentPayUController : BasePaymentController
    {
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly IWorkContext _workContext;

        public PaymentPayUController(
            IStoreService storeService,
            ISettingService settingService, 
            IWorkContext workContext)
        {
            _storeService = storeService;
            _settingService = settingService;
            _workContext = workContext;
        }

        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            var storeScope = GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var payUPaymentSettings = _settingService.LoadSetting<PayUPaymentSettings>(storeScope);

            var model = new ConfigurationModel()
            {
                ActiveStoreScopeConfiguration = storeScope,
                UseSandbox = payUPaymentSettings.UseSandbox,
                SandboxClientId = payUPaymentSettings.SandboxClientId,
                SandboxClientSecret = payUPaymentSettings.SandboxClientSecret,
                ClientId = payUPaymentSettings.ClientId,
                ClientSecret = payUPaymentSettings.ClientSecret
            };

            return View("~/Plugins/Payments.PayU/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [AdminAntiForgery]
        [Area(AreaNames.Admin)]
        public IActionResult Configure(ConfigurationModel model)
        {
            return Configure();
        }

        #endregion
    }
}
