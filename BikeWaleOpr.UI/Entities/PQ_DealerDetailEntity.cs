using System;
using System.Collections.Generic;
using System.Text;

namespace BikeWaleOpr.Entities
{
    public class PQ_DealerDetailEntity
    {
        public NewBikeDealers objDealer { get; set; }
        public PQ_QuotationEntity objQuotation { get; set; }
        public List<OfferEntity> objOffers { get; set; }
    }
}
