using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.Interfaces.Finance
{
    public interface ICapitalFirst
    {
        string SaveVoucherDetails(string ctLeadId, CapitalFirstVoucherEntityBase entity);
    }
}
