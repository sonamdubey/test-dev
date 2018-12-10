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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Carwale.Entity.Insurance.Coverfox;
using AutoMapper;
using System.Text;
using Carwale.Interfaces.CarData;
using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces.Geolocation;
using Carwale.Utility;

namespace Carwale.BL.Insurance
{
    public class CW : IInsurance
    {
        protected readonly CacheManager _cache;
        protected static readonly int clientId = 10;
        protected ICarModelCacheRepository _modelCache;
        protected ICarVersionCacheRepository _versionCache;
        protected ICarMakesCacheRepository _makeCache;
        protected IRepository<Cities> _citiesCache;
        protected IGeoCitiesCacheRepository _geoRepo;
        protected readonly bool showInternalQuote = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ShowInsuranceInternalQuote"] ?? "true");
        protected readonly ICommon _common;

        public CW(ICarMakesCacheRepository makeCache,ICarModelCacheRepository modelCache, ICarVersionCacheRepository versionCache,IRepository<Cities> citiesCache,IGeoCitiesCacheRepository geoRepo,ICommon common)
        {
            _cache = new CacheManager();
            _modelCache = modelCache;
            _versionCache = versionCache;
            _makeCache = makeCache;
            _citiesCache = citiesCache;
            _geoRepo = geoRepo;
            _common = common;
        }

        public QuotationDto SubmitLead(InsuranceLead inputs)
        {
            QuotationDto dto = new QuotationDto();

            if (inputs.VersionId > 0 && inputs.CityId > 0 && inputs.CarManufactureYear >= 0)
            {
                var location = _geoRepo.GetStateAndAllCities(inputs.CityId);

                inputs.StateId = location.State.StateId;
                inputs.StateName = location.State.StateName;

                var carDetails = _versionCache.GetVersionDetailsById(inputs.VersionId);

                inputs.ModelId = carDetails.ModelId;
                inputs.MakeId = carDetails.MakeId;

                InsuranceRepository insRepo = new InsuranceRepository();
                //Common insCommon = new Common();

                dto.UniqueId = insRepo.SaveLead(inputs);

                double premium = _common.GetInsurancePremium(inputs.VersionId.ToString(), inputs.CityId.ToString(), inputs.CarManufactureYear);

                if (premium <= 0)
                {
                    dto.ConfirmationStatus = "false";
                    return dto;
                }               
                if (showInternalQuote)
                {
                    int total;
                    int.TryParse(premium.ToString(), out total);
                    dto.Quotation = total.ToString();
                    dto.ConfirmationStatus = "true";
                    return dto;
                }                
            }
            dto.ConfirmationStatus = "false";
            return dto;
        }

        public List<MakeEntity> GetMakes(Application application)
        {
            return Mapper.Map<List<CarMakeEntityBase>, List<MakeEntity>>(_makeCache.GetCarMakesFromLocalCache());
        }

        public List<ModelBase> GetModels(int makeId, Application application)
        {
            var list= _modelCache.GetCarModelsByType("nonfuturistic",makeId);
            return Mapper.Map<List<CarModelEntityBase>, List<ModelBase>>(list);
        }

        public List<VersionBase> GetVersions(int modelId, Application application)
        {
            return Mapper.Map<List<CarVersionEntity>, List<VersionBase>>(_versionCache.GetCarVersionsByType("all", modelId));
        }

        public List<InsuranceCity> GetCities(Application application)
        {
            var list = _citiesCache.GetAll().ToList();
            return Mapper.Map<List<Cities>, List<InsuranceCity>>(list);
        }

        public InsuranceResponse SubmitLeadV2(InsuranceLead inputs)
        {            
            return Mapper.Map<QuotationDto, InsuranceResponse>(SubmitLead(inputs));
        }
    }
}

