using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Nop.Core;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Plugins;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Tax;

namespace Nop.Plugin.Payments.PayU
{
    /// <summary>
    /// PayPalStandard payment processor
    /// </summary>
    public class PayUPaymentProcessor : BasePlugin, IPaymentMethod
    {
        private readonly PayUPaymentSettings _payUPaymentSettings;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        public bool SupportCapture => throw new NotImplementedException();
        public bool SupportPartiallyRefund => throw new NotImplementedException();
        public bool SupportRefund => throw new NotImplementedException();
        public bool SupportVoid => throw new NotImplementedException();
        public RecurringPaymentType RecurringPaymentType => throw new NotImplementedException();
        public PaymentMethodType PaymentMethodType => throw new NotImplementedException();
        public bool SkipPaymentInfo => throw new NotImplementedException();
        public string PaymentMethodDescription => throw new NotImplementedException();

        public PayUPaymentProcessor(
            PayUPaymentSettings payUPaymentSettings,
            ISettingService settingService,
            IWebHelper webHelper)
        {
            _payUPaymentSettings = payUPaymentSettings;
            _settingService = settingService;
            _webHelper = webHelper;
        }

        public override void Install()
        {
            _settingService.SaveSetting(new PayUPaymentSettings()
            {
                UseSandbox = true
            });

            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayU.Fields.UseSandbox", "Use Sandbox");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayU.Fields.SandboxClientId", "Sandbox client id");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayU.Fields.SandboxClientSecret", "Sandbox client secret");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayU.Fields.ClientId", "Client id");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayU.Fields.ClientSecret", "Client secret");

            base.Install(); 
        }

        public override void Uninstall()
        {
            _settingService.DeleteSetting<PayUPaymentSettings>();
            base.Uninstall();   
        }
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/PaymentPayU/Configure";
        }

        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public bool CanRePostProcessPayment(Order order)
        {
            throw new NotImplementedException();
        }

        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            throw new NotImplementedException();
        }

        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            throw new NotImplementedException();
        }

        public ProcessPaymentRequest GetPaymentInfo(IFormCollection form)
        {
            throw new NotImplementedException();
        }

        public string GetPublicViewComponentName()
        {
            throw new NotImplementedException();
        }

        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            throw new NotImplementedException();
        }

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public IList<string> ValidatePaymentForm(IFormCollection form)
        {
            throw new NotImplementedException();
        }

        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            throw new NotImplementedException();
        }
    }
}
