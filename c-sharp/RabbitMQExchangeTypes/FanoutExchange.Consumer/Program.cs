using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new(<Your CloudAMQP Address>);

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
	exchange: "fanout-exchange-example",
	type: ExchangeType.Fanout
);

Console.Write("Kuyruk adını giriniz: ");
string queueName = Console.ReadLine()!;

await channel.QueueDeclareAsync(
	queue: queueName,
	exclusive: false
);

await channel.QueueBindAsync(
	queue: queueName,
	exchange: "fanout-exchange-example",
	routingKey: string.Empty
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