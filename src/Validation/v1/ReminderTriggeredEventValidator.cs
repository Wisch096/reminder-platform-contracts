using FluentValidation;
using Reminder.Platform.Contracts.Events.v1;

namespace Reminder.Platform.Contracts.Validation.v1;

public class ReminderTriggeredEventValidator : AbstractValidator<ReminderTriggeredEvent>
{
    public ReminderTriggeredEventValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty);
    }
}
