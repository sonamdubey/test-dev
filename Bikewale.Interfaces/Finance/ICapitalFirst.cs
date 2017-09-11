using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.Interfaces.Finance
{
    public interface ICapitalFirst
    {
        string SaveVoucherDetails(string ctLeadId, string jsonData);
       
        bool SaveEmployeDetails(PersonalDetails objDetails);
    }
}
