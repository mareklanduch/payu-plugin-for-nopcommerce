using Newtonsoft.Json;

namespace Nop.Plugin.Payments.PayU.Models.Notifications
{
    public class PayMethod
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
