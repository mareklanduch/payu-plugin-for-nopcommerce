using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.PayU.Components
{
    [ViewComponent(Name = "PaymentPayU")]
    public class PaymentPayUViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Plugins/Payments.PayU/Views/PaymentInfo.cshtml");
        }
    }
}