using Carwale.DTOs.Classified;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.CarValuation;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Classified.CarValuation;
using Carwale.Interfaces.Elastic;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;

namespace Carwale.BL.Classified.CarValuation
{
    public class CarValuation : ICarValuation
    {
        private readonly IValuationRepository _valuationRepo;
        private readonly ICVFinalValueCalculator _valuationCalc;
        private readonly ICarVersionCacheRepository _carVersionsCacheRepo;
        private readonly IUnityContainer _container;
        private readonly IElasticSearchManager _searchManager;
        private static readonly string _elasticIndexName = ConfigurationManager.AppSettings["ElasticIndexName"];

        public CarValuation(IUnityContainer container, ICVFinalValueCalculator valuationCalc, IValuationRepository valuationRepo, ICarVersionCacheRepository carVersionsCacheRepo, IElasticSearchManager searchManager)
        {
            _container = container;
            _valuationCalc = valuationCalc;
            _valuationRepo = valuationRepo;
            _carVersionsCacheRepo = carVersionsCacheRepo;
            _searchManager = searchManager;
        }

        public CarValuationResults GetValuation(ValuationRequest valuationRequest)
        {
            string nearestValuationCity = _valuationRepo.GetNearestValuationCity(valuationRequest.CityID);
            if (!string.IsNullOrEmpty(nearestValuationCity))
            {
                string[] cityIdName = nearestValuationCity.Split(':');
                valuationRequest.CityID = Convert.ToInt32(cityIdName[0]);
                valuationRequest.City = cityIdName[1];
            }

            var valuationResult = CalculateValuation(valuationRequest);
            valuationResult.ValuationId = _valuationRepo.SaveValuationRequest(valuationRequest, valuationResult);
            return valuationResult;
        }

        public CarValuationResults CalculateValuation(ValuationRequest valuationRequest)
        {
            _valuationCalc.GetFinalValues(valuationRequest);
            var valuationResult = new CarValuationResults()
            {
                IndividualValueFair = Convert.ToUInt32(_valuationCalc.GetFairValueIndividual()),
                IndividualValueGood = Convert.ToUInt32(_valuationCalc.GetGoodValueIndividual()),
                IndividualValueExcellent = Convert.ToUInt32(_valuationCalc.GetExcellentValueIndividual()),
                IndividualValuePoor = Convert.ToUInt32(_valuationCalc.GetPoorValueIndividual()),

                DealerValueFair = Convert.ToUInt32(_valuationCalc.GetFairSaleValueDealer()),
                DealerValueGood = Convert.ToUInt32(_valuationCalc.GetGoodSaleValueDealer()),
                DealerValueExcellent = Convert.ToUInt32(_valuationCalc.GetExcellentSaleValueDealer()),

                DealerPurchaseValuePoor = Convert.ToUInt32(_valuationCalc.PoorPurchaseValueDealer()),
                DealerPurchaseValueFair = Convert.ToUInt32(_valuationCalc.FairPurchaseValueDealer()),
                DealerPurchaseValueGood = Convert.ToUInt32(_valuationCalc.GoodPurchaseValueDealer()),
                DealerPurchaseValueExcellent = Convert.ToUInt32(_valuationCalc.ExcellentPurchaseValueDealer())
            };
            return valuationResult;
        }

        public ResultsRecommendation GetValuationSuggestions(ValuationRequest valuationRequest, CarValuationResults valuationResults)
        {
            CarVersionDetails versionDetails = _carVersionsCacheRepo.GetVersionDetailsById(valuationRequest.VersionId);

            FilterInputs filterInputs = new FilterInputs();
            filterInputs.city = valuationRequest.CityID.ToString();
            string budgetInLakhs = (valuationResults.DealerValueGood / 100000.0).ToString();
            filterInputs.budget = budgetInLakhs + "-" + budgetInLakhs;
            filterInputs.kms = "0-" + Math.Round(valuationRequest.KmsTraveled / 1000.0);
            filterInputs.car = versionDetails.MakeId + "." + versionDetails.RootId;
            filterInputs.bodytype = versionDetails.BodyStyleId.ToString();
            filterInputs.subSegmentID = versionDetails.SubSegmentId.ToString();

            return _searchManager.SearchIndex<ResultsRecommendation>(_elasticIndexName, filterInputs);
        }
    }
}
