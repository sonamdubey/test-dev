﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.PriceQuote;

namespace Bikewale.Interfaces.PriceQuote
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Interface for the price quote.
    /// </summary>
    public interface IPriceQuote
    {
        ulong RegisterPriceQuote(PriceQuoteParametersEntity pqParams);
        BikeQuotationEntity GetPriceQuoteById(ulong pqId);
        BikeQuotationEntity GetPriceQuote(PriceQuoteParametersEntity pqParams);
        List<OtherVersionInfoEntity> GetOtherVersionsPrices(ulong pqId);
    }
}
