using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Payments.PayU.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayU.Fields.UseSandbox")]
        public bool UseSandbox { get; set; }
        public bool UseSandboxOverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayU.Fields.SandboxClientId")]
        public string SandboxClientId { get; set; }
        public bool SandboxClientIdOverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayU.Fields.SandboxClientSecret")]
        public string SandboxClientSecret { get; set; }
        public bool SandboxClientSecretOverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayU.Fields.SandboxSecondKey")]
        public string SandboxSecondKey { get; set; }
        public bool SandboxSecondKeyOverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayU.Fields.ClientId")]
        public string ClientId { get; set; }
        public bool ClientIdOverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayU.Fields.ClientSecret")]
        public string ClientSecret { get; set; }
        public bool ClientSecretOverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayU.Fields.SecondKey")]
        public string SecondKey { get; set; }
        public bool SecondKeyOverrideForStore { get; set; }

    }
}