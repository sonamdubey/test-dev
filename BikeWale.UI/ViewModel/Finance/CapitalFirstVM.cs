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

        public string objLeadJson { get; set; }

        public ushort PlatformId { get; set; }

        public string PageUrl { get; set; }

        public string BikeName { get; set; }

        public uint LoanAmount { get; set; }
        public bool PanStatus { get; set; }
    }

}
