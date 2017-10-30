
using Bikewale.Comparison.Interface;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
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
        private string modelList,modelIdList;
        private string compareModelId1, compareModelId2;
        public bool IsMobile { get; set; }
        public StatusCodes status { get; set; }
        public string redirectionUrl { get; set; }
        private string _originalUrl, _compareUrl;
        private readonly uint _maxComparisons;
        private string _bikeQueryString = string.Empty, _versionsList = string.Empty;
        private uint _sponsoredBikeVersionId, _cityId;
        private ushort bikeComparisions;

        public CompareDetails(ICMSCacheContent compareTest, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache, IBikeCompareCacheRepository objCompareCache, IBikeCompare objCompare, IBikeMakesCacheRepository objMakeCache, ISponsoredComparison objSponsored, uint maxComparisons)
        {
            _objModelMaskingCache = objModelMaskingCache;
            _objCompareCache = objCompareCache;
            _objCompare = objCompare;
            _objMakeCache = objMakeCache;
            _compareTest = compareTest;
            _objSponsored = objSponsored;
            _maxComparisons = maxComparisons;

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

                if (status != StatusCodes.RedirectPermanent)
                {
                    BindSimilarBikes(obj);
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareDetails.GetData()");
            }

            return obj;


        }
        /// <summary>
        /// Created By :- Subodh Jain 23 May 2017
        /// Summary :- Compare Bike BindSimilarBikes
        /// </summary>
        private void BindSimilarBikes(CompareDetailsVM obj)
        {
            try
            {
                ushort topCount = 8;
                obj.topBikeCompares = _objCompareCache.GetSimilarCompareBikes(_versionsList, topCount, (int)_cityId);
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareDetails.BindSimilarBikes()");
            }
        }

        /// <summary>
        /// Created By :- Subodh Jain 09 May 2017
        /// Summary :- Function for GetComparedBikeDetails
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
                        if (status != StatusCodes.RedirectPermanent)
                        {
                            GetComparisionTextAndMetas(obj);
                            obj.isUsedBikePresent = obj.Compare.BasicInfo.FirstOrDefault(x => x.UsedBikeCount.BikeCount > 0) != null;
                        }

                    }

                    obj.PQSourceId = PQSourceEnum.Desktop_CompareBike;
                    SimilarBikesComparisionWidget(obj);
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareDetails.GetComparedBikeDetails()");
                status = StatusCodes.ContentNotFound;
            }
        }
        /// <summary>
        /// Created By :- Subodh Jain 09 May 2017
        /// Summary :- Function for GetComparisionTextAndMetas
        /// Modified By:Snehal Dange on 12th August , 2017
        /// Description : Added reverseComparisonText for page title
        /// </summary>
        /// <returns></returns>
        private void GetComparisionTextAndMetas(CompareDetailsVM obj)
        {
            IList<string> bikeList = null, bikeMaskingList = null, bikeModels = null , bikeIdList = null;
            try
            {
                if (obj.Compare != null && obj.Compare.BasicInfo != null)
                {
                    bikeList = new List<string>(); bikeMaskingList = new List<string>(); bikeModels = new List<string>(); bikeIdList = new List<string>();


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
                    modelIdList = string.Join(",", bikeIdList);
                    if (bikeList.Count() == 2)
                    {
                        string reverseComparisonText = string.Join(" vs ", bikeModels.Reverse());
                        obj.PageMetaTags.Title = string.Format("{0} | {1} - BikeWale", obj.comparisionText, reverseComparisonText);
                    }
                    else
                    {
                        obj.PageMetaTags.Title = string.Format("{0} - BikeWale", obj.comparisionText);
                    }

                    obj.PageMetaTags.Keywords = "bike compare, compare bike, compare bikes, bike comparison, bike comparison India";
                    obj.PageMetaTags.Description = string.Format("Compare {0} at Bikewale. Compare Price, Mileage, Engine Power, Features, Specifications, Colours and much more.", string.Join(" and ", bikeList));
                    CreateCompareSummary(obj.Compare.BasicInfo, obj.Compare.CompareColors, obj);
                    obj.PageMetaTags.CanonicalUrl = string.Format("{0}/comparebikes/{1}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, _compareUrl);
                    obj.PageMetaTags.AlternateUrl = string.Format("{0}/m/comparebikes/{1}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, _compareUrl);
                    obj.Page_H1 = obj.comparisionText;

                    SetBreadcrumList(obj);
                    SetPageJSONLDSchema(obj);


                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareDetails.GetComparisionTextAndMetas");
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
                        bikePrice += string.Format(" {0} is   &#x20B9; {1} {2}", bikeName, price, (i < count - 1 ? ", " : " and "));
                        variants += string.Format(" {0} is available in {1} {4} and {2} {5}{3}", bikeName, colorCount, versionCount, (i < count - 1 ? ", " : " and "), colorCount > 1 ? "colours" : "colour", versionCount > 1 ? "variants" : "variant");
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareDetails.CreateCompareSummary()");
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareDetails.CreateCanonicalUrl()");
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

            try
            {
                var request = HttpContext.Current.Request;
                _originalUrl = request.ServerVariables["HTTP_X_ORIGINAL_URL"];
                if (String.IsNullOrEmpty(_originalUrl))
                    _originalUrl = request.ServerVariables["URL"];

                modelList = HttpUtility.ParseQueryString(request.QueryString.ToString()).Get("mo");
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
                    ModelMaskingResponse objResponse = null;
                    ModelMapping objCache = new ModelMapping();

                    for (ushort iTmp = 0; iTmp < models.Length; iTmp++)
                    {
                        string modelMaskingName = models[iTmp];
                        if (!string.IsNullOrEmpty(modelMaskingName) && _objModelMaskingCache != null)
                        {
                            objResponse = _objModelMaskingCache.GetModelMaskingResponse(modelMaskingName);
                        }

                        if (objResponse != null && objResponse.StatusCode == 200)
                        {
                            _versionsList = string.Format("{0},{1}", _versionsList, objCache.GetTopVersionId(modelMaskingName));
                            status = StatusCodes.ContentFound;
                            bikeComparisions = (ushort)(iTmp + 1);
                        }
                        else if (objResponse != null && objResponse.StatusCode == 301)
                        {
                            status = StatusCodes.RedirectPermanent;
                            if (String.IsNullOrEmpty(redirectionUrl))
                                redirectionUrl = request.RawUrl.Replace(models[iTmp].ToLower(), objResponse.MaskingName);
                            else
                                redirectionUrl = redirectionUrl.Replace(models[iTmp].ToLower(), objResponse.MaskingName);
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.CompareDetails.ProcessQueryString({0})", _originalUrl));
            }
            finally
            {
                if (!string.IsNullOrEmpty(_versionsList) && bikeComparisions >= 2)
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
                if (_objCompareCache!=null)
                {
                   
                    similarComparisons = _objCompareCache.GetSimilarBikes(modelIdList, topCount);
                    if(similarComparisons.BikeList.Any() && similarComparisons.SimilarBikes.Any())
                    {
                        IList<SimilarBikeComparisonWidget> comparisonList = new List<SimilarBikeComparisonWidget>();
                        foreach (var similarBikeObj in similarComparisons.SimilarBikes)
                        {
                            comparisonList.Add(new SimilarBikeComparisonWidget()
                            {
                                BikeMake = similarBikeObj.BikeMake,
                                BikeModel = similarBikeObj.BikeModel,
                                OriginalImagePath = similarBikeObj.OriginalImagePath,
                                HostUrl = similarBikeObj.HostUrl,
                                CompareBike1 = similarComparisons.BikeList.FirstOrDefault(l => l.Model.ModelId == similarBikeObj.ModelId1),
                                CompareBike2 = similarComparisons.BikeList.FirstOrDefault(l => l.Model.ModelId == similarBikeObj.ModelId2),
                                
                            });
                        }
                        if (obj.SimilarBikeWidget!=null)
                        {
                            obj.SimilarBikeWidget.SimilarBikeComparison = comparisonList;
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex,"Bikewale.Models.CompareDetails.SimilarBikesComparisionWidget()");
            }

        }
    }
}