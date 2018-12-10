using System;
using System.Collections.Generic;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.CarData;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.Geolocation;
using Carwale.DTOs;
using Carwale.DTOs.PriceQuote;
using System.Web;
using Carwale.DTOs.CarData;
using System.Configuration;

namespace Carwale.BL.PriceQuote
{
    public class PQMobileAndroid<PQMobileT> : IPQ<PQAndroid>
    {
        private readonly IPQRepository _pqRepo;
        private readonly ICarVersionRepository _versionsRepo;
        private readonly IDealerSponsoredAdRespository _newCarDealer;
        private readonly IQueueService<PriceQuoteEntity> _queueService;
        private readonly IPQCacheRepository _cachedRepo;
        private readonly ICarModelRepository _carModels;
        private readonly IGeoCitiesRepository _geocity;

        public PQMobileAndroid(IPQRepository pqRepo,
            ICarVersionRepository versionsRepo,
            IDealerSponsoredAdRespository newCarDealer,
            IQueueService<PriceQuoteEntity> queueService,
            IPQCacheRepository cachedRepo,
            ICarModelRepository carModels,
            IGeoCitiesRepository geoCity)
        {
            _pqRepo = pqRepo;
            _versionsRepo = versionsRepo;
            _newCarDealer = newCarDealer;
            _queueService = queueService;
            _cachedRepo = cachedRepo;
            _carModels = carModels;
            _geocity = geoCity;
        }


        /// <summary>
        /// For getting PriceQuote,All CarVersions,Dealers deatails And All carDetails based on customer datails
        /// </summary>
        /// <param name="pqInputes">Customer Details</param>
        /// <returns>A  List ot type PQ</returns>
        public List<PQAndroid> GetPQ(PQInput pqInputes)
        {
            var pqList = new List<PQAndroid>();

            var pq = _cachedRepo.GetPQ(pqInputes);

            pqList.Add(new PQAndroid()
            {
                InquiryId = pq.PQId,
                priceQuoteList = pq.PriceQuoteList,
                makeId = pq.MakeId,
                MakeName = pq.MakeName,
                modelId = pq.ModelId,
                modelName = pq.ModelName,
                versionId = pq.VersionId,
                versionName = pq.VersionName,
                cityId = pq.CityId,
                maskingName = pq.MaskingName,
                ZoneId = pq.ZoneId,
                sponsoredDealer = _newCarDealer.GetSponsoredDealer(pq.ModelId, pq.CityId, pq.ZoneId),
                otherVersions = _versionsRepo.GetCarVersionsByType("new", pq.ModelId),
                largePicUrl = "http://img.carwale.com/cars/" + pq.LargePic,
                smallPicUrl = "http://img.carwale.com/cars/" + pq.SmallPic,
                cityName = pq.CityName,
                zoneName = pq.ZoneName,
                onRoadPrice = pq.OnRoadPrice,
                otherCities = _geocity.GetPqCitiesByModelId(pq.ModelId),
                alternativeCars = ConvertPocoToDto(pq.ModelId),
                modelDetailUrl = GetAPIHostUrl() + "modeldetails/?budget=" + -1 + "&fuelTypes=" + -1 + "&bodyTypes=" + -1 + "&transmission=" + -1 + "&seatingCapacity=" + -1 + "&enginePower=" + -1 + "&importantFeatures=" + -1 + "&modelId=" + pq.ModelId,
                versionDetailUrl = GetAPIHostUrl() + "Versiondetails?versionId=" + pq.VersionId,
                otherCityUrl = GetAPIHostUrl() + "Pricequote?versionId=" + pq.VersionId + "&preferenceId=" + 10000 + "&name=" + pqInputes.Name + "&emailId=" + pqInputes.Email + "&mobileNo=" + pqInputes.Mobile,
                carName = pq.MakeName + " " + pq.ModelName + " " + pq.VersionName,
                reviewRate = pq.ReviewRate,
                specsSummery = pq.SpecSummery
            });

            // If valid quote generated, push data to queue to initiate PPQ(Post Price Quote Process)
            if (pq.PQId > 0)
            {
                //commented by vikas j on 13/10/2014 as PQ taken should not og to CRM anly form fill be pushed.
                //PostPQProcess.SendDataToRabbitMQ(_queueService, pqInputes, pq.PQId);

                string cwCookieValue = "";
                if (HttpContext.Current.Request.Cookies["CWC"] != null)
                {
                    cwCookieValue = HttpContext.Current.Request.Cookies["CWC"].Value.ToString();
                }
                var userTrackingObj = new PQUserInfoTrackEntity()
                {
                    PQId = pq.PQId,
                    ClientIp = pqInputes.ClientIp,
                    AspSessoinId = "",
                    CWCookievalue = cwCookieValue,
                    EntryDate = DateTime.Now.ToString()
                };

                _pqRepo.TrackClientInfo(userTrackingObj);
            }
            return pqList;
        }

        private List<SimilarCarsAndroidDTO> ConvertPocoToDto(int modelId)
        {
            var similarcarsAndroid = new List<SimilarCarsAndroidDTO>();

            var similarCars = _carModels.GetSimilarCarModelsByModel(modelId);

            foreach (var car in similarCars)
            {
                similarcarsAndroid.Add(new SimilarCarsAndroidDTO
                {
                    make = car.MakeName,
                    model = car.ModelName,
                    minPrice = car.MinPrice,
                    maxPrice = car.MaxPrice,
                    largePicUrl = "http://img3.aeplcdn.com/cars/" + car.LargePic,
                    smallPicUrl = "http://img3.aeplcdn.com/cars/" + car.SmallPic,
                    carModelUrl = GetAPIHostUrl() + "/modeldetails/?budget=" + -1 + "&fuelTypes=" + -1 + "&bodyTypes=" + -1 + "&transmission=" + -1 + "&seatingCapacity=" + -1 + "&enginePower=" + -1 + "&importantFeatures=" + -1 + "&modelId=" + car.ModelId,
                    reviewCount = car.ReviewCount,
                    reviewRate = car.ReviewRate,
                    versionId = car.PopularVersionId
                });
            }

            return similarcarsAndroid;
        }

        public List<PQAndroid> GetPQByIds(string pqIds)
        {
            throw new NotImplementedException();
        }

        private string GetAPIHostUrl()
        {
            return ConfigurationManager.AppSettings["WebApiHostUrl"];
        }
    }
}
