using Reminder.Platform.Contracts.Abstractions;

namespace Reminder.Platform.Contracts.Events.v1;

public class ReminderTriggeredEvent : IEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    public Guid ReminderId { get; set; }
    public Guid UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
