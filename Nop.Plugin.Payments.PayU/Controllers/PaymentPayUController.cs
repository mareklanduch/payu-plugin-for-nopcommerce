using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.PayU.Models;
using Nop.Plugin.Payments.PayU.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;
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
        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly IPayUService _payUService;
        private readonly IWorkContext _workContext;

        public PaymentPayUController(
            IStoreService storeService,
            ISettingService settingService, 
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IPayUService payUService,
            IWorkContext workContext)
        {
            _storeService = storeService;
            _settingService = settingService;
            _permissionService = permissionService;
            _localizationService = localizationService;
            _payUService = payUService;
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
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
            {
                return AccessDeniedView();
            }

            if (!ModelState.IsValid)
                return View("~/Plugins/Payments.PayU/Views/Configure.cshtml", model);

            var storeScope = GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var payUPaymentSettings = _settingService.LoadSetting<PayUPaymentSettings>(storeScope);

            //save settings
            payUPaymentSettings.UseSandbox = model.UseSandbox;
            payUPaymentSettings.SandboxClientId = model.SandboxClientId;
            payUPaymentSettings.SandboxClientSecret = model.SandboxClientSecret;
            payUPaymentSettings.ClientId = model.ClientId;
            payUPaymentSettings.ClientSecret= model.ClientSecret;

            _settingService.SaveSettingOverridablePerStore(payUPaymentSettings,
                x => x.UseSandbox, model.UseSandboxOverrideForStore, storeScope, false);

            _settingService.SaveSettingOverridablePerStore(payUPaymentSettings,
                x => x.SandboxClientId, model.SandboxClientIdOverrideForStore, storeScope, false);

            _settingService.SaveSettingOverridablePerStore(payUPaymentSettings,
                x => x.SandboxClientSecret, model.SandboxClientSecretOverrideForStore, storeScope, false);

            _settingService.SaveSettingOverridablePerStore(payUPaymentSettings,
                x => x.ClientId, model.ClientIdOverrideForStore, storeScope, false);

            _settingService.SaveSettingOverridablePerStore(payUPaymentSettings,
                x => x.ClientSecret, model.ClientSecretOverrideForStore, storeScope, false);

            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return View("~/Plugins/Payments.PayU/Views/Configure.cshtml", model);
        }
        #endregion
    }
}
