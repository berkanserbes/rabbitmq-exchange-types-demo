import 'dotenv/config';
import amqp from "amqplib"
import readline from 'readline/promises';
import { stdin as input, stdout as output } from 'node:process';

const AMQP_URI = process.env.CLOUDAMQP_URI;

const connection = await amqp.connect(AMQP_URI);
const channel = await connection.createChannel();

await channel.assertExchange(
    "direct-exchange-example",   // exchange 
    "direct"                     // exchange type
);

const rl = readline.createInterface({ input, output });

while(true) {
    const message = await rl.question("Enter a message: ");
    
    channel.publish(
        "direct-exchange-example",   // exchange
        "direct-routing-key",        // routing key
        Buffer.from(message)         // content
    );
}