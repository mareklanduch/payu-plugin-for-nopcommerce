using Newtonsoft.Json;

namespace Nop.Plugin.Payments.PayU.Models.Notifications
{
    public class NotificationRefund
    {
        [JsonProperty(PropertyName = "orderId")]
        public string OrderId { get; set; }

        [JsonProperty(PropertyName = "extOrderId")]
        public string ExtOrderId { get; set; }

        [JsonProperty(PropertyName = "refund")]
        public Refund Refund { get; set; }
    }
}
