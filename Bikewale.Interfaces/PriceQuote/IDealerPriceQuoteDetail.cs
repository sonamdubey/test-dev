using Bikewale.Entities.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.PriceQuote
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 15 March 2016
    /// Description : for Dealer Price quote page functionalies.
    /// </summary>
    public interface IDealerPriceQuoteDetail
    {
        DetailedDealerQuotationEntity GetDealerQuotation(UInt32 cityId, UInt32 versionID, UInt32 dealerId);
    }
}
