using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;

namespace Bikewale.Interfaces.PriceQuote
{
    public interface IPQByCityArea
    {
        PQByCityAreaEntity GetVersionListV2(int modelId, IEnumerable<BikeVersionMinSpecs> modelVersions,
                int? cityId, int? areaId, ushort? sourceId, string UTMA = null,
                string UTMZ = null, string DeviceId = null, string clientIP = null);
        PQByCityAreaEntity GetVersionList(int modelID, IEnumerable<BikeVersionMinSpecs> modelVersions, int? cityId, int? areaId, ushort? sourceId, string UTMA = null, string UTMZ = null, string DeviceId = null, string clientIP = null);
        IEnumerable<CityEntityBase> FetchCityByModelId(int modelId);
        IEnumerable<Bikewale.Entities.Location.AreaEntityBase> GetAreaForCityAndModel(int modelId, int cityId);
        Bikewale.Entities.PriceQuote.v2.PQByCityAreaEntity GetPriceQuoteByCityArea(PriceQuoteParametersEntity pqInput, bool isReload);
    }
}
