
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
    public class CompareBikesDetails
    {
        private readonly IBikeMakesCacheRepository<int> _objMakeCache = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelMaskingCache = null;
        private readonly IBikeCompareCacheRepository _objCompareCache = null;
        private readonly IBikeCompare _objCompare = null;

        public GlobalCityAreaEntity cityArea = null;
        public bool isPageNotFound, isPermanentRedirect, isUsedBikePresent, isCompareLandingRedirection;
        public string redirectionUrl = string.Empty, versionsList = string.Empty, bike1Name = string.Empty, bike2Name = string.Empty, ComparisionText = string.Empty, TargetedModels = string.Empty, FeaturedBikeLink = string.Empty;
        public uint versionId1, versionId2;
        public BikeCompareEntity comparedBikes = null;
        public PageMetaTags PageMetas = null;
        public Int64 SponsoredVersionId;
        public ICollection<BikeCompareEntity> objBikes = null;
        public IEnumerable<BikeMakeEntityBase> makes = null;
        public ushort maxComparisions = 5;

        /// <summary>
        /// Created By : Sushil kumar on 2nd Feb 2017 
        /// Description : Constructor to resolve unity containers and initialize model
        /// </summary>
        public CompareBikesDetails()
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
                    makes = _objMakeCache.GetMakesByType(EnumBikeType.New);
                    GetComparisionTextAndMetas();
                    isUsedBikePresent = comparedBikes.BasicInfo.FirstOrDefault(x => x.UsedBikeCount.BikeCount > 0) != null;

                    if (SponsoredVersionId > 0)
                    {
                        var objFeaturedComparision = comparedBikes.BasicInfo.FirstOrDefault(f => f.VersionId == SponsoredVersionId);
                        if (objFeaturedComparision != null)
                            FeaturedBikeLink = Bikewale.Utility.SponsoredComparision.FetchValue(objFeaturedComparision.ModelId.ToString());
                    }
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
        public bool ProcessQueryString()
        {
            bool IsValidQS = false;
            ushort bikeComparisions = 0;
            try
            {
                var request = HttpContext.Current.Request;
                string modelList = HttpUtility.ParseQueryString(request.QueryString.ToString()).Get("mo");

                if (request.QueryString.ToString().Contains("bike"))
                {
                    for (ushort i = 1; i <= maxComparisions; i++)
                    {
                        uint vId = 0;
                        if (uint.TryParse(request["bike" + i], out vId) && vId > 0)
                        {
                            versionsList += "," + vId;
                            bikeComparisions = i;
                        }
                    }

                    IsValidQS = true;
                }
                else if (!string.IsNullOrEmpty(modelList))
                {
                    string[] models = modelList.Split(',');
                    ModelMaskingResponse objResponse = null;
                    ModelMapping objCache = new ModelMapping();
                    int totalModels = models.Length;

                    for (ushort iTmp = 0; iTmp < maxComparisions; iTmp++)
                    {
                        string modelMaskingName = models[iTmp];
                        if (!string.IsNullOrEmpty(modelMaskingName) && _objModelMaskingCache != null)
                        {
                            objResponse = _objModelMaskingCache.GetModelMaskingResponse(modelMaskingName);
                        }

                        if (objResponse != null && objResponse.StatusCode == 200)
                        {
                            versionsList += "," + objCache.GetTopVersionId(modelMaskingName);
                            IsValidQS = true;
                            bikeComparisions = (ushort)(iTmp + 1);
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
            finally
            {
                if (!string.IsNullOrEmpty(versionsList) && bikeComparisions >= 2)
                {
                    versionsList = versionsList.Substring(1);
                }
                else
                {
                    isCompareLandingRedirection = true;
                }

            }

            return IsValidQS;
        }
        //End of getVersionIdList
    }
}