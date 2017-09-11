using Bikewale.Entities.Dealer;
using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.Models.Finance
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 08-Sep-2017
    /// Summary: view model for capital first page
    /// </summary>
    public class CapitalFirstVM : ModelBase
    {
        public PersonalDetails PersonalDetails { get; set; }

        public ManufacturerLeadEntity ObjLead { get; set; }


    }

}
