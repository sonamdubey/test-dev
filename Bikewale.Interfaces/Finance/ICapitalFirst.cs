using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.Interfaces.Finance
{
    public interface ICapitalFirst
    {
        string SaveVoucherDetails(string ctLeadId, string jsonData);
       bool SavePersonalDetails(PersonalDetails objDetails,string Utmz,string Utma);
        string SaveEmployeDetails(PersonalDetails objDetails);
    }
}
