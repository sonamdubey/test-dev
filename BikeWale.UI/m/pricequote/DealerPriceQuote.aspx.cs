using Bikewale.BAL.BikeData;
using Bikewale.BAL.PriceQuote;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.BikeBooking
{
    /// <summary>
    /// Modified By : Lucky Rathore
    /// Modified On : 31 March 2016
    /// Description : Removed rptColors and GetVersionColor function.
    /// </summary>  
    public class DealerPriceQuote : PageBase
    {
        protected Repeater rptPriceList, rptDisclaimer, rptOffers, rptDiscount, rptSecondaryDealers, rptBenefits;
        protected DropDownList ddlVersion;
        protected UInt64 totalPrice = 0;
        protected string pqId = string.Empty, areaId = string.Empty, MakeModel = string.Empty, BikeName = string.Empty, mpqQueryString = string.Empty;
        protected UInt32 dealerId = 0, cityId = 0, versionId = 0;
        protected bool isPriceAvailable = false;
        protected List<VersionColor> objColors = null;
        protected UInt32 insuranceAmount = 0;
        protected CustomerEntity objCustomer = new CustomerEntity();
        protected BikeVersionEntity objVersionDetails = null;
        protected List<BikeVersionsListEntity> versionList = null;
        protected bool IsInsuranceFree = false;
        protected NewAlternativeBikes ctrlAlternateBikes;
        protected string cityArea = string.Empty, currentCity = string.Empty, currentArea = string.Empty;
        protected uint bookingAmount = 0;
        protected String clientIP = CommonOpn.GetClientIP();
        protected bool IsDiscount = false;
        protected UInt32 totalDiscount = 0;
        protected Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity objPriceQuote = null;
        protected string dealerName = string.Empty, dealerArea = string.Empty, dealerAdd = string.Empty, maskingNum = string.Empty, contactHours = string.Empty;
        protected double latitude = 0, longitude = 0;
        protected uint offerCount = 0, secondaryDealersCount = 0;
        protected bool isEMIAvailable = false, isUSPAvailable = false, isOfferAvailable = false, isPrimaryDealer = false, isSecondaryDealer = false, isBookingAvailable = false;
        protected DealerPackageTypes dealerType = 0;
        protected DealerQuotationEntity primarydealer = null;
        IPriceQuote objIQuotation = null;
        protected BikeQuotationEntity objExQuotation = null;
        protected LeadCaptureControl ctrlLeadCapture;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (PriceQuoteQueryString.IsPQQueryStringExists())
            {
                if (!String.IsNullOrEmpty(PriceQuoteQueryString.DealerId))
                    dealerId = Convert.ToUInt32(PriceQuoteQueryString.DealerId);

                areaId = PriceQuoteQueryString.AreaId;
                cityId = Convert.ToUInt32(PriceQuoteQueryString.CityId);
                if (!IsPostBack)
                {
                    pqId = PriceQuoteQueryString.PQId;
                    versionId = Convert.ToUInt32(PriceQuoteQueryString.VersionId);

                    BindVersion();

                    GetDealerPriceQuote(cityId, versionId, dealerId);
                    BindAlternativeBikeControl(versionId.ToString());
                    mpqQueryString = EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(Convert.ToString(cityId), Convert.ToString(pqId), Convert.ToString(areaId), Convert.ToString(versionId), Convert.ToString(dealerId)));

                    ctrlLeadCapture.CityId = cityId;
                    ctrlLeadCapture.ModelId = Convert.ToUInt32(objVersionDetails.ModelBase.ModelId);
                    ctrlLeadCapture.AreaId = 0;
                }
                else
                    SavePriceQuote();

                cityArea = GetLocationCookie();
                currentCity = Bikewale.Utility.GlobalCityArea.GetGlobalCityArea().City;
                currentArea = Bikewale.Utility.GlobalCityArea.GetGlobalCityArea().Area;
            }
            else
            {
                Response.Redirect("~/m/pricequote/default.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        protected void GetDealerPriceQuote(uint cityId, uint versionId, uint dealerId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuoteDetail, DealerPriceQuoteDetail>();
                    IDealerPriceQuoteDetail objIPQ = container.Resolve<IDealerPriceQuoteDetail>();
                    objPriceQuote = objIPQ.GetDealerQuotationV2(cityId, versionId, dealerId, Convert.ToUInt32(areaId));

                    if (objPriceQuote != null)
                    {
                        BikeName = (objPriceQuote.objMake != null ? objPriceQuote.objMake.MakeName : "") + " " + (objPriceQuote.objModel != null ? objPriceQuote.objModel.ModelName : "");
                        ctrlLeadCapture.AreaId = Convert.ToUInt32(areaId);
                        ctrlLeadCapture.CityId = cityId;
                        ctrlLeadCapture.ModelId = Convert.ToUInt32(objPriceQuote.objModel.ModelId);
                        if (objPriceQuote.PrimaryDealer.DealerDetails != null)
                        {
                            isPrimaryDealer = true;
                        }

                        if (objPriceQuote.PrimaryDealer != null && objPriceQuote.PrimaryDealer.PriceList != null && objPriceQuote.PrimaryDealer.PriceList.Count() > 0)
                        {
                            MakeModel = (objPriceQuote.objMake != null ? objPriceQuote.objMake.MakeName : "") + " " +
                                        (objPriceQuote.objModel != null ? objPriceQuote.objModel.ModelName : "");

                            rptPriceList.DataSource = objPriceQuote.PrimaryDealer.PriceList;
                            rptPriceList.DataBind();

                            foreach (var price in objPriceQuote.PrimaryDealer.PriceList)
                            {
                                totalPrice += price.Price;
                            }

                            dealerId = objPriceQuote.PrimaryDealer.DealerDetails.DealerId;

                            foreach (var price in objPriceQuote.PrimaryDealer.PriceList)
                            {
                                Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(), objPriceQuote.objModel.ModelId.ToString(), price.CategoryName, price.Price, ref insuranceAmount);
                            }
                            if (insuranceAmount > 0)
                            {
                                IsInsuranceFree = true;
                            }
                            isPriceAvailable = true;
                        }

                        else
                        {
                            container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                            objIQuotation = container.Resolve<IPriceQuote>();
                            objExQuotation = objIQuotation.GetPriceQuoteById(Convert.ToUInt64(pqId), LeadSourceEnum.DPQ_Mobile);
                        }

                        if (objPriceQuote.PrimaryDealer != null)
                        {
                            primarydealer = objPriceQuote.PrimaryDealer;
                            IEnumerable<PQ_Price> priceList = objPriceQuote.PrimaryDealer.PriceList;
                            if (priceList != null && priceList.Count() > 0)
                            {
                                rptPriceList.DataSource = priceList;
                                rptPriceList.DataBind();
                            }
                            //set primary dealer Detail
                            if (primarydealer.DealerDetails != null)
                            {
                                NewBikeDealers dealerDetails = primarydealer.DealerDetails;
                                dealerName = dealerDetails.Organization;
                                dealerArea = dealerDetails.objArea.AreaName;
                                dealerAdd = dealerDetails.Address;
                                maskingNum = dealerDetails.MaskingNumber;
                                latitude = dealerDetails.objArea.Latitude;
                                longitude = dealerDetails.objArea.Longitude;
                                dealerType = dealerDetails.DealerPackageType;
                                contactHours = dealerDetails.WorkingTime;
                            }

                            //bind Offer

                            offerCount = Convert.ToUInt32(primarydealer.OfferList.Count());

                            if (primarydealer.OfferList != null && offerCount > 0)
                            {
                                isOfferAvailable = true;
                                rptOffers.DataSource = primarydealer.OfferList;
                                rptOffers.DataBind();
                            }

                            if (primarydealer.Benefits != null && primarydealer.Benefits.Count() > 0)
                            {
                                isUSPAvailable = true;
                                rptBenefits.DataSource = primarydealer.Benefits;
                                rptBenefits.DataBind();
                            }

                            //bind secondary Dealer
                            if (objPriceQuote.SecondaryDealers != null && objPriceQuote.SecondaryDealers.Count() > 0)
                            {
                                secondaryDealersCount = Convert.ToUInt32(objPriceQuote.SecondaryDealers.Count());
                                if (secondaryDealersCount > 0)
                                {
                                    isSecondaryDealer = true;
                                    rptSecondaryDealers.DataSource = objPriceQuote.SecondaryDealers;
                                    rptSecondaryDealers.DataBind();
                                }
                            }

                            //booking amount
                            if (primarydealer.IsBookingAvailable)
                            {
                                isBookingAvailable = true;
                                bookingAmount = primarydealer.BookingAmount;
                            }

                            //EMI details
                            if (primarydealer.EMIDetails != null)
                            {
                                EMI _objEMI = setEMIDetails();
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
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("GetDealerPriceQuote Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
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
                Trace.Warn("getEMIDetails Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return _objEMI;
        }


        /// <summary>
        /// Created By : Sadhana Upadhyay on 2 Dec 2014
        /// Modified By Vivek Gupta on 02-05-2016
        /// Desc : redirection condition isDealerAvailbale added
        /// Summary : To Fill version dropdownlist
        /// </summary>
        protected void BindVersion()
        {
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
                    versionList = objCache.GetVersionsByType(EnumBikeType.PriceQuote, objVersionDetails.ModelBase.ModelId, Convert.ToInt32(PriceQuoteQueryString.CityId));

                    if (versionList != null && versionList.Count > 0)
                    {
                        ddlVersion.DataSource = versionList;
                        ddlVersion.DataValueField = "VersionId";
                        ddlVersion.DataTextField = "VersionName";
                        ddlVersion.DataBind();
                        ddlVersion.SelectedValue = Convert.ToString(versionId);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("DeaperPriceQuote.BindVersion Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Lucky Rathore on 27 June 2016
        /// Description : replace cookie __utmz with _bwutmz
        /// </summary>
        protected void SavePriceQuote()
        {
            PQOutputEntity objPQOutput = null;
            uint cityId = Convert.ToUInt32(PriceQuoteQueryString.CityId), areaId = Convert.ToUInt32(PriceQuoteQueryString.AreaId);
            uint selectedVersionId = Convert.ToUInt32(ddlVersion.SelectedValue);
            try
            {
                versionId = selectedVersionId;
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
                        objPQEntity.ClientIP = CommonOpn.GetClientIP();
                        objPQEntity.SourceId = Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["mobileSourceId"]);
                        objPQEntity.VersionId = selectedVersionId;
                        objPQEntity.PQLeadId = Convert.ToUInt16(PQSourceEnum.Mobile_DPQ_Quotation);
                        objPQEntity.UTMA = Request.Cookies["__utma"] != null ? Request.Cookies["__utma"].Value : "";
                        objPQEntity.UTMZ = Request.Cookies["_bwutmz"] != null ? Request.Cookies["_bwutmz"].Value : "";
                        objPQEntity.DeviceId = Request.Cookies["BWC"] != null ? Request.Cookies["BWC"].Value : "";
                        objPQOutput = objIPQ.ProcessPQ(objPQEntity);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (objPQOutput != null && objPQOutput.PQId > 0)
                {
                    Response.Redirect("~/m/pricequote/dealerpricequote.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), Convert.ToString(dealerId))), false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Sorry, Price for this Version is not available.');", true);
                }
            }
        }

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

        private void BindAlternativeBikeControl(string versionId)
        {
            ctrlAlternateBikes.TopCount = 6;

            if (!String.IsNullOrEmpty(versionId) && versionId != "0")
            {
                ctrlAlternateBikes.VersionId = Convert.ToInt32(versionId);
                ctrlAlternateBikes.PQSourceId = (int)PQSourceEnum.Mobile_DPQ_Alternative;
                ctrlAlternateBikes.modelName = (objPriceQuote != null && objPriceQuote.objModel != null) ? objPriceQuote.objModel.ModelName : string.Empty;

            }
        }

    }   //End of class
}   //End of namespace