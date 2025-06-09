using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new(<Your CloudAMQP Address>);

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
	exchange: "main-exchange",
	type: ExchangeType.Direct
);

await channel.ExchangeDeclareAsync(
	exchange: "dlx-exchange",
	type: ExchangeType.Direct
);

await channel.QueueDeclareAsync(
	queue: "main-queue",
	durable: true,
	arguments: new Dictionary<string, object?>
	{
		{ "x-dead-letter-exchange", "dlx-exchange" },
		{ "x-dead-letter-routing-key", "dlx-routing" },
		{ "x-message-ttl", 5000 }
    }
);

await channel.QueueBindAsync(
	queue: "main-queue",
	exchange: "main-exchange",
	routingKey: "main-routing-key"
);

for (int i = 0; i < 10; i++)
{
	
	var message = $"Message {i + 1}";
	var body = Encoding.UTF8.GetBytes(message);

	await channel.BasicPublishAsync(
		exchange: "main-exchange",
		routingKey: "main-routing-key",
		body: body
	);

	Console.WriteLine($"Sent: {message}");
	await Task.Delay(1000);
}

Console.Read();