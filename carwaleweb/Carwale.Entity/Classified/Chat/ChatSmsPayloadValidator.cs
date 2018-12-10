
using FluentValidation;

namespace Carwale.Entity.Classified.Chat
{
    public class ChatSmsPayloadValidator : AbstractValidator<ChatSmsPayload>
    {
        public ChatSmsPayloadValidator()
        {
            RuleFor(payload => payload.To).NotEmpty();
            RuleFor(payload => payload.From).NotEmpty();
        }
    }
}
