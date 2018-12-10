using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.CompareCars;
using System.Collections.Generic;

namespace Carwale.Entity.ViewModels
{
    public class CompareCarsDetailModel
    {
        public int FeaturedVersionId = -1;
        public int TblHeaderWidth;
        public string TargetLabel;
        public bool IsDiscountAvailable;
        public string ValVersionIDs;
        public string TargetValue;
        public List<ComparisonData> CarData;
        public List<List<Color>> Colors;
        public List<int> ValidVersionIds;
        public List<int> ModelIds;
        public List<CarWithImageEntity> CarDetails;
        public FeaturedCarDataEntity FeaturedCarData;
        public MetaTagsEntity PageMetaTags;
        public List<BreadcrumbEntity> BreadcrumbEntitylist;
        public Dictionary<int,List<SimilarCarModels>> SimilarCars;
        public Dictionary<int, List<CarVersionEntity>> ModelsVersionDict;
        public Dictionary<int, UsedCarComparisonEntity> UsedCars;
        public Dictionary<int, UsedCarComparisonEntity> UsedCarComparison { get; set; }
        public List<CarMakeEntityBase> CarMakes;
        public bool ShowCampaignSlab;
        public List<int> VersionsWithTyres;
        public string Summary;
    }
}
