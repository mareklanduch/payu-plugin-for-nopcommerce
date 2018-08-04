using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nop.Plugin.Payments.PayU.Models.Notifications
{
    public class Notification
    {
        [JsonProperty(PropertyName = "order")]
        public NotificationOrderRequest OrderRequest { get; set; }

        [JsonProperty(PropertyName = "localReceiptDateTime")]
        public string LocalReceiptDateTime { get; set; }

        [JsonProperty(PropertyName = "properties")]
        public List<NameValue> Properties { get; set; }
    }
}
