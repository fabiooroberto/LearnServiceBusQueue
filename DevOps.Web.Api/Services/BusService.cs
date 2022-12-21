using Azure.Messaging.ServiceBus.Administration;
using Azure.Messaging.ServiceBus;
using System.Text.Json;
using DevOps.Web.Api.Services.Interface;

namespace DevOps.Web.Api.Services
{
    public class BusService : IBusService
    {
        private readonly ServiceBusClient _busClient;
        private readonly ServiceBusAdministrationClient _adminClient;

        public BusService(ServiceBusClient busClient,
                          ServiceBusAdministrationClient adminClient)
        {
            _busClient = busClient;
            _adminClient = adminClient;
        }

        public async Task SenderAsync<T>(string queueName, T request)
        {
            if (!await _adminClient.QueueExistsAsync(queueName))
                await _adminClient.CreateQueueAsync(queueName);

            var senderBus = _busClient.CreateSender(queueName);

            await senderBus.SendMessageAsync(new ServiceBusMessage(JsonSerializer.Serialize(request))
            {
                Subject = queueName
            });
        }
    }
}
