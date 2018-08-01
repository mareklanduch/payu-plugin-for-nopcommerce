using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nop.Plugin.Payments.PayU.Models;
using Nop.Services.Configuration;

namespace Nop.Plugin.Payments.PayU.Services
{
    public class PayUService : IPayUService
    {
        private readonly PayUPaymentSettings _payUPaymentSettings;

        //pl/standard/user/oauth/authorize
        private string GetPayUUrl => _payUPaymentSettings.UseSandbox
            ? "https://merch-prod.snd.payu.com/"
            : "https://secure.payu.com/";

        private string ClientId => _payUPaymentSettings.UseSandbox
            ? _payUPaymentSettings.SandboxClientId
            : _payUPaymentSettings.ClientId;

        private string ClientSecret => _payUPaymentSettings.UseSandbox
            ? _payUPaymentSettings.SandboxClientSecret
            : _payUPaymentSettings.ClientSecret;

        public PayUService(PayUPaymentSettings payUPaymentSettings)
        {
            _payUPaymentSettings = payUPaymentSettings;

        }

        public async Task<AuthorizationData> GetAuthorizationData()
        {
            using (var httpClient = new HttpClient {BaseAddress = new Uri(GetPayUUrl)})
            {
                var content =
                    new StringContent(
                        $"grant_type=client_credentials&client_id={ClientId}&client_secret={ClientSecret}",
                        Encoding.Default,
                        "application/x-www-form-urlencoded");
                using (var response = await httpClient.PostAsync("pl/standard/user/oauth/authorize", content))
                {
                    AuthorizationData authData;
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        authData = JsonConvert.DeserializeObject<AuthorizationData>(responseContent);
                    }
                    catch
                    {
                        authData = null;
                    }

                    return authData;
                }
            }
        }
    }
}
