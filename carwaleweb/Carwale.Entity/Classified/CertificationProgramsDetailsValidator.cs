using FluentValidation;

namespace Carwale.Entity.Classified
{
    public class CertificationProgramsDetailsValidator : AbstractValidator<CertificationProgramsDetails>
    {
        public CertificationProgramsDetailsValidator()
        {
            RuleFor(details => details.Name).NotEmpty();
            RuleFor(details => details.LogoUrl).NotEmpty().Matches(@"\.(?i)(jpg|png|gif|jpeg)(?:\?[^?]+)?$");
        }
    }
}
