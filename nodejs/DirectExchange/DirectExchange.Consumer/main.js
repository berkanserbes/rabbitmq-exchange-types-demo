import 'dotenv/config';
import amqp from "amqplib"

const AMQP_URI = process.env.CLOUDAMQP_URI;

const connection = await amqp.connect(AMQP_URI);
const channel = await connection.createChannel();

await channel.assertExchange(
    "direct-exchange-example",   // exchange 
    "direct"                     // exchange type
);

var newQueue = await  channel.assertQueue(
    "direct-exchange-queue"     // queue name
);

await channel.bindQueue(
    newQueue.queue,             // queue name
    "direct-exchange-example",  // exchange
    "direct-routing-key"        // routing key
);

await channel.consume(
    newQueue.queue,
    (msg) => {
        console.log(`Received message: ${msg.content.toString()}`);
    }
);