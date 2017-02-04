
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
    /// 
    /// </summary>
    public class CompareBikes
    {
        private IBikeMakesCacheRepository<int> _objMakeCache = null;
        private IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelMaskingCache = null;
        private IBikeCompareCacheRepository _objCompareCache = null;
        private IBikeCompare _objCompare = null;

        public GlobalCityAreaEntity cityArea = null;
        public bool isPageNotFound, isPermanentRedirect;
        public string redirectionUrl = string.Empty, versionsList, bike1Name = string.Empty, bike2Name = string.Empty;
        public uint versionId1, versionId2;
        public BikeCompareEntity comparedBikes = null;
        public PageMetaTags PageMetas = null;
        public Int64 SponsoredVersionId;
        public ICollection<BikeCompareEntity> objBikes = null;
        public string ComparisionText = string.Empty;
        public ICollection<BikeMakeEntityBase> makes = null;
        public bool isUsedBikePresent;

        /// <summary>
        /// 
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

            cityArea = GlobalCityArea.GetGlobalCityArea();
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
                SponsoredVersionId = _objCompare.GetFeaturedBike(versionsList);
                if (SponsoredVersionId > 0) versionsList = string.Format("{0},{1}", versionsList, SponsoredVersionId);
                comparedBikes = _objCompareCache.DoCompare(versionsList, cityArea.CityId);
                if (comparedBikes != null && comparedBikes.BasicInfo != null)
                {
                    BindPageMetas(comparedBikes.BasicInfo.ElementAt(0), comparedBikes.BasicInfo.ElementAt(1));
                    makes = _objMakeCache.GetMakesByType(EnumBikeType.New).ToList();
                    GetComparisionText();
                    isUsedBikePresent = comparedBikes.BasicInfo.FirstOrDefault(x => x.UsedBikeCount.BikeCount > 0) != null;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.Compare.CompareBikes.GetCompareBikeDetails");
                isPageNotFound = false;
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Common logic to bind meta tags
        /// </summary>
        private void BindPageMetas(BikeEntityBase bike1, BikeEntityBase bike2)
        {
            if (bike1 != null && bike2 != null)
            {
                PageMetas = new PageMetaTags();

                try
                {
                    bike1Name = string.Format("{0} {1}", bike1.Make, bike1.Model);
                    bike2Name = string.Format("{0} {1}", bike2.Make, bike2.Model);

                    PageMetas.Title = string.Format("Compare {0} vs {1} - BikeWale", bike1Name, bike2Name);
                    PageMetas.Keywords = "bike compare, compare bike, compare bikes, bike comparison, bike comparison india";
                    PageMetas.Description = string.Format("Compare {0} and {1} at Bikewale. Compare Price, Mileage, Engine Power, Space, Features, Specifications, Colors and much more.", bike1Name, bike2Name);
                    //PageMetas.ShareImage = Image.GetPathToShowImages(ArticleDetails.OriginalImgUrl, ArticleDetails.HostUrl, Bikewale.Utility.ImageSize._640x348);
                    PageMetas.CanonicalUrl = string.Format("{0}/{1}-{2}-vs-{3}-{4}", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, bike1.MakeMaskingName, bike1.ModelMaskingName, bike2.MakeMaskingName, bike2.ModelMaskingName);
                    PageMetas.AlternateUrl = string.Format("{0}/m/{1}-{2}-vs-{3}-{4}", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, bike1.MakeMaskingName, bike1.ModelMaskingName, bike2.MakeMaskingName, bike2.ModelMaskingName);
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.Compare.CompareBikes.BindPageMetas");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetComparisionText()
        {
            try
            {
                IList<string> bikeList = new List<string>();
                foreach (var bike in comparedBikes.BasicInfo)
                {
                    bikeList.Add(string.Format("{0} {1}", bike.Make, bike.Model));
                }

                ComparisionText = string.Join(" vs ", bikeList);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.Compare.CompareBikes.GetComparisionText");
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