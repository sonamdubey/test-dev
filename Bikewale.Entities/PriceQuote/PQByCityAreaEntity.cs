﻿using Bikewale.Entities.BikeData;
using Bikewale.ManufacturerCampaign.Entities;
using System.Collections.Generic;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 18 Apr 2016
    /// modified By : vivek gupta on 17 june 2016, added DealerPackageType,SecondaryDealerCount,OfferList
    /// Summary   : To hold entities which will be used for Model API v3 mapper
    /// </summary>
    public class PQByCityAreaEntity
    {
        public bool IsCityExists { get; set; }
        public bool IsAreaExists { get; set; }
        public bool IsAreaSelected { get; set; }
        public bool IsExShowroomPrice { get; set; }
        public uint DealerId { get; set; }
        public ulong PqId { get; set; }
        public IEnumerable<BikeVersionMinSpecs> VersionList { get; set; }
        public DealerQuotationEntity PrimaryDealer { get; set; }
        public bool IsPremium { get; set; }
        public int SecondaryDealerCount { get; set; }
        public Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity DealerEntity { get; set; }
        public ManufacturerCampaignEntity ManufacturerCampaign { get; set; }
    }
}
