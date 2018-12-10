using Bhrigu;
using Carwale.DAL.ApiGateway;
using Carwale.Entity.CarData;
using Carwale.Interfaces.CarData;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using Carwale.DAL.ApiGateway.Extensions.SimilarCar;
using AutoMapper;
using Carwale.BL.Experiments;

namespace Carwale.BL.CarData
{
    public class CarRecommendationLogic : ICarRecommendationLogic
    {
        private readonly ICarModelCacheRepository _modelsCacheRepo;

        public CarRecommendationLogic(ICarModelCacheRepository modelsCacheRepo)
        {
            _modelsCacheRepo = modelsCacheRepo;
        }

        public List<SimilarCarModels> GetSimilarCarsByModel(SimilarCarRequest request)
        {
            try
            {
                var carList = GetSimilarCars(request);
                List<SimilarCarModels> similarCarModels = new List<SimilarCarModels>();
                if (carList != null)
                {
                    foreach (var carId in carList)
                    {
                        similarCarModels.Add(Mapper.Map<SimilarCarModels>(_modelsCacheRepo.GetModelDetailsById(carId)));
                    }
                }
                return similarCarModels;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public List<int> GetSimilarCars(SimilarCarRequest request)
        {
            try
            {
                if (request != null && request.CarId > 0)
                {
                    List<int> similarIds = null;
                    similarIds = GetSimilarNewCarIds(request);
                    return request.Count > 0 ? similarIds?.Take(request.Count)?.ToList() : similarIds;
                }
            }
            catch (Exception ex) when (ex is NullReferenceException && ex is IndexOutOfRangeException)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        private List<int> GetSimilarNewCarIds(SimilarCarRequest request)
        {
            try
            {
                if (request.IsVersion)
                {
                    return null;
                }
                else
                {
                    var _apiGatewayCaller = new ApiGatewayCaller();
                    _apiGatewayCaller.GenerateSimilarCarCallerRequest(request);
                    _apiGatewayCaller.Call();
                    return _apiGatewayCaller.GetResponse<RecommendationResponse>(0)?.Result?.ToList();
                }
            }
            catch (Exception ex) when (ex is NullReferenceException || ex is IndexOutOfRangeException)
            {
                Logger.LogException(ex);
            }
            return null;
        }
    }
}
