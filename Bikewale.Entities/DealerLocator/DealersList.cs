﻿using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using System;

namespace Bikewale.Entities.DealerLocator
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created on : 21 March 2016
    /// Description : DealersList for dealer locator
    /// </summary>
    [Serializable]
    public class DealersList : NewBikeDealerBase
    {
        public AreaEntityBase objArea { get; set; }
        public UInt16 DealerType { get; set; }
        public string City { get; set; }
        public string EMail { get; set; }
        public string Address { get; set; }
        public string WorkingHours { get; set; }
        public uint CampaignId { get; set; }
    }
}
