using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Enum
{
    public enum BuyerProcessResponseCode
    {
        Success = 1,
        InvalidUser = 2,
        Unverified = 3,
        AccessForbidden = 4,
        LimitReached = 5,
        IpBlocked = 6,
        CertificationReportSuccess = 7,
        CertificationReportNotAvailable = 8
    }
}
