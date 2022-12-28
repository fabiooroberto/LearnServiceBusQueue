using Azure.Messaging.ServiceBus.Administration;
using Azure.Messaging.ServiceBus;
using DevOps.Web.Api.Handler;
using DevOps.Web.Api.Services.Interface;
using Azure.Identity;
using DevOps.Web.Api.Services;

namespace DevOps.Web.Api.Core
{
    public static class RegisterServices
    {
        public static void AddServiceBusExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton((s) => {
                return new ServiceBusClient(configuration.GetConnectionString("ServiceBus"), new DefaultAzureCredential(), new ServiceBusClientOptions() { TransportType = ServiceBusTransportType.AmqpWebSockets });
            });
            services.AddSingleton((s) =>
            {
                return new ServiceBusAdministrationClient(configuration.GetConnectionString("ServiceBus"), new DefaultAzureCredential());
            });
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IDevOpsReceiveResourceHandler, DevOpsReceiveResourceHandler>();
            services.AddSingleton<IBusService, BusService>();
        }

        public static void AddBuilderServices(this WebApplication app)
        {
            var bus = app.Services.GetRequiredService<IDevOpsReceiveResourceHandler>();
            bus.RegisterOnMessageHandlerAndReceiveMessages().GetAwaiter().GetResult();
        }
    }
}
