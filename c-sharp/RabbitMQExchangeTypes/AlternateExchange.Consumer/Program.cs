using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new(<Your CloudAMQP Address>);

IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync +=  (sender, e) =>
{
	string message = Encoding.UTF8.GetString(e.Body.Span);
	Console.WriteLine($"Alternate exchange message received: {message}");

	return Task.CompletedTask;
};

await channel.BasicConsumeAsync(
	queue: "unroutable-queue",
	autoAck: true,
	consumer: consumer
);

Console.Read();