using Carwale.Entity.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.CarDetails
{
    [Serializable]
    public class CarDetailsEntity
    {
        public BasicCarInfo BasicCarInfo { get; set; }
        public NonAbsureCarCondition NonAbsureCarCondition { get; set; }
        public CarDetailsImageGallery ImageList { get; set; }
        public DealerInfo DealerInfo { get; set; }
        public List<Features> Features { get; set; }
        public Modifications Modifications { get; set; }
        public OwnersComment OwnerComments { get; set; }
        public List<Specification> Specification { get; set; }
        public IndividualWarranty IndividualWarranty { get; set; }
        public List<FeatureList> FeatureList { get; set; }
        public List<SpecificationList> SpecificationList { get; set; }
        public UsedCarFeatures UsedCarFeatures { get; set; }
        public Finance Finance { get; set; }
        public string ValuationUrl { get; set; }
        public bool IsSold { get; set; }
        public StockPackageInfo StockPackageInfo { get; set; }
    }
}
