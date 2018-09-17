using Bikewale.Entities.Finance.BajajAuto;

namespace Bikewale.Interfaces.Finance.BajajAuto
{
    public interface IBajajAutoRepository
    {
        uint SaveBasicDetails(UserDetails userDetails);
        void SaveOtherDetails(UserDetails userDetails);
        void SaveEmployeeDetails(UserDetails userDetails);
        BajajBikeMappingEntity GetBajajFinanceBikeMappingInfo(uint versionId, uint pincodeId);
    }
}
