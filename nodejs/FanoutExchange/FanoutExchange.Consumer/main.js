import "dotenv/config";
import amqplib from "amqplib";
import readline from "readline/promises";
import { stdin as input, stdout as output } from "node:process";
import { read } from "node:fs";

const AMQP_URI = process.env.CLOUDAMQP_URI;

const connection = await amqplib.connect(AMQP_URI);
const channel = await connection.createChannel();

await channel.assertExchange(
  "fanout-exchange-example", // exchange
  "fanout" // exchange type
);

const rl = readline.createInterface({ input, output });
const queueName = await rl.question("Kuyruk adını giriniz: ");
rl.close();

const newQueue = await channel.assertQueue(queueName);

await channel.bindQueue(
  newQueue.queue, // queue name
  "fanout-exchange-example", // exchange
  "" // routing key
);

await channel.consume(newQueue.queue, (msg) => {
  console.log(`Received message: ${msg.content.toString()}`);
});
