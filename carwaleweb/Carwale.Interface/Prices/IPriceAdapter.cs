using Carwale.DTOs.PriceQuote;
using System.Collections.Generic;

namespace Carwale.Interfaces.Prices
{
    public interface IPriceAdapter
    {
        List<VersionPriceQuoteDTOV2> GetVersionCompulsoryPrices(int versionId, int cityId);
        VersionPriceDto GetVersionPriceAndEmi(int versionId, int cityId);
    }
}
