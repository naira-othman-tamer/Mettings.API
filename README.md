Show more
7:51 PM
markdown
# 🐇 MassTransit + RabbitMQ Messaging Project

A .NET 8 project demonstrating messaging patterns using MassTransit with RabbitMQ, running via Docker.

## 🏗️ Project Structure
Solution
├── Mettings.API # Producer – sends & publishes messages
├── Mettings.Worker # Consumer – processes messages
└── SharedMessages # Shared message contracts (interfaces)


## 📦 Patterns Implemented

### Send (Point-to-Point)
Sends a message directly to a specific queue; only one consumer receives it.

```csharp
var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:notify-recipients"));
await endpoint.Send(new { ... });
```

### Publish (Fan-out)
Broadcasts a message to all consumers subscribed to that message type.

```csharp
await _publishEndpoint.Publish(new { ... });
```

## 🚀 How to Run

1. Start RabbitMQ using Docker:
```bash
   docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```
2. Run **Mettings.API** (producer)
3. Run **Mettings.Worker** (consumer)
4. Send a test request:
```http
   POST http://localhost:5167/Meeting/ScheduleMeeting
   Content-Type: application/json

   {
     "participantEmails": ["test@gmail.com"],
     "scheduledTime": "2025-01-01T12:00:00Z"
   }
```

## 🛠️ Tech Stack

- .NET 8 / ASP.NET Core
- MassTransit 8
- RabbitMQ (Docker)
- Worker Service (BackgroundService)
