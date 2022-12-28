using Azure.Messaging.ServiceBus;
using DevOps.Web.Api.Services.Interface;
using DevOps.Web.Api.ViewModel;

namespace DevOps.Web.Api.Handler
{
    public class DevOpsReceiveResourceHandler : IDevOpsReceiveResourceHandler
    {
        private ServiceBusClient _client;
        private readonly IBusService _busService;
        private const string QUEUE_NAME = "devops-create-resource";
        private readonly ILogger _logger;
        private ServiceBusProcessor _processor;

        public DevOpsReceiveResourceHandler(

            ServiceBusClient client,
            ILogger<DevOpsReceiveResourceHandler> logger,
            IBusService busService)
        {
            _logger = logger;
            _client = client;
            _busService = busService;
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
            var request = args.Message.Body.ToObjectFromJson<ResourceViewModel>();
            _logger.LogInformation($"object: {args.Message.Body.ToString()}");
            // Verificar todos os dados
            // Criar o repositório

            //Montar o Objeto que irá mandar de volta para o catalogo, para atualizar na base
            var idRepository = Guid.NewGuid();

            _logger.LogInformation($"start send bus catalogo");
            await SendBusUpdateCatalogo(request, idRepository);
            _logger.LogInformation($"end send bus catalogo");

            _logger.LogInformation($"start send bus handman");
            await SendBusHandMan(request, idRepository);
            _logger.LogInformation($"end send bus handman");

            await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
        }

        private async Task SendBusHandMan(ResourceViewModel request, Guid idRepository)
        {
            var handman = new HandManViewModel()
            {
                Name = request.Name,
                IdProject = request.IdProject,
                IdRepository = idRepository,
                IdResource = request.IdResource,
                NameRepository = $"{request.Name}-api"
            };
            await _busService.SenderAsync("handman-create", handman);
        }

        private async Task SendBusUpdateCatalogo(ResourceViewModel request, Guid idRepository)
        {
            var catalogo = new CatalogoViewModel
            {
                UrlRepository = "asdfsadf",
                NameRepository = $"{request.Name}-api",
                IdResource = request.IdResource,
                IdProject = request.IdProject,
                IdRepository = idRepository,
                Name = request.Name
            };

            await _busService.SenderAsync("catalogo-update-resource", catalogo);
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
