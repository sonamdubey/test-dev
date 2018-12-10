using AutoMapper;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Deals;
using Carwale.Entity.Enum;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Deals.Cache;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.Prices;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.NewCars
{
    public class CompareCarDetailsAdaptorMobile : IServiceAdapterV2
    {
        private readonly ICarDataLogic _carDataLogic;
        private readonly ICarVersions _carVersionsBL;
        private readonly IEmiCalculatorAdapter _emiCalculatorAdapter;
        public CompareCarDetailsAdaptorMobile(ICarDataLogic carDataLogic, ICarVersions carVersionsBL, IEmiCalculatorAdapter emiCalculatorAdapter)
        {
            _carDataLogic = carDataLogic;
            _carVersionsBL = carVersionsBL;
            _emiCalculatorAdapter = emiCalculatorAdapter;
        }

        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetCompareCarDetailModel<U>(input), typeof(T));
        }

        private CompareDetailsModel GetCompareCarDetailModel<U>(U input)
        {
            CompareDetailsModel compareCarsDetailModel = new CompareDetailsModel();
            try
            {
                var inputval = (CompareCarInputParam)Convert.ChangeType(input, typeof(U));
                var carData = new CCarData();
                int featuredCarVersionId = -1;
                List<int> versionList = inputval.VersionIds;
                carData.ValidVersionIds = versionList;
                var featuredCar = _carDataLogic.GetFeaturedCar(inputval);
                carData.FeaturedCarData = featuredCar;
                if (featuredCar != null && featuredCar.FeaturedVersionId > 0)
                {
                    featuredCarVersionId = featuredCar.FeaturedVersionId;
                    carData.FeaturedVersionId = featuredCar.FeaturedVersionId;
                }
                var vehicleData = _carDataLogic.GetCombinedCarData(versionList);
                var comparisonData = new List<ComparisonDataDto>();
                if (vehicleData != null)
                {
                    for (int i = 0; i < versionList.Count; i++)
                    {
                        var data = AutoMapper.Mapper.Map<CarDataPresentation, ComparisonDataDto>(vehicleData[i]);
                        comparisonData.Add(data);
                    }
                }
                compareCarsDetailModel.ComparisonData = comparisonData;
                carData.Colors = _carVersionsBL.GetVersionsColors(versionList);

                carData.CarDetails = _carDataLogic.GetVersionDetails(versionList, featuredCarVersionId, inputval.CustLocation, Platform.CarwaleMobile);
                compareCarsDetailModel.CarData = carData;

                string vsString = String.Join(" vs ", compareCarsDetailModel.CarData.CarDetails.Select(c => string.Format("{0} {1}", c.MakeName, c.ModelName)));
                compareCarsDetailModel.Title = string.Format("Compare {0}", vsString);
                compareCarsDetailModel.Description = string.Format("Compare {0} at Carwale. Compare {1} Prices, Mileage, Features, Specs, Colours and much more.", vsString, String.Join(" and ", compareCarsDetailModel.CarData.CarDetails.Select(c => c.ModelName)));
                compareCarsDetailModel.TargetLabel = string.Format("|{0}", String.Join("|", compareCarsDetailModel.CarData.CarDetails.Select(c => string.Format("{0}_{1}", c.MaskingName, c.VersionName))));

                compareCarsDetailModel.ShowCampaignSlab = _carDataLogic.CheckForCampaign(compareCarsDetailModel.CarData.CarDetails, inputval.CustLocation);
                compareCarsDetailModel.Location = inputval.CustLocation;
                compareCarsDetailModel.Summary = GetSummary(compareCarsDetailModel);

                foreach (var carDetail in carData.CarDetails)
                {
                    GetEmiCalculatorModelData(carDetail, inputval.CustLocation.CityId, ref compareCarsDetailModel);
                }

                return compareCarsDetailModel;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }

        }

        private void GetEmiCalculatorModelData(CarWithImageEntity carDetail, int cityId, ref CompareDetailsModel compareCarsDetailModel)
        {
            if (carDetail.PriceOverview.PriceStatus != (int)Carwale.Entity.PriceBucket.HaveUserCity)
            {
                compareCarsDetailModel.EmiCalculatorModelData.Add(null);
                return;
            }

            var overview = Mapper.Map<CarWithImageEntity, CarOverviewDTOV2>(carDetail);
            //overview.LocationData = new Entity.Geolocation.Location { CityId = cityId }; // Found no use of it can be removed after proper testing
            int onRoadPrice = carDetail.PriceOverview.Price;

            compareCarsDetailModel.EmiCalculatorModelData.Add(_emiCalculatorAdapter.GetEmiData(overview, null,
                new LeadSourceDTO { LeadClickSourceId = 398 }, onRoadPrice, cityId));
        }
        private static string GetSummary(CompareDetailsModel compareCarsDetailModel)
        {
            StringBuilder summary = new StringBuilder();
            string fristCarName = string.Format("{0} {1} {2}", compareCarsDetailModel.CarData.CarDetails[0].MakeName, compareCarsDetailModel.CarData.CarDetails[0].ModelName,
                 compareCarsDetailModel.CarData.CarDetails[0].VersionName);
            string secondCarName = string.Format("{0} {1} {2}", compareCarsDetailModel.CarData.CarDetails[1].MakeName, compareCarsDetailModel.CarData.CarDetails[1].ModelName,
                 compareCarsDetailModel.CarData.CarDetails[1].VersionName);
            summary.AppendFormat("Compare {0} {1} vs {2} {3} on the basis of price, offers, detailed tech specs & other features. ", compareCarsDetailModel.CarData.CarDetails[0].MakeName,
                compareCarsDetailModel.CarData.CarDetails[0].ModelName, compareCarsDetailModel.CarData.CarDetails[1].MakeName, compareCarsDetailModel.CarData.CarDetails[1].ModelName);
            if (compareCarsDetailModel.CarData.CarDetails[0].PriceOverview.Price > 0 && compareCarsDetailModel.CarData.CarDetails[1].PriceOverview.Price > 0)
            {
                summary.AppendFormat("The price of {0} and {1} starts at Rs.{2} and Rs.{3} respectively. ",fristCarName, secondCarName,
                    Format.GetFormattedPriceV2(compareCarsDetailModel.CarData.CarDetails[0].PriceOverview.Price.ToString()),
                    Format.GetFormattedPriceV2(compareCarsDetailModel.CarData.CarDetails[1].PriceOverview.Price.ToString()));
            }
            if (!string.IsNullOrWhiteSpace(compareCarsDetailModel.ComparisonData[0].Overview[8].Value) && !string.IsNullOrWhiteSpace(compareCarsDetailModel.ComparisonData[1].Overview[8].Value))
            {
                summary.AppendFormat("The claimed mileage for the {0} is {1} kmpl and for the {2} is {3} kmpl. ", fristCarName, compareCarsDetailModel.ComparisonData[0].Overview[8].Value,
                    secondCarName, compareCarsDetailModel.ComparisonData[1].Overview[8].Value);
            }
            if (!string.IsNullOrWhiteSpace(compareCarsDetailModel.ComparisonData[0].Overview[4].Value) && !string.IsNullOrWhiteSpace(compareCarsDetailModel.ComparisonData[1].Overview[4].Value))
            {
                summary.AppendFormat("In technical specifications, {0} is powered by {1} cc engine whereas {2} is powered by {3} cc engine.", fristCarName,
                    compareCarsDetailModel.ComparisonData[0].Overview[4].Value, secondCarName, compareCarsDetailModel.ComparisonData[1].Overview[4].Value);
            }
            return summary.ToString();
        }
    }
}
