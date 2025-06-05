using RabbitMQ.Client;
using System.Net;
using System.Text;

Console.WriteLine("Direct Exchange - Publisher");
ConnectionFactory factory = new();
factory.Uri = new(<Your CloudAMQP address>);

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
	exchange: "direct-exchange-example",
	type: ExchangeType.Direct,
	durable: false,
	autoDelete: false,
	noWait: false
);

while (true)
{
	Console.Write("Enter a message: ");
	string message = Console.ReadLine()!;
	byte[] byteMessage = Encoding.UTF8.GetBytes(message);

	await channel.BasicPublishAsync(
		exchange: "direct-exchange-example",
		routingKey: "direct-routing-key",
		body: byteMessage
	);
}