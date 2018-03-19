using BikewaleOpr.Entity.Dealers;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.Dealers
{
    /// <summary>
    /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
    /// Description :   Performs all BAL operations for Manage Dealer Pricing page.
    /// </summary>
    public interface IDealerPrice
    {
        IEnumerable<DealerVersionPriceEntity> GetDealerPriceQuotes(uint cityId, uint makeId, uint dealerId);
        bool DeleteVersionPriceQuotes(uint dealerId, uint cityId, IEnumerable<uint> versionIds, IEnumerable<uint> bikeModelIds);
        bool SaveVersionPriceQuotes(IEnumerable<uint> dealerIds, IEnumerable<uint> cityIds, IEnumerable<uint> versionIds,
             IEnumerable<uint> itemIds, IEnumerable<uint> itemValues, uint enteredBy, IEnumerable<uint> bikeModelIds);
        UpdatePricingRulesResponseEntity SaveVersionPriceQuotes(IEnumerable<uint> dealerIds, IEnumerable<uint> cityIds, IEnumerable<uint> versionIds,
             IEnumerable<uint> itemIds, IEnumerable<uint> itemValues, IEnumerable<uint> bikeModelIds, IEnumerable<string> bikeModelNames, uint enteredBy, uint makeId);

        bool CopyDealerPriceToOtherDealer(IEnumerable<uint> dealerIds, IEnumerable<uint> cityIds, IEnumerable<uint> versionIds,
             IEnumerable<uint> itemIds, IEnumerable<uint> itemValues, uint enteredBy, IEnumerable<uint> bikeModelIds);
    }
}
