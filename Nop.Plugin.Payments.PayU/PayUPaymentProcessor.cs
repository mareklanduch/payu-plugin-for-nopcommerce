using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Plugins;
using Nop.Plugin.Payments.PayU.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;

namespace Nop.Plugin.Payments.PayU
{
    public class PayUPaymentProcessor : BasePlugin, IPaymentMethod
    {
        private readonly ISettingService _settingService;
        private readonly IPayUService _payUService;
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;

        public bool SupportCapture => false; // Not imlpemented
        public bool SupportVoid => false; // Not implemented
        public bool SupportPartiallyRefund => true;
        public bool SupportRefund => true;

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;
        public PaymentMethodType PaymentMethodType => PaymentMethodType.Redirection;
        public bool SkipPaymentInfo => false;
        public string PaymentMethodDescription =>
            _localizationService.GetResource("Plugins.Payments.PayU.PaymentMethodDescription");

        public PayUPaymentProcessor(
            ISettingService settingService,
            IPayUService payUService,
            IWebHelper webHelper,
            ILocalizationService localizationService)
        {
            _settingService = settingService;
            _payUService = payUService;
            _webHelper = webHelper;
            _localizationService = localizationService;
        }

        public override void Install()
        {
            _settingService.SaveSetting(new PayUPaymentSettings()
            {
                UseSandbox = true,
                SandboxClientId = "300746",
                SandboxClientSecret = "2ee86a66e5d97e3fadc400c9f19b065d",
                SandboxSecondKey = "b6ca15b0d1020e8094d9b5f8d163db54"
            });

            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayU.Fields.UseSandbox", "Use Sandbox");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayU.Fields.SandboxClientId", "Sandbox client id");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayU.Fields.SandboxClientSecret",
                "Sandbox client secret");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayU.Fields.SandboxSecondKey",
                "Sandbox second key");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayU.Fields.ClientId", "Client id");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayU.Fields.ClientSecret", "Client secret");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayU.Fields.SecondKey", "Second key");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayU.PaymentMethodDescription",
                "You will be redirected to PayU site to complete the payment");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayU.PaymentInfo",
                "You will be redirected to PayU site to complete the order.");

            base.Install(); 
        }

        public override void Uninstall()
        {
            _settingService.DeleteSetting<PayUPaymentSettings>();

            this.DeletePluginLocaleResource("Plugins.Payments.PayU.Fields.UseSandbox");
            this.DeletePluginLocaleResource("Plugins.Payments.PayU.Fields.SandboxClientId");
            this.DeletePluginLocaleResource("Plugins.Payments.PayU.Fields.SandboxClientSecret");
            this.DeletePluginLocaleResource("Plugins.Payments.PayU.Fields.SandboxSecondKey");
            this.DeletePluginLocaleResource("Plugins.Payments.PayU.Fields.ClientId");
            this.DeletePluginLocaleResource("Plugins.Payments.PayU.Fields.ClientSecret");
            this.DeletePluginLocaleResource("Plugins.Payments.PayU.Fields.SecondKey");

            this.DeletePluginLocaleResource("Plugins.Payments.PayU.PaymentMethodDescription");
            this.DeletePluginLocaleResource("Plugins.Payments.PayU.PaymentInfo");

            base.Uninstall();   
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/PaymentPayU/Configure";
        }

        public bool CanRePostProcessPayment(Order order)
        {
            return false;
        }

        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            return 0;
        }

        public ProcessPaymentRequest GetPaymentInfo(IFormCollection form)
        {
            return new ProcessPaymentRequest();
        }

        public string GetPublicViewComponentName()
        {
            return "PaymentPayU";
        }

        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            return false;
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult();
        }

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            _payUService.RedirectToPayUPayment(postProcessPaymentRequest);
        }

        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            return _payUService.Refund(refundPaymentRequest);
        }

        public IList<string> ValidatePaymentForm(IFormCollection form)
        {
            return new List<string>();
        }

        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            return new CapturePaymentResult();
        }

        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            return new VoidPaymentResult();
        }

        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult();
        }

        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            return new CancelRecurringPaymentResult();
        }
    }
}
