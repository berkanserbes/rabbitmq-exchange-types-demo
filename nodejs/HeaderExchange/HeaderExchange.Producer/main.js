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

for (let i = 0; i < 20; i++) {
    const message = `Message ${i + 1}`;

    const rl = readline.createInterface({ input, output });
    const headerValue = await rl.question("Enter header value: ");
    rl.close();
    const basicProperties = {
        headers: {
            "header-key": headerValue
        }
    }
    
    channel.publish(
        "header-exchange-example",   // exchange
        "",                          // routing key
        Buffer.from(message),        // content
        basicProperties              // options
    );
}