using AutoMapper;
using Carwale.BL.CarWaleCalculatePremium;
using Carwale.BL.RoyalSundaram;
using AEPLCore.Cache;
using Carwale.Cache.Geolocation;
using Carwale.DTOs.Insurance;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Insurance;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Insurance;
using Carwale.Notifications;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.Insurance
{
    public class RoyalSundaram : IInsurance
    {
        private readonly IUnityContainer _container;
        private readonly ICarVersionCacheRepository _carVersionCacheRepository;
        protected readonly ICarMakesCacheRepository _carMakeCacheRepo;
        protected readonly ICarModelCacheRepository _carModelCacheRepo;
        protected readonly ICarVersionRepository _carVersionRepo;
        private readonly static List<Cities> stateList = new GeoCitiesCache(new CacheManager()).GetAll().ToList();

        public RoyalSundaram(IUnityContainer container, ICarVersionCacheRepository carVersionCacheRepository, ICarMakesCacheRepository carMakeCacheRepo, ICarModelCacheRepository carModelCacheRepo, ICarVersionRepository carVersionRepo)
        {
            _carMakeCacheRepo = carMakeCacheRepo;
            _carModelCacheRepo = carModelCacheRepo;
            _container = container;
            _carVersionCacheRepository = carVersionCacheRepository;
            _carVersionRepo = carVersionRepo;
        }
        public QuotationDto SubmitLead(InsuranceLead inputs)
        {
            var quotation = new QuotationDto();
            InsuranceResponse quote = SubmitLeadV2(inputs);
            quotation.UniqueId = Convert.ToInt32(quote.QuoteId);
            quotation.ConfirmationStatus = quote.Success == true ? "true" : "false";
            return quotation;
        }

        public InsuranceResponse SubmitLeadV2(InsuranceLead inputs)
        {
            var quotation = new InsuranceResponse();
            try
            {   
                var _insuranceRepo = _container.Resolve<IInsuranceRepository>("generic");
                inputs.InsuranceLeadId = _insuranceRepo.SaveLead(inputs);

                quotation.QuoteId = inputs.InsuranceLeadId.ToString();

                var carDetails = _carVersionCacheRepository.GetVersionDetailsById(inputs.VersionId);

                inputs.MakeName = carDetails.MakeName;
                inputs.ModelName = carDetails.ModelName;
                inputs.VersionName = carDetails.VersionName;

                string Contact = "Dial a Policy";
                string Products = "Motor";
                string SRCCookie = "Nil";
                string MediumCookie = "Nil";
                string SearchEngine = "Display";
                string ClientName = "Nil";
                string Campaign = "Carwale";
                string AdGroup = "Nil";
                string KeyWord = "Carwale";
                string SearchContent = "Nil";
                string IpAddress = "";
                string ReferralUrl = "";
                string PolicyType = inputs.InsuranceNew ? "New" : "Renewal";
                string car = inputs.MakeName + "_" + inputs.ModelName + "_" + inputs.VersionName;

                inputs.CarPurchaseDate = string.IsNullOrWhiteSpace(inputs.CarPurchaseDate) ? (DateTime.Now.AddDays(30)).ToString("yyyy/MM/dd") : CustomParser.parseDateObject(inputs.CarPurchaseDate).ToString("yyyy/MM/dd");
                inputs.CarManufactureYear = inputs.CarManufactureYear > 0 ? inputs.CarManufactureYear : 2017;

                RoyalServicesSoapClient RsClient = new RoyalServicesSoapClient();
                inputs.ApiResponse = RsClient.GetLeadValues(inputs.Name, inputs.Mobile, inputs.Email, Contact, Products, SRCCookie, MediumCookie, SearchEngine, ClientName,
                                                            Campaign, AdGroup, KeyWord, SearchContent, IpAddress, ReferralUrl, PolicyType, inputs.CarPurchaseDate, inputs.CityName, car, inputs.ModelName, inputs.CarManufactureYear.ToString());

                quotation.Success = inputs.ApiResponse == "success" ? true : false;
                _insuranceRepo.UpdateLeadResponse(inputs.InsuranceLeadId, inputs.ApiResponse);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Insurance.RoyalSundaram.SubmitLead() inputs: " + JsonConvert.SerializeObject(inputs));
                objErr.LogException();
            }
            return quotation;
        }

        public List<MakeEntity> GetMakes(Application application)
        {
            try
            {
                List<MakeEntity> carMakes = Mapper.Map<List<CarMakeEntityBase>, List<MakeEntity>>(_carMakeCacheRepo.GetCarMakesFromLocalCache());

                return carMakes;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Insurance.RoyalSundaramBL.GetMakes()");
                objErr.LogException();
            }
            return null;
        }

        public List<ModelBase> GetModels(int makeId, Application application)
        {
            try
            {
                List<ModelBase> carModels = Mapper.Map<List<CarModelEntityBase>, List<ModelBase>>(_carModelCacheRepo.GetCarModelsByType("nonfuturistic", makeId));
                return carModels;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Insurance.RoyalSundaramBL.GetModels()");
                objErr.LogException();
            }
            return null;
        }

        public List<VersionBase> GetVersions(int modelId, Application application)
        {
            try
            {
                List<VersionBase> carVersions = Mapper.Map<List<CarVersionEntity>, List<VersionBase>>(_carVersionCacheRepository.GetCarVersionsByType("all", modelId));

                return carVersions;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Insurance.RoyalSundaramBL.GetVersions()");
                objErr.LogException();
            }
            return null;
        }

        public List<InsuranceCity> GetCities(Application application)
        {
            try
            {
                return Mapper.Map<List<Cities>, List<InsuranceCity>>(stateList);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Insurance.RoyalSundaramBL.GetCities()");
                objErr.LogException();
            }
            return null;
        }
    }
}
