using Azure.Messaging.ServiceBus;
using Azure.Identity;
using System.Text.Json;

ServiceBusClient client;

ServiceBusSender sender;

const int numOfMessages = 3;

var clientOptions = new ServiceBusClientOptions
{
    TransportType = ServiceBusTransportType.AmqpWebSockets
};

client = new ServiceBusClient(
    "event-bus-wx1.servicebus.windows.net",
    new DefaultAzureCredential(),
    clientOptions);

sender = client.CreateSender("queue-gustavera");

// create a batch 
using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

for (int i = 0; i < numOfMessages; i++)
{
    var body = new Janduy
    {
        Nome = "Lorem",
        IdRepository = Guid.NewGuid().ToString()
    };
    if (!messageBatch.TryAddMessage(new ServiceBusMessage(JsonSerializer.Serialize(body))))
    {
        // if it is too large for the batch
        throw new Exception($"The message {i} is too large to fit in the batch.");
    }
}

try
{
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
}
finally
{
    await sender.DisposeAsync();
    await client.DisposeAsync();
}

Console.WriteLine("Press any key to end the application");
Console.ReadKey();

class Janduy
{
    public string Nome { get; set; }
    public string IdRepository { get; set; }
}