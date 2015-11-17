using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.PriceQuote
{
    enum PriceQuoteStates
    {
        LeadSubmitted = 1,
        InitiatedBooking = 2,
        InitiatedPayment = 3,
        SuccessfulPayment = 4
    }
}
