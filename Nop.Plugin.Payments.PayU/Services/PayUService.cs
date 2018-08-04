using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Plugin.Payments.PayU.Models;
using Nop.Plugin.Payments.PayU.Models.Notifications;
using Nop.Plugin.Payments.PayU.Models.Requests;
using Nop.Services.Directory;
using Nop.Services.Payments;
using Nop.Services.Logging;
using Nop.Services.Orders;

namespace Nop.Plugin.Payments.PayU.Services
{
    public class PayUService : IPayUService
    {
        private readonly ILogger _logger;
        private readonly PayUPaymentSettings _payUPaymentSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHelper _webHelper;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly ICurrencyService _currencyService;
        private readonly IOrderService _orderService;

        private string GetPayUUrl => _payUPaymentSettings.UseSandbox
            ? "https://secure.snd.payu.com"
            : "https://secure.payu.com";

        private string ClientId => _payUPaymentSettings.UseSandbox
            ? _payUPaymentSettings.SandboxClientId
            : _payUPaymentSettings.ClientId;

        private string ClientSecret => _payUPaymentSettings.UseSandbox
            ? _payUPaymentSettings.SandboxClientSecret
            : _payUPaymentSettings.ClientSecret;

        public PayUService(
            ILogger logger,
            PayUPaymentSettings payUPaymentSettings,
            IHttpContextAccessor httpContextAccessor,
            IWebHelper webHelper,
            IOrderProcessingService orderProcessingService,
            ICurrencyService currencyService,
            IOrderService orderService)
        {
            _logger = logger;
            _payUPaymentSettings = payUPaymentSettings;
            _httpContextAccessor = httpContextAccessor;
            _webHelper = webHelper;
            _orderProcessingService = orderProcessingService;
            _currencyService = currencyService;
            _orderService = orderService;
        }

        public void RedirectToPayUPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var bearer = GetAuthorizationData().AccessToken;
            var order = PrepareOrder(postProcessPaymentRequest);
            var orderJson = JsonConvert.SerializeObject(order);

            using (var httpClient =
                new HttpClient(new HttpClientHandler {AllowAutoRedirect = false}) {BaseAddress = new Uri(GetPayUUrl)})
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
                httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content =
                    new StringContent(
                        orderJson,
                        Encoding.UTF8,
                        "application/json");

