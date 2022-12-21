using Azure.Identity;
using Azure.Messaging.ServiceBus.Administration;
using Azure.Messaging.ServiceBus;
using CatalogoWiz.Web.Api.Services.Interface;
using CatalogoWiz.Web.Api.Services;
using CatalogoWiz.Web.Api.Handler;

namespace CatalogoWiz.Web.Api.Core
{
    public static class RegisterServices
    {
        public static void AddServiceBusExtension(this IServiceCollection services, IConfiguration configuration) {
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
            services.AddSingleton<IReceiveResource, ReceiveResource>();
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IBusService, BusService>();
        }
    }
}
