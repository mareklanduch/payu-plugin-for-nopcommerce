using Newtonsoft.Json;

namespace Nop.Plugin.Payments.PayU.Models.Notifications
{
    public class NameValue
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}
