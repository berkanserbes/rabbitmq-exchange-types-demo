using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new(<Your CloudAMQP Address>);

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
	exchange: "dlx-exchange",
	type: ExchangeType.Direct
);

await channel.QueueDeclareAsync(
	queue: "dlx-queue",
	durable: true
);

await channel.QueueBindAsync(
	queue: "dlx-queue",
	exchange: "dlx-exchange",
	routingKey: "dlx-routing"
);

AsyncEventingBasicConsumer consumer = new(channel);

consumer.ReceivedAsync += (sender, e) =>
{
	string message = Encoding.UTF8.GetString(e.Body.Span);
	Console.WriteLine($"DLX received dead-lettered message: {message}");
	return Task.CompletedTask;
};

await channel.BasicConsumeAsync(
	queue: "dlx-queue",
	autoAck: true,
	consumer: consumer
);

Console.Read();