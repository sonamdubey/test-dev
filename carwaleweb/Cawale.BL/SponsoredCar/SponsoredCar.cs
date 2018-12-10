using Campaigns.DealerCampaignClient;
using Carwale.Entity.CarData;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Enum;
using Carwale.Entity.Sponsored;
using Carwale.Interfaces.SponsoredCar;
using Carwale.Notifications;
using Carwale.Utility;
using ProtoBufClass.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.SponsoredCar
{
    public class SponsoredCar : ISponsoredCarBL
    {
        protected readonly ISponsoredCar _sponsoredCar;
        protected readonly ISponsoredCarCache _sponsoredCache;

        public SponsoredCar(ISponsoredCar sponsoredCar, ISponsoredCarCache sponsoredCache)
        {
            _sponsoredCar = sponsoredCar;
            _sponsoredCache = sponsoredCache;
        }
        /// <summary>
        /// Gives VersionId of Sponsored Car based on versionids,categoryId,platformId
        /// categoryId For IsPriceQuote or IsCompare
        /// PlatformId For DesctopWeb,MobileWeb or Android
        /// Written By : Ashish Verma on 24/8/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetFeaturedCar(string versionIds, int categoryId, int platformId)
        {
            return _sponsoredCar.GetFeaturedCar(versionIds, categoryId, platformId);
        }

        public FeaturedCarDataEntity GetFeaturedCarData(string versionIds, int categoryId, int cityId = -1)
        {
            FeaturedCarDataEntity result = new FeaturedCarDataEntity();

            try
            {
                var campaign = DealerCampaignClient.GetSponsoredCarComparision(new SponsoredComparisonInput
                {
                    VersionIds = versionIds,
                    CampaignCategory = categoryId,
                    CityId = cityId
                });

                result = AutoMapper.Mapper.Map<SponsoredCarComparisonData, FeaturedCarDataEntity>(campaign);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "SponsoredCar.GetFeaturedCarData(): " + ex.Message);
                objErr.LogException();
            }


            return result;
        }

        public GlobalSearchSponsoredModelEntity GetSponsoredTrendingCar(int platformId)
        {
            try
            {
                GlobalSearchSponsoredModelEntity trendingModelsData = new GlobalSearchSponsoredModelEntity();
                var campaignList = _sponsoredCache.GetAllSponsoredTrendingCars(platformId);
                if (campaignList != null && campaignList.Count > 0)
                {
                    DateTime now = DateTime.Now;
                    Random rand = new Random();
                    int minPriority = campaignList.Min(item => item.Priority);
                    var sponsoredModelsWithSamePriority = campaignList.Where(c => c.Priority == minPriority && DateTime.Compare(c.StartDate, now) <= 0 && DateTime.Compare(c.EndDate, now) > 0);
                    int toSkip = rand.Next(0, sponsoredModelsWithSamePriority.Count());
                    if (sponsoredModelsWithSamePriority.Count() > 0)
                        trendingModelsData = sponsoredModelsWithSamePriority.ElementAt(toSkip);
                    return trendingModelsData;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelBL.GetTrendingModelDetails()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return null;
        }
        /// <summary>
        /// Get Sponsored models to be shown in History section in global car search
        /// Written By : Piyush Sahu on 08/03/2017
        /// </summary>
        /// <param name="platformId"></param>        
        public SponsoredHistoryModels GetAllSponsoredHistoryModels(string modelIds, int platformId)
        {
            SponsoredHistoryModels historyModelsdata = new SponsoredHistoryModels();
            List<SponsoredHistoryModels> sponsoredHistoryModels = new List<SponsoredHistoryModels>();
            try
            {
                string[] modelList = modelIds.Split(',');
                var allSponsoredHistoryModels = _sponsoredCache.GetAllSponsoredHistoryModels(platformId);
                DateTime now = DateTime.Now;
                if (allSponsoredHistoryModels != null && allSponsoredHistoryModels.Count > 0)
                {
                    foreach (var modelId in modelList)
                    {
                        var sponsoredModelData = allSponsoredHistoryModels.FirstOrDefault(item => item.TargetModelId == CustomParser.parseIntObject(modelId) && DateTime.Compare(item.StartDate, now) <= 0 && DateTime.Compare(item.EndDate, now) > 0);
                        if (sponsoredModelData != null)
                            sponsoredHistoryModels.Add(sponsoredModelData);
                    }
                    if (sponsoredHistoryModels != null && sponsoredHistoryModels.Count > 0)
                    {
                        Random rand = new Random();
                        int minPriority = sponsoredHistoryModels.Min(item => item.Priority);
                        var sponsoredModelsWithSamePriority = sponsoredHistoryModels.Where(item => item.Priority == minPriority);
                        int toSkip = rand.Next(0, sponsoredModelsWithSamePriority.Count());
                        historyModelsdata = sponsoredModelsWithSamePriority.ElementAt(toSkip);                 //fetch random sponsored model if priority is same
                        return historyModelsdata;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "SponsoredCar.GetAllSponsoredHistoryModels()");
                objErr.LogException();
            }
            return null;
        }

        public Sponsored_Car GetDoodle(int platformId)
        {
            var doodleData = _sponsoredCache.GetSponsoredCampaigns((int)CampaignCategory.Doodle, platformId, (int)CategorySection.AllPages);
            if (doodleData == null || doodleData.Count < 1 || string.IsNullOrWhiteSpace(doodleData[0].Ad_Html))
                return null;

            return doodleData[0];
        }
    }
}
