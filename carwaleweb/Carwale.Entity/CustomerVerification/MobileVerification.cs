using Carwale.Entity.Enum;
using System;

namespace Carwale.Entity.CustomerVerification
{
    public class MobileVerification
    {
        public string Mobile { get; set; }
        public Platform Source { get; set; }
        public string ClientId { get; set; }
        public DateTime VerificationTime { get; set; }
    }
}
