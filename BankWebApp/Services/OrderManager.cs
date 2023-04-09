using Azure.Storage.Queues;
using System.Text.Json;

namespace BankWebApp.Services;

public class OrderManager
{
	private readonly QueueClient _queueClient;
	private const string c_QueueName = "messages";

	public OrderManager(QueueServiceClient serviceClient)
	{
		_queueClient = serviceClient.GetQueueClient(c_QueueName);

    }

	public async Task DequeueMessage()
	{
        var response = await _queueClient.ReceiveMessageAsync();

		_queueClient.DeleteMessage(response.Value.MessageId, response.Value.PopReceipt);

    }

	public async Task<Order?> Peek()
	{
		var response = await _queueClient.PeekMessageAsync();

		if (response.Value == null)
		{
			return null;
		}

		return JsonSerializer.Deserialize<Order>(response.Value.Body.ToString());

    }

	public async Task<List<Order>> GetAll()
	{
        var result = new List<Order>();

        while (true)
		{
            var response = await _queueClient.ReceiveMessageAsync();

			try
			{
				var value = response.Value;

				if (value == null)
				{
					break;
				}

                result.Add(JsonSerializer.Deserialize<Order>(value.Body.ToString())!);
            }
			catch
			{
				break;
			}
        }

		return result;
	}
}
