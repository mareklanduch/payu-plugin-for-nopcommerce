namespace Nop.Plugin.Payments.PayU.Models
{
    public class OrderResponse
    {
        public string OrderId { get; set; }
        public Status Status { get; set; }
        public string RedirectUri { get; set; }
    }
}
