using Azure.Identity;
using MassTransit;

namespace LearnMassTransit.Sender.Extensions
{
    public static class MasstransitExtensions
    {
        public static void AddMasstransitExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(m =>
            {
                m.AddDelayedMessageScheduler();
                m.SetKebabCaseEndpointNameFormatter();
                m.UsingAzureServiceBus((ctx, cfg) =>
                {
                    cfg.Host(configuration.GetSection("ConnectionString:ServiceBus").Value);
                    cfg.UseDelayedMessageScheduler();
                    cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));
                    cfg.UseMessageRetry(retry => { retry.Interval(3, TimeSpan.FromSeconds(30)); });
                });
            });
        }
    }
}
