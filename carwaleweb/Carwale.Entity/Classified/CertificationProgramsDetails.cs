using FluentValidation.Attributes;

namespace Carwale.Entity.Classified
{
    [Validator(typeof(CertificationProgramsDetailsValidator))]
    public class CertificationProgramsDetails
    {
        public string Name { get; set; }
        public string LogoUrl { get; set; }
    }
}
