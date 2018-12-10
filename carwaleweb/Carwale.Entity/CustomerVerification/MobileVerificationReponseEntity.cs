using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CustomerVerification
{
    public class MobileVerificationReponseEntity
    {
        public bool IsMobileVerified { get; set; }

        public string TollFreeNumber { get; set; }
        public bool IsVerificationCodeSent { get; set; }
    }
}
