

using Bikewale.Entities.Finance.CapitalFirst;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Finance.CapitalFirst
{
    public interface IFinanceRepository
    {
        CapitalFirstBikeEntity GetCapitalFirstBikeMapping(uint versionId);
        uint SaveCapitalFirstLeadData(PersonalDetails objDetails, CTFormResponse formResponse);
        CapitalFirstLeadEntity GetLeadDetails(uint ctLeadId);

        IEnumerable<CityPanMapping> GetCapitalFirstPanCityMapping();
    }
}
