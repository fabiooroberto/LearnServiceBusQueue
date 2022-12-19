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

processor = client.CreateProcessor("learn-queue", new ServiceBusProcessorOptions());

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
    string body = args.Message.Body.ToString();
    Console.WriteLine($"Received: {body}");

    await args.CompleteMessageAsync(args.Message);
}

Task ErroHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}
