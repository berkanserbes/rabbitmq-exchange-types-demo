import "dotenv/config";
import amqplib from "amqplib";
import readline from 'readline/promises';
import { stdin as input, stdout as output } from 'node:process';

const AMQP_URI = process.env.CLOUDAMQP_URI;

const connection = await amqplib.connect(AMQP_URI);
const channel = await connection.createChannel();

await channel.assertExchange(
    "topic-exchange-example",   // exchange 
    "topic"                     // exchange type
);

for (let i = 0; i < 20; i++) {
    const msg = `Message ${i + 1}`;
    const rl = readline.createInterface({input, output});
    const routingKey = await rl.question("Enter routing key: ");
    rl.close();

    channel.publish(
        "topic-exchange-example", // exchange
        routingKey,               // routing key
        Buffer.from(msg)          // content
    );
}

