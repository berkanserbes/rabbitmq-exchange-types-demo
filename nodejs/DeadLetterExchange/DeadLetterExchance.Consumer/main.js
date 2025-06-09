import "dotenv/config"
import amqplib from "amqplib"

const AMQP_URI = process.env.CLOUDAMQP_URI;

const connection = await amqplib.connect(AMQP_URI);
const channel = await connection.createChannel();

await channel.assertExchange(
    "dlx-exchange",
    "direct"
);

await channel.assertQueue(
    "dlx-queue",
    {
        durable: true
    }
);

await channel.bindQueue(
    "dlx-queue",
    "dlx-exchange",
    "dlx-routing"
);

await channel.consume(
    "dlx-queue",
    (msg) => {
        console.log(`Received message: ${msg.content.toString()}`);
    }
);
