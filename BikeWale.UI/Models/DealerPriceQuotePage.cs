using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
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
        private readonly IPriceQuote _objPQ = null;

        public string redirectUrl;
        public StatusCodes status;



        public PQ_QuotationEntity objPrice = null;

        public PopularDealerServiceCenter cityDealers;
        public DealersEntity dealers { get; set; }
        public List<VersionColor> objColors = null;

        public EMI _objEMI;

        private uint modelId, versionId, cityId, areaId, pqId, dealerId;

        public string bikeName = string.Empty, bikeVersionName = string.Empty, minspecs = string.Empty, pageUrl = string.Empty, clientIP = CommonOpn.GetClientIP(),
            location = string.Empty, leadBtnLargeText = "Get offers from dealer", dealerName, dealerArea, dealerAddress, makeName, modelName, versionName, mpqQueryString, pq_leadsource = "34", pq_sourcepage = "58", currentCity = string.Empty, currentArea = string.Empty;

        public uint totalPrice = 0, offerCount = 0, bookingAmount;
        public bool isBWPriceQuote, isPrimaryDealer, isUSPBenfits, isoffer, isEMIAvailable, IsDiscount, isSecondaryDealerAvailable = false, isPremium, isStandard, isDeluxe;

        public double latitude, longitude;

        public DealerPackageTypes dealerType;
        public DealerQuotationEntity primarydealer = null;
        public BikeQuotationEntity objQuotation = null;
        public IEnumerable<PQ_Price> primaryPriceList = null;

        /// <summary>
        /// 
        /// </summary>
        public DealerPriceQuotePage(IDealerPriceQuoteDetail objDealerPQDetails, IDealerPriceQuote objDealerPQ, IBikeVersionCacheRepository<BikeVersionEntity, uint> objVersionCache, IAreaCacheRepository objAreaCache, ICityCacheRepository objCityCache, IPriceQuote objPQ)
        {
            _objDealerPQDetails = objDealerPQDetails;
            _objDealerPQ = objDealerPQ;
            _objVersionCache = objVersionCache;
            _objAreaCache = objAreaCache;
            _objCityCache = objCityCache;
            _objPQ = objPQ;

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
                if (versionId > 0)
                {
                    objData.DealerId = dealerId;
                    objData.CityId = cityId;
                    objData.AreaId = areaId;
                    objData.PQId = pqId;
                    GetBikeVersions(objData);
                    //hdnVariant.Value = versionId.ToString();
                    SetDealerPriceQuoteDetail(cityId, versionId, dealerId, ref objData);
                    objData.Location = GetLocationCookie();
                    //objData.mpqQueryString = EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(Convert.ToString(cityId), Convert.ToString(pqId), Convert.ToString(areaId), Convert.ToString(versionId), Convert.ToString(dealerId)));
                    BindPageWidgets();
                }


            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Models.DealerPriceQuotePage.GetData()");
            }

            return objData;
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 11th Jan 2016
        /// Description : Bind page related widgets
        /// Modifued By :- Subodh Jain 01 march 2017
        /// Summary :- lead capture pop up
        /// </summary>
        private void BindPageWidgets()
        {

            //try
            //{
            //    if (objVersionDetails != null)
            //    {
            //        ctrlAlternativeBikes.VersionId = Convert.ToUInt32(versionId);
            //        ctrlAlternativeBikes.PQSourceId = (int)PQSourceEnum.Desktop_DPQ_Alternative;
            //        ctrlAlternativeBikes.cityId = cityId;

            //        if (objVersionDetails.ModelBase != null)
            //        {
            //            ctrlAlternativeBikes.model = objVersionDetails.ModelBase.ModelName;

            //            if (ctrlDealers != null && objVersionDetails.MakeBase != null)
            //            {
            //                ctrlDealers.MakeId = (uint)objVersionDetails.MakeBase.MakeId;
            //                ctrlDealers.CityId = cityId;
            //                ctrlDealers.IsDiscontinued = false;
            //                ctrlDealers.TopCount = 3;
            //                ctrlDealers.ModelId = modelId;
            //                ctrlDealers.PQSourceId = (int)PQSourceEnum.Desktop_Dealerpricequote_DealersCard_GetOfferButton;
            //                ctrlDealers.widgetHeading = string.Format("{0} showrooms {1}", objVersionDetails.MakeBase.MakeName, !string.IsNullOrEmpty(currentCity) ? "in " + currentCity : string.Empty);
            //                ctrlDealers.pageName = "DealerPriceQuote_Page";

            //                ctrlLeadCapture.AreaId = areaId;
            //                ctrlLeadCapture.ModelId = modelId;
            //                ctrlLeadCapture.CityId = cityId;

            //            }
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    ErrorClass objErr = new ErrorClass(ex, "Bikewale.BikeBooking.Dealerpricequote.BindPageWidgets");
            //}

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
                var cities = _objCityCache.GetPriceQuoteCities(modelId);

                if (cities != null)
                {
                    Entities.Location.CityEntityBase city = cities.FirstOrDefault(m => m.CityId == cityId);
                    currentCity = city != null ? city.CityName : String.Empty;

                    IEnumerable<Entities.Location.AreaEntityBase> areas = _objAreaCache.GetAreaList(modelId, cityId);
                    if (areas != null)
                    {
                        Entities.Location.AreaEntityBase area = areas.FirstOrDefault(m => m.AreaId == areaId);
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
        private void SetDealerPriceQuoteDetail(uint cityId, uint versionId, uint dealerId, ref DealerPriceQuotePageVM objData)
        {
            Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity detailedDealer = null;
            try
            {
                detailedDealer = _objDealerPQDetails.GetDealerQuotationV2(cityId, versionId, dealerId, areaId);
                if (detailedDealer != null)
                {
                    //if (detailedDealer.objVersion != null)
                    //{
                    //    objData.versionName = detailedDealer.objVersion.VersionName;
                    //}

                    if (detailedDealer.PrimaryDealer != null)
                    {
                        primarydealer = detailedDealer.PrimaryDealer;
                        //if (detailedDealer.PrimaryDealer.DealerDetails != null)
                        //{
                        //    objData.LeadBtnLargeText = primarydealer.DealerDetails.DisplayTextLarge;
                        //}
                        objData.primaryPriceList = primarydealer.PriceList;
                        IEnumerable<OfferEntityBase> offerList = primarydealer.OfferList;
                        if (primaryPriceList != null && primaryPriceList.Count() > 0)
                        {
                            objData.TotalPrice = (uint)primaryPriceList.Sum(x => x.Price);
                        }
                        else
                        {
                            objQuotation = _objPQ.GetPriceQuoteById(Convert.ToUInt64(pqId), LeadSourceEnum.DPQ_Desktop);
                            objData.IsBWPriceQuote = true;
                            objData.TotalPrice = (uint)objQuotation.OnRoadPrice;
                            if (objQuotation != null)
                            {
                                objData.ManufacturerCampaign = new ManufacturerCampaign()
                                {
                                    ShowAd = primarydealer.DealerDetails == null && detailedDealer.SecondaryDealerCount == 0,
                                    Ad = Format.FormatManufacturerAd(objQuotation.ManufacturerAd, objQuotation.CampaignId, objQuotation.ManufacturerName, objQuotation.MaskingNumber, objQuotation.ManufacturerId, objQuotation.Area, pq_leadsource, pq_sourcepage, string.Empty, string.Empty, string.Empty, string.IsNullOrEmpty(objQuotation.MaskingNumber) ? "hide" : string.Empty, objQuotation.LeadCapturePopupHeading, objQuotation.LeadCapturePopupDescription, objQuotation.LeadCapturePopupMessage, objQuotation.PinCodeRequired),
                                    Name = objQuotation.ManufacturerName,
                                    Id = objQuotation.ManufacturerId
                                };
                            }
                            objData.Quotation = objQuotation;

                        }

                        if (primarydealer.DealerDetails != null)
                        {
                            NewBikeDealers dealerDetails = primarydealer.DealerDetails;
                            objData.dealerName = dealerDetails.Organization;
                            objData.dealerArea = dealerDetails.objArea.AreaName;
                            objData.dealerAddress = dealerDetails.Address;
                            objData.latitude = dealerDetails.objArea.Latitude;
                            objData.longitude = dealerDetails.objArea.Longitude;
                            objData.DealerType = dealerDetails.DealerPackageType;

                            switch (objData.DealerType)
                            {
                                case DealerPackageTypes.Premium: objData.IsPremium = true;
                                    break;
                                case DealerPackageTypes.Standard: objData.IsStandard = true;
                                    break;
                                case DealerPackageTypes.Deluxe: objData.IsDeluxe = true;
                                    break;
                            }
                        }

                        if (primarydealer.OfferList != null && primarydealer.OfferList.Count() > 0)
                        {
                            objData.offerCount = (uint)primarydealer.OfferList.Count();
                            objData.isoffer = true;
                        }

                        //booking amount
                        if (primarydealer.IsBookingAvailable)
                        {
                            objData.bookingAmount = primarydealer.BookingAmount;
                        }
                        //EMI details
                        if (primarydealer.EMIDetails != null)
                        {
                            objData._objEMI = setDefaultEMIDetails();
                            if (primarydealer.EMIDetails.MinDownPayment < 1 || primarydealer.EMIDetails.MaxDownPayment < 1)
                            {
                                primarydealer.EMIDetails.MinDownPayment = _objEMI.MinDownPayment;
                                primarydealer.EMIDetails.MaxDownPayment = _objEMI.MaxDownPayment;
                            }

                            if (primarydealer.EMIDetails.MinTenure < 1 || primarydealer.EMIDetails.MaxTenure < 1)
                            {
                                primarydealer.EMIDetails.MinTenure = _objEMI.MinTenure;
                                primarydealer.EMIDetails.MaxTenure = _objEMI.MaxTenure;
                            }

                            if (primarydealer.EMIDetails.MinRateOfInterest < 1 || primarydealer.EMIDetails.MaxRateOfInterest < 1)
                            {
                                primarydealer.EMIDetails.MinRateOfInterest = _objEMI.MinRateOfInterest;
                                primarydealer.EMIDetails.MaxRateOfInterest = _objEMI.MaxRateOfInterest;
                            }
                        }
                        else
                        {
                            primarydealer.EMIDetails = setDefaultEMIDetails();
                        }
                    }
                    objData.PrimaryDealer = primarydealer;
                }
                else
                {
                    redirectUrl = "/pricequote/";
                    status = StatusCodes.RedirectTemporary;
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Desktop: PriceQuote.DealerPriceQuote.aspx ==> SetDealerPriceQuoteDetail(): versionId {0}", versionId));
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

                objData.SelectedVersion = _objVersionCache.GetById(versionId);
                if (objData.SelectedVersion != null && objData.SelectedVersion.MakeBase != null && objData.SelectedVersion.ModelBase != null)
                {
                    objData.BikeName = String.Format("{0} {1}", objData.SelectedVersion.MakeBase.MakeName, objData.SelectedVersion.ModelBase.ModelName);
                    modelId = (uint)objData.SelectedVersion.ModelBase.ModelId;
                    objData.VersionsList = _objVersionCache.GetVersionsByType(EnumBikeType.PriceQuote, objData.SelectedVersion.ModelBase.ModelId, (int)cityId);

                    if (objData.VersionsList != null)
                    {
                        minSpecs = _objVersionCache.GetVersionMinSpecs(modelId, true);
                        var objMin = minSpecs.FirstOrDefault(x => x.VersionId == versionId);
                        if (objMin != null)
                            objData.MinSpecsHtml = FormatVarientMinSpec(objMin);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.DealerPriceQuotePage.BindVersion() versionId {0}", versionId));
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
                if (cityId > 0)
                {
                    objPQEntity.CityId = cityId;
                    objPQEntity.AreaId = areaId;
                    objPQEntity.SourceId = Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["sourceId"]);
                    objPQEntity.VersionId = versionId;
                    objPQEntity.PQLeadId = Convert.ToUInt16(PQSourceEnum.Desktop_DPQ_Quotation);
                    objPQEntity.UTMA = request.Cookies["__utma"] != null ? request.Cookies["__utma"].Value : "";
                    objPQEntity.UTMZ = request.Cookies["_bwutmz"] != null ? request.Cookies["_bwutmz"].Value : "";
                    objPQEntity.DeviceId = request.Cookies["BWC"] != null ? request.Cookies["BWC"].Value : "";
                    objPQOutput = _objDealerPQ.ProcessPQ(objPQEntity);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Desktop: PriceQuote.DealerPriceQuote.aspx ==> SavePriceQuote(): versionId {0}", versionId));
            }
            finally
            {
                if (objPQOutput != null && objPQOutput.PQId > 0)
                {
                    redirectUrl = "/pricequote/dealerpricequote.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), versionId.ToString(), Convert.ToString(dealerId)));
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("Desktop: PriceQuote.DealerPriceQuote.aspx ==> setEMIDetails(): versionId {0}", versionId));
                objErr.SendMail();
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

                if (PriceQuoteQueryString.IsPQQueryStringExists() && UInt32.TryParse(PriceQuoteQueryString.PQId, out pqId) && UInt32.TryParse(PriceQuoteQueryString.VersionId, out versionId))
                {
                    UInt32.TryParse(PriceQuoteQueryString.DealerId, out dealerId);
                    UInt32.TryParse(PriceQuoteQueryString.CityId, out cityId);
                    UInt32.TryParse(PriceQuoteQueryString.AreaId, out areaId);
                    pageUrl = request.ServerVariables["URL"];
                }
                else
                {
                    redirectUrl = "/pricequote/quotation.aspx";
                    status = StatusCodes.RedirectTemporary;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Desktop: PriceQuote.DealerPriceQuote.aspx ==> ParseQueryString() versionid {0}", versionId));
                status = StatusCodes.ContentNotFound;
            }

            if (request.HttpMethod.Equals("POST"))
            {
                if (!String.IsNullOrEmpty(request.Form["hdnVersionId"]))
                    versionId = Convert.ToUInt32(request.Form["hdnVersionId"]);
                SavePriceQuote();
            }
        }
    }
}