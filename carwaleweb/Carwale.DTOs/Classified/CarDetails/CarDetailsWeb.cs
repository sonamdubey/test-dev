using System;
using System.Collections.Generic;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Stock.Certification;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.CarDetails
{
    public class CarDetailsWeb
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
        public FinanceWeb Finance { get; set; }
        public string ValuationUrl { get; set; }
        public bool IsSold { get; set; }
    }
}
