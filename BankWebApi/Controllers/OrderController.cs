using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Text.Json;

namespace BankWebApi.Controllers;

[ApiController]
[Route("api/v1/order")]
public class OrderController : ControllerBase
{
    private const string c_QueueName = "messages";
    private readonly QueueClient _queueClient;

    public OrderController(QueueServiceClient serviceClient)
    {
        _queueClient = serviceClient.GetQueueClient(c_QueueName);
    }

    [HttpPost]
    public async Task Add(Order order)
    {
        var sendTask = _queueClient.SendMessageAsync(JsonSerializer.Serialize(order), null, TimeSpan.FromDays(1));

        await sendTask;
    }
}