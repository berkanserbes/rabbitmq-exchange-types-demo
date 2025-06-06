using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new(<Your CloudAMQP Address>);

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
	exchange: "fanout-exchange-example",
	type: ExchangeType.Fanout
);

for(int i = 0; i < 10; i++)
{
	await Task.Delay(200);
	byte[] byteMessage = Encoding.UTF8.GetBytes($"Message {i + 1}");

	await channel.BasicPublishAsync(
		exchange: "fanout-exchange-example",
		routingKey: string.Empty,
		body: byteMessage
	);
}

Console.ReadLine();