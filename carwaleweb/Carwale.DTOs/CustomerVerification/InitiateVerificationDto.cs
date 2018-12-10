using Carwale.Entity.Vernam;

namespace Carwale.DTOs.CustomerVerification
{
    public class InitiateVerificationDto
    {
        public string Mobile { get; set; }
        public SourceModule SourceModule { get; set; }
        public MobileVerificationByType MobileVerificationByType { get; set; }
        public int ValidityInMinutes { get; set; }
        public int OtpLength { get; set; }
    }
}
