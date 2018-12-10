using Carwale.Entity.Insurance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Carwale.Interfaces.Insurance;
using Carwale.Entity;
using AEPLCore.Cache;
using Carwale.BL.PolicyBoss;
using Carwale.DAL.Insurance;
using System.Text.RegularExpressions;
using System.Web;
using Carwale.DTOs.Insurance;
using System.Globalization;
using Carwale.Entity.Enum;
using Carwale.Notifications;
using Carwale.Entity.CarData;
using AutoMapper;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces;
using Carwale.Entity.Geolocation;
using Microsoft.Practices.Unity;
using Carwale.Interfaces.Geolocation;
using Carwale.Cache.Geolocation;

namespace Carwale.BL.Insurance
{
    public class PolicyBoss : IInsurance
    {
        protected readonly CacheManager _cache;
        private readonly static List<int> states = (System.Configuration.ConfigurationManager.AppSettings["cholastates"] ?? "-2").Split(',').Select(i => int.Parse(i)).ToList();
        private readonly static List<Cities> stateList = new GeoCitiesCache(new CacheManager()).GetAll().Where(city => Chola.states.Contains(city.StateId)).ToList();
        protected readonly short Car_ProductId = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["PolicyBossCar_ProductId"]);
        protected readonly short Bike_ProductId = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["PolicyBossBike_ProductId"]);
        protected readonly ICarModelCacheRepository _carModelCacheRepo;
        protected readonly ICarMakesCacheRepository _carMakeCacheRepo;
        protected readonly ICarVersionCacheRepository _carVersionCacheRepo;
        protected readonly IRepository<Cities> _cities;
        protected readonly IUnityContainer _container;
        private readonly IGeoCitiesCacheRepository _geoCitiescacheRepo;

        public PolicyBoss(ICarModelCacheRepository carModelCacheRepo, ICarMakesCacheRepository carMakeCacheRepo, ICarVersionCacheRepository carVersionCacheRepo, IRepository<Cities> cities, IUnityContainer container, IGeoCitiesCacheRepository geoCitiesCacheRepo)
        {
            _carModelCacheRepo = carModelCacheRepo;
            _carMakeCacheRepo = carMakeCacheRepo;
            _carVersionCacheRepo = carVersionCacheRepo;
            _cities = cities;
            _container = container;
            _geoCitiescacheRepo = geoCitiesCacheRepo;
            _cache = new CacheManager();
        }

        public QuotationDto SubmitLead(InsuranceLead inputs)
        {
            var state = _geoCitiescacheRepo.GetStateByCityId(inputs.CityId);
            inputs.StateName = state.StateName;
            inputs.StateId = state.StateId;
                
            if (inputs.Application == Application.BikeWale)
            {
                return SendToPolicyBoss(inputs);
            }
            else if (states.Contains(inputs.StateId))
            {
                IInsurance _insurance = _container.Resolve<IInsurance>("chola");
                return _insurance.SubmitLead(inputs);
            }
            else
            {
                IInsurance _insurance = _container.Resolve<IInsurance>("CW");
                return _insurance.SubmitLead(inputs);
            }
        }

        public List<MakeEntity> GetMakes(Application application)
        {
            if (application == Application.CarWale)
            {
                List<MakeEntity> carMakes = Mapper.Map<List<CarMakeEntityBase>, List<MakeEntity>>(_carMakeCacheRepo.GetCarMakesFromLocalCache());

                return carMakes;
            }
            else
                return _cache.GetFromCache<List<MakeEntity>>("PolicyBossMakes_" + application, new TimeSpan(15, 0, 0, 0), () => GetMakesFromPolicyBoss(application));//1 for product Carwale
        }


        public List<ModelBase> GetModels(int makeId, Application application)
        {
            if (application == Application.CarWale)
            {
                List<ModelBase> carModels = Mapper.Map<List<CarModelEntityBase>, List<ModelBase>>(_carModelCacheRepo.GetCarModelsByType("nonfuturistic", makeId));
               return carModels;
            }
            else 
                return _cache.GetFromCache<List<ModelBase>>("PolicyBossModels_" + application + "_" + makeId, new TimeSpan(1, 0, 0, 0), () => GetModelsFromPolicyBoss(makeId));
        }



        public List<VersionBase> GetVersions(int modelId, Application application)
        {
            if (application == Application.CarWale)
            {
                List<VersionBase> carVersions = Mapper.Map<List<CarVersionEntity>, List<VersionBase>>(_carVersionCacheRepo.GetCarVersionsByType("all", modelId));

                return carVersions;
            }
            else
                return _cache.GetFromCache<List<VersionBase>>("PolicyBossVersions_" + application + "_" + modelId, new TimeSpan(1, 0, 0, 0), () => GetVersionsFromPolicyBoss(modelId));
        }

        public List<InsuranceCity> GetCities(Application application)
        {
            if (application == Application.CarWale)
            {
                return Mapper.Map<List<Cities>, List<InsuranceCity>>(stateList);
            }
            else
                return _cache.GetFromCache<List<InsuranceCity>>("PolicyBossCities_", new TimeSpan(30,0, 0, 0), () => GetCitiesFromPolicyBoss());
        }

        List<MakeEntity> GetMakesFromPolicyBoss(Application application)
        {          
            var client = new SmartQuoteClient();
            short Product_Id = (int)application == 2 ? Bike_ProductId : Car_ProductId;

            var makes = from clientMakes in client.Get_Make(Product_Id).AsEnumerable()
                    select new MakeEntity()
                             {
                                 MakeId =clientMakes.Field<int>("Make_ID"),
                                 MakeName = clientMakes.Field<string>("Make_Name")
                             };

                client.Close();

                return makes.ToList<MakeEntity>();
        }

        List<ModelBase> GetModelsFromPolicyBoss(int makeId)
        {            
            var client = new SmartQuoteClient();

            var models =  from clientModels in client.get_model(makeId).AsEnumerable()
                    select new ModelBase()
                   {
                       ModelId = clientModels.Field<int>("Model_ID"),
                       ModelName = clientModels.Field<string>("Model_Name")
                   };

            client.Close();

            return models.ToList<ModelBase>();                      
        }

        List<VersionBase> GetVersionsFromPolicyBoss(int modelId)
        {
            var client = new SmartQuoteClient();

            var versions = from clientVersions in client.Get_Variant(modelId).AsEnumerable()
                    select new VersionBase()
                    {
                        VersionId = clientVersions.Field<int>("Variant_ID"),
                        VersionName = clientVersions.Field<string>("Variant_Name"),
                        ExShowroomPrice = clientVersions.Field<int>("ExShoroomPrice")
                    };

            client.Close();

            return versions.ToList<VersionBase>();       
        }

        List<InsuranceCity> GetCitiesFromPolicyBoss()
        {
            var client = new SmartQuoteClient();
            var cities = (from clientCities in client.Get_Vechical_city().AsEnumerable().Distinct(new DataRowComparer())
                          select new InsuranceCity()
                   {
                       CityId = clientCities.Field<Int16>("VehicleCity_Id"),
                       CityName = clientCities.Field<string>("RTO_City"),
                       StateName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(clientCities.Field<string>("State_Name").ToLower())         //First letter should be capital
                   });
            
            client.Close();

            return cities.ToList<InsuranceCity>();
        }


        public InsuranceResponse SubmitLeadV2(InsuranceLead inputs)
        {
            throw new NotImplementedException();
        }   
        private QuotationDto SendToPolicyBoss(InsuranceLead inputs)
        {
            QuotationDto dto = null;

            try
            {
                var client = new SmartQuoteClient();
                inputs.Platform = (int)inputs.Application == 2 ? Platform.BikewaleDesktop : inputs.Platform;
                Credentials credentials = ApiCredentials.GetCredentials(inputs.Platform, inputs.clientId);
                double premiumAmount;
                IInsuranceRepository objInsurance = new InsuranceRepository();
                if (!string.IsNullOrWhiteSpace(inputs.CarPurchaseDate))
                {
                    string[] CarPurchaseDate = Regex.Split(inputs.CarPurchaseDate, "[-/]");
                    inputs.CarManufactureYear = Convert.ToInt32(CarPurchaseDate[0]);
                }
                string Partner_UserId = credentials.UserName;
                string Partner_Password = credentials.Password;

                short Product_Id = (int)inputs.Application == 2 ? Bike_ProductId : Car_ProductId;

                inputs.LeadSource = (int)inputs.Application == 2 ? (int)inputs.Application : (int)inputs.Platform;
                inputs.InsuranceLeadId = objInsurance.SaveLead(inputs);
                string CarType = "1";
                string PolicyExpiryDate = inputs.InsuranceExpDate;
                string DateofPurchaseofCar = inputs.CarPurchaseDate;
                int ManufactureYear = inputs.CarManufactureYear;
                string DOBofOwner = inputs.CustomerDOB;
                int Variant_ID = inputs.VersionId;
                int VehicleCity_Id = inputs.CityId;
                short Preveious_Insurer_Id = inputs.InsuranceNew == true ? (short)1 : (short)0;
                short VD_Amount = 0;
                int PACoverValue = 0;
                int IDVinExpiryPolicy = 0;
                int ExpectedIDV = inputs.Price;
                int ValueOfElectricalAccessories = 0;
                int ValueOfNonElectricalAccessories = 0;
                int ValueOfBiFuelKit = 0;
                byte NoClaimBonusPercentage = inputs.NCBPercent;
                bool IsNCBApplicable = inputs.IsNCBApplicable;
                bool ApplyAntiTheftDiscount = false;
                bool ApplyAutomobileAssociationDiscount = false;
                string AutomobileAssociationName = null;
                string AutomobileAssociationMembershipNumber = null;
                string AutomobileMembershipExpiryDate = null;
                bool PaidDriverCover = true;
                string RegistrationNo = "UP60AVD234";
                string RegisterintheName = "Individual";
                string CityofRegitration = inputs.CityName;
                byte ProfessionofOwner = 0;
                int VehicleType = 1;
                string SessionID = Guid.NewGuid().ToString("N").Substring(0, 10); ;
                long Existing_CustomerReferenceID = 0;
                string ContactName = inputs.Name;
                string ContactEmail = inputs.Email;
                string ContactMobile = inputs.Mobile;
                int SupportsAgentID = 0;
                string CallingPartners_Code = "CWTP";
                long CustomerReferenceID;
                string LandMarkEmployeeCode = null;

                DataSet ds = client.GetMotorQuotePartner(Partner_UserId, Partner_Password, Product_Id, CarType, PolicyExpiryDate, DateofPurchaseofCar, ManufactureYear, DOBofOwner, Variant_ID, VehicleCity_Id, Preveious_Insurer_Id, VD_Amount, PACoverValue, IDVinExpiryPolicy, ExpectedIDV, ValueOfElectricalAccessories, ValueOfNonElectricalAccessories, ValueOfBiFuelKit, NoClaimBonusPercentage, IsNCBApplicable, ApplyAntiTheftDiscount, ApplyAutomobileAssociationDiscount, AutomobileAssociationName, AutomobileAssociationMembershipNumber, AutomobileMembershipExpiryDate, PaidDriverCover, RegistrationNo, RegisterintheName, CityofRegitration, ProfessionofOwner, VehicleType, SessionID, Existing_CustomerReferenceID, ContactName, ContactEmail, ContactMobile, SupportsAgentID, CallingPartners_Code, LandMarkEmployeeCode, out CustomerReferenceID);
                
                client.Close();

                dto = new QuotationDto();

                var dt = ds.Tables["PolicyDetails"];
                if ((int)inputs.Application == 2 && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    long temp = 0;
                    long.TryParse(ds.Tables[0].Rows[0][1].ToString(), out temp);
                    dto.UniqueId = temp;
                    CustomerReferenceID = temp;
                    dto.ConfirmationStatus = ds.Tables[0].Rows[0][0].ToString();
                    inputs.ApiResponse = CustomerReferenceID.ToString();
                }

                premiumAmount = !string.IsNullOrWhiteSpace(dto.Quotation) ? double.Parse(dto.Quotation) : 0;
                objInsurance.UpdateLeadResponse(inputs.InsuranceLeadId, inputs.ApiResponse, premiumAmount);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Insurance.Policyboss.SubmitLead() cityitd: " + inputs.CityId + " cityname : " + inputs.CityName + " version :" + inputs.VersionId + " mobile name email : " + inputs.Name + '-' + inputs.Mobile + '-' + inputs.Email + " lead source : " + inputs.LeadSource + " make model" + inputs.MakeId + '-' + inputs.ModelId);
                objErr.LogException();
            }
            return dto;
        }
    }
    public class DataRowComparer : IEqualityComparer<DataRow>
    {
        public bool Equals(DataRow t1, DataRow t2)
        {
            return ((t1.Field<string>("RTO_City").Trim() == t2.Field<string>("RTO_City").Trim()) && (t1.Field<string>("State_Name").Trim() == t2.Field<string>("State_Name").Trim()));
        }
        public int GetHashCode(DataRow t)
        {
            return t.ToString().GetHashCode();
        }
    }  
}

