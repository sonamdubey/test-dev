using Bikewale.Entities.PriceQuote;
using System;
using System.Collections.Generic;

namespace Bikewale.Interfaces.PriceQuote
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Interface for the price quote.
    /// Modified By :   Sumit Kate
    /// Date        :   16 Oct 2015
    /// Description :   Added new method UpdatePriceQuote to update the price quote details
    /// Modified By :   Vivek Gupta on 20-05-2016
    /// Description :   Added new reference of method FetchPriceQuoteOfTopCities to fetch top city prices
    /// </summary>
    public interface IPriceQuote
    {
        ulong RegisterPriceQuote(PriceQuoteParametersEntity pqParams);
        BikeQuotationEntity GetPriceQuoteById(ulong pqId);
        BikeQuotationEntity GetPriceQuote(PriceQuoteParametersEntity pqParams);
        List<OtherVersionInfoEntity> GetOtherVersionsPrices(ulong pqId);
        bool UpdatePriceQuote(UInt32 pqId, PriceQuoteParametersEntity pqParams);
        bool SaveBookingState(UInt32 pqId, PriceQuoteStates state);
        PriceQuoteParametersEntity FetchPriceQuoteDetailsById(UInt64 pqId);
        IEnumerable<PriceQuoteOfTopCities> FetchPriceQuoteOfTopCities(uint modelId, uint topCount);
        IEnumerable<PriceQuoteOfTopCities> GetModelPriceInNearestCities(uint modelId, uint cityId, ushort topCount);
    }
}
