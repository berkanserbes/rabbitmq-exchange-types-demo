using RabbitMQ.Client;
using System.Net;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new(<Your CloudAMQP Address>);

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
	exchange: "header-exchange-example",
	type: ExchangeType.Headers
);

for (int i = 0; i < 20; i++)
{
	await Task.Delay(200);
	byte[] message = Encoding.UTF8.GetBytes($"Message {i + 1}");

	Console.Write("Enter header value: ");
	var headerValue = Console.ReadLine();

	var basicProperties = new BasicProperties
	{
		Headers = new Dictionary<string, object?>
		{
			["header-key"] = headerValue
		}
	};

	await channel.BasicPublishAsync(
		exchange: "header-exchange-example",
		routingKey: string.Empty,
		body: message,
		basicProperties: basicProperties,
		mandatory: false
	);
}

Console.Read();