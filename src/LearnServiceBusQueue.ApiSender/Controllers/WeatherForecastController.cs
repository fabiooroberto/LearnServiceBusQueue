using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using LearnServiceBusQueue.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net;
using System.Text.Json;

namespace LearnServiceBusQueue.ApiSender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private ServiceBusSender _sender;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusAdministrationClient _adminClient;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            ServiceBusClient client,
            ServiceBusAdministrationClient adminClient)
        {
            _logger = logger;
            _client = client;
            _adminClient = adminClient;
        }

        [HttpGet(Name = "GetWeatherForecast/{queue}")]
        public async Task<IEnumerable<WeatherForecast>> Get(string queue)
        {
            var credential = new DefaultAzureCredential();
            
            var proxy = new WebProxy
            {
                BypassProxyOnLocal = false,
                UseDefaultCredentials = true,
                Credentials = null
            };

            // Now create a client handler which uses that proxy
            var httpClientHandler = new HttpClientHandler
            {
                Proxy = proxy,
            };

            var httpClient = new HttpClient(handler: httpClientHandler);

            // Set the base URL for the Azure DevOps REST API
            httpClient.BaseAddress = new Uri("https://dev.azure.com/wizsolucoes/");

            // List the projects in the organization
            HttpResponseMessage response = await httpClient.GetAsync("/wizsolucoes/_apis/projects");
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            //JArray projects = JArray.Parse(responseContent);
            //foreach (JObject project in projects)
            //{
            //    Console.WriteLine(project["name"]);
            //}

            //if (!await _adminClient.QueueExistsAsync(queue))
            //    await _adminClient.CreateQueueAsync(queue);

            //_sender = _client.CreateSender(queue);
            //await _sender.SendMessageAsync(new ServiceBusMessage(JsonSerializer.Serialize(PocSampleQueue.Mock())));

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("Receiver/{queue}")]
        public async Task<ActionResult> Receive(string queue)
        {
            var processor = _client.CreateProcessor(queue, new ServiceBusProcessorOptions());
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErroHandler;
            await processor.StartProcessingAsync();
            Console.WriteLine("Wait for a minute and then press any key to end the processing");
            Console.ReadKey();

            // stop processing 
            Console.WriteLine("\nStopping the receiver...");
            await processor.StopProcessingAsync();
            Console.WriteLine("Stopped receiving messages");
            return Ok();
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            //var body = JsonSerializer.Deserialize<object>(args.Message.Body);
            Console.WriteLine($"Received: {args.Message.Body}");

            await args.CompleteMessageAsync(args.Message);
        }
        Task ErroHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }

    public class TesteLorem
    {
        public string Nome { get; set; }
    }
}