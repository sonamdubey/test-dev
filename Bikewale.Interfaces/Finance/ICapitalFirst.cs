using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.Interfaces.Finance
{
    public interface ICapitalFirst
    {
        LeadResponseMessage SaveLead(PersonalDetails objDetails, string utmz, string utma, ushort leadSource);
    }
}
