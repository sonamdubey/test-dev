using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.LandingPage;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.LandingPage;
using Carwale.Notifications;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Carwale.BL.TestDrive
{
    public class LandingPageAdapter : ILandingPageBL
    {
        private readonly ILandingPageCacheRepo _lpCache;
        private readonly ICarModelCacheRepository _carModelscache;
        private readonly IRepository<Cities> _cities;
        private readonly IGeoCitiesCacheRepository _geoCache;
        private readonly IPhotos _photosCacheRepo;
        private static readonly List<int> _versaTagCampaigns = ConfigurationManager.AppSettings["VersaTagCampaigns"].Split(',').Select(int.Parse).ToList();
        public LandingPageAdapter(ILandingPageCacheRepo lpCache, ICarModelCacheRepository carModelscache, IRepository<Cities> cities, IGeoCitiesCacheRepository geoCache, IPhotos photosCacheRepo)
        {
            _lpCache = lpCache;
            _carModelscache = carModelscache;
            _cities = cities;
            _geoCache = geoCache;
            _photosCacheRepo = photosCacheRepo;
        }

        public LandingPageDTO Get(int campaignId, int? modelId)
        {
            LandingPageDTO LpDTO = new LandingPageDTO();
            var Models = new List<CarModelSummary>();
            var Cities = new List<Cities>();
            LpDTO.PhotoGalleryDTO = new PhotoGalleryDTO();
            try
            {
                var LandingPageDetail = _lpCache.GetLandingPageDetails(campaignId);
                LpDTO.CampaignDetails = LandingPageDetail.Item1;
                LpDTO.CampaignDetails.Id = campaignId;

                var DefaultModel = CustomParser.parseIntObject(LpDTO.CampaignDetails.DefaultModel);

                if (DefaultModel > 0)
                {
                    LpDTO.DefaultModelsDetails = _carModelscache.GetModelDetailsById(DefaultModel);
                }

                try
                {
                    var ModelList = LandingPageDetail.Item2;

                    foreach (var model in ModelList)
                    {
                        if (model.ModelId == -1)
                        {
                            Models.AddRange(_carModelscache.GetModelsByMake(model.MakeId).Where(i => i.New == true));
                        }
                        else
                        {
                            var ModelDetails = _carModelscache.GetModelDetailsById(model.ModelId);
                            Models.Add(Mapper.Map<CarModelDetails, CarModelSummary>(ModelDetails));
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler err = new ExceptionHandler(ex, "LandingPageAdapter.ModelDetails");
                    err.LogException();
                }

                if (CustomParser.parseIntObject(modelId) > 0 && Models.Any(x => x.ModelId == modelId))
                {
                    DefaultModel = (int)modelId;
                    LpDTO.CampaignDetails.DefaultModel = DefaultModel;
                    LpDTO.DefaultModelsDetails = _carModelscache.GetModelDetailsById(DefaultModel);
                }

                try
                {
                    var CityList = LandingPageDetail.Item3;
                    foreach (var city in CityList)
                    {
                        if (city.StateId == -1)
                        {
                            Cities.AddRange(_cities.GetAll());
                        }

                        else if (city.CityId == -1)
                        {
                            var CitiesByState = _geoCache.GetCitiesByStateId(city.StateId);
                            Cities.AddRange(Mapper.Map<List<City>, List<Cities>>(CitiesByState));

                        }
                        else
                        {
                            var CityName = _geoCache.GetCityNameById(city.CityId.ToString());
                            Cities.Add(new Cities()
                            {
                                CityId = city.CityId,
                                CityName = CityName
                            });
                        }
                        if (Cities.Count > 0)
                            Cities =  Cities.OrderBy(x => x.CityName).ToList();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler err = new ExceptionHandler(ex, "LandingPageAdapter.CityDetails");
                    err.LogException();
                }


                ModelPhotosBycountURI galleryParam = new ModelPhotosBycountURI
                {
                    ApplicationId = 1,//Carwale
                    ModelId = DefaultModel,
                    CategoryIdList = "8,10",//All Photos
                    TotalRecords = 500
                };

                LpDTO.Models = Models;
                LpDTO.Cities = Cities;
                LpDTO.PhotoGalleryDTO.modelImages = _photosCacheRepo.GetModelPhotosByCount(galleryParam);

                LpDTO.IncludeVersaTag = (_versaTagCampaigns.IndexOf(LpDTO.CampaignDetails.PQCampaignId) > -1);
            }
            catch (Exception ex)
            {
                ExceptionHandler err = new ExceptionHandler(ex, "LandingPageAdapter.Get");
                err.LogException();
            }
            return LpDTO;
        }

    }//class
}//namespace
