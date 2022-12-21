using Azure;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using LearnServiceBusQueue.ApiSender.Handler;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAzureClients(clients =>
//{
//    clients.AddServiceBusClient(builder.Configuration.GetConnectionString("ServiceBus"));
//    clients.UseCredential(tokenCredential: new DefaultAzureCredential());
//});

builder.Services.AddSingleton((s) => {
    return new ServiceBusClient(builder.Configuration.GetConnectionString("ServiceBus"), new DefaultAzureCredential(), new ServiceBusClientOptions() { TransportType = ServiceBusTransportType.AmqpWebSockets });
});
builder.Services.AddSingleton((s) =>
{
    return new ServiceBusAdministrationClient(builder.Configuration.GetConnectionString("ServiceBus"), new DefaultAzureCredential());
});

builder.Services.AddControllers();
builder.Services.AddSingleton<IServiceBusConsumer, ServiceBusConsumer>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var bus = app.Services.GetRequiredService<IServiceBusConsumer>();
bus.RegisterOnMessageHandlerAndReceiveMessages().GetAwaiter().GetResult();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
