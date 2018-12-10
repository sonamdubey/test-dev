using Carwale.Entity.Enum;
using System;

namespace Carwale.Entity.Vernam
{
    public class RequestData
    {
        public VerificationType VerificationType { get; set; }
        public VerificationMedium VerificationMedium { get; set; }
        public string VerificationValue { get; set; }
        public DateTime? VerificationExpiry { get; set; }
        public int Validity { get; set; }
        public int OtpLength { get; set; }
        public Application ApplicationId { get; set; }
        public Platform PlatformId { get; set; }
        public SourceModule SourceModule { get; set; }
        public string ClientIp { get; set; }
        public string DeviceId { get; set; }
        public string OtpCode { get; set; }
        public string VendorResponse { get; set; }
    }
}
