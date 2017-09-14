﻿using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.Interfaces.Finance
{
    public interface ICapitalFirst
    {
        string SaveVoucherDetails(string ctLeadId, CapitalFirstVoucherEntityBase entity);
       uint SavePersonalDetails(PersonalDetails objDetails,string Utmz,string Utma);
        string SaveEmployeDetails(PersonalDetails objDetails);
    }
}
