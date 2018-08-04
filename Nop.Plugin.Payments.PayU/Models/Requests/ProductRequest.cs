using Newtonsoft.Json;

namespace Nop.Plugin.Payments.PayU.Models.Requests
{
    public class ProductRequest
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "unitPrice")]
        public string UnitPrice { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public string Quantity { get; set; }
    }
}
