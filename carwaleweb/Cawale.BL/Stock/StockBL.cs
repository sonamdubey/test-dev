using Carwale.DAL.Classified.Stock;
using Carwale.DTOs.Stock;
using Carwale.DTOs.Stock.Details;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Classified.CarDetail;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.Stock;
using Carwale.Utility;
using Carwale.Utility.Classified;
using RabbitMqPublishing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace Carwale.BL.Stock
{
    public class StockBL : IStockBL
    {
        private readonly ICarDetailsCache _carDetailsCacheRepo;
        private readonly ICompareCarsCacheRepository _compareCacheRepo;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepo;
        private readonly IESStockQueryRepository _esStockQueryRepository;
        private static readonly string _rupeeSymbol = ConfigurationManager.AppSettings["rupeeSymbol"];
        private static readonly List<int> _mumbaiAndNearbyCitiesList = ConfigurationManager.AppSettings["MumbaiAroundCityIds"].Split(',').Select(Int32.Parse).ToList();
        private const string _notAvailableText = "Not Available";
        private readonly IStockRepository _stockRepository;

        public StockBL(ICarDetailsCache carDetailsCacheRepo, ICompareCarsCacheRepository compareCacheRepo, IGeoCitiesCacheRepository geoCitiesCacheRepo
            , IStockRepository stockRepository
            , IESStockQueryRepository esStockQueryRepository)
        {
            _carDetailsCacheRepo = carDetailsCacheRepo;
            _compareCacheRepo = compareCacheRepo;
            _geoCitiesCacheRepo = geoCitiesCacheRepo;
            _stockRepository = stockRepository;
            _esStockQueryRepository = esStockQueryRepository;
        }

        public static int GetInquiryId(string profileId)
        {
            int inquiryId = -1;
            bool isValid = StockValidations.IsProfileIdValid(profileId);
            if (isValid)
            {
                inquiryId = Convert.ToInt32(profileId.Substring(1));
            }
            return inquiryId;
        }

        public static bool IsDealerStock(string profileId)
        {
            bool isDealer = false;
            bool isValid = StockValidations.IsProfileIdValid(profileId);
            if (isValid)
            {
                isDealer = char.ToUpper(profileId[0]) == 'D';
            }
            return isDealer;
        }

        public static int GetInquiryId(string profileId, out bool isDealer)
        {
            int inquiryId = -1;
            isDealer = false;
            bool isValid = StockValidations.IsProfileIdValid(profileId);
            if (isValid)
            {
                inquiryId = Convert.ToInt32(profileId.Substring(1));
                isDealer = char.ToUpper(profileId[0]) == 'D';
            }
            return inquiryId;
        }

        public static string GetProfileId(int inquiryId, bool isDealer)
        {
            string profileId = null;
            if (inquiryId > 0)
            {
                profileId = $"{(isDealer ? 'D' : 'S')}{inquiryId}";
            }
            return profileId;
        }

        public static string FormatOwnerInfo(short? owner)
        {
            string ownerStr = string.Empty;
            if (owner != null)
            {
                switch (owner.Value)
                {
                    case -1:
                        ownerStr = "N/A";
                        break;
                    case 0:
                        ownerStr = "UnRegistered Car";
                        break;
                    case 1:
                        ownerStr = "First";
                        break;
                    case 2:
                        ownerStr = "Second";
                        break;
                    case 3:
                        ownerStr = "Third";
                        break;
                    case 4:
                        ownerStr = "Fourth";
                        break;
                    default:
                        ownerStr = "4 or More";
                        break;
                }
            }
            return ownerStr;
        }

        public CarDetailsEntity GetStock(string profileId)
        {
            bool isDealer;
            int inquiryId = GetInquiryId(profileId, out isDealer);
            return GetStock(inquiryId, isDealer);
        }

        public CarDetailsEntity GetStock(int inquiryId, bool isDealer)
        {
            CarDetailsEntity stock = null;
            if (inquiryId > 0)
            {
                uint inqId = Convert.ToUInt32(inquiryId);
                stock = isDealer ? _carDetailsCacheRepo.GetDealerListingDetails(inqId) : _carDetailsCacheRepo.GetIndividualListingDetails(inqId);
            }
            return stock;
        }

        public Tuple<List<Features>, List<Specification>> GetSpecificationsAndFeatures(int versionId)
        {
            Tuple<List<Features>, List<Specification>> featuresAndSpecs = null;
            Tuple<Hashtable, Hashtable> tSubCat = _compareCacheRepo.GetSubCategories();
            Hashtable htItemData = _compareCacheRepo.GetItems();
            var versionData = _compareCacheRepo.GetVersionData(Convert.ToInt32(versionId));

            if (tSubCat != null && htItemData != null && versionData != null)
            {
                Hashtable featureTable = GetFeaturesTable(tSubCat.Item2);
                Hashtable specTable = GetSpecsTable(tSubCat.Item1);
                Hashtable tVersionData = versionData.Item1;

                if (featureTable != null && specTable != null && tVersionData != null)
                {
                    foreach (string key in tVersionData.Keys)
                    {
                        ValueData data = tVersionData[key] as ValueData;
                        ItemData item = htItemData[key] as ItemData;
                        if (data != null && item != null && item.NodeCode != null && item.NodeCode.Length > 1)
                        {
                            int sortOrder;
                            int.TryParse(item.SortOrder, out sortOrder);
                            if (item.NodeCode[1] == '1')
                            {
                                Specification spec = (Specification)specTable[item.NodeCode];
                                if (spec != null && spec.SpecificationList != null)
                                {
                                    spec.SpecificationList.Add(new SpecItems()
                                    {
                                        SpecName = item.Name.Replace("~", ""),
                                        SpecValue = data.Value.Replace("~", ""),
                                        SpecUnit = item.UnitType.Replace("CUSTOM", null).Replace("BIT", null),
                                        SortOrder = sortOrder
                                    });
                                }
                            }
                            else
                            {
                                Features feature = (Features)featureTable[item.NodeCode];
                                if (feature != null && feature.FeatureItemList != null)
                                {
                                    feature.FeatureItemList.Add(new FeatureItems()
                                    {
                                        ItemName = data.Value == "No" ? "No " + item.Name : item.Name,
                                        ItemValue = !(data.Value == "No"),
                                        SortOrder = sortOrder
                                    });
                                }
                            }
                        }
                    }

                    List<Features> features = GetFeaturesList(featureTable);
                    List<Specification> specs = GetSpecsList(specTable);
                    featuresAndSpecs = new Tuple<List<Features>, List<Specification>>(features, specs);
                }
            }
            return featuresAndSpecs;
        }

        private static Hashtable GetSpecsTable(Hashtable tSpecsSubCat)
        {
            Hashtable specsTable = null;
            if (tSpecsSubCat != null)
            {
                specsTable = new Hashtable();
                int sortOrder;
                foreach (string key in tSpecsSubCat.Keys)
                {
                    SubCategoryData data = tSpecsSubCat[key] as SubCategoryData;
                    if (data != null)
                    {
                        int.TryParse(data.SortOrder, out sortOrder);
                        specsTable[key] = new Specification()
                        {
                            CategoryName = data.Name,
                            SortOrder = sortOrder,
                            SpecificationList = new List<SpecItems>()
                        };
                    }
                }
            }
            return specsTable;
        }

        private static Hashtable GetFeaturesTable(Hashtable tFeaturesSubCat)
        {
            Hashtable featuresTable = null;
            if (tFeaturesSubCat != null)
            {
                featuresTable = new Hashtable();
                int sortOrder;
                foreach (string key in tFeaturesSubCat.Keys)
                {
                    SubCategoryData data = tFeaturesSubCat[key] as SubCategoryData;
                    if (data != null)
                    {
                        int.TryParse(data.SortOrder, out sortOrder);
                        featuresTable[key] = new Features()
                        {
                            CategoryName = data.Name,
                            SortOrder = sortOrder,
                            FeatureItemList = new List<FeatureItems>()
                        };
                    }
                }
            }
            return featuresTable;
        }

        private static List<Specification> GetSpecsList(Hashtable specTable)
        {
            List<Specification> specs = new List<Specification>();
            foreach (Specification spec in specTable.Values)
            {
                if (spec.SpecificationList.Count > 0)
                {
                    spec.SpecificationList.Sort();
                    specs.Add(spec);
                }
            }
            specs.Sort();
            return specs;
        }

        private static List<Features> GetFeaturesList(Hashtable featureTable)
        {
            List<Features> features = new List<Features>();
            foreach (Features feature in featureTable.Values)
            {
                if (feature.FeatureItemList != null && feature.FeatureItemList.Count > 0 &&
                    ((feature.FeatureItemList.Exists(f => f.ItemValue) && feature.CategoryName != "Manufacturer Warranty")
                    || feature.CategoryName == "Safety" || feature.CategoryName == "Braking & Traction"
                    || feature.CategoryName == "Locks & Security" || feature.CategoryName == "Comfort & Convenience"))
                {
                    feature.FeatureItemList.Sort();
                    features.Add(feature);
                }
            }
            features.Sort();
            return features;
        }

        public static SectionsAvailable GetAvailableSections(CarDetailsEntity carDetails)
        {
            return new SectionsAvailable()
            {
                ImageGallery = carDetails.ImageList != null && carDetails.ImageList.ImageUrlAttributes != null && carDetails.ImageList.ImageUrlAttributes.Count > 0,
                CarCondition = carDetails.NonAbsureCarCondition != null && !String.IsNullOrEmpty(carDetails.NonAbsureCarCondition.OverAll),
                Certification = carDetails.BasicCarInfo != null && carDetails.BasicCarInfo.CertificationId == ConfigurationManager.AppSettings["CartradeCertificationId"].ToString(),
                ExtraInfo = (carDetails.OwnerComments != null && (!String.IsNullOrEmpty(carDetails.OwnerComments.SellerNote) || !String.IsNullOrEmpty(carDetails.OwnerComments.ReasonForSell))) ||
                    (carDetails.Modifications != null && !String.IsNullOrEmpty(carDetails.Modifications.Comments)) ||
                    (carDetails.IndividualWarranty != null && !String.IsNullOrEmpty(carDetails.IndividualWarranty.WarrantyDescription)),
            };
        }

        public static List<OverviewItemApp> GetOverview(BasicCarInfo basicCarInfo)
        {
            List<OverviewItemApp> overview = null;
            if (basicCarInfo != null)
            {
                overview = new List<OverviewItemApp>{
                    new OverviewItemApp() { Name = "Price", Value = _rupeeSymbol + Format.FormatNumericCommaSep(basicCarInfo.Price)},
                    new OverviewItemApp() { Name = "Year", Value = basicCarInfo.MakeYear.ToString("MMM yyyy") },
                    new OverviewItemApp() { Name = "Kilometer", Value = basicCarInfo.Kilometers + " km" },
                    new OverviewItemApp() { Name = "Fuel type", Value = !String.IsNullOrEmpty(basicCarInfo.AdditionalFuel) ? basicCarInfo.FuelName + " + " + basicCarInfo.AdditionalFuel : basicCarInfo.FuelName },
                    new OverviewItemApp() { Name = "Transmission", Value = !String.IsNullOrEmpty(basicCarInfo.TransmissionType) ? basicCarInfo.TransmissionType : _notAvailableText },
                    new OverviewItemApp() { Name = "No. of owners", Value = basicCarInfo.OwnerNumber != null ? FormatOwnerInfo(basicCarInfo.OwnerNumber.Value) : _notAvailableText },
                    new OverviewItemApp() { Name = "Registered at", Value = !String.IsNullOrEmpty(basicCarInfo.RegisterCity) ? basicCarInfo.RegisterCity : _notAvailableText },
                    new OverviewItemApp() { Name = "Exterior Color", Value = !String.IsNullOrEmpty(basicCarInfo.Color) ? basicCarInfo.Color : _notAvailableText },
                    new OverviewItemApp() { Name = "Interior Color", Value = !String.IsNullOrEmpty(basicCarInfo.InteriorColor) ? basicCarInfo.InteriorColor : _notAvailableText },
                    new OverviewItemApp() { Name = "Car Available at", Value = !String.IsNullOrEmpty(basicCarInfo.AreaName) ? basicCarInfo.AreaName + ", " + basicCarInfo.CityName : basicCarInfo.CityName },
                    new OverviewItemApp() { Name = "Insurance", Value = !String.IsNullOrEmpty(basicCarInfo.Insurance) ? !String.IsNullOrEmpty(basicCarInfo.InsuranceExpiry) ? basicCarInfo.Insurance + "#till " + basicCarInfo.InsuranceExpiry : basicCarInfo.Insurance : _notAvailableText },
                    new OverviewItemApp() { Name = "Lifetime Tax", Value = !String.IsNullOrEmpty(basicCarInfo.LifeTimeTax) ? basicCarInfo.LifeTimeTax : _notAvailableText },
                    new OverviewItemApp() { Name = "Last Updated", Value = !String.IsNullOrEmpty(basicCarInfo.LastUpdatedDate) ? Format.GetDisplayTimeSpan(basicCarInfo.LastUpdatedDate) : _notAvailableText }
                };
            }
            return overview;
        }

        public static ICollection<ConditionItem> GetCarCondition(NonAbsureCarCondition carCondition)
        {
            ICollection<ConditionItem> conditionInfo = null;
            const string imagePath = "/cw/static/cw-apps/carcondition/";
            if (carCondition != null)
            {
                conditionInfo = new List<ConditionItem>{
                    new ConditionItem { CarItem = "AC", Condition = !String.IsNullOrEmpty(carCondition.AC) ? carCondition.AC : _notAvailableText, IconImagePath = imagePath + "ac@3x.png" },
                    new ConditionItem { CarItem = "Engine", Condition = !String.IsNullOrEmpty(carCondition.Engine) ? carCondition.Engine : _notAvailableText, IconImagePath = imagePath + "engine@3x.png" },
                    new ConditionItem { CarItem = "Suspensions", Condition = !String.IsNullOrEmpty(carCondition.Suspensions) ? carCondition.Suspensions : _notAvailableText, IconImagePath = imagePath + "suspension@3x.png" },
                    new ConditionItem { CarItem = "Brakes", Condition = !String.IsNullOrEmpty(carCondition.Brakes) ? carCondition.Brakes : _notAvailableText, IconImagePath = imagePath + "brakes@3x.png" },
                    new ConditionItem { CarItem = "Battery", Condition = !String.IsNullOrEmpty(carCondition.Battery) ? carCondition.Battery : _notAvailableText, IconImagePath = imagePath + "battery@3x.png" },
                    new ConditionItem { CarItem = "Tyres", Condition = !String.IsNullOrEmpty(carCondition.Tyres) ? carCondition.Tyres : _notAvailableText, IconImagePath = imagePath + "tyres@3x.png" },
                    new ConditionItem { CarItem = "Electricals", Condition = !String.IsNullOrEmpty(carCondition.Electricals) ? carCondition.Electricals : _notAvailableText, IconImagePath = imagePath + "electrical@3x.png" },
                    new ConditionItem { CarItem = "Seats", Condition = !String.IsNullOrEmpty(carCondition.Seats) ? carCondition.Seats : _notAvailableText, IconImagePath = imagePath + "seats@3x.png" },
                    new ConditionItem { CarItem = "Interior", Condition = !String.IsNullOrEmpty(carCondition.Interior) ? carCondition.Interior : _notAvailableText, IconImagePath = imagePath + "interior@3x.png" },
                    new ConditionItem { CarItem = "Exterior", Condition = !String.IsNullOrEmpty(carCondition.Exterior) ? carCondition.Exterior : _notAvailableText, IconImagePath = imagePath + "exterior@3x.png" }
                };
            }
            return conditionInfo;
        }

        public string GetDeliveryText(int deliveryCityId)
        {
            string deliveryText = null;
            if (deliveryCityId > 0)
            {
                string deliveryCity;
                switch (deliveryCityId)
                {
                    case 3000:
                        deliveryCity = "Mumbai";
                        break;
                    case 3001:
                        deliveryCity = "Delhi NCR";
                        break;
                    default:
                        deliveryCity = _geoCitiesCacheRepo.GetCityNameById(deliveryCityId.ToString());
                        break;
                }
                if (!String.IsNullOrEmpty(deliveryCity))
                {
                    deliveryText = "Delivery available in " + deliveryCity;
                }
            }
            return deliveryText;
        }

        public void RefreshESStockOfDealer(int dealerId)
        {
            IEnumerable<string> profileIds = _stockRepository.GetProfileIdsOfDealer(dealerId);
            if (profileIds != null)
            {
                PublishToESQueue(profileIds);
            }
        }

        public void RefreshESStocksOfCertProg(int certificationId)
        {
            IEnumerable<string> profileIds = _stockRepository.GetLiveStocksByCertProgId(certificationId);
            if (profileIds != null)
            {
                PublishToESQueue(profileIds);
            }
        }

        public void RefreshESStock(string profileId)
        {
            RabbitMqPublish rabbitMqPublish = new RabbitMqPublish();
            string queueName = ConfigurationManager.AppSettings["UsedCarESStockQueue"];
            NameValueCollection nameValueCollection = new NameValueCollection();

            nameValueCollection.Set("actionname", "UPDATE");
            nameValueCollection.Set("profileid", profileId);
            rabbitMqPublish.PublishToQueue(queueName, nameValueCollection);
        }

        public void PublishToESQueue(IEnumerable<string> profileIds)
        {
            RabbitMqPublish rabbitMqPublish = new RabbitMqPublish();
            string queueName = ConfigurationManager.AppSettings["UsedCarESStockQueue"];
            NameValueCollection nameValueCollection = new NameValueCollection();
            nameValueCollection.Set("actionname", "UPDATE");
            foreach (var profileId in profileIds)
            {
                nameValueCollection.Set("profileid", profileId);
                rabbitMqPublish.PublishToQueue(queueName, nameValueCollection);
            }
        }

        public static string AddRankInUrl(string absoluteUrl, int rank)
        {
            return string.Format("{0}?rk=r{1}", absoluteUrl, rank);
        }
        
        public static string GetDetailsPageUrl(string cityName, string makeName, string maskingName, string profileId, bool isMsite)
        {
            var hostUrl = HttpContextUtils.GetBaseUrl(System.Web.HttpContext.Current.Request);
            return $"{hostUrl}{ (isMsite ? "m/" : string.Empty)}used/cars-in-{Format.FormatSpecial(cityName)}/{Format.FormatSpecial(makeName)}-{maskingName}-{profileId}/";
        }

        public static List<UsedCarComparisonEntity> GetUsedCarComparison(int cityId, List<int> rootIds)
        {
            List<int> cityIds = new List<int>();
            if (cityId > 0)
            {
                if (cityId == 1)
                {
                    cityIds = _mumbaiAndNearbyCitiesList;
                }
                else
                {
                    cityIds.Add(cityId);
                } 
            }
            return StockRepository.GetUsedCarComparison(cityIds, rootIds);
        }

        public static bool AreCtePackagesValid(IEnumerable<Carwale.Entity.Stock.Stock> stocks)
        {
            return stocks.All(stock => stock.CtePackageId > 0);
        }

        public string GetDetailsPageUrlFromRegistrationNumber(string regNo)
        {
            // remove spaces and hyphens and change to uppercase.
            var formattedRegNo = Regex.Replace(regNo, @"[\s-]", string.Empty).ToUpper();
            return _esStockQueryRepository.GetDetailsPageUrlByRegistrationNumber(formattedRegNo);
        }

        public static string GetLogoUrlForStock(CwBasePackageId cwBasePackageId, string certProgLogoUrl)
        {
            return cwBasePackageId == CwBasePackageId.Franchise ? Constants.FranchiseLogoUrl : certProgLogoUrl;
        }
    }
}
