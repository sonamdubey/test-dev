﻿
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Memcache;
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
        private readonly IBikeMakesCacheRepository<int> _objMakeCache = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelMaskingCache = null;
        private readonly IBikeCompareCacheRepository _objCompareCache = null;
        private readonly IBikeCompare _objCompare = null;
        private readonly ICMSCacheContent _compareTest = null;

        public StatusCodes status { get; set; }
        public string redirectionUrl { get; set; }
        private string originalUrl;
        private string _baseUrl = string.Empty, _bikeQueryString = string.Empty, _versionsList = string.Empty;

        public CompareDetails(ICMSCacheContent compareTest, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache, IBikeCompareCacheRepository objCompareCache, IBikeCompare objCompare, IBikeMakesCacheRepository<int> objMakeCache, string originalUrl)
        {
            _objModelMaskingCache = objModelMaskingCache;
            _objCompareCache = objCompareCache;
            _objCompare = objCompare;
            _objMakeCache = objMakeCache;
            _compareTest = compareTest;
            this.originalUrl = originalUrl;

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
                BindSimilarBikes(obj);
                GetComparedBikeDetails(obj);

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
                int cityId = (int)GlobalCityArea.GetGlobalCityArea().CityId;
                ushort topCount = 8;
                obj.topBikeCompares = _objCompareCache.GetSimilarCompareBikes(_versionsList, topCount, cityId);
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
                GlobalCityAreaEntity cityArea = GlobalCityArea.GetGlobalCityArea();
                uint CityId = 0;
                string cityName = string.Empty;
                if (cityArea != null)
                {
                    cityName = cityArea.City;
                    CityId = cityArea.CityId;
                }
                else
                {
                    cityName = BWConfiguration.Instance.DefaultName;
                }
                obj.sponsoredVersionId = _objCompare.GetFeaturedBike(_versionsList);

                if (obj.sponsoredVersionId > 0) _versionsList = string.Format("{0},{1}", _versionsList, obj.sponsoredVersionId);
                var arrayVersionList = _versionsList.Split(',');
                _versionsList = string.Join(",", arrayVersionList.Take(4));
                obj.Compare = _objCompareCache.DoCompare(_versionsList, CityId);

                if (obj.Compare != null && obj.Compare.BasicInfo != null)
                {
                    GetComparisionTextAndMetas(obj);
                    obj.isUsedBikePresent = obj.Compare.BasicInfo.FirstOrDefault(x => x.UsedBikeCount.BikeCount > 0) != null;


                    var objFeaturedComparision = obj.Compare.BasicInfo.FirstOrDefault(f => f.VersionId == obj.sponsoredVersionId);
                    if (objFeaturedComparision != null)
                        obj.FeaturedBike = Bikewale.Utility.SponsoredComparision.FetchValue(objFeaturedComparision.ModelId.ToString());

                }

                obj.PQSourceId = PQSourceEnum.Desktop_CompareBike;
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
        /// </summary>
        /// <returns></returns>
        private void GetComparisionTextAndMetas(CompareDetailsVM obj)
        {
            IList<string> bikeList = null, bikeMaskingList = null, bikeModels = null;
            string ComparisionUrls = string.Empty;
            try
            {
                if (obj.Compare != null && obj.Compare.BasicInfo != null)
                {
                    bikeList = new List<string>(); bikeMaskingList = new List<string>(); bikeModels = new List<string>();


                    foreach (var bike in obj.Compare.BasicInfo)
                    {
                        if (bike.VersionId != obj.sponsoredVersionId)
                        {
                            bikeList.Add(string.Format("{0} {1}", bike.Make, bike.Model));
                            bikeMaskingList.Add(string.Format("{0}-{1}", bike.MakeMaskingName, bike.ModelMaskingName));
                            bikeModels.Add(bike.Model);

                        }
                    }

                    obj.comparisionText = string.Join(" vs ", bikeList);
                    obj.templateSummaryTitle = string.Join(" vs ", bikeModels);
                    obj.targetModels = string.Join(",", bikeModels);

                    obj.PageMetaTags.Title = string.Format("Compare {0} - BikeWale", obj.comparisionText);
                    obj.PageMetaTags.Keywords = "bike compare, compare bike, compare bikes, bike comparison, bike comparison India";
                    obj.PageMetaTags.Description = string.Format("Compare {0} at Bikewale. Compare Price, Mileage, Engine Power, Features, Specifications, Colours and much more.", string.Join(" and ", bikeList));
                    string compareUrl = CreateCanonicalUrl(obj);
                    CheckForRedirection(compareUrl);
                    CreateCompareSummary(obj.Compare.BasicInfo, obj.Compare.CompareColors, obj);
                    obj.PageMetaTags.CanonicalUrl = string.Format("{0}/comparebikes/{1}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, compareUrl);
                    obj.PageMetaTags.AlternateUrl = string.Format("{0}/m/comparebikes/{1}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, compareUrl);

                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareDetails.GetComparisionTextAndMetas");
            }
        }

        /// <summary>
        /// Created By :- Subodh Jain 09 May 2017
        /// Summary :- Function for CheckForRedirection
        /// </summary>
        /// <returns></returns>
        private void CheckForRedirection(string canonicalUrl)
        {
            try
            {
                status = _baseUrl == canonicalUrl ? 0 : StatusCodes.RedirectPermanent;
                if (status == Entities.StatusCodes.RedirectPermanent)
                {
                    redirectionUrl = string.Format("/comparebikes/{0}/?{1}", canonicalUrl, _bikeQueryString);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareDetails.CheckForRedirection");
            }
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
                }
                bikeNames = bikeNames.Remove(bikeNames.Length - 5);
                bikePrice = bikePrice.Remove(bikePrice.Length - 6);
                variants = variants.Remove(variants.Length - 5);
                obj.compareSummaryText = string.Format("BikeWale brings you comparison of {0}. The ex-showroom price of{1}.{2}. Apart from prices, you can also find comparison of these bikes based on displacement, mileage, performance, and many more parameters. Comparison between these bikes have been carried out to help users make correct buying decison between {0}.", bikeNames, bikePrice, variants);

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
        private string CreateCanonicalUrl(CompareDetailsVM obj)
        {
            string canon = string.Empty;
            try
            {
                canon = string.Join("-vs-", obj.Compare.BasicInfo.Where(x => x.VersionId != obj.sponsoredVersionId).OrderBy(x => x.ModelId).Select(x => string.Format("{0}-{1}", x.MakeMaskingName, x.ModelMaskingName)));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareDetails.CreateCanonicalUrl()");
            }
            return canon;
        }
        /// <summary>
        /// Created By :- Subodh Jain 09 May 2017
        /// Summary :- Function for ParseQueryString
        /// </summary>
        /// <returns></returns>
        private void ParseQueryString(string originalUrl)
        {
            string[] strArray = originalUrl.Trim().Split('/');
            if (strArray.Length > 1)
            {
                _baseUrl = strArray[2];
            }
            string[] queryArr = originalUrl.Split('?');
            if (queryArr.Length > 1)
            {
                _bikeQueryString = queryArr[1];
            }
        }
        /// <summary>
        /// Created By :- Subodh Jain 09 May 2017
        /// Summary :- Function for ProcessQueryString
        /// </summary>
        /// <returns></returns>
        private void ProcessQueryString()
        {
            ushort bikeComparisions = 0;
            ushort maxComparisions = 5;
            try
            {
                var request = HttpContext.Current.Request;
                string modelList = HttpUtility.ParseQueryString(request.QueryString.ToString()).Get("mo");

                ParseQueryString(originalUrl);
                if (request.QueryString.ToString().Contains("bike"))
                {
                    for (ushort i = 1; i <= maxComparisions; i++)
                    {
                        uint vId = 0;
                        if (uint.TryParse(request["bike" + i], out vId) && vId > 0)
                        {
                            _versionsList += "," + vId;
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
                    int totalModels = models.Length;

                    for (ushort iTmp = 0; iTmp < totalModels; iTmp++)
                    {
                        string modelMaskingName = models[iTmp];
                        if (!string.IsNullOrEmpty(modelMaskingName) && _objModelMaskingCache != null)
                        {
                            objResponse = _objModelMaskingCache.GetModelMaskingResponse(modelMaskingName);
                        }

                        if (objResponse != null && objResponse.StatusCode == 200)
                        {
                            _versionsList += "," + objCache.GetTopVersionId(modelMaskingName);
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.Compare.GetCompareBikeDetails.ParseQueryString");
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
    }
}