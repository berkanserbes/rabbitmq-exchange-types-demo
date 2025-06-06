using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new(<Your CloudAMQP Address>);

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
	exchange: "topic-exchange-example",
	type: ExchangeType.Topic
);

Console.Write("Enter topic format: ");
string topic = Console.ReadLine()!;

var queue = await channel.QueueDeclareAsync();
string queueName = queue.QueueName;

await channel.QueueBindAsync(
	queue: queueName,
	exchange: "topic-exchange-example",
	routingKey: topic
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
	consumer: consumer,
	autoAck: true
);

Console.Read();