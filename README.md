# Reminder.Platform.Contracts

Biblioteca compartilhada de contratos de eventos para a `Reminder Platform`.

## Objetivo

Este projeto centraliza os contratos usados entre microserviços em uma arquitetura orientada a eventos com RabbitMQ.

Serviços que consomem este pacote:
- `Reminder Orchestrator Service` (publisher)
- `Notification Email Service` (consumer)
- `Notification SMS Service` (consumer)
- `Notification WhatsApp Service` (consumer)

## Stack

- `.NET 10` (`net10.0`)
- `System.Text.Json`
- `FluentValidation`

## Estrutura

- `Abstractions/`
  - `IEvent.cs`
- `Events/v1/`
  - `ReminderTriggeredEvent.cs`
- `Validation/v1/`
  - `ReminderTriggeredEventValidator.cs`

## Versionamento de eventos

O versionamento é feito por namespace e pasta:
- Versão atual: `Reminder.Platform.Contracts.Events.v1`
- Futuras versões: `Reminder.Platform.Contracts.Events.v2`

Recomendação para evolução:
1. Não quebrar contratos existentes em `v1`.
2. Para mudanças breaking, criar novo evento em `v2`.
3. Manter consumidores antigos apontando para `v1` até migração completa.

## Exemplo de serialização com System.Text.Json

```csharp
using System.Text.Json;
using Reminder.Platform.Contracts.Events.v1;

var @event = new ReminderTriggeredEvent
{
    ReminderId = Guid.NewGuid(),
    UserId = Guid.NewGuid(),
    Message = "Seu lembrete foi disparado",
    Timestamp = DateTime.UtcNow
};

var json = JsonSerializer.Serialize(@event);
var deserialized = JsonSerializer.Deserialize<ReminderTriggeredEvent>(json);
```

## Exemplo MassTransit - Publish (Orchestrator)

```csharp
using MassTransit;
using Reminder.Platform.Contracts.Events.v1;

public class ReminderPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ReminderPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync(Guid reminderId, Guid userId, string message)
    {
        return _publishEndpoint.Publish(new ReminderTriggeredEvent
        {
            ReminderId = reminderId,
            UserId = userId,
            Message = message,
            Timestamp = DateTime.UtcNow
        });
    }
}
```

## Exemplo MassTransit - Consume (Notificação)

```csharp
using FluentValidation;
using MassTransit;
using Reminder.Platform.Contracts.Events.v1;
using Reminder.Platform.Contracts.Validation.v1;

public class ReminderTriggeredConsumer : IConsumer<ReminderTriggeredEvent>
{
    private readonly IValidator<ReminderTriggeredEvent> _validator = new ReminderTriggeredEventValidator();

    public async Task Consume(ConsumeContext<ReminderTriggeredEvent> context)
    {
        var validation = await _validator.ValidateAsync(context.Message, context.CancellationToken);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation.Errors);
        }

        // processar envio de notificação (email, sms, whatsapp)
    }
}
```

## Uso em outros serviços

1. Referencie este projeto/pacote no microserviço.
2. Use os tipos de `Events.v1` para publicar/consumir.
3. Aplique os validadores de `Validation.v1` no boundary do consumer.
