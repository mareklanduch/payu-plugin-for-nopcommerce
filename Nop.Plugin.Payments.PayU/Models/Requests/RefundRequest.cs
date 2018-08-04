using Newtonsoft.Json;

namespace Nop.Plugin.Payments.PayU.Models.Requests
{
    public class RefundRequest
    {
        [JsonProperty(PropertyName = "refund")]
        public RefundDataRequest Refund { get; set; }
    }
}
