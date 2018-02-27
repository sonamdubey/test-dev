
using Bikewale.Comparison.Interface;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.Compare;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pages;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Memcache;
using Bikewale.Models.Compare;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created By :- Subodh Jain 23 May 2017
    /// Summary :- Compare Bike CompareDetails
    /// </summary>
    public class CompareDetails
    {
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelMaskingCache = null;
        private readonly IBikeCompareCacheRepository _objCompareCache = null;
        private readonly IBikeCompare _objCompare = null;
        private readonly ICMSCacheContent _compareTest = null;
        private readonly ISponsoredComparison _objSponsored = null;
        private readonly IArticles _objArticles = null;
        private string modelIdList;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objVersionCache = null;
        public bool IsMobile { get; set; }
        public StatusCodes status { get; set; }
        public string redirectionUrl { get; set; }
        private string _originalUrl, _compareUrl, _modelNameList;
        private readonly uint _maxComparisons;
        private string _bikeQueryString = string.Empty, _versionsList = string.Empty;
        private uint _sponsoredBikeVersionId, _cityId;
        private ushort bikeComparisions;

        public CompareDetails(ICMSCacheContent compareTest, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache, IBikeCompareCacheRepository objCompareCache, IBikeCompare objCompare, IBikeMakesCacheRepository objMakeCache, ISponsoredComparison objSponsored, IArticles objArticles, IBikeVersionCacheRepository<BikeVersionEntity, uint> objVersionCache, uint maxComparisons)
        {
            _objModelMaskingCache = objModelMaskingCache;
            _objCompareCache = objCompareCache;
            _objCompare = objCompare;
            _objMakeCache = objMakeCache;
            _compareTest = compareTest;
            _objSponsored = objSponsored;
            _maxComparisons = maxComparisons;
            _objArticles = objArticles;
            _objVersionCache = objVersionCache;
            ProcessQueryString();
        }
        /// <summary>
        /// Created By :- Subodh Jain 23 May 2017
        /// Summary :- Compare Bike GetData
        /// </summary>
        public CompareDetailsVM GetData()
        {
            CompareDetailsVM obj = new CompareDetailsVM();
            try
            {
                GlobalCityAreaEntity cityArea = GlobalCityArea.GetGlobalCityArea();

                if (cityArea != null)
                {
                    _cityId = cityArea.CityId;
                }

                GetComparedBikeDetails(obj);
                obj.Page = GAPages.Compare_Bikes;
                if (status != StatusCodes.RedirectPermanent)
                {
                    BindExpertReviewsWidget(obj);
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Models.CompareDetails.GetData()");
            }

            return obj;


        }

        /// <summary>
        /// Created by Sajal Gupta on 30-10-2017
        /// Description: code to Bind Expert Reviews Widget
        /// </summary>
        /// <param name="obj"></param>
        private void BindExpertReviewsWidget(CompareDetailsVM obj)
        {
            try
            {
                obj.ArticlesList = new RecentExpertReviewsVM();
                obj.ArticlesList.ArticlesList = _objArticles.GetMostRecentArticlesByIdList(Convert.ToString((int)EnumCMSContentType.RoadTest), 5, 0, modelIdList);
                obj.ArticlesList.Title = string.Format(" More from experts on {0}", _modelNameList);
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Models.CompareDetails.BindExpertReviewsWidget()");
            }
        }

        /// <summary>
        /// Created By :- Subodh Jain 09 May 2017
        /// Summary :- Function for GetComparedBikeDetails
        /// Modified by Sajal Gupta  on 07-11-2017
        /// Description : Chenged logic to show similar bikews widget
        /// </summary>
        /// <returns></returns>
        private void GetComparedBikeDetails(CompareDetailsVM obj)
        {
            try
            {
                if (_versionsList.Split(',').Count() > 1)
                {
                    bool fetchSponsoredComparison = IsMobile ? bikeComparisions <= _maxComparisons : bikeComparisions < _maxComparisons;

                    if (fetchSponsoredComparison)
                    {
                        var SponsoredBike = _objSponsored.GetSponsoredVersion(_versionsList);

                        if (SponsoredBike != null)
                        {
                            obj.sponsoredVersionId = _sponsoredBikeVersionId > 0 ? _sponsoredBikeVersionId : SponsoredBike.SponsoredVersionId;
                            obj.KnowMoreLinkUrl = SponsoredBike.LinkUrl;
                            obj.KnowMoreLinkText = !String.IsNullOrEmpty(SponsoredBike.LinkText) ? SponsoredBike.LinkText : "Know more";
                        }

                        if (obj.sponsoredVersionId > 0)
                        {
                            _versionsList = string.Format("{0},{1}", _versionsList, obj.sponsoredVersionId);
                        }
                    }

                    obj.Compare = _objCompareCache.DoCompare(_versionsList, _cityId);

                    if (obj.Compare != null && obj.Compare.BasicInfo != null)
                    {
                        CreateCanonicalUrlAndCheckRedirection(obj);
                        CreateDisclaimerText(obj);
                        if (status != StatusCodes.RedirectPermanent)
                        {
                            GetComparisionTextAndMetas(obj);
                            obj.isUsedBikePresent = obj.Compare.BasicInfo.FirstOrDefault(x => x.UsedBikeCount.BikeCount > 0) != null;
                        }

                    }

                    obj.PQSourceId = PQSourceEnum.Desktop_CompareBike;

                    if (!string.IsNullOrEmpty(modelIdList) && modelIdList.Split(',').Count() > 1)
                    {
                        SimilarBikesComparisionWidget(obj);
                    }
                    else
                    {
                        BindSimilarBikes(obj);
                    }
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.CompareDetails.GetComparedBikeDetails()");
                status = StatusCodes.ContentNotFound;
            }
        }

        private static void CreateDisclaimerText(CompareDetailsVM obj)
        {

            try
            {

                string BikeValue = string.Join(" vs ", obj.Compare.BasicInfo
                    .Where(x => x.VersionId != obj.sponsoredVersionId)
                    .OrderBy(x => x.ModelId)
                    .Select(x => string.Format("{0} {1}", x.Make, x.Model)));

                obj.DisclaimerText = string.Format(@"BikeWale take utmost care in providing you the accurate information about prices, 
                                    feature, specs, and colors for comparison of {0}. However, BikeWale can't be held liable for 
                                    any direct/indirect damage or loss. For comparison of {0}, the base version has been considered. 
                                    You can compare any version for the comparison of {0}.", BikeValue);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.CompareDetails.CreateDisclaimerText()");

            }
        }


        /// <summary>
        /// Created by Sajal Gupta on  07-11-2017
        /// Description : Added similar bikes widget
        /// </summary>
        /// <param name="obj"></param>
        private void BindSimilarBikes(CompareDetailsVM obj)
        {
            try
            {
                var objSimilarBikes = new SimilarBikesWidget(_objVersionCache, !string.IsNullOrEmpty(_versionsList) ? Convert.ToUInt32(_versionsList.Split(',')[0]) : 0, PQSourceEnum.Desktop_CompareBike);

                objSimilarBikes.TopCount = 9;
                objSimilarBikes.CityId = _cityId;
                obj.SimilarBikes = objSimilarBikes.GetData();
                obj.SimilarBikes.IsNew = true;

                BikeEntityBase basicDetails = obj.Compare.BasicInfo.FirstOrDefault();

                if (basicDetails != null)
                {
                    obj.SimilarBikes.Make = new BikeMakeEntityBase() { MakeName = basicDetails.Make, MaskingName = basicDetails.MakeMaskingName };
                    obj.SimilarBikes.Model = new BikeModelEntityBase() { ModelId = (int)basicDetails.ModelId, ModelName = basicDetails.Model, MaskingName = basicDetails.ModelMaskingName };
                }

                obj.SimilarBikes.VersionId = Convert.ToUInt32(_versionsList.Split(',')[0]);

                obj.SimilarBikesCompareWidgetText = string.Format("Bikes Similar to {0}", _modelNameList);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.CompareDetails.BindSimilarBikes");
            }
        }

        /// <summary>
        /// Created By :- Subodh Jain 09 May 2017
        /// Summary :- Function for GetComparisionTextAndMetas
        /// Modified By:Snehal Dange on 12th August , 2017
        /// Description : Added reverseComparisonText for page title
        /// Modified by sajal Gupta on 10-11-2017
        /// Descriptiotion : Added GAPages;
        /// Modified by : Snehal dange on 29th Jan 2018
        /// Desc: Modifed title for the page
        /// </summary>
        /// <returns></returns>
        private void GetComparisionTextAndMetas(CompareDetailsVM obj)
        {
            IList<string> bikeList = null, bikeMaskingList = null, bikeModels = null, bikeIdList = null;
            try
            {
                if (obj.Compare != null && obj.Compare.BasicInfo != null)
                {
                    bikeList = new List<string>(); bikeMaskingList = new List<string>(); bikeModels = new List<string>();
                    bikeIdList = new List<string>();

                    foreach (var bike in obj.Compare.BasicInfo)
                    {
                        if (bike.VersionId != obj.sponsoredVersionId)
                        {
                            bikeList.Add(string.Format("{0} {1}", bike.Make, bike.Model));
                            bikeMaskingList.Add(string.Format("{0}-{1}", bike.MakeMaskingName, bike.ModelMaskingName));
                            bikeModels.Add(bike.Model);
                            bikeIdList.Add(bike.ModelId.ToString());
                        }
                    }


                    obj.comparisionText = string.Join(" vs ", bikeList);
                    obj.templateSummaryTitle = string.Join(" vs ", bikeModels);
                    obj.targetModels = string.Join(",", bikeModels);
                    modelIdList = string.Join(",", bikeIdList.Distinct());

                    _modelNameList = string.Join(", ", bikeModels.Distinct());
                    if (!string.IsNullOrEmpty(_modelNameList))
                    {
                        int place = _modelNameList.LastIndexOf(",");
                        if (place > 1)
                        {
                            _modelNameList = _modelNameList.Remove(place, 1).Insert(place, " &");
                        }
                    }

                    obj.PageMetaTags.Title = string.Format("Compare {0} - BikeWale", obj.comparisionText);

                    string ComparePriceText = string.Join(" and ", obj.Compare.BasicInfo.Take(2).Select(x => string.Format("{0} {1} Ex-showroom starts at - ₹ {2}", x.Make, x.Model, Format.FormatPrice(x.Price.ToString()))));
                    string CompareMileageText = string.Join(" whereas ", obj.Compare.BasicInfo.Take(2).Where(x => x.Mileage > 0).Select(x => string.Format("{0} has a mileage of {1} kmpl", x.Model, x.Mileage)));
                    string CompareModelText = string.Join(" and ", bikeList.Take(2));

                    obj.PageMetaTags.Keywords = "bike compare, compare bike, compare bikes, bike comparison, bike comparison India";
                    obj.PageMetaTags.Description = string.Format("{0}. {1}.Compare {2} specs, colors, reviews and ratings. Also, read comparison test of {3} from our experts.", ComparePriceText, CompareMileageText, CompareModelText, string.Join(" vs ", bikeList.Take(2)));


                    CreateCompareSummary(obj.Compare.BasicInfo, obj.Compare.CompareColors, obj);
                    obj.PageMetaTags.CanonicalUrl = string.Format("{0}/comparebikes/{1}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, _compareUrl);
                    obj.PageMetaTags.AlternateUrl = string.Format("{0}/m/comparebikes/{1}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, _compareUrl);
                    obj.Page_H1 = string.Format("Compare {0}", obj.comparisionText);
                    obj.Page = GAPages.Compare_Bikes;

                    SetBreadcrumList(obj);
                    SetPageJSONLDSchema(obj);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.CompareDetails.GetComparisionTextAndMetas");
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 14th Sep 2017
        /// Description : Added breadcrum and webpage schema
        /// </summary>
        private void SetPageJSONLDSchema(CompareDetailsVM objPageMeta)
        {
            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta.PageMetaTags, objPageMeta.BreadcrumbList);

            if (webpage != null)
            {
                objPageMeta.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// Modified by : Snehal Dange on 28th Dec 2017
        /// Descritption : Added 'New Bikes' in Breadcrumb
        /// </summary>
        private void SetBreadcrumList(CompareDetailsVM objPage)
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;
            if (IsMobile)
            {
                url += "m/";
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}new-bikes-in-india/", url), "New Bikes"));

            url += "comparebikes/";

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Compare bikes"));

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, null, objPage.Page_H1));


            objPage.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

        }


        /// <summary>
        /// Created By :- Subodh Jain 09 May 2017
        /// Summary :- Function for CreateCompareSummary
        /// </summary>
        /// <returns></returns>
        private void CreateCompareSummary(IEnumerable<BikeEntityBase> basicInfo, CompareBikeColorCategory colors, CompareDetailsVM obj)
        {
            try
            {
                int count = basicInfo.Count() - 1, i = 0;
                string bikeNames = string.Empty, bikePrice = string.Empty, variants = string.Empty;
                foreach (var bike in basicInfo)
                {
                    if (bike.VersionId != obj.sponsoredVersionId)
                    {
                        string bikeName = string.Format("{0} {1}", bike.Make, bike.Model);
                        int versionCount = bike.Versions.Count();
                        uint versionId = bike.VersionId;
                        string price = Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(bike.Price));
                        int colorCount = colors.bikes[i].bikeColors.Count;
                        bikeNames += bikeName + (i < count - 1 ? ", " : " and ");
                        bikePrice += string.Format(" {0} is &#x20B9; {1} {2}", bikeName, price, (i < count - 1 ? ", " : " and "));
                        variants += string.Format(" {0} is available in {1} {4}{2}{3}", bikeName, colorCount, (versionCount > 0 ? string.Format(" and {0} {1}", versionCount, versionCount > 1 ? "variants" : "variant") : ""), (i < count - 1 ? ", " : " and "), colorCount > 1 ? "colours" : "colour");
                        i++;
                    }
                }
                bikeNames = bikeNames.Remove(bikeNames.Length - 5);
                bikePrice = bikePrice.Remove(bikePrice.Length - 6);
                variants = variants.Remove(variants.Length - 5);
                obj.compareSummaryText = string.Format("BikeWale brings you comparison of {0}. The ex-showroom price of{1}.{2}. Apart from prices, you can also find comparison of these bikes based on displacement, mileage, performance, and many more paramete  &#x20B9; Comparison between these bikes have been carried out to help users make correct buying decison between {0}.", bikeNames, bikePrice, variants);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.CompareDetails.CreateCompareSummary()");
            }
        }

        /// <summary>
        /// Created By :- Subodh Jain 09 May 2017
        /// Summary :- Function for CreateCanonicalUrl
        /// </summary>
        /// <returns></returns>
        private void CreateCanonicalUrlAndCheckRedirection(CompareDetailsVM obj)
        {
            try
            {
                _compareUrl = string.Join("-vs-", obj.Compare.BasicInfo.Where(x => x.VersionId != obj.sponsoredVersionId).OrderBy(x => x.ModelId).Select(x => string.Format("{0}-{1}", x.MakeMaskingName, x.ModelMaskingName)));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.CompareDetails.CreateCanonicalUrl()");
            }
            finally
            {
                if (_originalUrl.IndexOf(_compareUrl) < 0)
                {
                    status = StatusCodes.RedirectPermanent;
                    redirectionUrl = string.Format("{0}comparebikes/{1}/?{2}", IsMobile ? "/m/" : "/", _compareUrl, _bikeQueryString);
                }
            }
        }

        /// <summary>
        /// Created By :- Subodh Jain 09 May 2017
        /// Summary :- Function for ProcessQueryString
        /// </summary>
        /// <returns></returns>
        private void ProcessQueryString()
        {
            Queue<string> compareUrl = new Queue<string>();
            bool isPermanentRedirection = false;
            string modelList, makeList;
            string newMakeMasking = string.Empty;
            bool isMakeRedirection = false;
            try
            {
                var request = HttpContext.Current.Request;
                _originalUrl = request.ServerVariables["HTTP_X_ORIGINAL_URL"];
                if (String.IsNullOrEmpty(_originalUrl))
                    _originalUrl = request.ServerVariables["URL"];

                modelList = HttpUtility.ParseQueryString(request.QueryString.ToString()).Get("mo");
                makeList = HttpUtility.ParseQueryString(request.QueryString.ToString()).Get("ma");
                string[] queryArr = _originalUrl.Split('?');
                if (queryArr.Length > 1)
                {
                    _bikeQueryString = queryArr[1];
                }

                if (_bikeQueryString.Contains("sponsoredbike"))
                {
                    uint vId = 0;
                    if (uint.TryParse(request["sponsoredbike"], out vId) && vId > 0)
                    {
                        _sponsoredBikeVersionId = vId;
                    }
                }

                if (_bikeQueryString.Contains("bike"))
                {
                    for (ushort i = 1; i <= _maxComparisons; i++)
                    {
                        uint vId = 0;
                        if (uint.TryParse(request["bike" + i], out vId) && vId > 0)
                        {
                            _versionsList = string.Format("{0},{1}", _versionsList, vId);
                            bikeComparisions = i;
                        }
                    }
                    status = StatusCodes.ContentFound;

                }
                else if (!string.IsNullOrEmpty(modelList))
                {
                    string[] models = modelList.Split(',');
                    string[] makes = makeList.Split(',');

                    ModelMaskingResponse objResponse = null;
                    ModelMapping objCache = new ModelMapping();

                    for (ushort iTmp = 0; iTmp < models.Length; iTmp++)
                    {
                        string modelMaskingName = models[iTmp];
                        string makeMaskingName = makes[iTmp];

                        newMakeMasking = ProcessMakeMaskingName(makeMaskingName, out isMakeRedirection);

                        if (!string.IsNullOrEmpty(newMakeMasking) && !string.IsNullOrEmpty(makeMaskingName) && !string.IsNullOrEmpty(modelMaskingName) && _objModelMaskingCache != null)
                        {
                            objResponse = _objModelMaskingCache.GetModelMaskingResponse(string.Format("{0}_{1}", makeMaskingName, modelMaskingName));
                        }
                        var topVersionId = objCache.GetTopVersionId(modelMaskingName);
                        if (objResponse != null && objResponse.StatusCode == 200 && topVersionId > 0)
                        {
                            _versionsList = string.Format("{0},{1}", _versionsList, topVersionId);
                            status = StatusCodes.ContentFound;
                            bikeComparisions = (ushort)(iTmp + 1);
                            compareUrl.Enqueue(string.Format("{0}-{1}", makeMaskingName, modelMaskingName));


                        }
                        else if (objResponse != null && (objResponse.StatusCode == 301 || isMakeRedirection))
                        {
                            status = StatusCodes.RedirectPermanent;
                            isPermanentRedirection = true;

                            //if (String.IsNullOrEmpty(redirectionUrl))
                            //    redirectionUrl = request.RawUrl.Replace(makes[iTmp].ToLower(), newMakeMasking).Replace(models[iTmp].ToLower(), objResponse.MaskingName);
                            //else
                            //    redirectionUrl = redirectionUrl.Replace(makes[iTmp].ToLower(), newMakeMasking).Replace(models[iTmp].ToLower(), objResponse.MaskingName);

                            compareUrl.Enqueue(string.Format("{0}-{1}", newMakeMasking, objResponse.MaskingName));

                        }
                        else
                        {
                            status = StatusCodes.ContentNotFound;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.CompareDetails.ProcessQueryString({0})", _originalUrl));
            }
            finally
            {
                if (isPermanentRedirection)
                {
                    redirectionUrl = string.Join("-vs-", compareUrl);

                    if (IsMobile)
                    {
                        redirectionUrl = string.Format("/m/comparebikes/{0}/", redirectionUrl);
                    }
                    else
                    {
                        redirectionUrl = string.Format("/comparebikes/{0}/", redirectionUrl);
                    }
                    status = StatusCodes.RedirectPermanent;
                }
                else if (!string.IsNullOrEmpty(_versionsList) && bikeComparisions >= 2)
                {
                    _versionsList = _versionsList.Substring(1);
                }
                else
                {
                    status = StatusCodes.RedirectTemporary;
                }
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 11th Dec 2017
        /// Description : Process make masking name for redirection
        /// </summary>
        /// <param name="make"></param>
        /// <param name="isMakeRedirection"></param>
        /// <returns></returns>
        private string ProcessMakeMaskingName(string make, out bool isMakeRedirection)
        {
            MakeMaskingResponse makeResponse = null;
            Common.MakeHelper makeHelper = new Common.MakeHelper();
            isMakeRedirection = false;
            if (!string.IsNullOrEmpty(make))
            {
                makeResponse = makeHelper.GetMakeByMaskingName(make);
            }
            if (makeResponse != null)
            {
                if (makeResponse.StatusCode == 200)
                {
                    return makeResponse.MaskingName;
                }
                else if (makeResponse.StatusCode == 301)
                {
                    isMakeRedirection = true;
                    return makeResponse.MaskingName;
                }
                else
                {
                    return "";
                }
            }

            return "";
        }

        /// <summary>
        /// Created By:Snehal Dange on 25th Oct 2017
        /// Description : Function for similar bikes comparison
        /// </summary>
        /// <param name="obj"></param>
        private void SimilarBikesComparisionWidget(CompareDetailsVM obj)
        {
            try
            {
                ushort topCount = 10;
                SimilarBikeComparisonWrapper similarComparisons = null;
                obj.SimilarBikeWidget = new SimilarBikesComparisionVM();
                if (_objCompareCache != null)
                {

                    similarComparisons = _objCompareCache.GetSimilarBikes(modelIdList, topCount);
                    if (similarComparisons != null && similarComparisons.BikeList.Any() && similarComparisons.SimilarBikes.Any())
                    {
                        IList<SimilarBikeComparisonWidget> comparisonList = new List<SimilarBikeComparisonWidget>();
                        foreach (var similarBikeObj in similarComparisons.SimilarBikes)
                        {
                            comparisonList.Add(new SimilarBikeComparisonWidget
                            {
                                BikeMake = similarBikeObj.BikeMake,
                                BikeModel = similarBikeObj.BikeModel,
                                OriginalImagePath = similarBikeObj.OriginalImagePath,
                                HostUrl = similarBikeObj.HostUrl,
                                CompareBike1 = similarComparisons.BikeList.FirstOrDefault(l => l.Model.ModelId == similarBikeObj.ModelId1),
                                CompareBike2 = similarComparisons.BikeList.FirstOrDefault(l => l.Model.ModelId == similarBikeObj.ModelId2),

                            });
                        }
                        if (obj.SimilarBikeWidget != null)
                        {
                            obj.SimilarBikeWidget.SimilarBikeComparison = comparisonList;
                            obj.SimilarBikeWidget.ModelComparisionText = obj.comparisionText;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.CompareDetails.SimilarBikesComparisionWidget()");
            }

        }
    }

}

