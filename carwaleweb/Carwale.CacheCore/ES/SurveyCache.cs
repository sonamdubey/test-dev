using AEPLCore.Cache.Interfaces;
using Carwale.Entity.ES;
using Carwale.Interfaces;
using Carwale.Interfaces.ES;
using Carwale.Notifications;
using System;

namespace Carwale.Cache.ES
{
    public class SurveyCache : ISurveyCache
    {
        private readonly ICacheManager _cacheProvider;
        private ISurveyRepository _surveyRepo;

        public SurveyCache(ICacheManager cacheProvider, ISurveyRepository surveyRepo)
        {
            _cacheProvider = cacheProvider;
            _surveyRepo = surveyRepo;
        }
        
        public ESSurveyEnity GetSurveyQuestions(int campaignId)
        {
            string key = "ESSurvey_v2_" + campaignId ;
            var CacheObj = _cacheProvider.GetFromCache<ESSurveyEnity>(key);

            try
            {
                if (CacheObj == null)
                {
                    CacheObj = _surveyRepo.GetSurveyQuestionAnswers(campaignId);

                    if (CacheObj != null)
                    {
                        TimeSpan cacheDuration = CacheObj.Campaign.EndDate - DateTime.Now;
                        return _cacheProvider.GetFromCache<ESSurveyEnity>(key, cacheDuration, () => CacheObj);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "SurveyCache.GetSurveyQuestions()");
                objErr.LogException();
            }

            return CacheObj;
        }
    }
}
