using Carwale.Entity.CarData;
using Carwale.Entity.Deals;
using Carwale.Entity.PriceQuote;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.ElasticEntities
{
    public class CarBaseEntity
    {

        public int MakeId { get; set; }

        public int ModelId { get; set; }

        public int VersionId { get; set; }

        public int RootId { get; set; }

        public string MakeName { get; set; }

        public string ModelName { get; set; }

        public string ModelMaskingName { get; set; }

        public string VersionName { get; set; }

        public string VersionMaskingName { get; set; }

        public int VersionsCount { get; set; }

        public int MatchingVersionsCount { get; set; }

        public bool IsSpecialVersion { get; set; }

        public bool New { get; set; }

        public bool Used { get; set; }

        public bool Futuristic { get; set; }

        public bool Indian { get; set; }

        public bool Imported { get; set; }

        public bool Classic { get; set; }

        public bool Modified { get; set; }

        public bool IsHybrid { get; set; }

        public DateTime? ModelLaunchDate { get; set; }

        public DateTime? VersionLaunchDate { get; set; }

        public DateTime? DiscontinuationDate { get; set; }

        public bool IsTopVersion { get; set; }

        public bool IsSpecsAvailable { get; set; }

        public bool IsSpecsExist { get; set; }

        public string SpecSummary { get; set; }

        public string HostUrl { get; set; }

        public string ImagePath { get; set; }

        public int AvgPrice { get; set; }

        public int ModelMinPrice { get; set; }

        public int ModelMaxPrice { get; set; }

        public int ReviewCount { get; set; }

        public decimal ReviewRate { get; set; }

        public int BodyStyle { get; set; }

        public int FuelType { get; set; }

        public int TransmissionType { get; set; }

        public int PowerBHP { get; set; }

        public int SeatingCapacity { get; set; }

        public int EMI { get; set; }

        public int Displacement { get; set; }

        public int ModelPopularity { get; set; }

        public string MileageSummary { get; set; }
        public string Arai { get; set; }
        public string City { get; set; }
        public string Highway { get; set; }

        public int VideoCount { get; set; }
        public int ImagesCount { get; set; }
        public int NewsCount { get; set; }
        public int SpecialReportsCount { get; set; }
        public int ExpertReviewsCount { get; set; }
        public List<ModelColors> ColorsData { get; set; }

        public PriceOverview PriceOverview { get; set; }

        public DiscountSummary DiscountSummary { get; set; }
    }

    public class RangeLimit
    {
        public int LowerLimit;
        public int UpperLimit;
    }


    public class NewCarSearchInputs {
		    public const string defaultSortField1 = "modelMinPrice";
		    public const string defaultSortField2 = "avgPrice";
        public List<int> ModelIds = new List<int>();
        public List<int> ExcludedModelIds = new List<int>();
        public RangeLimit EMI = new RangeLimit();
        public List<RangeLimit> budgets = new List<RangeLimit>();
        public List<RangeLimit> seatingCapacity = new List<RangeLimit>();
        public List<RangeLimit> enginePower = new List<RangeLimit>();
        public string[] makes = { };
        public string[] fueltype = { };
        public string[] transmission = { };
        public string[] bodytype = { };
        public int cityId = -1;
        public bool useEMI = false;
        public bool newCarsOnly = true;
        public bool CountsOnly = false;
        public Tuple<Field, SortOrder> sortField1 = new Tuple<Field, SortOrder>(new Field(defaultSortField1), SortOrder.Ascending);
        public Tuple<Field, SortOrder> sortField2 = new Tuple<Field, SortOrder>(new Field(defaultSortField2), SortOrder.Ascending);
        public int pageNo = 0;
        public int pageSize = 10;
        public bool ShowOrp;
        public bool IsBodyTypeByBudgetPage = false;
        public bool IsMobile = false;
    }

    public class ElasticCarData
    {
        public Dictionary<int, Dictionary<int, CarBaseEntity>> ModelVersionDict = new Dictionary<int, Dictionary<int, CarBaseEntity>>();
        public int TotalVersions;
        public int TotalModels;
        public List<CarVersionDetails> SponsoredVersions { get; set; }
        public bool ShowSponsoredAd { get; set; }
    }
    public class NewCarFinderEntity : CarBaseEntity
    {
        public List<CityPrice> OnRoadCityPrice { get; set; }
    }
    public class CityPrice
    {
        public short CityId { get; set; }
        public int Price { get; set; }
    }
    public class NCFElasticCarData : ElasticCarData
    {
        public new Dictionary<int, Dictionary<int, NewCarFinderEntity>> ModelVersionDict = new Dictionary<int, Dictionary<int, NewCarFinderEntity>>();
    }
    public class ElasticModel
    {
        public int Id { get; set; }
        public int MakeId { get; set; }
        public string MakeName { get; set; }
        public int ModelPopularity { get; set; }
    }
}
