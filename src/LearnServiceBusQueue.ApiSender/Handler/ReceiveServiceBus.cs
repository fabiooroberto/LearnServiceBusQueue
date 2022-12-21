using Azure.Messaging.ServiceBus;
using LearnServiceBusQueue.ApiSender.Controllers;
using LearnServiceBusQueue.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace LearnServiceBusQueue.ApiSender.Handler
{
    public interface IServiceBusConsumer
    {
        Task RegisterOnMessageHandlerAndReceiveMessages();
        Task CloseQueueAsync();
        ValueTask DisposeAsync();
    }

    public class ServiceBusConsumer : IServiceBusConsumer
    {
        private readonly ServiceBusClient _client;
        private const string QUEUE_NAME = "queue-gustavera";
        private readonly ILogger _logger;
        private ServiceBusProcessor _processor;

        public ServiceBusConsumer(

            ServiceBusClient client,
            ILogger<ServiceBusConsumer> logger)
        {
            _logger = logger;
            _client = client;
        }

        public async Task RegisterOnMessageHandlerAndReceiveMessages()
        {
            ServiceBusProcessorOptions _serviceBusProcessorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
            };

            _processor = _client.CreateProcessor(QUEUE_NAME, _serviceBusProcessorOptions);
            _processor.ProcessMessageAsync += ProcessMessagesAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;
            
            

            await _processor.StartProcessingAsync().ConfigureAwait(false);
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            _logger.LogError(arg.Exception, "Message handler encountered an exception");
            _logger.LogDebug($"- ErrorSource: {arg.ErrorSource}");
            _logger.LogDebug($"- Entity Path: {arg.EntityPath}");
            _logger.LogDebug($"- FullyQualifiedNamespace: {arg.FullyQualifiedNamespace}");

            return Task.CompletedTask;
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            var myPayload = args.Message.Body.ToObjectFromJson<PocSampleQueue>();
            
            var sender = _client.CreateSender("handman-queue-create");

            await sender.SendMessageAsync(new ServiceBusMessage(JsonSerializer.Serialize(PocSampleQueue.Mock())));
            
            await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            if (_processor != null)
            {
                await _processor.DisposeAsync().ConfigureAwait(false);
            }

            if (_client != null)
            {
                await _client.DisposeAsync().ConfigureAwait(false);
            }
        }

        public async Task CloseQueueAsync()
        {
            await _processor.CloseAsync().ConfigureAwait(false);
        }
    }
}
