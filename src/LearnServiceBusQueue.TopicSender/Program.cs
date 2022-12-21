using Azure.Messaging.ServiceBus;
using Azure.Identity;

ServiceBusClient client;

ServiceBusSender sender;

const int numOfMessages = 5;

client = new ServiceBusClient(
    "event-bus-wx1.servicebus.windows.net",
    new DefaultAzureCredential());

sender = client.CreateSender(queueOrTopicName: "learn-topic");

// create a batch 
using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

for (int i = 0; i < numOfMessages; i++)
{
    if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
    {
        // if it is too large for the batch
        throw new Exception($"The message {i} is too large to fit in the batch.");
    }
}

try
{
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine($"A batch of {numOfMessages} messages has been published to the topic.");
}
finally
{
    await sender.DisposeAsync();
    await client.DisposeAsync();
}

Console.WriteLine("Press any key to end the application");
Console.ReadKey();