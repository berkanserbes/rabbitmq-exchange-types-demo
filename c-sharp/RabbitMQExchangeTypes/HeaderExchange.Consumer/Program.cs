using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new(<Your CloudAMQP Address>);

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
	exchange: "header-exchange-example",
	type: ExchangeType.Headers
);

Console.Write("Enter header value: ");
var headerValue = Console.ReadLine();

var queue = await channel.QueueDeclareAsync();
string queueName = queue.QueueName;

await channel.QueueBindAsync(
	queue: queueName,
	exchange: "header-exchange-example",
	routingKey: string.Empty,
	arguments: new Dictionary<string, object?>
	{
		{ "x-match", "all" }, // or "any" for any match
		{ "header-key", headerValue }
	}
);

AsyncEventingBasicConsumer consumer = new(channel);

consumer.ReceivedAsync += (sender, e) =>
{
	string message = Encoding.UTF8.GetString(e.Body.Span);
	Console.WriteLine($"Received message: {message}");
	return Task.CompletedTask;
};

await channel.BasicConsumeAsync(
	queue: queueName,
	autoAck: true,
	consumer: consumer
);

Console.Read();