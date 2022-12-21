using System.Text.Json;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.ServiceBus;

ServiceBusClient client;

ServiceBusProcessor processor;

var clientOptions = new ServiceBusClientOptions()
{
    TransportType = ServiceBusTransportType.AmqpWebSockets
};

client = new ServiceBusClient(
    "event-bus-wx1.servicebus.windows.net",
    new DefaultAzureCredential(),
    clientOptions);

processor = client.CreateProcessor("queue-gustavera", new ServiceBusProcessorOptions());

try
{
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErroHandler;


    await processor.StartProcessingAsync();

    Console.WriteLine("Wait for a minute and then press any key to end the processing");
    Console.ReadKey();

    // stop processing 
    Console.WriteLine("\nStopping the receiver...");
    await processor.StopProcessingAsync();
    Console.WriteLine("Stopped receiving messages");
}
finally
{
    await processor.DisposeAsync();
    await client.DisposeAsync();
}

async Task MessageHandler(ProcessMessageEventArgs args)
{
    var body = JsonSerializer.Deserialize<Gustavera>(args.Message.Body);
    Console.WriteLine($"Received: {body.Nome}");

    await args.CompleteMessageAsync(args.Message);
}

Task ErroHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}


class Gustavera
{
    public string UserEmail { get; set; }
    public string Nome { get; set; }
    public string IdRepository { get; set; }
}