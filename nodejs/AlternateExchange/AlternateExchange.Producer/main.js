import "dotenv/config"
import amqplib from "amqplib"

const AMQP_URI = process.env.CLOUDAMQP_URI;

const connection = await amqplib.connect(AMQP_URI);
const channel = await connection.createChannel();

await channel.assertExchange(
    "unroutable-exchange",   // exchange
    "fanout",                // exchange type
    {
        durable: true
    }
);

var exchangeArguments = {
    "alternate-exchange": "unroutable-exchange"
}

await channel.assertExchange(
     "main-exchange",   // exchange
     "direct",          // exchange type
     {
        durable: true,
        arguments: exchangeArguments
     }
)

await channel.assertQueue(
    "unroutable-queue",    // queue name
    {
        durable: true,
        exclusive: false,
        autoDelete: false
    }
);

await channel.bindQueue(
    "unroutable-queue",    // queue
    "unroutable-exchange", // exchange
    ""                     // routing key
)


const message = "Message with invalid routing key";

channel.publish(
    "main-exchange",       // exchange
    "invalid-routing-key", // routing key
    Buffer.from(message)
)

console.log(`Producer send a message: ${message}`);