# RabbitMQ Exchange Types Demo

This repository contains implementations of various RabbitMQ exchange types, demonstrating different message routing patterns. The project includes both C# and Node.js implementations for different exchange types.

## 📚 Navigation

|   Exchange Type   | Node.js Producer | Node.js Consumer | C# Producer | C# Consumer |
|---------------|------------------|------------------|-------------|-------------|
| Direct Exchange | [Producer](nodejs/DirectExchange/DirectExhange.Producer/main.js) | [Consumer](nodejs/DirectExchange/DirectExchange.Consumer/main.js) | [Producer](c-sharp/RabbitMQExchangeTypes/DirectExchange.Producer/Program.cs) | [Consumer](c-sharp/RabbitMQExchangeTypes/DirectExchange.Consumer/Program.cs) |
| Fanout Exchange | [Producer](nodejs/FanoutExchange/FanoutExchange.Producer/main.js) | [Consumer](nodejs/FanoutExchange/FanoutExchange.Consumer/main.js) | [Producer](c-sharp/RabbitMQExchangeTypes/FanoutExchange.Producer/Program.cs) | [Consumer](c-sharp/RabbitMQExchangeTypes/FanoutExchange.Consumer/Program.cs) |
| Topic Exchange | [Producer](nodejs/TopicExchange/TopicExchange.Producer/main.js) | [Consumer](nodejs/TopicExchange/TopicExchange.Consumer/main.js) | [Producer](c-sharp/RabbitMQExchangeTypes/TopicExchange.Producer/Program.cs) | [Consumer](c-sharp/RabbitMQExchangeTypes/TopicExchange.Consumer/Program.cs) |
| Headers Exchange | [Producer](nodejs/HeaderExchange/HeaderExchange.Producer/main.js) | [Consumer](nodejs/HeaderExchange/HeaderExchange.Consumer/main.js) | [Producer](c-sharp/RabbitMQExchangeTypes/HeaderExchange.Producer/Program.cs) | [Consumer](c-sharp/RabbitMQExchangeTypes/HeaderExchange.Consumer/Program.cs) |
| Dead Letter Exchange | [Producer](nodejs/DeadLetterExchange/DeadLetterExchange.Producer/main.js) | [Consumer](nodejs/DeadLetterExchange/DeadLetterExchance.Consumer/main.js) | [Producer](c-sharp/RabbitMQExchangeTypes/DeadLetterExchange.Producer/Program.cs) | [Consumer](c-sharp/RabbitMQExchangeTypes/DeadLetterExchange.Consumer/Program.cs) |
| Alternate Exchange | [Producer](nodejs/AlternateExchange/AlternateExchange.Producer/main.js) | [Consumer](nodejs/AlternateExchange/AlternateExchange.Consumer/main.js) | [Producer](c-sharp/RabbitMQExchangeTypes/AlternateExchange.Producer/Program.cs) | [Consumer](c-sharp/RabbitMQExchangeTypes/AlternateExchange.Consumer/Program.cs) |

## Project Structure

The project is organized into two main directories:

### C# Implementations
- Located in the `c-sharp` directory
- Contains implementations of different RabbitMQ exchange types in C#

### Node.js Implementations
- Located in the `nodejs` directory
- Contains implementations of different RabbitMQ exchange types in Node.js

## Exchange Types Demonstrated

The project demonstrates several RabbitMQ exchange types:

1. **Direct Exchange**
   - Routes messages to queues based on exact matching of the routing key

2. **Fanout Exchange**
   - Broadcasts messages to all bound queues regardless of the routing key

3. **Topic Exchange**
   - Routes messages based on pattern matching of the routing key

4. **Headers Exchange**
   - Routes messages based on message headers rather than routing keys

5. **Dead Letter Exchange**
   - Routes messages to a dead letter queue when they cannot be delivered to the intended queue

6. **Alternate Exchange**
   - Routes messages to an alternate exchange when they cannot be delivered to the intended exchange

## Getting Started

### Prerequisites
- Cloud RabbitMQ server
- Node.js (for Node.js implementations)
- .NET SDK (for C# implementations)

### Running the Examples
Each exchange type implementation contains both producer and consumer components. The specific setup and running instructions can be found in each implementation directory.

## Implementation Details

### Node.js Implementation
- Uses the `amqplib` package for RabbitMQ communication
- Each exchange type has its own producer and consumer implementation
- Configuration is handled through environment variables

### C# Implementation
- Uses the RabbitMQ .NET client library
- Implements different exchange types with proper error handling
- Includes examples of message publishing and consuming

## Contributing
Feel free to submit issues and enhancement requests!

## License
This project is licensed under the terms of the MIT license.