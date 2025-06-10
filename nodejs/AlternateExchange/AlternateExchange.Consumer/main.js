import "dotenv/config"
import amqplib from "amqplib"

const AMQP_URI = process.env.CLOUDAMQP_URI;

const connection = await amqplib.connect(AMQP_URI);
const channel =await connection.createChannel();

await channel.consume(
    "unroutable-queue",
    (msg) => {
        console.log(`Alternate exchange received message: ${msg.content.toString()}`);
    },
    {
        autoAck: true
    }
)