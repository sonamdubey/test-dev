
using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Compare;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Compare;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Compare;
using Bikewale.Memcache;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Web;
namespace Bikewale.BindViewModels.Webforms.Compare
{
    /// <summary>
    /// 
    /// </summary>
    public class CompareBikes
    {
        private IBikeModelsCacheRepository<int> _objModelCache = null;
        private IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelMaskingCache = null;
        private IBikeCompareCacheRepository _objCompareCache = null;
        private IBikeCompare _objCompare = null;
        public GlobalCityAreaEntity cityArea = null;
        public bool isPageNotFound, isPermanentRedirect;
        public string redirectionUrl = string.Empty, versionsList;
        public uint versionId1, versionId2;
        public BikeCompareEntity comparedBikes = null;
        public PageMetaTags PageMetas = null;
        public Int64 SponsoredVersionId;

        /// <summary>
        /// 
        /// </summary>
        public CompareBikes()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                        .RegisterType<IBikeCompareCacheRepository, BikeCompareCacheRepository>()
                        .RegisterType<IBikeCompare, BikeCompareRepository>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeCompare, Bikewale.BAL.Compare.BikeComparison>();


                    _objModelCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                    _objModelMaskingCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

                }

                ParseQueryString();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BindViewModels.Webforms.CompareBikes : CompareBikes");
            }

            cityArea = GlobalCityArea.GetGlobalCityArea();
        }

        /// <summary>
        /// Created By : Sushil Kumar on 31st Jan 2017 
        /// Description : To get featured bike versionId,Append sponsored bike to versionsList if exists
        ///               Bind compared bikes along with sponsored bike
        ///               Bind PageMetas for the comparisions pages
        /// </summary>
        protected void BindComparedBikeDetails()
        {
            try
            {
                SponsoredVersionId = _objCompare.GetFeaturedBike(versionsList);
                if (SponsoredVersionId > 0) versionsList = string.Format("{0},{1}", versionsList, SponsoredVersionId);
                comparedBikes = _objCompareCache.DoCompare(versionsList, cityArea.CityId);
                BindPageMetas();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.Compare.CompareBikes.GetCompareBikeDetails");
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Common logic to bind meta tags
        /// </summary>
        private void BindPageMetas()
        {
            PageMetas = new PageMetaTags();

            try
            {
                //PageMetas.Title = string.Format("{0} - BikeWale News", ArticleDetails.Title);
                //PageMetas.ShareImage = Image.GetPathToShowImages(ArticleDetails.OriginalImgUrl, ArticleDetails.HostUrl, Bikewale.Utility.ImageSize._640x348);
                //PageMetas.Description = string.Format("BikeWale coverage on {0}. Get the latest reviews and photos for {0} on BikeWale coverage.", ArticleDetails.Title);
                //PageMetas.CanonicalUrl = string.Format("https://www.bikewale.com/news/{0}-{1}.html", ArticleDetails.BasicId, ArticleDetails.ArticleUrl);
                //PageMetas.NextPageUrl = string.Format("/news/{0}-{1}.html", ArticleDetails.PrevArticle.BasicId, ArticleDetails.PrevArticle.ArticleUrl);
                //PageMetas.PreviousPageUrl = string.Format("/news/{0}-{1}.html", ArticleDetails.NextArticle.BasicId, ArticleDetails.NextArticle.ArticleUrl);
                //PageMetas.AlternateUrl = string.Format("https://www.bikewale.com/m/news/{0}-{1}.html", ArticleDetails.BasicId, ArticleDetails.ArticleUrl);
                //PageMetas.AmpUrl = String.Format("{0}/m/news/{1}-{2}/amp/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, ArticleDetails.ArticleUrl, ArticleDetails.BasicId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.Compare.CompareBikes.BindPageMetas");
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar on 30th Jan 2017
        /// Summary : To get version list from querystring and memcache
        /// </summary>
        protected void ParseQueryString()
        {
            try
            {
                var request = HttpContext.Current.Request;
                string QueryString = request.QueryString.ToString();


                string bike1 = request["bike1"], bike2 = request["bike2"], modelList = HttpUtility.ParseQueryString(QueryString).Get("mo");
                uint.TryParse(bike1, out versionId1); uint.TryParse(bike2, out versionId2);

                if (versionId1 > 0 && versionId2 > 0)
                {
                    versionsList = string.Format("{0},{1}", versionId1, versionId2);
                }
                else if (!string.IsNullOrEmpty(modelList))
                {
                    string[] models = HttpUtility.ParseQueryString(QueryString).Get("mo").Split(',');
                    ModelMaskingResponse objResponse = null;
                    ModelMapping objCache = new ModelMapping();
                    int totalModels = models.Length;

                    for (int iTmp = 0; iTmp < totalModels; iTmp++)
                    {
                        string modelMaskingName = models[iTmp].ToLower();
                        if (!string.IsNullOrEmpty(modelMaskingName) && _objModelMaskingCache != null)
                        {
                            objResponse = _objModelMaskingCache.GetModelMaskingResponse(modelMaskingName);
                        }

                        if (objResponse != null && objResponse.StatusCode == 200)
                        {
                            versionsList += objCache.GetTopVersionId(models[iTmp].ToLower()) + (((iTmp + 1) < totalModels) ? "," : "");
                        }
                        else if (objResponse != null && objResponse.StatusCode == 301)
                        {
                            isPermanentRedirect = true;
                            if (String.IsNullOrEmpty(redirectionUrl))
                                redirectionUrl = request.RawUrl.Replace(models[iTmp].ToLower(), objResponse.MaskingName);
                            else
                                redirectionUrl = redirectionUrl.Replace(models[iTmp].ToLower(), objResponse.MaskingName);
                        }
                        else
                        {
                            isPageNotFound = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.Compare.CompareBikes.ParseQueryString");
            }
        }
        //End of getVersionIdList
    }
}