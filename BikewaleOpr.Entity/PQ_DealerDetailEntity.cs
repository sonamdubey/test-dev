﻿using System;
using System.Collections.Generic;

namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Written By : Ashwini Todkar on 28 Oct 2014
    public class PQ_DealerDetailEntity
    {
        public NewBikeDealers objDealer { get; set; }
        public PQ_QuotationEntity objQuotation { get; set; }
        public List<OfferEntity> objOffers { get; set; }
        public List<FacilityEntity> objFacilities { get; set; }
        public EMI objEmi { get; set; }
        public BookingAmountEntityBase objBookingAmt { get; set; }

    }

    public class PQParameterEntity
    {
        public UInt32 VersionId { get; set; }
        public UInt32 CityId { get; set; }
        public UInt32 DealerId { get; set; }
    }

}
