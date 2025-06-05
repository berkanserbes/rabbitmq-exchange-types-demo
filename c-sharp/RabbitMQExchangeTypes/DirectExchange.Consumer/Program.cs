using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Direct Exchange - Consumer");
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

var newQueue = await channel.QueueDeclareAsync(
	queue: "direct-exchange-queue",
	durable: false,
	exclusive: false,
	autoDelete: false,
	noWait: false
);

string queueName = newQueue.QueueName;

await channel.QueueBindAsync(
	queue: queueName,
	exchange: "direct-exchange-example",
	routingKey: "direct-routing-key"
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
	autoAck: false,
	consumer: consumer
);

Console.ReadLine();