                using (var response = httpClient.PostAsync("/api/v2_1/orders", content).Result)
                {
                        var responseContent = response.Content.ReadAsStringAsync().Result;
                        var orderResponse = JsonConvert.DeserializeObject<OrderResponse>(responseContent);
                        _httpContextAccessor.HttpContext.Response.Redirect(orderResponse.RedirectUri);
                }
            }
        }

        public void Notify(Notification notification)
        {
            if (!int.TryParse(notification.OrderRequest.ExtOrderId, out var orderId))
            {
                return;
            }

            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return;
            }

            switch (notification.OrderRequest.Status.ToUpperInvariant())
            {
                case "PENDING":
                    OrderPending(order);
                    break;
                case "WAITING_FOR_CONFIRMATION":
                    //Not implemented. This status is only available when "auto odbiór" is disabled in PayU settings.
                    break;
                case "COMPLETED":
                    OrderCompleted(notification, order);
                    break;
                case "CANCELED":
                    OrderCanceled(order);
                    break;
                case "REJECTED":
                    OrderRejected(order);
                    break;
            }
        }

        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            if (string.IsNullOrEmpty(refundPaymentRequest?.Order?.CaptureTransactionId))
            {
                return RefundPayUOrderIdNotFound();
            }

            var bearer = GetAuthorizationData().AccessToken;

            using(var httpClient = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = false
            })
            {
                BaseAddress = new Uri(GetPayUUrl)
            })
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
                httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = GetRefundJson(refundPaymentRequest);

                using (var response = httpClient.PostAsync($"/api/v2_1/orders/{refundPaymentRequest.Order.CaptureTransactionId}/refunds", content).Result)
                {
                    return RefundResult((int)response.StatusCode, refundPaymentRequest.IsPartialRefund);
                }
            }
        }

        private AuthorizationRequest GetAuthorizationData()
        {
            using (var httpClient = new HttpClient { BaseAddress = new Uri(GetPayUUrl) })
            {
                var content =
                    new StringContent(
                        $"grant_type=client_credentials&client_id={ClientId}&client_secret={ClientSecret}",
                        Encoding.Default,
                        "application/x-www-form-urlencoded");
                using (var response = httpClient.PostAsync("/pl/standard/user/oauth/authorize", content).Result)
                {
                    AuthorizationRequest authRequest;
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        var responseContent = response.Content.ReadAsStringAsync().Result;
                        authRequest = JsonConvert.DeserializeObject<AuthorizationRequest>(responseContent);
                    }
                    catch
                    {
                        authRequest = null;
                    }

                    return authRequest;
                }
            }
        }

        private OrderRequest PrepareOrder(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var orderId = postProcessPaymentRequest.Order.Id.ToString();

            var targetCurrency =
                _currencyService.GetCurrencyByCode(postProcessPaymentRequest.Order.CustomerCurrencyCode);

            var orderTotal = PriceInPayUStandard(postProcessPaymentRequest.Order.OrderTotal, targetCurrency);

            var result = new OrderRequest
            {
                ExtOrderId = orderId,
                NotifyUrl = $"{_webHelper.GetStoreLocation()}Plugins/PaymentPayU/Notify",
                ContinueUrl = $"{_webHelper.GetStoreLocation()}Plugins/PaymentPayU/ProcessingPayment?orderId={orderId}",
                BuyerRequest = new BuyerRequest
                {
                    ExtCustomerId = postProcessPaymentRequest.Order.CustomerId.ToString(),
                    Email = postProcessPaymentRequest.Order.Customer.Email
                },
                CurrencyCode = postProcessPaymentRequest.Order.CustomerCurrencyCode,
                CustomerIp = postProcessPaymentRequest.Order.CustomerIp,
                Description = $"External id {orderId}",
                MerchantPosId = ClientId,
                TotalAmount = orderTotal.ToString(CultureInfo.InvariantCulture),
                Products = new List<ProductRequest>()
            };

            foreach (var item in postProcessPaymentRequest.Order.OrderItems)
            {
                var itemPrice = PriceInPayUStandard(item.UnitPriceInclTax, targetCurrency);

                result.Products.Add(new ProductRequest
                {
                    Name = item.Product.Name,
                    Quantity = item.Quantity.ToString(),
                    UnitPrice = itemPrice.ToString(CultureInfo.InvariantCulture)
                });
            }

            return result;
        }

        private decimal PriceInPayUStandard(decimal price, Currency targetCurrency)
        {
            return Math.Round(_currencyService.ConvertFromPrimaryStoreCurrency(price * 100, targetCurrency),
                MidpointRounding.ToEven);
        }

        private void OrderPending(Order order)
        {
            order.OrderNotes.Add(new OrderNote
            {
                Note = $"Order id {order.Id}. PayU order pending.",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });

            _orderService.UpdateOrder(order);
        }

        private void OrderCompleted(Notification notification, Order order)
        {
            if (!decimal.TryParse(notification?.OrderRequest?.TotalAmount, out var totalAmount))
            {
                return;
            }

            var targetCurrency =
                _currencyService.GetCurrencyByCode(order.CustomerCurrencyCode);

            var orderTotal = PriceInPayUStandard(order.OrderTotal, targetCurrency);

            if (totalAmount == orderTotal)
            {
                if (_orderProcessingService.CanMarkOrderAsPaid(order))
                {
                    order.CaptureTransactionId = notification?.OrderRequest?.OrderId;

                    order.OrderNotes.Add(new OrderNote
                    {
                        Note = $"PayU order id {order.CaptureTransactionId}",
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });

                    _orderService.UpdateOrder(order);
                    _orderProcessingService.MarkOrderAsPaid(order);
                }
            }
            else
            {
                var error =
                    $"PayU order id {notification?.OrderRequest?.OrderId}. Order id {order.Id}. PayU returned order total {totalAmount}. Order total should be equal to {order.OrderTotal}.";
                
                _logger.Error(error);

                order.OrderNotes.Add(new OrderNote
                {
                    Note = error,
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });

                _orderService.UpdateOrder(order);
            }
        }

        private void OrderCanceled(Order order)
        {
            if (_orderProcessingService.CanCancelOrder(order))
            {
                _orderProcessingService.CancelOrder(order, false);
            }

            order.OrderNotes.Add(new OrderNote
            {
                Note = $"Order id {order.Id}. PayU order canceled.",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });

            _orderService.UpdateOrder(order);
        }

        private void OrderRejected(Order order)
        {
            OrderCanceled(order);

            order.OrderNotes.Add(new OrderNote
            {
                Note = $"Order id {order.Id}. PayU order rejected.",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });

            _orderService.UpdateOrder(order);
        }

        private RefundPaymentResult RefundPayUOrderIdNotFound()
        {
            var refund = new RefundPaymentResult();
            var error =
                "PayU order id not found. Probably payment settled manually. Refund can be done using PayU site.";
            refund.Errors.Add(error);
            _logger.Error(error);

            return refund;
        }

        private RefundPaymentResult RefundResult(int statusCode, bool partialRefund)
        {
            switch (statusCode)
            {
                case 200:
                case 204:
                    return new RefundPaymentResult
                    {
                        NewPaymentStatus = partialRefund ? PaymentStatus.PartiallyRefunded : PaymentStatus.Refunded
                    };
                default:
                    var error = $"PayU refund error code {statusCode}";
                    _logger.Error(error);
                    return new RefundPaymentResult
                    {
                        Errors = new List<string>
                        {
                            error
                        }
                    };
            }
        }

        private StringContent GetRefundJson(RefundPaymentRequest refundPaymentRequest)
        {
            var refundData = new RefundRequest();
            if (refundPaymentRequest.IsPartialRefund)
            {
                var targetCurrency = _currencyService.GetCurrencyByCode(refundPaymentRequest.Order.CustomerCurrencyCode);
                var refundAmount = PriceInPayUStandard(refundPaymentRequest.AmountToRefund, targetCurrency);
                refundData.Refund = new ParialRefundDataRequest
                {
                    Amount = refundAmount.ToString(CultureInfo.InvariantCulture),
                    Description = $"Partial refund, amount: {refundAmount} {targetCurrency.CurrencyCode}"
                };
            }
            else
            {
                refundData.Refund = new RefundDataRequest
                {
                    Description = "Full refund"
                };
            }

            var refundJson = JsonConvert.SerializeObject(refundData);

            return new StringContent(refundJson, Encoding.UTF8, "application/json");
        }
    }
}
