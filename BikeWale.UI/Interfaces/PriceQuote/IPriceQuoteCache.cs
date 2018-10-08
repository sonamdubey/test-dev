
using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.manufacturecampaign;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;

namespace Bikewale.Interfaces.PriceQuote
{
    /// <summary>
    /// Author : Vivek Gupta
    /// Date : 20-05-2016
    /// Description : Price Quote Cache method references
    /// Modified by : Rajan Chauhan on 28 September 2018
    /// Description : Added GetVersionPricesByModelId method
    /// </summary>
    public interface IPriceQuoteCache
    {
        IEnumerable<PriceQuoteOfTopCities> FetchPriceQuoteOfTopCitiesCache(uint modelId, uint topCount);
        IEnumerable<PriceQuoteOfTopCities> GetModelPriceInNearestCities(uint modelId, uint cityId, ushort topCount);
        IEnumerable<OtherVersionInfoEntity> GetOtherVersionsPrices(uint modelId, uint cityId);
        IEnumerable<ManufacturerDealer> GetManufacturerDealers(uint cityId, uint dealerId);
        uint GetDefaultPriceQuoteVersion(uint modelId, uint cityId);
        Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity GetDealerPriceQuoteByPackageV2(PQParameterEntity objParams);

        string GetManufacturerCampaignMobileRenderedTemplate(string key, ManufactureCampaignLeadEntity leadCampaign);
        string GetManufacturerCampaignMobileRenderedTemplateV2(string key, Entities.manufacturecampaign.v2.ManufactureCampaignLeadEntity leadCampaign);
        IEnumerable<BikeQuotationEntity> GetVersionPricesByModelId(uint modelId, uint cityId);
    }
}
