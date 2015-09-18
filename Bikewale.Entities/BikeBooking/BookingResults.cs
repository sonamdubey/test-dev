using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeBooking
{
    public enum BookingResults
    {
        None = 0,
        Success = 1,
        TransactionFailure = 2,
        GenericFailure = 3,
        InvalidInformation = 4,
        InvalidPlatform = 5
    }
}
