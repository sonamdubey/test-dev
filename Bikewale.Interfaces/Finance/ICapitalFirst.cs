using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.Interfaces.Finance
{
    public interface ICapitalFirst
    {
        string SaveVoucherDetails(string ctLeadId, CapitalFirstVoucherEntityBase entity);
        LeadResponseMessage SavePersonalDetails(PersonalDetails objDetails, string Utmz, string Utma, ushort leadSource);
        LeadResponseMessage SaveEmployeDetails(PersonalDetails objDetails, string Utmz, string Utma, ushort leadSource);
        LeadResponseMessage PushLeadinCTandAutoBiz(PersonalDetails objDetails, ushort leadSource);
    }
}
