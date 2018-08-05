using Nop.Plugin.Payments.PayU.Models.Notifications;
using Nop.Services.Payments;

namespace Nop.Plugin.Payments.PayU.Services
{
    public interface IPayUService
    {
        void RedirectToPayUPayment(PostProcessPaymentRequest postProcessPaymentRequest);
        void Notify(Notification notification);
        RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest);
        bool VerifySignature(string body);
    }
}
