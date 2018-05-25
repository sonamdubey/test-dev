

using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.Interfaces.Finance.CapitalFirst
{
    public interface IFinanceRepository
    {
        CapitalFirstBikeEntity GetCapitalFirstBikeMapping(uint versionId);
        uint SaveCapitalFirstLeadData(PersonalDetails objDetails, CTFormResponse formResponse);
        CapitalFirstLeadEntity GetLeadDetails(string ctLeadId);
    }
}
