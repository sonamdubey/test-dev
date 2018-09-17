using Bikewale.Entities.Finance.BajajAuto;

namespace Bikewale.Interfaces.Finance.BajajAuto
{
    public interface IBajajAuto
    {
        uint SaveBasicDetails(UserDetails userDetails);
        BASupplierResponse SaveEmployeeDetails(UserDetails userDetails);
        LeadResponse SaveOtherDetails(UserDetails userDetails, string utmz, string utma, ushort leadSource);
    }
}
