using Bikewale.BAL.BikeData;
using Bikewale.BAL.PriceQuote;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.BikeBooking
{
    /// <summary>
    /// Modified By : Lucky Rathore
    /// Modified On : 31 March 2016
    /// Description : Removed rptColors function.
    /// Modified By : Sangram Nandkhile
    /// Modified On : 15 Dec 2016
    /// Description : Better designed dealer property, removed unnecessary code
    /// </summary>
    public class DealerPriceQuote : System.Web.UI.Page
    {
        #region Variables

        protected GlobalCityAreaEntity CityArea { get; set; }
        protected DropDownList ddlVersion;
        protected HtmlGenericControl div_GetPQ;
        protected PQ_QuotationEntity objPrice = null;
        protected List<VersionColor> objColors = null;
        protected BikeVersionEntity objVersionDetails = null;
        protected List<BikeVersionsListEntity> versionList = null;
        protected NewAlternativeBikes ctrlAlternativeBikes;
        protected EMI _objEMI;
        protected string bikeName = string.Empty, bikeVersionName = string.Empty, minspecs = string.Empty, pageUrl = string.Empty, clientIP = CommonOpn.GetClientIP(),
            location = string.Empty, dealerName, dealerArea, dealerAddress, makeName, modelName, versionName, mpqQueryString, pq_leadsource = "34", pq_sourcepage = "58";

        protected uint totalPrice = 0, bookingAmount, dealerId = 0, cityId = 0, versionId = 0, pqId = 0, areaId = 0, insuranceAmount = 0, totalDiscount = 0;
        protected bool isBWPriceQuote, isPrimaryDealer, IsInsuranceFree, isUSPBenfits, isoffer, isEMIAvailable, IsDiscount, isSecondaryDealerAvailable = false, isPremium, isStandard, isDeluxe;
        protected CustomerEntity objCustomer = new CustomerEntity();
        protected Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity detailedDealer = null;
        protected double latitude, longitude;
        protected HiddenField hdnVariant, hdnDealerId;
        protected Label defaultVariant;
        protected DealerPackageTypes dealerType;
        protected DealerQuotationEntity primarydealer = null;
        private IPriceQuote objPriceQuote = null;
        protected BikeQuotationEntity objQuotation = null;
        protected IEnumerable<PQ_Price> primaryPriceList = null;

        #endregion Variables

        #region events

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified By : Lucky Rathore
        /// Modified On : 16 March 2016
        /// Description :  Chages for updating version id functionality, function SetDealerPriceQuoteDetail(cityId, versionId, dealerId) used.
        /// Modified By : Sushil Kumar on 18th March 2016
        /// Description : Moved process query functionality to new function ProcessQueryString();
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            DetectDevice();
            ParseQueryString();
            if (versionId > 0)
            {
                BindVersion();
                hdnVariant.Value = versionId.ToString();
                SetDealerPriceQuoteDetail(cityId, versionId, dealerId);
                location = GetLocationCookie();
                BindAlternativeBikeControl(versionId.ToString());
                mpqQueryString = EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(Convert.ToString(cityId), Convert.ToString(pqId), Convert.ToString(areaId), Convert.ToString(versionId), Convert.ToString(dealerId)));
            }
            else
            {
                Response.Redirect("/pricequote/quotation.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        #endregion

        #region methods
        /// <summary>
        /// Created By : Lucky Rathore
        /// Created on : 15 March 2016
        /// Description : for Dealer Basics details.
        /// Modified By : Sushil Kumar on 17th March 2016
        /// Description  : Added default values for emi if no emi details is available
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="versionId"></param>
        /// <param name="dealerId"></param>
        private void SetDealerPriceQuoteDetail(uint cityId, uint versionId, uint dealerId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuoteDetail, DealerPriceQuoteDetail>();
                    IDealerPriceQuoteDetail objIPQ = container.Resolve<IDealerPriceQuoteDetail>();
                    detailedDealer = objIPQ.GetDealerQuotationV2(cityId, versionId, dealerId, areaId);

                    if (detailedDealer != null)
                    {
                        if (detailedDealer.objVersion != null)
                        {
                            versionName = detailedDealer.objVersion.VersionName;
                        }

                        if (detailedDealer.PrimaryDealer != null)
                        {
                            primarydealer = detailedDealer.PrimaryDealer;
                            if (detailedDealer.PrimaryDealer.DealerDetails != null)
                                isPrimaryDealer = true;
                            primaryPriceList = primarydealer.PriceList;
                            IEnumerable<OfferEntityBase> offerList = primarydealer.OfferList;
                            if (primaryPriceList != null && primaryPriceList.Count() > 0)
                            {
                                totalPrice = (uint)primaryPriceList.Sum(x => x.Price);
                            }
                            else
                            {
                                container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                                objPriceQuote = container.Resolve<IPriceQuote>();
                                objQuotation = objPriceQuote.GetPriceQuoteById(Convert.ToUInt64(pqId), LeadSourceEnum.DPQ_Desktop);
                                isBWPriceQuote = true;
                                totalPrice = (uint)objQuotation.OnRoadPrice;
                                if (objQuotation != null)
                                    objQuotation.ManufacturerAd = Format.FormatManufacturerAd(objQuotation.ManufacturerAd, objQuotation.CampaignId, objQuotation.ManufacturerName, objQuotation.MaskingNumber, Convert.ToString(objQuotation.ManufacturerId), objQuotation.Area, pq_leadsource, pq_sourcepage, string.Empty, string.Empty, string.Empty, string.IsNullOrEmpty(objQuotation.MaskingNumber) ? "hide" : string.Empty);
                            }

                            if (primarydealer.DealerDetails != null)
                            {
                                NewBikeDealers dealerDetails = primarydealer.DealerDetails;
                                dealerName = dealerDetails.Organization;
                                dealerArea = dealerDetails.objArea.AreaName;
                                dealerAddress = dealerDetails.Address;
                                latitude = dealerDetails.objArea.Latitude;
                                longitude = dealerDetails.objArea.Longitude;
                                dealerType = dealerDetails.DealerPackageType;

                                switch (dealerType)
                                {
                                    case DealerPackageTypes.Premium: isPremium = true;
                                        break;
                                    case DealerPackageTypes.Standard: isStandard = true;
                                        break;
                                    case DealerPackageTypes.Deluxe: isDeluxe = true;
                                        break;
                                }
                            }

                            if (primarydealer.OfferList != null && primarydealer.OfferList.Count() > 0)
                            {
                                isoffer = true;
                            }

                            if (primarydealer.Benefits != null && primarydealer.Benefits.Count() > 0)
                            {
                                isUSPBenfits = true;
                            }

                            //booking amount
                            if (primarydealer.IsBookingAvailable)
                            {
                                bookingAmount = primarydealer.BookingAmount;
                            }
                            //EMI details
                            if (primarydealer.EMIDetails != null)
                            {
                                _objEMI = setEMIDetails();
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
                                primarydealer.EMIDetails = setEMIDetails();
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("/pricequote/quotation.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Desktop: PriceQuote.DealerPriceQuote.aspx ==> SetDealerPriceQuoteDetail(): versionId {0}", versionId));
                objErr.SendMail();
            }

        }

        /// <summary>
        /// Created BY : Sushil Kumar on 14th March 2015
        /// Summary : To set EMI details for the dealer if no EMI Details available for the dealer
        /// </summary>
        private EMI setEMIDetails()
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
        /// Created By : Sadhana Upadhyay on 2 Dec 2014
        /// Summary : To Fill version dropdownlist
        /// Created By : Sangram Nandkhile on 16 Dec 2016
        /// Summary : Fetch minspecs and show on DPQ
        /// </summary>
        protected void BindVersion()
        {
            IEnumerable<BikeVersionMinSpecs> minSpecs = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeVersionCacheRepository<BikeVersionEntity, uint>, BikeVersionsCacheRepository<BikeVersionEntity, uint>>()
                            .RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersions<BikeVersionEntity, uint>>()
                            .RegisterType<ICacheManager, MemcacheManager>()
                            ;
                    var objCache = container.Resolve<IBikeVersionCacheRepository<BikeVersionEntity, uint>>();

                    objVersionDetails = objCache.GetById(versionId);
                    if (objVersionDetails != null && objVersionDetails.ModelBase != null)
                    {
                        modelName = objVersionDetails.ModelBase.ModelName;
                        makeName = objVersionDetails.MakeBase.MakeName;
                        bikeName = String.Format("{0} {1}", makeName, modelName);
                        bikeVersionName = objVersionDetails.BikeName;
                        versionList = objCache.GetVersionsByType(EnumBikeType.PriceQuote, objVersionDetails.ModelBase.ModelId, Convert.ToInt32(PriceQuoteQueryString.CityId));
                        minSpecs = objCache.GetVersionMinSpecs((uint)objVersionDetails.ModelBase.ModelId, true);

                        if (versionList != null && versionList.Count > 0)
                        {
                            ddlVersion.DataSource = versionList;
                            ddlVersion.DataValueField = "VersionId";
                            ddlVersion.DataTextField = "VersionName";
                            if (versionId > 0)
                                ddlVersion.SelectedValue = versionId.ToString();
                            ddlVersion.DataBind();
                            var objMin = minSpecs.FirstOrDefault(x => x.VersionId == versionId);
                            if (objMin != null)
                                minspecs = FormatVarientMinSpec(objMin);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Desktop: PriceQuote.DealerPriceQuote.aspx ==> BindVersion() versionId {0}", versionId));
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Description : To bind alternative bikes controle.
        /// </summary>
        /// <param name="versionId"></param>
        private void BindAlternativeBikeControl(String versionId)
        {
            ctrlAlternativeBikes.TopCount = 9;

            if (!String.IsNullOrEmpty(versionId) && versionId != "0")
            {
                ctrlAlternativeBikes.VersionId = Convert.ToUInt32(versionId);
                ctrlAlternativeBikes.PQSourceId = (int)PQSourceEnum.Desktop_DPQ_Alternative;
                ctrlAlternativeBikes.cityId = cityId;
                ctrlAlternativeBikes.model = modelName;
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
        protected void SavePriceQuote()
        {
            uint cityId = Convert.ToUInt32(PriceQuoteQueryString.CityId), areaId = Convert.ToUInt32(PriceQuoteQueryString.AreaId);
            PQOutputEntity objPQOutput = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    // save price quote
                    container.RegisterType<IDealerPriceQuote, BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objIPQ = container.Resolve<IDealerPriceQuote>();

                    PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                    if (cityId > 0)
                    {
                        objPQEntity.CityId = cityId;
                        objPQEntity.AreaId = areaId;
                        objPQEntity.SourceId = Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["sourceId"]);
                        objPQEntity.VersionId = versionId;
                        objPQEntity.PQLeadId = Convert.ToUInt16(PQSourceEnum.Desktop_DPQ_Quotation);
                        objPQEntity.UTMA = Request.Cookies["__utma"] != null ? Request.Cookies["__utma"].Value : "";
                        objPQEntity.UTMZ = Request.Cookies["_bwutmz"] != null ? Request.Cookies["_bwutmz"].Value : "";
                        objPQEntity.DeviceId = Request.Cookies["BWC"] != null ? Request.Cookies["BWC"].Value : "";
                        objPQOutput = objIPQ.ProcessPQ(objPQEntity);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Desktop: PriceQuote.DealerPriceQuote.aspx ==> SavePriceQuote(): versionId {0}", versionId));
                objErr.SendMail();
            }
            finally
            {
                if (objPQOutput != null && objPQOutput.PQId > 0)
                {
                    // Save pq cookie
                    Response.Redirect("/pricequote/dealerpricequote.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), versionId.ToString(), Convert.ToString(dealerId))), false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                else
                {
                    Response.Redirect("/pricequote/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 15th March 2016
        /// Description : To set user location
        /// Modified By : Aditi srivastava on 17 Nov 2016
        /// Description : get city area name from global city
        /// </summary>
        /// <returns></returns>

        private string GetLocationCookie()
        {
            string location = String.Empty;
            if (this.Context.Request.Cookies.AllKeys.Contains("location") && this.Context.Request.Cookies["location"].Value != "0")
            {
                location = this.Context.Request.Cookies["location"].Value.Replace('-', ' ');
                string[] arr = location.Split('_');
                if (arr.Length > 0)
                {
                    if (arr.Length > 2)
                    {
                        return String.Format("<span>{0}</span>, <span>{1}</span>", arr[3], arr[1]);
                    }
                    return String.Format("<span>{0}</span>", arr[1]);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 16th March 2016
        /// Description : Private Method to proceess mpq queryString and set the for queried parameters viz. versionId,dealerId,cityId,pqId and areaId
        /// Modified By : Lucky Rathore
        /// Description : DealerId Assingment moved in "if" condition
        /// </summary>
        private void ParseQueryString()
        {
            try
            {
                if (PriceQuoteQueryString.IsPQQueryStringExists() && UInt32.TryParse(PriceQuoteQueryString.PQId, out pqId) && UInt32.TryParse(PriceQuoteQueryString.VersionId, out versionId))
                {
                    UInt32.TryParse(PriceQuoteQueryString.DealerId, out dealerId);
                    UInt32.TryParse(PriceQuoteQueryString.CityId, out cityId);
                    UInt32.TryParse(PriceQuoteQueryString.AreaId, out areaId);
                    pageUrl = Request.ServerVariables["URL"];
                }
                else
                {
                    Response.Redirect("/pricequote/quotation.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Desktop: PriceQuote.DealerPriceQuote.aspx ==> ParseQueryString() versionid {0}", versionId));
                objErr.SendMail();
            }
            if (IsPostBack)
            {
                if (hdnVariant != null && !string.IsNullOrEmpty(hdnVariant.Value))
                    versionId = Convert.ToUInt32(hdnVariant.Value);
                SavePriceQuote();
            }
        }

        /// <summary>
        /// Created By:Sangram Nandkhile on 07 Dec 2016
        /// Summary: Detect device and redirect
        /// </summary>
        private void DetectDevice()
        {
            Form.Action = Request.RawUrl;
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();
        }

        /// <summary>
        /// Createdby: Sangram Nandkhile on 15 Dec 2016
        /// Summary: Format Minspecs for each version
        /// </summary>
        /// <returns></returns>
        protected string FormatVarientMinSpec(BikeVersionMinSpecs objVersion)
        {
            string returnStr = string.Empty;
            StringBuilder strMinSpecs = new StringBuilder();
            strMinSpecs.Append("<ul id='version-specs-list'>");

            if (objVersion.AlloyWheels)
            {
                strMinSpecs.Append("<li>Alloy Wheels</li>");
            }
            else
            {
                strMinSpecs.Append("<li>Spoke Wheels</li>");
            }
            if (objVersion.ElectricStart)
            {
                strMinSpecs.Append("<li>Electric Start</li>");
            }
            else
            {
                strMinSpecs.Append("<li>Kick Start</li>");
            }
            if (objVersion.AntilockBrakingSystem)
            {
                strMinSpecs.Append("<li>ABS</li>");
            }
            if (!String.IsNullOrEmpty(objVersion.BrakeType))
            {
                strMinSpecs.Append(string.Format("<li>{0} Brake</li>", objVersion.BrakeType));
            }
            strMinSpecs.Append("</ul>");
            returnStr = strMinSpecs.ToString();
            if (String.IsNullOrEmpty(returnStr))
            {
                return string.Empty;
            }
            else
            {
                return returnStr;
            }
        }

        #endregion
    }   //End of Class
}   //End of namespace