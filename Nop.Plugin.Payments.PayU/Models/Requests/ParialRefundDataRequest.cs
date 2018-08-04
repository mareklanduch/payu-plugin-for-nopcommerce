using Newtonsoft.Json;

namespace Nop.Plugin.Payments.PayU.Models.Requests
{
    public class ParialRefundDataRequest : RefundDataRequest
    {
        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }
    }
}
