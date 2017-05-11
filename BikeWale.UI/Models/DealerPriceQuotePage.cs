using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Models.Price;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Sushil Kumar on 23rd March 2017
    /// Description  :To manage dealerpricequote mobile and desktop page
    /// </summary>
    public class DealerPriceQuotePage
    {

        private readonly IDealerPriceQuoteDetail _objDealerPQDetails = null;
        private readonly IDealerPriceQuote _objDealerPQ = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objVersionCache = null;
        private readonly IAreaCacheRepository _objAreaCache = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IPriceQuote _objPQ = null;

        public string RedirectUrl { get; set; }
        public uint OtherTopCount { get; set; }
        public StatusCodes Status { get; set; }

        private uint _modelId, _versionId, _cityId, _areaId, _pqId, _dealerId, _makeId;
        private string pageUrl, mpqQueryString, currentCity = string.Empty, currentArea = string.Empty;



        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Description  : Resolve unity containers
        /// </summary>
        public DealerPriceQuotePage(IDealerPriceQuoteDetail objDealerPQDetails, IDealerPriceQuote objDealerPQ, IBikeVersionCacheRepository<BikeVersionEntity, uint> objVersionCache, IAreaCacheRepository objAreaCache, ICityCacheRepository objCityCache, IPriceQuote objPQ, IDealerCacheRepository objDealerCache)
        {
            _objDealerPQDetails = objDealerPQDetails;
            _objDealerPQ = objDealerPQ;
            _objVersionCache = objVersionCache;
            _objAreaCache = objAreaCache;
            _objCityCache = objCityCache;
            _objPQ = objPQ;
            _objDealerCache = objDealerCache;

            ProcessQueryString();
        }

        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Description  : To get dealerpricequote page data 
        /// </summary>
        /// <returns></returns>
        public DealerPriceQuotePageVM GetData()
        {
            DealerPriceQuotePageVM objData = null;
            try
            {
                objData = new DealerPriceQuotePageVM();
                if (_versionId > 0)
                {
                    GetBikeVersions(objData);
                    SetDealerPriceQuoteDetail(objData);
                    SetModelVariables(objData);
                    BindPageWidgets(objData);
                    SetPageMetas(objData);
                }


            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Models.DealerPriceQuotePage.GetData()");
            }

            return objData;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Description  : To set Page Meta tags
        /// </summary>
        /// <param name="objData"></param>
        private void SetPageMetas(DealerPriceQuotePageVM objData)
        {
            try
            {
                if (objData != null && objData.SelectedVersion != null && objData.SelectedVersion.MakeBase != null && objData.SelectedVersion.ModelBase != null)
                {
                    objData.PageMetaTags.Title = String.Format("{0} {1} {2} Price Quote", objData.SelectedVersion.MakeBase.MakeName, objData.SelectedVersion.ModelBase.ModelName, objData.SelectedVersion.VersionName);
                    objData.PageMetaTags.ShareImage = Image.GetPathToShowImages(objData.SelectedVersion.OriginalImagePath, objData.SelectedVersion.HostUrl, Bikewale.Utility.ImageSize._360x202);
                    objData.PageMetaTags.Description = String.Format("{0} {1} {2} price quote", objData.SelectedVersion.MakeBase.MakeName, objData.SelectedVersion.ModelBase.ModelName, objData.SelectedVersion.VersionName);
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.DealerPriceQuotePage.SetMetaTags");
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Description  : To set dealerpricequote page variables 
        /// </summary>
        /// <param name="objData"></param>
        private void SetModelVariables(DealerPriceQuotePageVM objData)
        {
            try
            {
                objData.DealerId = _dealerId;
                objData.CityId = _cityId;
                objData.AreaId = _areaId;
                objData.PQId = _pqId;
                objData.ClientIP = CommonOpn.GetClientIP();
                objData.Location = GetLocationCookie();
                objData.CiyName = currentCity;
                objData.MPQQueryString = mpqQueryString;
                objData.PageUrl = pageUrl;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.DealerPriceQuotePage.SetModelVariables()");
            }

        }

        /// <summary>
        /// Created By  : Sushil Kumar on 11th Jan 2016
        /// Description : Bind page related widgets
        /// Modifued By :- Subodh Jain 01 march 2017
        /// Summary :- lead capture pop up
        /// </summary>
        private void BindPageWidgets(DealerPriceQuotePageVM objData)
        {

            try
            {
                if (objData != null)
                {

                    DealerCardWidget objDealer = new DealerCardWidget(_objDealerCache, _cityId, _makeId);
                    objDealer.TopCount = OtherTopCount;
                    objData.OtherDealers = objDealer.GetData();


                    objData.LeadCapture = new LeadCaptureEntity()
                    {
                        ModelId = _modelId,
                        CityId = _cityId,
                        AreaId = _areaId,
                        Area = currentArea,
                        City = currentCity,
                        Location = objData.Location,
                        BikeName = objData.BikeName

                    };

                    var objSimilarBikes = new SimilarBikesWidget(_objVersionCache, _versionId, PQSourceEnum.Desktop_DPQ_Alternative);
                    if (objSimilarBikes != null)
                    {
                        objSimilarBikes.TopCount = 9;
                        objSimilarBikes.CityId = _cityId;
                        objData.SimilarBikesVM = objSimilarBikes.GetData();
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.DealerPriceQuotePage.BindPageWidgets");
            }

        }

        /// <summary>
        /// Created By : Sushil Kumar on 15th March 2016
        /// Description : To set user location
        /// Modified By : Aditi srivastava on 17 Nov 2016
        /// Description : get city area name from global city
        /// Modified By :Sushil Kumar on 12th Jan 2017
        /// Description : To set city area from bikewale common utility function
        /// </summary>
        /// <returns></returns>
        private string GetLocationCookie()
        {
            string location = String.Empty;

            try
            {
                var cities = _objCityCache.GetPriceQuoteCities(_modelId);

                if (cities != null)
                {
                    Entities.Location.CityEntityBase city = cities.FirstOrDefault(m => m.CityId == _cityId);
                    currentCity = city != null ? city.CityName : String.Empty;


                    IEnumerable<Entities.Location.AreaEntityBase> areas = _objAreaCache.GetAreaList(_modelId, _cityId);
                    if (areas != null)
                    {
                        Entities.Location.AreaEntityBase area = areas.FirstOrDefault(m => m.AreaId == _areaId);
                        if (area != null)
                        {
                            currentArea = area != null ? area.AreaName : String.Empty;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(currentCity))
                {
                    if (!string.IsNullOrEmpty(currentArea))
                    {
                        location = String.Format("<span>{0}</span>, <span>{1}</span>", currentArea, currentCity);
                    }
                    else
                    {
                        location = String.Format("<span>{0}</span>", currentCity);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.DealerPriceQuotePage.GetLocationCookie");
            }

            return location;
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created on : 15 March 2016
        /// Description : for Dealer Basics details.
        /// Modified By : Sushil Kumar on 17th March 2016
        /// Description  : Added default values for emi if no emi details is available
        /// Modifide By :- Subodh jain on 02 March 2017
        /// Summary:- added manufacturer campaign leadpopup changes
        /// <param name="cityId"></param>
        /// <param name="versionId"></param>
        /// <param name="dealerId"></param>
        /// <param name="objData"></param>
        private void SetDealerPriceQuoteDetail(DealerPriceQuotePageVM objData)
        {
            Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity detailedDealer = null;
            try
            {
                detailedDealer = _objDealerPQDetails.GetDealerQuotationV2(_cityId, _versionId, _dealerId, _areaId);
                if (detailedDealer != null)
                {

                    if (detailedDealer.PrimaryDealer != null)
                    {

                        if (detailedDealer.PrimaryDealer.DealerDetails != null)
                        {
                            #region Set Dealer Details
                            if (string.IsNullOrEmpty(detailedDealer.PrimaryDealer.DealerDetails.DisplayTextLarge))
                                detailedDealer.PrimaryDealer.DealerDetails.DisplayTextLarge = objData.LeadBtnLongText;
                            if (string.IsNullOrEmpty(detailedDealer.PrimaryDealer.DealerDetails.DisplayTextSmall))
                                detailedDealer.PrimaryDealer.DealerDetails.DisplayTextSmall = objData.LeadBtnShortText;

                            //EMI details
                            #region Set EMI Details
                            var objEMI = detailedDealer.PrimaryDealer.EMIDetails;
                            if (objEMI != null)
                            {
                                var _objEMI = setDefaultEMIDetails();
                                if (objEMI.MinDownPayment < 1 || objEMI.MaxDownPayment < 1)
                                {
                                    objEMI.MinDownPayment = _objEMI.MinDownPayment;
                                    objEMI.MaxDownPayment = _objEMI.MaxDownPayment;
                                }

                                if (objEMI.MinTenure < 1 || objEMI.MaxTenure < 1)
                                {
                                    objEMI.MinTenure = _objEMI.MinTenure;
                                    objEMI.MaxTenure = _objEMI.MaxTenure;
                                }

                                if (objEMI.MinRateOfInterest < 1 || objEMI.MaxRateOfInterest < 1)
                                {
                                    objEMI.MinRateOfInterest = _objEMI.MinRateOfInterest;
                                    objEMI.MaxRateOfInterest = _objEMI.MaxRateOfInterest;
                                }
                            }
                            else
                            {
                                objEMI = setDefaultEMIDetails();
                            }

                            detailedDealer.PrimaryDealer.EMIDetails = objEMI;

                            #endregion
                            #endregion

                        }
                        if (detailedDealer.PrimaryDealer.PriceList != null && detailedDealer.PrimaryDealer.PriceList.Count() > 0)
                        {
                            #region Dealer PriceQuote
                            objData.TotalPrice = (uint)detailedDealer.PrimaryDealer.TotalPrice;
                            #endregion
                        }
                        else
                        {
                            #region Bikewale PriceQuote
                            objData.Quotation = _objPQ.GetPriceQuoteById(Convert.ToUInt64(_pqId), LeadSourceEnum.DPQ_Desktop);
                            if (objData.Quotation != null)
                            {
                                objData.TotalPrice = (uint)objData.Quotation.OnRoadPrice;

                                #region Set manufacturer campaign details
                                objData.ManufacturerCampaign = new ManufacturerCampaign()
                                                       {
                                                           ShowAd = detailedDealer.PrimaryDealer.DealerDetails == null && detailedDealer.SecondaryDealerCount == 0,
                                                           Ad = Format.FormatManufacturerAd(objData.Quotation.ManufacturerAd, objData.Quotation.CampaignId, objData.Quotation.ManufacturerName, objData.Quotation.MaskingNumber, objData.Quotation.ManufacturerId, objData.Quotation.Area, objData.PQLeadSource.ToString(), objData.PQSourcePage.ToString(), string.Empty, string.Empty, string.Empty, string.IsNullOrEmpty(objData.Quotation.MaskingNumber) ? "hide" : string.Empty, objData.Quotation.LeadCapturePopupHeading, objData.Quotation.LeadCapturePopupDescription, objData.Quotation.LeadCapturePopupMessage, objData.Quotation.PinCodeRequired),
                                                           Name = objData.Quotation.ManufacturerName,
                                                           Id = objData.Quotation.ManufacturerId
                                                       };
                                #endregion
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    RedirectUrl = "/pricequote/";
                    Status = StatusCodes.RedirectPermanent;
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.DealerPriceQuotePage.SetDealerPriceQuoteDetail(): versionId {0}", _versionId));
            }

            objData.DetailedDealer = detailedDealer;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 2 Dec 2014
        /// Summary : To Fill version dropdownlist
        /// Created By : Sangram Nandkhile on 16 Dec 2016
        /// Summary : Fetch minspecs and show on DPQ
        /// </summary>
        private void GetBikeVersions(DealerPriceQuotePageVM objData)
        {
            IEnumerable<BikeVersionMinSpecs> minSpecs = null;
            try
            {

                objData.SelectedVersion = _objVersionCache.GetById(_versionId);
                if (objData.SelectedVersion != null && objData.SelectedVersion.MakeBase != null && objData.SelectedVersion.ModelBase != null)
                {
                    objData.BikeName = String.Format("{0} {1}", objData.SelectedVersion.MakeBase.MakeName, objData.SelectedVersion.ModelBase.ModelName);
                    _modelId = (uint)objData.SelectedVersion.ModelBase.ModelId;
                    _makeId = (uint)objData.SelectedVersion.MakeBase.MakeId;

                    objData.VersionsList = _objVersionCache.GetVersionsByType(EnumBikeType.PriceQuote, objData.SelectedVersion.ModelBase.ModelId, (int)_cityId);

                    if (objData.VersionsList != null)
                    {
                        minSpecs = _objVersionCache.GetVersionMinSpecs(_modelId, true);
                        var objMin = minSpecs.FirstOrDefault(x => x.VersionId == _versionId);
                        if (objMin != null)
                            objData.MinSpecsHtml = FormatVarientMinSpec(objMin);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.DealerPriceQuotePage.GetBikeVersions() versionId {0}", _versionId));
            }
        }

        /// <summary>
        /// Created BY : Sushil Kumar on 14th March 2015
        /// Summary : To set EMI details for the dealer if no EMI Details available for the dealer
        /// </summary>
        private EMI setDefaultEMIDetails()
        {
            EMI _objEMI = null;
            try
            {
                _objEMI = new EMI();
                _objEMI.MaxDownPayment = 40;
                _objEMI.MinDownPayment = 10;
                _objEMI.MaxTenure = 48;
                _objEMI.MinTenure = 12;
                _objEMI.MaxRateOfInterest = 15;
                _objEMI.MinRateOfInterest = 10;
                _objEMI.ProcessingFee = 2000;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.DealerPriceQuotePage.SetEMIDetails(): versionId {0}", _versionId));
            }
            return _objEMI;
        }

        /// <summary>
        /// Createdby: Sangram Nandkhile on 15 Dec 2016
        /// Summary: Format Minspecs for each version
        /// </summary>
        /// <returns></returns>
        private string FormatVarientMinSpec(BikeVersionMinSpecs objVersion)
        {
            string minSpecsStr = string.Empty;

            try
            {
                minSpecsStr = string.Format("{0}<li>{1} Wheels</li>", minSpecsStr, objVersion.AlloyWheels ? "Alloy" : "Spoke");
                minSpecsStr = string.Format("{0}<li>{1} Start</li>", minSpecsStr, objVersion.ElectricStart ? "Electric" : "Kick");

                if (objVersion.AntilockBrakingSystem)
                {
                    minSpecsStr = string.Format("{0}<li>ABS</li>", minSpecsStr);
                }

                if (!String.IsNullOrEmpty(objVersion.BrakeType))
                {
                    minSpecsStr = string.Format("{0}<li>{1} Brake</li>", minSpecsStr, objVersion.BrakeType);
                }


                if (!string.IsNullOrEmpty(minSpecsStr))
                {
                    minSpecsStr = string.Format("<ul id='version-specs-list'>{0}</ul>", minSpecsStr);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.DealerPriceQuotePage.FormatVarientMinSpec(): versionId {0}", _versionId));
            }

            return minSpecsStr;

        }

        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 16th March 2016
        /// Description : Private Method to proceess mpq queryString and set the for queried parameters viz. versionId,dealerId,cityId,pqId and areaId
        /// Modified By : Lucky Rathore
        /// Description : DealerId Assingment moved in "if" condition
        /// </summary>
        private void ProcessQueryString()
        {
            var request = HttpContext.Current.Request;
            try
            {

                if (PriceQuoteQueryString.IsPQQueryStringExists() && UInt32.TryParse(PriceQuoteQueryString.PQId, out _pqId) && UInt32.TryParse(PriceQuoteQueryString.VersionId, out _versionId) && UInt32.TryParse(PriceQuoteQueryString.CityId, out _cityId) && _pqId > 0 && _versionId > 0 && _cityId > 0)
                {
                    UInt32.TryParse(PriceQuoteQueryString.DealerId, out _dealerId);
                    UInt32.TryParse(PriceQuoteQueryString.AreaId, out _areaId);
                    pageUrl = request.ServerVariables["URL"];
                    mpqQueryString = request.QueryString["MPQ"];
                    Status = StatusCodes.ContentFound;
                }
                else
                {
                    RedirectUrl = "/pricequote/";
                    Status = StatusCodes.RedirectPermanent;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.DealerPriceQuotePage.ParseQueryString() versionid {0}, CityId {1}, PQId {2}", _versionId, _cityId, _pqId));
                Status = StatusCodes.ContentNotFound;
            }
        }
    }
}