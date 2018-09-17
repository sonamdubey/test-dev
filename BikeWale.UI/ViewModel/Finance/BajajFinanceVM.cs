using Bikewale.Entities.Dealer;
using Bikewale.Entities.Finance.CapitalFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.Finance
{

    public class BajajFinanceVM : ModelBase
    {
        public PersonalDetails PersonalDetails { get; set; }

        public ManufacturerLeadEntity ObjLead { get; set; }

        public string objLeadJson { get; set; }

        public ushort PlatformId { get; set; }

        public string PageUrl { get; set; }

        public string BikeName { get; set; }

        public uint LoanAmount { get; set; }
    }
}
