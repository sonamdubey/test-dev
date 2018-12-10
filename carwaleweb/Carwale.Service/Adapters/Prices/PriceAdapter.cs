using AutoMapper;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.Prices;
using Carwale.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.Service.Adapters.Prices
{
    public class PriceAdapter : IPriceAdapter
    {
        private readonly IPrices _iPrices;
        private readonly ICarPriceQuoteAdapter _iPriceQuote;

        public PriceAdapter(IPrices iPrices, ICarPriceQuoteAdapter iPriceQuote)
        {
            _iPrices = iPrices;
            _iPriceQuote = iPriceQuote;
        }

        public List<VersionPriceQuoteDTOV2> GetVersionCompulsoryPrices(int versionId, int cityId)
        {
            var priceQuoteList = _iPrices.GetVersionCompulsoryPrices(versionId, cityId, true);
            priceQuoteList = priceQuoteList.Where(x => x.PricesList != null && x.PricesList.Count > 0).ToList();

            return Mapper.Map<List<VersionPriceQuoteDTOV2>>(priceQuoteList);
        }

        public VersionPriceDto GetVersionPriceAndEmi(int versionId, int cityId)
        {
            var versionPrice = new VersionPriceDto();
            
            var priceOverview = _iPriceQuote.GetAvailablePriceForVersion(versionId, cityId, true);
            versionPrice.PriceOverview = Mapper.Map<PriceOverview, PriceOverviewDtoV3>(priceOverview ?? new PriceOverview());

            var price = CustomParser.parseIntObject(versionPrice.PriceOverview.PriceForSorting);
            versionPrice.EmiInformation = Mapper.Map<EMIInformation, EmiInformationDtoV2>(price > 0 ? _iPrices.GetEmiInformation(price) : null);

            return versionPrice;
        }
    }
}
