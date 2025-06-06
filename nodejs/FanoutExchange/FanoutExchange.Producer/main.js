import "dotenv/config";
import amqplib from "amqplib";

const AMQP_URI = process.env.CLOUDAMQP_URI;

const connection = await amqplib.connect(AMQP_URI);
const channel = await connection.createChannel();

await channel.assertExchange(
  "fanout-exchange-example", // exchange
  "fanout" // exchange type
);

for (let i = 0; i < 10; i++) {
  const msg = `Message ${i + 1}`;

  channel.publish(
    "fanout-exchange-example", // exchange
    "", // routing key
    Buffer.from(msg) // content
  );
}
