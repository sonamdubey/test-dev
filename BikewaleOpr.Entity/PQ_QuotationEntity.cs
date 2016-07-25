using BikewaleOpr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikewaleOpr.Entities
{
    public class PQ_QuotationEntity
    {
        public List<PQ_Price> PriceList { get; set; }
        public List<PQ_Price> discountedPriceList { get; set; }

        public List<string> Disclaimer { get; set; }
        public List<OfferEntity> objOffers { get; set; }
        public BikeMakeEntityBase objMake { get; set; }
        public BikeModelEntityBase objModel { get; set; }
        public BikeVersionEntityBase objVersion { get; set; }

        public string HostUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string SmallPicUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public IEnumerable<PQ_BikeVarient> Varients { get; set; }
    }
}
