using Bikewale.Entities.BikeBooking;
using Carwale.Entity.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.BikeBooking
{
    public interface IBikeBooking
    {
        BookingResults DoBooking(TransactionDetails entity, string sourceType);
    }
}
