using Azure.Messaging.ServiceBus;
using CatalogoWiz.Web.Api.Services.Interface;
using CatalogoWiz.Web.Api.ViewModel;

namespace CatalogoWiz.Web.Api.Handler
{
    public class ReceiveResource : IReceiveResource
    {
        private ServiceBusClient _client;
        private const string QUEUE_NAME = "catalogo-update-resource";
        private readonly ILogger<ReceiveResource> _logger;
        private ServiceBusProcessor _processor;

        public ReceiveResource(
            ServiceBusClient client,
            ILogger<ReceiveResource> logger)
        {
            _client = client;
            _logger = logger;
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

        public async Task RegisterOnMessageHandlerAndReceiveMessages()
        {
            ServiceBusProcessorOptions _serviceBusProcessorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
            };
            _logger.LogInformation($"queue: {QUEUE_NAME}, start process");
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
            _logger.LogInformation($"iniciar processo: {QUEUE_NAME}");
            var request = args.Message.Body.ToObjectFromJson<ResourceDevopsViewModel>();
            
            _logger.LogInformation($"object: {args.Message.Body.ToString()}");
            // atualizar no banco
            // signalr

            await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
        }
    }
}
