using Carwale.BL.Stock;
using Carwale.DAL.Classified.Stock;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Enum;
using Carwale.Entity.ViewModels;
using Carwale.Interfaces;
using Carwale.Interfaces.Accessories.Tyres;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Carwale.BL.NewCars
{
    public class CompareCarDetailsAdapter : IServiceAdapterV2
    {
        private readonly IUnityContainer _container;
        private readonly ICarVersionCacheRepository _carVersionsCacheRepo;
        private readonly ITyresBL _tyres;
        public CompareCarDetailsAdapter(IUnityContainer container, ICarVersionCacheRepository carVersionsCacheRepo, ITyresBL tyres)
        {
            _container = container;
            _carVersionsCacheRepo = carVersionsCacheRepo;
            _tyres = tyres;
        }

        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetCompareCarDetailModel<U>(input), typeof(T));
        }

        private CompareCarsDetailModel GetCompareCarDetailModel<U>(U input)
        {
            var compareCarsDetailModel = new CompareCarsDetailModel();
            var inputval = (CompareCarInputParam)Convert.ChangeType(input, typeof(U));
            ICompareCarsBL compareCarBL = _container.Resolve<ICompareCarsBL>();
            var modelsVersionDict = new Dictionary<int, List<CarVersionEntity>>();
            ICarDataLogic carDataLogic = _container.Resolve<ICarDataLogic>();
            ICarVersions carVersionsBL = _container.Resolve<ICarVersions>();
            ICarMakesCacheRepository _makeCacheRepo = _container.Resolve<ICarMakesCacheRepository>();
            
            string targetValue2 = string.Empty;
            try
            {
                int featuredCarVersionId = -1;
                List<int> versionList = inputval.VersionIds;
                if (versionList.Count < 2)
                {
                    return null;
                }
                compareCarsDetailModel.ValidVersionIds = versionList;
                var featuredCar = carDataLogic.GetFeaturedCar(inputval);
                compareCarsDetailModel.FeaturedCarData = featuredCar;
                if (featuredCar != null && featuredCar.FeaturedVersionId > 0)
                {
                    featuredCarVersionId = featuredCar.FeaturedVersionId;

                    if (!versionList.Contains(featuredCar.FeaturedVersionId))
                    {
                        compareCarsDetailModel.FeaturedVersionId = featuredCar.FeaturedVersionId;
                        versionList.Add(featuredCar.FeaturedVersionId);
                    }
                }

                var vehicleData = carDataLogic.GetCombinedCarData(versionList);
                var carData = new List<ComparisonData>();
                if (vehicleData != null)
                {
                    foreach (var vehicle in vehicleData)
                    {
                        var result = new ComparisonData();
                        result.Specs = AutoMapper.Mapper.Map<List<Carwale.Entity.CarData.CarData>, List<SubCategory>>(vehicle.Specifications);
                        result.Features = AutoMapper.Mapper.Map<List<Carwale.Entity.CarData.CarData>, List<SubCategory>>(vehicle.Features);
                        result.Overview = AutoMapper.Mapper.Map<List<Carwale.Entity.CarData.CategoryItem>, List<Item>>(vehicle.Overview);
                        carData.Add(result);
                    }
                }
                compareCarsDetailModel.CarData = carData;
                compareCarsDetailModel.Colors = carVersionsBL.GetVersionsColors(versionList);

                compareCarsDetailModel.CarDetails = carDataLogic.GetVersionDetails(versionList, featuredCarVersionId, inputval.CustLocation, Platform.CarwaleDesktop);

                compareCarsDetailModel.TblHeaderWidth = CalculateWidth(compareCarsDetailModel.FeaturedVersionId, compareCarsDetailModel.CarDetails.Count);
                compareCarsDetailModel.CarDetails.ForEach(x => x.CloseUrl = GetCloseUrl(x.VersionId, compareCarsDetailModel));

                compareCarsDetailModel.CarDetails.ForEach(x => targetValue2 += !string.IsNullOrWhiteSpace(x.ModelName) ? string.Format("\"{0}\",", x.ModelName) : string.Empty);
                compareCarsDetailModel.IsDiscountAvailable = GetDiscountCount(compareCarsDetailModel.CarDetails);
                compareCarsDetailModel.TargetValue = targetValue2.TrimEnd(',');
                compareCarsDetailModel.ValVersionIDs = string.Join(",", compareCarsDetailModel.ValidVersionIds);

                compareCarsDetailModel.ModelIds = GetModelIds(compareCarsDetailModel.CarDetails, compareCarsDetailModel.FeaturedVersionId);
                compareCarsDetailModel.ModelIds.ForEach(id => { if (!modelsVersionDict.ContainsKey(id)) modelsVersionDict.Add(id, _carVersionsCacheRepo.GetCarVersionsByType("compareall", id)); });
                compareCarsDetailModel.ModelsVersionDict = modelsVersionDict;
                compareCarsDetailModel.PageMetaTags = compareCarBL.GetCanonical(compareCarsDetailModel.CarDetails, compareCarsDetailModel.FeaturedVersionId);
                Tuple<string, string> tuple = GetDescription(compareCarsDetailModel.CarDetails, compareCarsDetailModel.FeaturedVersionId);
                compareCarsDetailModel.PageMetaTags.Description = tuple.Item1;
                compareCarsDetailModel.TargetLabel = tuple.Item2;
                compareCarsDetailModel.UsedCarComparison = GetUsedCarsComparison(compareCarsDetailModel.CarDetails, inputval.CustLocation.CityId);
                compareCarsDetailModel.BreadcrumbEntitylist = BindBreadCrumb(compareCarsDetailModel.PageMetaTags);
                compareCarsDetailModel.CarMakes = _makeCacheRepo.GetCarMakesByType("compareall");
                compareCarsDetailModel.SimilarCars = GetSimilarComparisons(compareCarsDetailModel.FeaturedVersionId, compareCarsDetailModel.CarDetails, inputval.CwcCookie);

                compareCarsDetailModel.ShowCampaignSlab = carDataLogic.CheckForCampaign(compareCarsDetailModel.CarDetails, inputval.CustLocation);
                compareCarsDetailModel.VersionsWithTyres = _tyres.CheckForTyres(inputval.VersionIds);
                //summary on compare car 
                compareCarsDetailModel.Summary = GetSummary(compareCarsDetailModel);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return compareCarsDetailModel;
        }

        private bool GetDiscountCount(List<CarWithImageEntity> carDetails)
        {
            foreach (var car in carDetails)
            {
                if (car.DiscountSummary.Savings > 0 || (car.DiscountSummary.Savings == 0 && !string.IsNullOrWhiteSpace(car.DiscountSummary.Offers)))
                {
                    return true;
                }
            }
            return false;
        }
        private List<int> GetModelIds(List<CarWithImageEntity> carDetails, int featuredVersionId)
        {
            var modelIdlist = new HashSet<int>();
            try
            {
                for (int modelCount = 0; modelCount < carDetails.Count; modelCount++)
                {
                    if (carDetails[modelCount].VersionId != featuredVersionId)
                        modelIdlist.Add(carDetails[modelCount].ModelId);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return modelIdlist.ToList();
        }
        private Tuple<string, string> GetDescription(List<CarWithImageEntity> carDetails, int featuredVersionId)
        {
            StringBuilder description = new StringBuilder(string.Empty);
            StringBuilder result = new StringBuilder(string.Empty);
            StringBuilder targetLabel = new StringBuilder(string.Empty);

            try
            {
                for (int l = 0; l < carDetails.Count; l++)
                {
                    if (carDetails[l].VersionId != featuredVersionId)
                    {
                        description.AppendFormat("{0} {1} vs ", carDetails[l].MakeName, carDetails[l].ModelName);
                        result.AppendFormat("{0}, ", carDetails[l].ModelName);
                    }
                    targetLabel.AppendFormat("|{0}_{1}", carDetails[l].MaskingName, carDetails[l].VersionName);
                }
                result = result.Remove(result.Length - 2, 2);
                int indexcomma = result.ToString().LastIndexOf(',');
                result = result.Remove(indexcomma, 1).Insert(indexcomma, " and");
                description.Remove(description.Length - 3, 3);
                description.AppendFormat("- Which car should you buy? CarWale helps you compare {0}  on over 170 parameters, including detailed tech specs, features, colours and prices.", result);

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return new Tuple<string, string>(description.ToString(), targetLabel.ToString());
        }

        private static Dictionary<int, UsedCarComparisonEntity> GetUsedCarsComparison(List<CarWithImageEntity> carDetails, int cityId)
        {
            List<int> rootIds = new List<int>(); 
            foreach (var car in carDetails)
            {
                rootIds.Add(car.RootId);
            }
            var usedComparisonList = StockBL.GetUsedCarComparison(cityId, rootIds);
            var usedCompareDict = new Dictionary<int, UsedCarComparisonEntity>();

            if (usedComparisonList != null)
            {
                foreach (var comparisonItem in usedComparisonList)
                {
                    if (!usedCompareDict.ContainsKey(comparisonItem.RootId) && carDetails.Any(p => p.RootId == comparisonItem.RootId))
                    {
                        usedCompareDict.Add(comparisonItem.RootId, comparisonItem);
                    }
                } 
            }
            return usedCompareDict;
        }
        
        private List<BreadcrumbEntity> BindBreadCrumb(MetaTagsEntity pageMetaTags)
        {
            List<BreadcrumbEntity> _BreadcrumbEntitylist = new List<BreadcrumbEntity>();
            try
            {
                _BreadcrumbEntitylist.Add(new BreadcrumbEntity { Title = "Carwale", Link = "/comparecars/", Text = "Compare Cars" });
                _BreadcrumbEntitylist.Add(new BreadcrumbEntity { Text = pageMetaTags.Title });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return _BreadcrumbEntitylist;
        }
        private Dictionary<int, List<SimilarCarModels>> GetSimilarComparisons(int featuredVersionId, List<CarWithImageEntity> carDetails, string cwcCookie)
        {
            var similarCars = new Dictionary<int, List<SimilarCarModels>>();
            try
            {
                ICarModels carModelsBL = _container.Resolve<ICarModels>();
                HashSet<int> modelIds = new HashSet<int>();
                foreach (var car in carDetails)
                {
                    int modelId = car.ModelId;
                    int versionId = car.VersionId;
                    if (car.IsNew && versionId != featuredVersionId && !modelIds.Contains(modelId))
                    {
                        var _similarCars = carModelsBL.GetSimilarCarsByModel(modelId, cwcCookie);
                        if (_similarCars != null)
                        {
                            _similarCars.RemoveAll(x => carDetails.Any(y => y.ModelId == x.ModelId));
                            _similarCars = _similarCars.GroupBy(x => x.ModelId).Select(x => x.First()).ToList();
                            if (_similarCars.Count > 0 && !similarCars.ContainsKey(modelId)) similarCars.Add(modelId, _similarCars);
                        }
                        modelIds.Add(modelId);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return similarCars;
        }

        private int CalculateWidth(int featuredVersionID, int VersionCount)
        {
            int tbHeaderWidth = 0;
            try
            {
                tbHeaderWidth = VersionCount == 5 ? 756 : VersionCount == 4 ? (featuredVersionID != -1 ? 633 : 756) : VersionCount == 3 ? 593 :
                                VersionCount == 2 ? 528 : VersionCount == 1 ? 378 : 0;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return tbHeaderWidth;
        }
        private string GetCloseUrl(int versionId, CompareCarsDetailModel carData)
        {
            string url = string.Empty;
            try
            {
                StringBuilder qStr = new StringBuilder("?");
                StringBuilder closeUrl = new StringBuilder(string.Empty);

                int counter = 1;
                bool IsCompareLanding = carData.ValidVersionIds.Count == 2;
                var data = carData.CarDetails.OrderByDescending(x => x.ModelId).ToList();
                for (int l = 0; l < data.Count; l++)
                {
                    if (versionId != data[l].VersionId && data[l].VersionId != carData.FeaturedVersionId)
                    {
                        closeUrl.AppendFormat("{0}-{1}-vs-", Format.FormatSpecial(Convert.ToString(data[l].MakeName)), data[l].MaskingName);
                        qStr.AppendFormat(IsCompareLanding ? "car{0}={1}&" : "c{0}={1}&", counter++, data[l].VersionId);
                    }
                }
                if (closeUrl.Length > 4)
                {
                    closeUrl.Remove(closeUrl.Length - 4, 4);
                }
                qStr.Remove(qStr.Length - 1, 1);

                url = string.Format("/comparecars/{0}/{1}&source={2}", closeUrl, qStr, (int)WidgetSource.CompareCarDetailsCloseCarColumnDesktop);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return url;
        }
        private string GetSummary(CompareCarsDetailModel compareCarsDetailModel)
        {
            StringBuilder summary = new StringBuilder();
            string fristCarName = "";
            string secondCarName = "";
            if (compareCarsDetailModel.CarDetails.Count == 2 || (compareCarsDetailModel.CarDetails.Count == 3 && compareCarsDetailModel.FeaturedVersionId != -1))
            {
                fristCarName = string.Format("{0} {1} {2}", compareCarsDetailModel.CarDetails[0].MakeName, compareCarsDetailModel.CarDetails[0].ModelName, compareCarsDetailModel.CarDetails[0].VersionName);
                secondCarName = string.Format("{0} {1} {2}", compareCarsDetailModel.CarDetails[1].MakeName, compareCarsDetailModel.CarDetails[1].ModelName, compareCarsDetailModel.CarDetails[1].VersionName);
                summary.AppendFormat("Compare {0} {1} vs {2} {3} on the basis of price, offers, detailed tech specs & other features. ", compareCarsDetailModel.CarDetails[0].MakeName, compareCarsDetailModel.CarDetails[0].ModelName, compareCarsDetailModel.CarDetails[1].MakeName, compareCarsDetailModel.CarDetails[1].ModelName);
                if (compareCarsDetailModel.CarDetails[0].PriceOverview.Price > 0 && compareCarsDetailModel.CarDetails[1].PriceOverview.Price > 0)
                {
                    summary.AppendFormat("The price of {0} and {1} starts at Rs.{2} and Rs.{3} respectively. ",fristCarName, secondCarName, Format.GetFormattedPriceV2(compareCarsDetailModel.CarDetails[0].PriceOverview.Price.ToString()), Format.GetFormattedPriceV2(compareCarsDetailModel.CarDetails[1].PriceOverview.Price.ToString()));
                }
                if (!string.IsNullOrWhiteSpace(compareCarsDetailModel.CarData[0].Overview[8].ItemValue) && !string.IsNullOrWhiteSpace(compareCarsDetailModel.CarData[1].Overview[8].ItemValue))
                {
                    summary.AppendFormat("The claimed mileage for the {0} is {1} kmpl and for the {2} is {3} kmpl. ",fristCarName, compareCarsDetailModel.CarData[0].Overview[8].ItemValue, secondCarName, compareCarsDetailModel.CarData[1].Overview[8].ItemValue);
                }
                if (!string.IsNullOrWhiteSpace(compareCarsDetailModel.CarData[0].Overview[4].ItemValue) && !string.IsNullOrWhiteSpace(compareCarsDetailModel.CarData[1].Overview[4].ItemValue))
                {
                    summary.AppendFormat("In technical specifications, {0} is powered by {1} cc engine whereas {2} is powered by {3} cc engine.",fristCarName, compareCarsDetailModel.CarData[0].Overview[4].ItemValue, secondCarName, compareCarsDetailModel.CarData[1].Overview[4].ItemValue);
                }
            }
            return summary.ToString();
        }
    }
}
