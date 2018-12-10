using AutoMapper;
using Carwale.BL.Chola;
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
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.Insurance;
using Carwale.Notifications;
using Carwale.Notifications.MailTemplates;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Carwale.BL.Insurance
{
    public class Chola : IInsurance
    {
        private static bool _mailToChola = Convert.ToBoolean(ConfigurationManager.AppSettings["MailLeadToChola"] ?? "false");

        public static List<int> states = (System.Configuration.ConfigurationManager.AppSettings["cholastates"] ?? "-2").Split(',').Select(i => int.Parse(i)).ToList();        
        private readonly IUnityContainer _container;
        private readonly ICarVersionCacheRepository _carVersionCacheRepository;
        protected readonly ICarMakesCacheRepository _carMakeCacheRepo;
        protected readonly ICarModelCacheRepository _carModelCacheRepo;
        protected readonly ICarVersionRepository _carVersionRepo;
        public static List<Cities> stateList = new GeoCitiesCache(new CacheManager()).GetAll().ToList();

        public Chola(IUnityContainer container, ICarVersionCacheRepository carVersionCacheRepository, ICarMakesCacheRepository carMakeCacheRepo, ICarModelCacheRepository carModelCacheRepo, ICarVersionRepository carVersionRepo)
        {
            _container = container;
            _carVersionCacheRepository = carVersionCacheRepository;
            _carMakeCacheRepo = carMakeCacheRepo;
            _carModelCacheRepo = carModelCacheRepo;
            _carVersionRepo = carVersionRepo;
        }

        public QuotationDto SubmitLead(InsuranceLead inputs)
        {
            var quotation = new QuotationDto();
            var quote = new InsuranceResponse();
            try
            {
                quote = SubmitLeadV2(inputs);
                quotation.UniqueId = Convert.ToInt32(quote.QuoteId);
                quotation.ConfirmationStatus = quote.Success == true ? "true" : "false";
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Insurance.Chola.SubmitLead() inputs: " + JsonConvert.SerializeObject(inputs));
                objErr.LogException();
            }
            return quotation;
        }

        public InsuranceResponse SubmitLeadV2(InsuranceLead inputs)
        {
            var response = new InsuranceResponse();
            try
            {                                                

                var location = _container.Resolve<IGeoCitiesCacheRepository>().GetStateByCityId(inputs.CityId);

                inputs.StateId = location.StateId;
                inputs.StateName = location.StateName;

                var carDetails = _carVersionCacheRepository.GetVersionDetailsById(inputs.VersionId);

                inputs.ModelId = carDetails.ModelId;
                inputs.MakeId = carDetails.MakeId;
                inputs.MakeName = carDetails.MakeName;
                inputs.ModelName = carDetails.ModelName;
                inputs.VersionName = carDetails.VersionName;
                string publisher = "Carwale";
                string productCode = "Car Insurance";
                string policyType = inputs.InsuranceNew ? "New Car" : "Rollover";
                string modelVariant = inputs.ModelName + "," + inputs.VersionName;
                string Vehicleno = "";
                string RTO = "";

                var _insuranceRepo = _container.Resolve<IInsuranceRepository>("generic");

                inputs.InsuranceLeadId = _insuranceRepo.SaveLead(inputs);

                response.QuoteId = inputs.InsuranceLeadId.ToNullSafeString();
                inputs.CarPurchaseDate = string.IsNullOrWhiteSpace(inputs.CarPurchaseDate) ? (DateTime.Now.AddDays(7)).ToString("dd/MM/yyyy") : CustomParser.parseDateObject(inputs.CarPurchaseDate).ToString("dd/MM/yyyy");

                if (states.Contains(inputs.StateId))
                {
                    CholaProductServiceSoapClient CholaClient = new CholaProductServiceSoapClient();                   
                    inputs.ApiResponse = CholaClient.CholaCampaignService(inputs.Name, inputs.Email, inputs.Mobile, inputs.MakeName, modelVariant, policyType, inputs.CarPurchaseDate, Vehicleno, RTO, publisher, productCode);
                    response.Success = inputs.ApiResponse == "success";
                }
                else
                    inputs.ApiResponse = "Other than Chola States";

                _insuranceRepo.UpdateLeadResponse(inputs.InsuranceLeadId, inputs.ApiResponse);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Insurance.Chola.SubmitLeadV2() inputs: " + JsonConvert.SerializeObject(inputs));
                objErr.LogException();
            }
            return response;
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
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Insurance.Chola.GetMakes()");
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
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Insurance.Chola.GetModels()");
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
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Insurance.Chola.GetVersions()");
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
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Insurance.Chola.GetCities()");
                objErr.LogException();
            }
            return null;
        }

        private int CalculateNoClaimBonus(int regYear)
        {
            int insNoClaimBonus = 0;

            switch (System.DateTime.Now.Year - regYear)
            {
                case 0:
                    insNoClaimBonus = 0;
                    break;
                case 1:
                    insNoClaimBonus = 20;
                    break;
                case 2:
                    insNoClaimBonus = 25;
                    break;
                case 3:
                    insNoClaimBonus = 35;
                    break;
                case 4:
                    insNoClaimBonus = 45;
                    break;
                case 5:
                    insNoClaimBonus = 50;
                    break;
                case 6:
                    insNoClaimBonus = 55;
                    break;
                default:
                    insNoClaimBonus = 65;
                    break;
            }

            return insNoClaimBonus;
        }

        private void SendMailToChola(InsuranceLead inputs)
        {
            try
            {
                var clientMailEntity = new ClientMailEntity();
                clientMailEntity.Car = new CarNameEntity();
                clientMailEntity.Customer = new CustomersBasicInfo();

                clientMailEntity.ClientEmails = ConfigurationManager.AppSettings["CholaMailReceipient"] ?? string.Empty;
                clientMailEntity.InsuranceType = inputs.InsuranceNew ? "New" : "Renew";
                clientMailEntity.RegistrationDate = inputs.InsuranceNew ? "N.A" : inputs.CarRegDate;
                clientMailEntity.Car.MakeName = inputs.MakeName;
                clientMailEntity.Car.ModelName = inputs.ModelName;
                clientMailEntity.Car.VersionName = inputs.VersionName;
                clientMailEntity.Customer.City = inputs.CityName;
                clientMailEntity.Customer.State = inputs.StateName;
                clientMailEntity.Customer.Name = inputs.Name;
                clientMailEntity.Customer.Mobile = inputs.Mobile;
                clientMailEntity.Customer.Email = inputs.Email;
                clientMailEntity.LeadId = inputs.InsuranceLeadId;
                var email = new InsuranceMailTemplate().GetInsuranceEmailTemplate(clientMailEntity);
                new Email().SendMail(email.Email, email.Subject, email.Body);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CholaBL.SendMailToChola()" + inputs.CityId + " cityname : " + inputs.CityName + " version :" + inputs.VersionId + " mobile name email : " + inputs.Name + '-' + inputs.Mobile + '-' + inputs.Email + " lead source : " + inputs.LeadSource + " make model" + inputs.MakeId + '-' + inputs.ModelId);
                objErr.LogException();
            }
        }
    }
}

