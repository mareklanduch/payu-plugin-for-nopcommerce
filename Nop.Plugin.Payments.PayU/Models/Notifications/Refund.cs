using Newtonsoft.Json;

namespace Nop.Plugin.Payments.PayU.Models.Notifications
{
    public class Refund
    {
        [JsonProperty(PropertyName = "refundId")]
        public string RefundId { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }

        [JsonProperty(PropertyName = "currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "statusDateTime")]
        public string StatusDateTime { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }

        [JsonProperty(PropertyName = "reasonDescription")]
        public string ReasonDescription { get; set; }

        [JsonProperty(PropertyName = "refundDate")]
        public string RefundDate { get; set; }
    }
}
