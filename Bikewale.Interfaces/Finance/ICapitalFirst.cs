using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.Interfaces.Finance
{
    public interface ICapitalFirst
    {
        string SaveVoucherDetails(string ctLeadId, CapitalFirstVoucherEntityBase entity);
        Iddetails SavePersonalDetails(PersonalDetails objDetails, string Utmz, string Utma, ushort leadSource);
        string SaveEmployeDetails(PersonalDetails objDetails, string Utmz, string Utma, ushort leadSource);

        void PushLeadinCTandAutoBiz(PersonalDetails objDetails, ushort leadSource);
    }
}
