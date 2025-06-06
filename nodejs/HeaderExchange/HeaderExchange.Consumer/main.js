import "dotenv/config"
import amqplib from "amqplib"
import readline from "readline/promises";
import { stdin as input, stdout as output } from "node:process";

const AMQP_URI = process.env.CLOUDAMQP_URI;

const connection = await amqplib.connect(AMQP_URI);
const channel = await connection.createChannel();

await channel.assertExchange(
    "header-exchange-example",    // exchange 
    "headers"                     // exchange type
);

const rl = readline.createInterface({ input, output });
const headerValue = await rl.question("Enter header value: ");
rl.close();

const newQueue = await channel.assertQueue("header-exchange-queue");
const basicProperties = {
    headers: {
        "header-key": headerValue,
        "x-match": "all"
    }
}

await channel.bindQueue(
    newQueue.queue,             // queue name
    "header-exchange-example",  // exchange
    "",                         // routing key
    basicProperties.headers     // args
);

await channel.consume(newQueue.queue, (msg) => {
    console.log(`Received message: ${msg.content.toString()} ${msg.properties.headers}`);
});
