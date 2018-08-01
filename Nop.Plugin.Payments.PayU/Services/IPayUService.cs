using System.Threading.Tasks;
using Nop.Plugin.Payments.PayU.Models;

namespace Nop.Plugin.Payments.PayU.Services
{
    public interface IPayUService
    {
        Task<AuthorizationData> GetAuthorizationData();
    }
}
