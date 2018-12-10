using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.PriceQuote;
using System.Collections.Generic;

namespace Carwale.Interfaces.Prices
{
    public interface IEmiCalculatorAdapter
    {
        EmiCalculatorModelData GetEmiData(CarOverviewDTOV2 overview, DealerAdDTO dealerAd, LeadSourceDTO leadSource, int versionORP, int cityId);
        EmiCalculatorModelData GetEmiSummary(int versionId, bool isMetallic, int orp, EmiCalculatorModelData emiCalculatorModelData = null);
        EmiCalculatorDto GetEmiCalculatorData(int versionId, int cityId);
    }
}
