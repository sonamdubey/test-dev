using AutoMapper;
using Carwale.BL.Campaigns;
using Carwale.BL.PriceQuote;
using Carwale.Cache.Campaigns;
using Carwale.Cache.CarData;
using Carwale.Cache.Core;
using Carwale.Cache.Geolocation;
using Carwale.Cache.PriceQuote;
using Carwale.DAL.Campaigns;
using Carwale.DAL.Geolocation;
using Carwale.DAL.PriceQuote;
using Carwale.DTOs.CarData;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.CarData
{
    /// <summary>
    /// Created By : Shalini
    /// </summary>
    public class CarModelsBL : ICarModels
    {
        protected readonly ICarModelCacheRepository _modelsCacheRepo;
        protected readonly IUnityContainer _container;

        public CarModelsBL(IUnityContainer container,ICarModelCacheRepository modelsCacheRepo)
        {
            _modelsCacheRepo = modelsCacheRepo;
            _container = container;
        }

        /// <summary>
        /// Returns the List of New Cars 
        /// Written By : Shalini Nair on 19/09/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns>List of New CarModels </returns>
        public List<CarModelSummary> GetNewModelsByMake(int makeId, int dealerId = 0)
        {
            var newModelsList = new List<CarModelSummary>();
            try
            {
                newModelsList = _modelsCacheRepo.GetModelsByMake(makeId, dealerId).FindAll(x => x.New == true);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelsBL.GetNewModelsByMake()");
                objErr.LogException();
            }
            return newModelsList;
        }

        /// <summary>
        /// Returns the List of Discontinued Cars
        /// Written By : Shalini Nair on 19/09/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns>List of Discontinued CarModels</returns>
        public List<CarModelSummary> GetDiscontinuedModelsByMake(int makeId)
        {
            var discontinuedModelsList = new List<CarModelSummary>();
            try
            {
                discontinuedModelsList = _modelsCacheRepo.GetModelsByMake(makeId).FindAll(x => x.New == false);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelsBL.GetDiscontinuedModelsByMake()");
                objErr.LogException();
            }
            return discontinuedModelsList;
        }

        /// <summary>
        /// Sets IsFeatured=1 for Sponsored Car
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>List of SimilarCar models</returns>
        public List<SimilarCarModels> GetSimilarCarsByModel(int modelId)
        {
            List<SimilarCarModels> similarCars = null;
            try
            {
                similarCars = _modelsCacheRepo.GetSimilarCarModelsByModel(modelId);

                foreach (var x in similarCars)
                {
                    if (x.FeaturedModelId == x.ModelId) // Check if its a featured car
                    {
                        x.IsFeatured = 1;
                    }
                }
            }

            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelsBL.GetSimilarCarsByModel()");
                objErr.LogException();
                similarCars = new List<SimilarCarModels>();
            }
            return similarCars;
        }

        /// <summary>
        /// Returns car model specs 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public List<CarModelSpecs> GetProcessedCarModelSpecs(int modelId)
        {
            var carSpecsList = new List<CarModelSpecs>();
            try
            {
                carSpecsList = _modelsCacheRepo.GetCarModelSpecs(modelId);

                foreach (var x in carSpecsList)
                {
                    x.ItemValue = x.ItemValue.Replace("~", "");  // Formatting the data 
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelsBL.GetProcessedCarModelSpecs()");
                objErr.LogException();
            }
            return carSpecsList;
        }

        /// <summary>
        /// Returns the heading i.e MakeModel Name 
        /// </summary>
        /// <param name="heading">Heading in Page Metatags table</param>
        /// <param name="makeName">MakeName</param>
        /// <param name="modelName">ModelName</param>
        /// <returns></returns>
        public string Heading(string heading, string makeName, string modelName)
        {
            if (!string.IsNullOrEmpty(heading))
                return heading;
            else
                return makeName + " " + modelName;
        }

        /// <summary>
        /// Returns the Model Summary mentioning no. of versions, engines, fueltypes etc.
        /// Written By :Shalini on 09/10/14
        /// </summary>
        /// <param name="summary">Summary from Page Meta Tags </param>
        /// <param name="makeName"></param>
        /// <param name="modelName"></param>
        /// <param name="processVersionData"></param>
        /// <returns></returns>
        public string Summary(string summary, string makeName, string modelName, int[] processVersionData)
        {
            if (!string.IsNullOrEmpty(summary))
                return summary;
            else
                return makeName + " " + modelName + " comes in following " + processVersionData[0].ToString() + " versions with " + processVersionData[1].ToString()
                    + " engine and " + processVersionData[2].ToString() + " transmission and "
                    + processVersionData[3].ToString() + " fuel options. Click on a "
                    + modelName + " version name to know on-road price in your city, specifications and features.";
        }

        /// <summary>
        /// Returns the Title for Model Details page 
        /// </summary>
        /// <param name="title">Title from Page Meta Tags</param>
        /// <param name="makeName"></param>
        /// <param name="modelName"></param>
        /// <param name="futuristic"></param>
        /// <returns></returns>
        public string Title(string title, string makeName, string modelName, bool futuristic)
        {
            if (!string.IsNullOrEmpty(title)) // title from Page meta tags 
                return title;
            else
            {
                if (futuristic)
                    return makeName + " " + modelName + " in India-Know Price, Launch Date of " + modelName; // default title for upcoming cars
                else
                    return makeName + " " + modelName + " Price in India, Photos & Review";// default title for new cars
            }
        }

        /// <summary>
        /// Returns the Keywords for Model Details page 
        /// </summary>
        /// <param name="keyword">Keyword from Page Meta tags</param>
        /// <param name="makeName"></param>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public string Keywords(string keyword, string makeName, string modelName)
        {
            if (!string.IsNullOrEmpty(keyword)) // keywords from Page Meta Tags
                return keyword;
            else
                return makeName + " " + modelName + ", " + modelName + " car, buy " + makeName + " car, "
                    + modelName + " India, new " + modelName + " price, car reviews, features";
        }

        public List<CarMakeModelEntityBase> GetAllModels(string type)
        {
            throw new NotImplementedException();
        }

        public List<CarModelEntityBase> GetCarModelsByType(string type, int makeId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the User Reviews List of Car Model 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<UserReviews> GetUserReviews(int modelId, int count)
        {
            var userReviewsList = new List<UserReviews>();
            try
            {
                userReviewsList = _modelsCacheRepo.GetUserReviewsByModel(modelId, count);
                foreach (var x in userReviewsList)
                {
                    x.FormattedDateTime = x.EntryDateTime.ConvertDateToDays();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelsBL.GetUserReviews()");
                objErr.LogException();
            }
            return userReviewsList;
        }

        #region FRQ - Frequently Requested Queries
        /// <summary>
        /// Written By : Ashwini Todkar on 24 July 2015
        /// Method to get pagewise top selling car models
        /// </summary>
        /// <param name="page">pageno = current page,pagesize = number of records for that page</param>
        /// <returns></returns>
        public List<TopSellingCarModel> GetTopSellingCarModels(Pagination page)
        {
            List<TopSellingCarModel> _topSell = null;

            try
            {
                var _allCar = _modelsCacheRepo.GetTopSellingModels(50);

                ushort _startIndex, _endIndex;
                Calculation.GetStartEndIndex(page.PageNo, page.PageSize, out _startIndex, out _endIndex);

                _topSell = _allCar.Where((x, i) => i >= _startIndex - 1 && i < _endIndex).ToList<TopSellingCarModel>();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelsBL.GetTopSellingCarModels()\n Exception : " + ex.Message);
                objErr.LogException();
            }

            return _topSell;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 24 July 2015
        /// Method to get pagewise top newly launched car models
        /// </summary>
        /// <param name="page">pageno = current page,pagesize = number of records for that page</param>
        /// <returns></returns>
        public List<LaunchedCarModel> GetLaunchedCarModels(Pagination page)
        {
            List<LaunchedCarModel> _topLauches = null;

            try
            {
                var _allCar = _modelsCacheRepo.GetLaunchedCarModels(50);

                ushort _startIndex, _endIndex;
                Calculation.GetStartEndIndex(page.PageNo, page.PageSize, out _startIndex, out _endIndex);

                _topLauches = _allCar.Where((x, i) => i >= _startIndex - 1 && i < _endIndex).ToList<LaunchedCarModel>();

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelsBL.GetLaunchedCarModels()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return _topLauches;
        }

        /// Written By : Ashwini Todkar on 24 July 2015
        /// Method to get pagewise top upcoming car models
        public List<UpcomingCarModel> GetUpcomingCarModels(Pagination page)
        {
            List<UpcomingCarModel> _topUpcoming = null;

            try
            {
                var _allCar = _modelsCacheRepo.GetUpcomingCarModelsByMake(0, 100);

                ushort _startIndex, _endIndex;
                Calculation.GetStartEndIndex(page.PageNo, page.PageSize, out _startIndex, out _endIndex);

                _topUpcoming = _allCar.Where((x, i) => i >= _startIndex - 1 && i < _endIndex).ToList<UpcomingCarModel>();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelsBL.GetUpcomingCarModels()\n Exception : " + ex.Message);
                objErr.LogException();
            }

            return _topUpcoming;
        }
        #endregion

        /// <summary>
        /// Created By : Chetan T. 
        /// Returns the list of similar models based on city and no. of Recommendation
        /// </summary>
        /// <returns></returns>

        public List<SimilarModelRecommendation> GetModelRecommendations(int modelId, int cityId, int noOfRecommendation)
        {
            List<SimilarModelRecommendation> campaignRecommendations = new List<SimilarModelRecommendation>();

            Mapper.CreateMap<CarModelDetails, CarModelsDTO>();
            Mapper.CreateMap<CarModelDetails, CarImageBaseDTO>()
                .ForMember(x => x.OriginalImgPath, o => o.MapFrom(s => s.OriginalImage));
            Mapper.CreateMap<CarModelDetails, CarMakesDTO>();
            Mapper.CreateMap<ModelPriceDTO, CarPricesDTO>();
            Mapper.CreateMap<CustLocation, CustLocationDTO>();

            try
            {
                List<CarModelDetails> similarModels = GetSimilarModelsOnBodyStyle(modelId);

                IUnityContainer container = new UnityContainer();
                container.RegisterType<IPrices, CarPrices>()
                    .RegisterType<IPQCacheRepository, PQCacheRepository>()
                    .RegisterType<IPQRepository, PQRepository>()
                    .RegisterType<IGeoCitiesCacheRepository, GeoCitiesCacheRepository>()
                    .RegisterType<IGeoCitiesRepository, GeoCitiesRepository>()
                    .RegisterType<ICacheProvider, MemcacheManager>();
                var prices = container.Resolve<IPrices>();
                var geoCitiesCacheRepository = container.Resolve<IGeoCitiesCacheRepository>();

                if (similarModels != null)
                {
                    //if (similarModels.Count() < noOfRecommendation)
                    //    noOfRecommendation = similarModels.Count();
                    for (int modelIndex = 0; modelIndex < similarModels.Count(); modelIndex++)
                    {
                        SimilarModelRecommendation campaignRecommendation = new SimilarModelRecommendation();
                        var ModelVersionPrices = prices.GetOnRoadPrice(similarModels[modelIndex].ModelId, cityId);
                        campaignRecommendation.CarModel = Mapper.Map<CarModelDetails, CarModelsDTO>(_modelsCacheRepo.GetModelDetailsById(similarModels[modelIndex].ModelId)); // To get Model Detail
                        campaignRecommendation.CarImageBase = Mapper.Map<CarModelDetails, CarImageBaseDTO>(_modelsCacheRepo.GetModelDetailsById(similarModels[modelIndex].ModelId)); // To get Model Image
                        campaignRecommendation.CarMake = Mapper.Map<CarModelDetails, CarMakesDTO>(_modelsCacheRepo.GetModelDetailsById(similarModels[modelIndex].ModelId)); // To get Make Name
                        campaignRecommendation.CarPrices = Mapper.Map<ModelPriceDTO, CarPricesDTO>(ModelVersionPrices); // To get On Road price
                        if (ModelVersionPrices != null)
                        campaignRecommendation.CarPrices.BaseVersionOnRoadPrice = prices.GetMinOnRoadPrice(ModelVersionPrices.Versions);//To get Min Price of Version List
                        campaignRecommendation.CustLocation = Mapper.Map<CustLocation, CustLocationDTO>(geoCitiesCacheRepository.GetCustLocation(cityId, "")); // To get City Name
                        
                        if (campaignRecommendation.CarModel != null && campaignRecommendation.CarImageBase != null && campaignRecommendation.CarMake != null &&
                            campaignRecommendation.CarPrices != null && campaignRecommendation.CustLocation != null && campaignRecommendation.CarPrices.MinPrice > 0)
                        {
                            campaignRecommendations.Add(campaignRecommendation);
                        }
                        if (campaignRecommendations.Count == noOfRecommendation)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelsBL.GetModelRecommendations()" + modelId + "CityId" + cityId);
                objErr.LogException();
            }
            return campaignRecommendations;
        }

        /// <summary>
        /// Returns the list of similar models based on bodystyle and subsegment
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="subsegmentId"></param>
        /// <returns></returns>

        public List<CarModelDetails> GetSimilarModelsOnBodyStyle(int modelId)
        {
            List<CarModelDetails> similarModelsBySameBodystyle = new List<CarModelDetails>();

            var _campaignCacheRespository = _container.Resolve<ICampaignCacheRepository>();
            var _campaignRecommendationsBL = _container.Resolve<ICampaignRecommendationsBL>();

            try
            {
                var modelDetail = _modelsCacheRepo.GetModelDetailsById(modelId);

                var modelDetailList = _modelsCacheRepo.GetModelsByBodyStyle(modelDetail.BodyStyleId, true).ToList();

                modelDetailList.RemoveAll(item => item.ModelId == modelId);

                _campaignRecommendationsBL.FilterBySubsegment(ref modelDetailList, modelDetail.SubSegmentId, 0); // "0" for similar car same subsegment

                similarModelsBySameBodystyle = modelDetailList.OrderByDescending(p => p.ModelPopularity).ToList();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelBL.GetSimilarModelsOnBodyStyle()" + modelId);
                objErr.LogException();
            }

            return similarModelsBySameBodystyle;
        }
    }
}
