using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Enum
{
    public enum BillDeskTransactionStatusCode
    {
        Successfull = 0300,
        Failed=2,
        InvalidAuthentication=0399,
        FailedTemporarily = 0002,
        ErrorAtBillDesk = 0001
    }
}
