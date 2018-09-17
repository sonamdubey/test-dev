using System;
using System.Collections.Generic;

namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created By  : Pratibha Verma
    /// Description : Multioutlet Dealer Entity 
    /// </summary>
    [Serializable]
    public class MultioutletDealer
    {
        public uint MasterDealerId { set; get; }
        public IEnumerable<SecondaryDealerBase> SecondaryDealers { get; set; }
	}
}
