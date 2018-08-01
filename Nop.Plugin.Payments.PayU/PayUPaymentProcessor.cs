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
using Nop.Plugin.Payments.PayU.Services;
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
        private readonly IPayUService _payUService;
        private readonly IWebHelper _webHelper;

        public bool SupportCapture => false;
        public bool SupportPartiallyRefund => false;
        public bool SupportRefund => false;
        public bool SupportVoid => false;

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;
        public PaymentMethodType PaymentMethodType => PaymentMethodType.Button;
        public bool SkipPaymentInfo => false;
        public string PaymentMethodDescription => "$Description for payment method$";

        public PayUPaymentProcessor(
            PayUPaymentSettings payUPaymentSettings,
            ISettingService settingService,
            IPayUService payUService,
            IWebHelper webHelper)
        {
            _payUPaymentSettings = payUPaymentSettings;
            _settingService = settingService;
            _payUService = payUService;
            _webHelper = webHelper;
        }

        public override void Install()
        {
            _settingService.SaveSetting(new PayUPaymentSettings()
            {
                UseSandbox = true,
                SandboxClientId = "300746",
                SandboxClientSecret = "2ee86a66e5d97e3fadc400c9f19b065d"
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

        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult();
        }

        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            return new CancelRecurringPaymentResult();
        }

        public bool CanRePostProcessPayment(Order order)
        {
            //TODO: Usually this method is used when it redirects a customer to a third-party site for completing a payment.
            //If the third party payment fails this option will allow customers to attempt
            //the order again later without placing a new order.
            //CanRePostProcessPayment should return true to enable this feature.
            return false;
        }

        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            //TODO: Some payment gateways allow you to authorize payments before they're captured.
            //It allows store owners to review order details before the payment is actually done.
            //In this case you just authorize a payment in ProcessPayment or
            //PostProcessPayment method (described above),and then just capture it.
            //In this case a Capture button will be visible on the order details page in admin area.
            //Note that an order should be already authorized and SupportCapture property should returntrue.
            return new CapturePaymentResult();
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
            //TODO: Here you should redirect a customer toa third-party site for completing a payment
            var hidePayment = false;
            return hidePayment;
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var tmp = _payUService.GetAuthorizationData();
            return new ProcessPaymentResult();
        }

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            //TODO: Here you should authorize in PayU by client_id and client_secret
        }   //TODO: Here you should redirect a customer toa third-party site for completing a payment

        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            //TODO: Refund. This method allows you make a refund. In this case a Refund button will be visible
            //on the order details page in admin area. Note that an order should be paid,
            //and SupportRefund or SupportPartiallyRefund property should return true.
            return new RefundPaymentResult();
        }

        public IList<string> ValidatePaymentForm(IFormCollection form)
        {
            return new List<string>();
        }

        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            //TODO: This method allows you void an authorized but not captured payment.
            //In this case a Void button will be visible on the order details page in admin area.
            //Note that an order should be authorized and SupportVoid property should return true.
            return new VoidPaymentResult();
        }
    }
}
