using Newtonsoft.Json;
using Nop.Plugin.Payments.PayU.Models.Requests;

namespace Nop.Plugin.Payments.PayU.Models.Notifications
{
    public class NotificationOrderRequest : OrderRequest
    {
        [JsonProperty(PropertyName = "orderId")]
        public string OrderId { get; set; }

        [JsonProperty(PropertyName = "payMethod")]
        public PayMethod PayMethod { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }
}
