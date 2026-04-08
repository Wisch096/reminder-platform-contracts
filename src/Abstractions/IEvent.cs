namespace Reminder.Platform.Contracts.Abstractions;

public interface IEvent
{
    Guid Id { get; }
    DateTime OccurredAt { get; }
}
