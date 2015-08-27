using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Entities.BikeBooking;
using Carwale.Entity.PaymentGateway;

namespace Bikewale.BAL.BikeBooking
{
    public abstract class AbstractBikeBooking : IBikeBooking
    {
        public BookingResults DoBooking(TransactionDetails entity, string sourceType)
        {
            return DoBookingEx(entity, sourceType);
        }

        protected abstract BookingResults DoBookingEx(TransactionDetails entity, string sourceType);
    }
}
