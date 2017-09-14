﻿

using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.Interfaces.Finance.CapitalFirst
{
    public interface IFinanceRepository
    {
        bool IsValidLead(string leadId);
        uint SavePersonalDetails(PersonalDetails objDetails);

        bool SaveVoucherDetails(string leadIdCarTrade, CapitalFirstVoucherEntityBase voucherDetails);
        CapitalFirstBikeEntity GetCapitalFirstBikeMapping(uint versionId);

        bool SaveCTApiResponse(uint leadId, ushort status, string responseText);
    }
}
