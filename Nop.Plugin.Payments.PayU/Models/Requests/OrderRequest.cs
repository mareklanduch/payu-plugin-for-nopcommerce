using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nop.Plugin.Payments.PayU.Models.Requests
{
    public class OrderRequest
    {
        [JsonProperty(PropertyName = "extOrderId")]
        public string ExtOrderId { get; set; }

        [JsonProperty(PropertyName = "notifyUrl")]
        public string NotifyUrl { get; set; }

        [JsonProperty(PropertyName = "continueUrl")]
        public string ContinueUrl { get; set; }

        [JsonProperty(PropertyName = "customerIp")]
        public string CustomerIp { get; set; }

        [JsonProperty(PropertyName = "merchantPosId")]
        public string MerchantPosId { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "additionalDescription")]
        public string AdditionalDescription { get; set; }

        [JsonProperty(PropertyName = "currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty(PropertyName = "totalAmount")]
        public string TotalAmount { get; set; }

        [JsonProperty(PropertyName = "buyer")]
        public BuyerRequest BuyerRequest { get; set; }

        [JsonProperty(PropertyName = "products")]
        public List<ProductRequest> Products { get; set; }

    }
}
