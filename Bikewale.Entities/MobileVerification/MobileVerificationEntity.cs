using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.MobileVerification
{
    public class MobileVerificationEntity
    {
        public ulong CvId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobile { get; set; }
        public string CWICode { get; set; }
        public string CUICode { get; set; }
        public bool IsMobileVerified { get; set; }
    }   // class
}   // namespace
