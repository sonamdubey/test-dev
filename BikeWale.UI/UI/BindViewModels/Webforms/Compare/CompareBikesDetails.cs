
using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Compare;
using Bikewale.Cache.Core;
using Bikewale.Comparison.BAL;
using Bikewale.Comparison.DAL;
using Bikewale.Comparison.Interface;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Compare;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.Location;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Compare;
using Bikewale.Memcache;
using Bikewale.Models;
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
    /// Description : ViewModel for compare bike details  page
    /// </summary>
    public class CompareBikesDetails
    {
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelMaskingCache = null;
        private readonly IBikeCompareCacheRepository _objCompareCache = null;
        private readonly IBikeCompare _objCompare = null;
        private readonly ISponsoredComparison _objSponsored = null;

        public GlobalCityAreaEntity cityArea = null;
        public bool isPageNotFound, isPermanentRedirect, isUsedBikePresent, isCompareLandingRedirection;
        public string redirectionUrl = string.Empty, baseurl = string.Empty, originalUrl = string.Empty, bikeQueryString = string.Empty, compareUrl = string.Empty, versionsList = string.Empty, bike1Name = string.Empty,
            bike2Name = string.Empty, cityName = string.Empty, TemplateSummaryTitle = string.Empty, ComparisionText = string.Empty, TargetedModels = string.Empty, FeaturedBikeLink = string.Empty, summaryText = string.Empty;
        public uint versionId1, versionId2;
        public BikeCompareEntity comparedBikes = null;
        public PageMetaTags PageMetas = null;
        public Int64 SponsoredVersionId;
        public ICollection<BikeCompareEntity> objBikes = null;
        public IEnumerable<BikeMakeEntityBase> makes = null;
        public ushort maxComparisions = 5;
        public int CityId = 0;
        private uint _sponseredBikeVersionId;
        public Comparison.Entities.SponsoredVersionEntityBase SponsoredBike { get; set; }
        public BreadcrumbList Breadcrumb { get; set; }
        public string SchemaJSON { get; set; }

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
                        .RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeCompare, Bikewale.BAL.Compare.BikeComparison>()
                        .RegisterType<ISponsoredComparisonRepository, SponsoredComparisonRepository>()
                        .RegisterType<ISponsoredComparisonCacheRepository, Bikewale.Comparison.Cache.SponsoredComparisonCacheRepository>()
                        .RegisterType<ISponsoredComparison, SponsoredComparison>();

                    _objModelMaskingCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                    _objCompareCache = container.Resolve<IBikeCompareCacheRepository>();
                    _objCompare = container.Resolve<IBikeCompare>();
                    _objMakeCache = container.Resolve<IBikeMakesCacheRepository>();
                    _objSponsored = container.Resolve<ISponsoredComparison>();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.CompareBikesDetails : CompareBikesDetails");
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
                if (cityArea != null)
                {
                    cityName = cityArea.City;
                    CityId = (int)cityArea.CityId;
                }
                else
                {
                    cityName = BWConfiguration.Instance.DefaultName;
                }
                SponsoredBike = _objSponsored.GetSponsoredVersion(versionsList);

                if (SponsoredBike != null)
                {
                    SponsoredVersionId = _sponseredBikeVersionId > 0 ? _sponseredBikeVersionId : SponsoredBike.SponsoredVersionId;
                    FeaturedBikeLink = SponsoredBike.LinkUrl;
                }

                if (SponsoredVersionId > 0) versionsList = string.Format("{0},{1}", versionsList, SponsoredVersionId);

                comparedBikes = _objCompareCache.DoCompare(versionsList, cityArea.CityId);

                if (comparedBikes != null && comparedBikes.BasicInfo != null)
                {
                    makes = _objMakeCache.GetMakesByType(EnumBikeType.New);
                    GetComparisionTextAndMetas();
                    isUsedBikePresent = comparedBikes.BasicInfo.FirstOrDefault(x => x.UsedBikeCount.BikeCount > 0) != null;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.Compare.CompareBikesDetails.GetComparedBikeDetails");
                isPageNotFound = true;
            }
        }

        /// <summary>
        /// Created By :  Sushil kumar on 2nd Feb 2017 
        /// Description : To get bike comaprision text and create page metas for multiple bikes
        /// Modified By:Snehal Dange on 12th August , 2017
        /// Description : Added reverseComparisonText for page title
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

                    string reverseComparisonText = string.Join(" vs ", bikeModels.Reverse());
                    ComparisionText = string.Join(" vs ", bikeList);
                    TemplateSummaryTitle = string.Join(" vs ", bikeModels);
                    TargetedModels = string.Join(",", bikeModels);

                    PageMetas.Title = string.Format("{0} | {1} - BikeWale", ComparisionText, reverseComparisonText);
                    PageMetas.Keywords = "bike compare, compare bike, compare bikes, bike comparison, bike comparison India";
                    PageMetas.Description = string.Format("Compare {0} at Bikewale. Compare Price, Mileage, Engine Power, Space, Features, Specifications, Colours and much more.", string.Join(" and ", bikeList));
                    compareUrl = CreateCanonicalUrl(comparedBikes.BasicInfo);
                    CheckForRedirection(compareUrl);
                    CreateCompareSummary(comparedBikes.BasicInfo, comparedBikes.CompareColors);
                    PageMetas.CanonicalUrl = string.Format("{0}/comparebikes/{1}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, compareUrl);
                    PageMetas.AlternateUrl = string.Format("{0}/m/comparebikes/{1}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, compareUrl);

                    SetBreadcrumList();
                    SetPageJSONLDSchema(PageMetas);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.Compare.GetCompareBikeDetails.GetComparisionTextAndMetas");
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 14th Sep 2017
        /// Description : Added breadcrum and webpage schema
        /// </summary>
        private void SetPageJSONLDSchema(PageMetaTags objPageMeta)
        {
            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta, Breadcrumb);

            if (webpage != null)
            {
                SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList()
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;

            url += "m/";


            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));

            url += "comparebikes/";

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Compare bikes"));

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, null, ComparisionText));


            Breadcrumb = new BreadcrumbList() { BreadcrumListItem = BreadCrumbs };

        }


        /// <summary>
        /// Creates the compare summary.
        /// </summary>
        /// <param name="basicInfo">The basic information.</param>
        /// <param name="colors">The colors.</param>
        private void CreateCompareSummary(IEnumerable<BikeEntityBase> basicInfo, CompareBikeColorCategory colors)
        {
            try
            {
                int count = basicInfo.Count() - 1, i = 0;
                string bikeNames = string.Empty, bikePrice = string.Empty, variants = string.Empty;
                foreach (var bike in basicInfo)
                {
                    string bikName = string.Format("{0} {1}", bike.Make, bike.Model);
                    int versionCount = bike.Versions.Count();
                    uint versionId = bike.VersionId;
                    string price = Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(bike.Price));
                    int colorCount = colors.bikes[i].bikeColors.Count;
                    bikeNames += bikName + (i < count - 1 ? ", " : " and ");
                    bikePrice += string.Format(" {0} is Rs. {1} {2}", bikName, price, (i < count - 1 ? ", " : " and "));
                    variants += string.Format(" {0} is available in {1} {4} and {2} {5}{3}", bikName, colorCount, versionCount, (i < count - 1 ? ", " : " and "), colorCount > 1 ? "colours" : "colour", versionCount > 1 ? "variants" : "variant");
                    i++;
                }
                bikeNames = bikeNames.Remove(bikeNames.Length - 5);
                bikePrice = bikePrice.Remove(bikePrice.Length - 6);
                variants = variants.Remove(variants.Length - 5);
                summaryText = string.Format("BikeWale brings you comparison of {0}. The ex-showroom price of{1}.{2}. Apart from prices, you can also find comparison of these bikes based on displacement, mileage, performance, and many more parameters. Comparison between these bikes have been carried out to help users make correct buying decison between {0}.", bikeNames, bikePrice, variants);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "CompareBikesDetails.CreateCompareSummary()");
            }
        }


        private string CreateCanonicalUrl(IEnumerable<BikeEntityBase> basicInfo)
        {
            string canon = string.Empty;
            canon = string.Join("-vs-", basicInfo.Where(x => x.VersionId != SponsoredVersionId).OrderBy(x => x.ModelId).Select(x => string.Format("{0}-{1}", x.MakeMaskingName, x.ModelMaskingName)));
            return canon;
        }

        /// <summary>
        /// Checks for redirection.
        /// If current url is not equal to the Canonical url then redirect to canoncial url
        /// </summary>
        /// <param name="canonicalUrl">The canonical URL.</param>
        private void CheckForRedirection(string canonicalUrl)
        {
            isPermanentRedirect = baseurl == canonicalUrl ? false : true;
            if (isPermanentRedirect)
            {
                redirectionUrl = string.Format("/m/comparebikes/{0}/?{1}", canonicalUrl, bikeQueryString);
            }
        }

        /// <summary>
        /// Parses the query string.
        /// Created by: Sangram Nandkhile on 26 Apr 2017
        /// </summary>
        /// <param name="originalUrl">The original URL.</param>
        private void ParseQueryString()
        {
            string[] strArray = originalUrl.Trim().Split('/');
            if (strArray.Length > 1)
            {
                baseurl = strArray[3];
            }
            string[] queryArr = originalUrl.Split('?');
            if (queryArr.Length > 1)
            {
                bikeQueryString = queryArr[1];
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
                ParseQueryString();
                if (bikeQueryString.Contains("sponsoredbike"))
                {
                    uint vId = 0;
                    if (uint.TryParse(request["sponsoredbike"], out vId) && vId > 0)
                    {
                        _sponseredBikeVersionId = vId;
                    }
                }
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
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.Compare.GetCompareBikeDetails.ParseQueryString");
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