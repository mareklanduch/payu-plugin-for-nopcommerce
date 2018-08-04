using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Payments.PayU.Services;

namespace Nop.Plugin.Payments.PayU
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        int IDependencyRegistrar.Order => 0;

        void IDependencyRegistrar.Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<PayUService>().As<IPayUService>().InstancePerLifetimeScope().InstancePerLifetimeScope();
        }
    }
}
