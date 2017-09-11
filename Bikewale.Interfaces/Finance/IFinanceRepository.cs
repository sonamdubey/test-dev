﻿

using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.Interfaces.Finance.CapitalFirst
{
    public interface IFinanceRepository
    {
        bool IsValidLead(string leadId);
        uint SavePersonalDetails(PersonalDetails objDetails);
      
        bool SaveVoucherDetails(string leadIdCarTrade, CapitalFirstVoucherEntityBase voucherDetails);
    }
}
