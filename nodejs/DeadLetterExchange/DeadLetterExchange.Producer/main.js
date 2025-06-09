import "dotenv/config"
import amqplib from "amqplib"

const AMQP_URI = process.env.CLOUDAMQP_URI;

const connection = await amqplib.connect(AMQP_URI);
const channel = await connection.createChannel();

await channel.assertExchange(
    "main-exchange",
    "direct"
);

await channel.assertExchange(
    "dlx-exchange",
    "direct"
);

await channel.assertQueue(
    "main-queue",
    {
        durable: true,
        arguments: {
            "x-dead-letter-exchange": "dlx-exchange",
            "x-dead-letter-routing-key": "dlx-routing",
            "x-message-ttl": 5000
        }
    }
);

await channel.bindQueue(
    "main-queue",
    "main-exchange",
    "main-routing-key"
);

for (let i = 0; i < 10; i++) {
    const message = `Message ${i + 1}`;
    channel.publish(
        "main-exchange",
        "main-routing-key",
        Buffer.from(message)
    );

    console.log(`Sent: ${message}`);
    await new Promise(resolve => setTimeout(resolve, 1000));
}