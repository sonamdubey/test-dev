﻿

using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.Interfaces.Finance.CapitalFirst
{
    public interface IFinanceRepository
    {
        bool IsValidLead(string leadId);
        bool SavePersonalDetails(PersonalDetails objDetails);
        bool SaveEmployeDetails(EmployeDetails objDetails);
    }
}
