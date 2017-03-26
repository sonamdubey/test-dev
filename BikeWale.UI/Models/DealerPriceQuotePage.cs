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
    /// 
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

        public string redirectUrl;
        public StatusCodes status;

        private uint _modelId, _versionId, _cityId, _areaId, _pqId, _dealerId, _makeId;
        private string pageUrl, mpqQueryString, currentCity = string.Empty, currentArea = string.Empty;

        /// <summary>
        /// 
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
        /// Function to get the expert reviews landing page data
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

        private void SetPageMetas(DealerPriceQuotePageVM objData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
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
                //if (objVersionDetails != null)
                //{
                //    ctrlAlternativeBikes.VersionId = Convert.ToUInt32(versionId);
                //    ctrlAlternativeBikes.PQSourceId = (int)PQSourceEnum.Desktop_DPQ_Alternative;
                //    ctrlAlternativeBikes.cityId = cityId;

                //    if (objVersionDetails.ModelBase != null)
                //    {
                //        ctrlAlternativeBikes.model = objVersionDetails.ModelBase.ModelName;

                //        if (ctrlDealers != null && objVersionDetails.MakeBase != null)
                //        {
                //            //ctrlDealers.MakeId = (uint)objVersionDetails.MakeBase.MakeId;
                //            //ctrlDealers.CityId = cityId;
                //            //ctrlDealers.IsDiscontinued = false;
                //            //ctrlDealers.TopCount = 3;
                //            //ctrlDealers.ModelId = _modelId;
                //            //ctrlDealers.PQSourceId = (int)PQSourceEnum.Desktop_Dealerpricequote_DealersCard_GetOfferButton;
                //            //ctrlDealers.widgetHeading = string.Format("{0} showrooms {1}", objVersionDetails.MakeBase.MakeName, !string.IsNullOrEmpty(currentCity) ? "in " + currentCity : string.Empty);
                //            //ctrlDealers.pageName = "DealerPriceQuote_Page";

                //        }
                //    }

                //}
                objData.OtherDealers = _objDealerCache.GetDealerByMakeCity(_cityId, _makeId);

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
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BikeBooking.Dealerpricequote.BindPageWidgets");
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BikeBooking.Dealerpricequote.GetLocationCookie");
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
                            if (string.IsNullOrEmpty(detailedDealer.PrimaryDealer.DealerDetails.DisplayTextLarge))
                                detailedDealer.PrimaryDealer.DealerDetails.DisplayTextLarge = objData.LeadBtnLongText;

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

                        }
                        if (!objData.IsDealerPriceQuote)
                        {
                            objData.Quotation = _objPQ.GetPriceQuoteById(Convert.ToUInt64(_pqId), LeadSourceEnum.DPQ_Desktop);
                            SetManufacturerAd(objData);
                        }
                        else
                        {
                            objData.TotalPrice = (uint)detailedDealer.PrimaryDealer.TotalPrice;
                        }
                    }
                }
                else
                {
                    redirectUrl = "/pricequote/";
                    status = StatusCodes.RedirectTemporary;
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Desktop: PriceQuote.DealerPriceQuote.aspx ==> SetDealerPriceQuoteDetail(): versionId {0}", _versionId));
            }

            objData.DetailedDealer = detailedDealer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objData"></param>
        private void SetManufacturerAd(DealerPriceQuotePageVM objData)
        {
            try
            {
                var objQuotation = objData.Quotation;
                if (objQuotation != null)
                {
                    objData.TotalPrice = (uint)objQuotation.OnRoadPrice;

                    objData.ManufacturerCampaign = new ManufacturerCampaign()
                    {
                        ShowAd = objData.DetailedDealer.PrimaryDealer.DealerDetails == null && objData.DetailedDealer.SecondaryDealerCount == 0,
                        Ad = Format.FormatManufacturerAd(objQuotation.ManufacturerAd, objQuotation.CampaignId, objQuotation.ManufacturerName, objQuotation.MaskingNumber, objQuotation.ManufacturerId, objQuotation.Area, objData.PQLeadSource.ToString(), objData.PQSourcePage.ToString(), string.Empty, string.Empty, string.Empty, string.IsNullOrEmpty(objQuotation.MaskingNumber) ? "hide" : string.Empty, objQuotation.LeadCapturePopupHeading, objQuotation.LeadCapturePopupDescription, objQuotation.LeadCapturePopupMessage, objQuotation.PinCodeRequired),
                        Name = objQuotation.ManufacturerName,
                        Id = objQuotation.ManufacturerId
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.DealerPriceQuotePage.SetManufacturerAd()");
            }
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.DealerPriceQuotePage.BindVersion() versionId {0}", _versionId));
            }
        }

        /// <summary>
        /// Modified By : Sushil Kumar on 18th March 2016
        /// Description : Changed finally section from code as no check was made for objPQOutput == null
        /// Modified By : Vivek Gupta on 29-04-2016
        /// Desc : In case of dealerId=0 and isDealerAvailable = true , while redirecting to pricequotes ,don't redirect to BW PQ redirect to dpq
        /// Modified By : Lucky Rathore on 27 June 2016
        /// Description : replace cookie __utmz with _bwutmz
        /// </summary>
        private void SavePriceQuote()
        {
            PQOutputEntity objPQOutput = null;
            var request = HttpContext.Current.Request;
            try
            {
                PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                if (_cityId > 0)
                {
                    objPQEntity.CityId = _cityId;
                    objPQEntity.AreaId = _areaId;
                    objPQEntity.SourceId = Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["sourceId"]);
                    objPQEntity.VersionId = _versionId;
                    objPQEntity.PQLeadId = Convert.ToUInt16(PQSourceEnum.Desktop_DPQ_Quotation);
                    objPQEntity.UTMA = request.Cookies["__utma"] != null ? request.Cookies["__utma"].Value : "";
                    objPQEntity.UTMZ = request.Cookies["_bwutmz"] != null ? request.Cookies["_bwutmz"].Value : "";
                    objPQEntity.DeviceId = request.Cookies["BWC"] != null ? request.Cookies["BWC"].Value : "";
                    objPQOutput = _objDealerPQ.ProcessPQ(objPQEntity);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Desktop: PriceQuote.DealerPriceQuote.aspx ==> SavePriceQuote(): versionId {0}", _versionId));
            }
            finally
            {
                if (objPQOutput != null && objPQOutput.PQId > 0)
                {
                    redirectUrl = "/pricequote/dealerpricequote.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(_cityId.ToString(), objPQOutput.PQId.ToString(), _areaId.ToString(), _versionId.ToString(), Convert.ToString(_dealerId)));
                    status = StatusCodes.RedirectTemporary;
                }
                else
                {
                    redirectUrl = "/pricequote/";
                    status = StatusCodes.RedirectTemporary;
                }
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("Desktop: PriceQuote.DealerPriceQuote.aspx ==> setEMIDetails(): versionId {0}", _versionId));
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

            minSpecsStr = string.Format("{0}<li>{1} Wheels</li>", minSpecsStr, objVersion.AlloyWheels ? "Alloy" : "Spoke");
            minSpecsStr = string.Format("{0}<li>{1} Start</li>", minSpecsStr, objVersion.ElectricStart ? "Electric" : "Kick");

            if (objVersion.AntilockBrakingSystem)
            {
                minSpecsStr = string.Format("{0}<li>ABS</li>");
            }

            if (!String.IsNullOrEmpty(objVersion.BrakeType))
            {
                minSpecsStr = string.Format("{0}<li>{1} Brake</li>", objVersion.BrakeType);
            }


            if (!string.IsNullOrEmpty(minSpecsStr))
            {
                minSpecsStr = string.Format("<ul id='version-specs-list'>{0}</ul>", minSpecsStr);
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

                if (PriceQuoteQueryString.IsPQQueryStringExists() && UInt32.TryParse(PriceQuoteQueryString.PQId, out _pqId) && UInt32.TryParse(PriceQuoteQueryString.VersionId, out _versionId))
                {
                    UInt32.TryParse(PriceQuoteQueryString.DealerId, out _dealerId);
                    UInt32.TryParse(PriceQuoteQueryString.CityId, out _cityId);
                    UInt32.TryParse(PriceQuoteQueryString.AreaId, out _areaId);
                    pageUrl = request.ServerVariables["URL"];
                    mpqQueryString = request.QueryString["MPQ"];

                }
                else
                {
                    redirectUrl = "/pricequote/quotation.aspx";
                    status = StatusCodes.RedirectTemporary;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Desktop: PriceQuote.DealerPriceQuote.aspx ==> ParseQueryString() versionid {0}", _versionId));
                status = StatusCodes.ContentNotFound;
            }

            if (request.HttpMethod.Equals("POST"))
            {
                if (!String.IsNullOrEmpty(request.Form["hdnVersionId"]))
                    _versionId = Convert.ToUInt32(request.Form["hdnVersionId"]);
                SavePriceQuote();
            }
        }
    }
}