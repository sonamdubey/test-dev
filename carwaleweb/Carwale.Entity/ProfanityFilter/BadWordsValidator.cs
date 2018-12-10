using Carwale.Utility;
using FluentValidation;
using System.Linq;

namespace Carwale.Entity.ProfanityFilter
{
    public class BadWordsValidator : AbstractValidator<BadWords>
    {
        public BadWordsValidator()
        {
            RuleFor(x => x.Words).NotEmpty().ShouldNotContainMoreThan(100, "words")
                .Must(x => !x.Any(b => b.Length < 2 || b.Length > 20)).WithMessage("Word length should be between 2 to 20.");
        }
    }
}
