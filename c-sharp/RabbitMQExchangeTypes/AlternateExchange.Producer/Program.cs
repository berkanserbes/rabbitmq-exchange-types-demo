using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new(<Your CloudAMQP Address>);

IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

// Declare an alternate exchange to handle unroutable messages
await channel.ExchangeDeclareAsync(
   exchange: "unroutable-exchange",
   type: ExchangeType.Fanout,
   durable: true
);

var exchangeArguments = new Dictionary<string, object?>
{
	{ "alternate-exchange", "unroutable-exchange" }
};

// Declare the main exchange with the alternate exchange set
await channel.ExchangeDeclareAsync(
   exchange: "main-exchange",
   type: ExchangeType.Direct,
   durable: true,
   arguments: exchangeArguments
);

await channel.QueueDeclareAsync(
	queue: "unroutable-queue",
	durable: true,
	exclusive: false,
	autoDelete: false
);

await channel.QueueBindAsync(
	queue: "unroutable-queue",
	exchange: "unroutable-exchange",
	routingKey: string.Empty
);

string message = "Message with invalid routing key";
byte[] body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(
	exchange: "main-exchange",
	routingKey: "invalid-routing-key", // This routing key does not match any queue
	body: body
);

Console.WriteLine($"Producer send a message: {message}");
Console.Read();