using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Payments.PayU
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Plugin.Payments.PayU.Notify", "Plugins/PaymentPayU/Notify",
                new { controller = "PaymentPayU", action = "Notify" });
            routeBuilder.MapRoute("Plugin.Payments.PayU.ProcessingPayment", "Plugins/PaymentPayU/ProcessingPayment",
                new { controller = "PaymentPayU", action = "ProcessingPayment" });
        }

        public int Priority
        {
            get { return -1; }
        }
    }
}
