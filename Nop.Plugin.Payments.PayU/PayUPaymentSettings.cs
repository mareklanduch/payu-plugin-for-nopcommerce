using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.PayU
{
    public class PayUPaymentSettings : ISettings
    {
        public bool UseSandbox { get; set; }
        public string SandboxClientId { get; set; }
        public string SandboxClientSecret { get; set; }
        public string SandboxSecondKey { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SecondKey { get; set; }
    }
}
