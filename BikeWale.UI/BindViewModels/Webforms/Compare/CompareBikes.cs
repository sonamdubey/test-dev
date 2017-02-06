
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
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Bikewale.BindViewModels.Webforms.Compare
{
    /// <summary>
    /// Created By :  Sushil kumar on 2nd Feb 2017 
    /// Description : ViewModel for compare bikes for both desktop and mobile
    /// </summary>
    public class CompareBikes
    {
        private IBikeMakesCacheRepository<int> _objMakeCache = null;
        private IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelMaskingCache = null;
        private IBikeCompareCacheRepository _objCompareCache = null;
        private IBikeCompare _objCompare = null;

        public GlobalCityAreaEntity cityArea = null;
        public bool isPageNotFound, isPermanentRedirect, isUsedBikePresent;
        public string redirectionUrl = string.Empty, versionsList, bike1Name = string.Empty, bike2Name = string.Empty, ComparisionText = string.Empty, TargetedModels = string.Empty;
        public uint versionId1, versionId2;
        public BikeCompareEntity comparedBikes = null;
        public PageMetaTags PageMetas = null;
        public Int64 SponsoredVersionId;
        public ICollection<BikeCompareEntity> objBikes = null;
        public ICollection<BikeMakeEntityBase> makes = null;

        /// <summary>
        /// Created By : Sushil kumar on 2nd Feb 2017 
        /// Description : Constructor to resolve unity containers and initialize model
        /// </summary>
        public CompareBikes()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                        .RegisterType<IBikeCompareCacheRepository, BikeCompareCacheRepository>()
                        .RegisterType<IBikeCompare, BikeCompareRepository>()
                        .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakes<BikeMakeEntity, int>>()
                        .RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeCompare, Bikewale.BAL.Compare.BikeComparison>();


                    _objModelMaskingCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                    _objCompareCache = container.Resolve<IBikeCompareCacheRepository>();
                    _objCompare = container.Resolve<IBikeCompare>();
                    _objMakeCache = container.Resolve<IBikeMakesCacheRepository<int>>();

                }

                ParseQueryString();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BindViewModels.Webforms.CompareBikes : CompareBikes");
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 31st Jan 2017 
        /// Description : To get featured bike versionId,Append sponsored bike to versionsList if exists
        ///               Bind compared bikes along with sponsored bike
        ///               Bind PageMetas for the comparisions pages
        /// </summary>
        public void GetComparedBikeDetails()
        {
            try
            {
                cityArea = GlobalCityArea.GetGlobalCityArea();

                SponsoredVersionId = _objCompare.GetFeaturedBike(versionsList);

                if (SponsoredVersionId > 0) versionsList = string.Format("{0},{1}", versionsList, SponsoredVersionId);

                comparedBikes = _objCompareCache.DoCompare(versionsList, cityArea.CityId);

                if (comparedBikes != null && comparedBikes.BasicInfo != null)
                {
                    makes = _objMakeCache.GetMakesByType(EnumBikeType.New).ToList();
                    GetComparisionTextAndMetas();
                    isUsedBikePresent = comparedBikes.BasicInfo.FirstOrDefault(x => x.UsedBikeCount.BikeCount > 0) != null;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.Compare.CompareBikes.GetCompareBikeDetails");
                isPageNotFound = true;
            }
        }

        /// <summary>
        /// Created By :  Sushil kumar on 2nd Feb 2017 
        /// Description : To get bike comaprision text and create page metas for multiple bikes
        /// </summary>
        private void GetComparisionTextAndMetas()
        {
            IList<string> bikeList = null, bikeMaskingList = null, bikeModels = null;
            string ComparisionUrls = string.Empty;
            try
            {
                if (comparedBikes != null && comparedBikes.BasicInfo != null)
                {
                    bikeList = new List<string>(); bikeMaskingList = new List<string>(); bikeModels = new List<string>();
                    PageMetas = new PageMetaTags();

                    foreach (var bike in comparedBikes.BasicInfo)
                    {
                        if (bike.VersionId != SponsoredVersionId)
                        {
                            bikeList.Add(string.Format("{0} {1}", bike.Make, bike.Model));
                            bikeMaskingList.Add(string.Format("{0}-{1}", bike.MakeMaskingName, bike.ModelMaskingName));
                            bikeModels.Add(bike.Model);

                        }
                    }

                    ComparisionText = string.Join(" vs ", bikeList);
                    ComparisionUrls = string.Join("-vs-", bikeMaskingList);
                    TargetedModels = string.Join(",", bikeModels);

                    PageMetas.Title = string.Format("Compare {0} - BikeWale", ComparisionText);
                    PageMetas.Keywords = "bike compare, compare bike, compare bikes, bike comparison, bike comparison India";
                    PageMetas.Description = string.Format("Compare {0} at Bikewale. Compare Price, Mileage, Engine Power, Space, Features, Specifications, Colors and much more.", string.Join(" and ", bikeList));
                    PageMetas.CanonicalUrl = string.Format("{0}/comparebikes/{1}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, ComparisionUrls);
                    PageMetas.AlternateUrl = string.Format("{0}/m/comparebikes/{1}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, ComparisionUrls);
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.Compare.CompareBikes.GetComparisionTextAndMetas");
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
                string QueryString = request.QueryString.ToString(), bike1 = request["bike1"], bike2 = request["bike2"], modelList = HttpUtility.ParseQueryString(QueryString).Get("mo");
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