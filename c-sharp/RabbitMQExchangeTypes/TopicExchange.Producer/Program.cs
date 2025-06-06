using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new(<Your CloudAMQP Address>);

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
	exchange: "topic-exchange-example",
	type: ExchangeType.Topic
);

for (int i = 0; i < 20; i++)
{
	await Task.Delay(200);
	byte[] message = Encoding.UTF8.GetBytes($"Message {i + 1}");

	Console.Write("Enter routing key: ");
	string routingKey = Console.ReadLine()!;

	await channel.BasicPublishAsync(
		exchange: "topic-exchange-example",
		routingKey: routingKey,
		body: message
	);
}

Console.Read();