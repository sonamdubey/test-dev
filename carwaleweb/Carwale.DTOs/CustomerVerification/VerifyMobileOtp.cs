using Carwale.DTOs.CustomerVerification.Validators;
using Carwale.Entity.Vernam;
using FluentValidation.Attributes;

namespace Carwale.DTOs.CustomerVerification
{
    [Validator(typeof(VerifyMobileOtpDtoValidator))]
    public class VerifyMobileOtpDto
    {
        public string MobileNumber { get; set; }
        public string OtpCode { get; set; }
        public SourceModule SourceModule { get; set; }
    }
}
