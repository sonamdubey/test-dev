using Carwale.DTOs.PriceQuote;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Price;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;

namespace Carwale.Interfaces.PriceQuote
{
    public interface IQuotationAdapterCommon
    {
        bool IsBasicInputValid(PriceQuoteInput input, CarVersionDetails versionDetails);
        PriceQuoteInput GetCompleteInput(PriceQuoteInput input, CarVersionDetails versionDetails);
        bool IsCompleteInputValid(CarModelDetails modelDetails, CarVersionDetails versionDetails, Cities cityDetails);
        List<EmiCalculatorModelData> GetEmiCalculatorModelData(PQCarDetails carDetails, List<Entity.Price.PriceQuote> priceQuoteList, int cardNo, DealerAd dealerAd);
    }
}
