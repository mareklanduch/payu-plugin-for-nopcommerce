using Newtonsoft.Json;

namespace Nop.Plugin.Payments.PayU.Models.Requests
{
    public class RefundDataRequest
    {
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
