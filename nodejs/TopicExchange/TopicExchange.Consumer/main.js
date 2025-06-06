import "dotenv/config"
import amqplib from "amqplib"
import readline from "readline/promises";
import { stdin as input, stdout as output } from "node:process";

const AMQP_URI = process.env.CLOUDAMQP_URI;

const connection = await amqplib.connect(AMQP_URI);
const channel = await connection.createChannel();

await channel.assertExchange(
    "topic-exchange-example",   // exchange
    "topic"                     // exchange type
);

const rl = readline.createInterface({input, output});
const topic = await rl.question("Enter topic format: ");

const newQueue = await channel.assertQueue("topic-exchange-queue");

await channel.bindQueue(
    newQueue.queue,             // queue name
    "topic-exchange-example",   // exchange
    topic                       // routing key
)

await channel.consume(newQueue.queue, (msg) => {
    console.log(`Received message: ${msg.content.toString()}`);
});
