using Bikewale.BAL.BikeData;
using Bikewale.BAL.Customer;
using Bikewale.BAL.PriceQuote;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.BikeBooking
{
    public class DealerPriceQuote : System.Web.UI.Page
    {
        protected Repeater rptPriceList, rptColors, rptDisclaimer, rptOffers, rptDiscount, rptVersion, rptUSPBenefits, rptDealers;
        protected DropDownList ddlVersion;
        protected HtmlGenericControl div_GetPQ, div_ShowErrorMsg;
        protected PQ_QuotationEntity objPrice = null;
        protected List<VersionColor> objColors = null;
        protected BikeVersionEntity objVersionDetails = null;
        protected List<BikeVersionsListEntity> versionList = null;
        protected AlternativeBikes ctrlAlternativeBikes;
        protected UInt64 totalPrice = 0;
        protected string BikeName = string.Empty, pageUrl = string.Empty;
        protected UInt32 dealerId = 0, cityId = 0, versionId = 0, pqId = 0, areaId = 0;
        protected UInt32 insuranceAmount = 0;
        protected UInt32 totalDiscount = 0;
        protected bool IsInsuranceFree = false;
        protected bool IsDiscount = false;
        protected CustomerEntity objCustomer = new CustomerEntity();
        protected string cityArea = string.Empty;
        protected uint bookingAmount = 0;
        protected String clientIP = string.Empty;
        protected bool isEMIAvailable = true;
        //new vairable 
        protected DetailedDealerQuotationEntity detailedDealer = null;
        protected string city = string.Empty, area = string.Empty;
        //Primary Dealer Detail
        protected string dealerName, dealerArea, maskingNum, dealerAddress, makeName, modelName, versionName, mpqQueryString;
        protected double latitude, longitude;
        protected bool isUSPBenfits, isoffer;
        protected HiddenField hdnVariant, hdnDealerId;
        protected Label defaultVariant;
        protected DealerPackageTypes dealerType;
        protected DealerQuotationEntity primarydealer = null;



        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified By : Lucky Rathore
        /// Modified On : 16 March 2016
        /// Description :  Chages for updating version id functionality, function SetDealerPriceQuoteDetail(cityId, versionId, dealerId) used.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            ProcessQueryString();

            div_ShowErrorMsg.Visible = false;


            if (String.IsNullOrEmpty(hdnVariant.Value) || hdnVariant.Value == "0" || !UInt32.TryParse(hdnVariant.Value, out versionId))
            {
                versionId = Convert.ToUInt32(PriceQuoteQueryString.VersionId);
                hdnVariant.Value = Convert.ToString(versionId);
            }

            if (versionId > 0)
            {
                BindVersion();
                BindAlternativeBikeControl(versionId.ToString());
                clientIP = CommonOpn.GetClientIP();
                cityArea = GetLocationCookie();
                SetDealerPriceQuoteDetail(cityId, versionId, dealerId);
                mpqQueryString = EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(Convert.ToString(cityId), Convert.ToString(pqId), Convert.ToString(areaId), Convert.ToString(versionId), Convert.ToString(dealerId)));

            }
            else
            {
                Response.Redirect("/pricequote/quotation.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

        }

        /// <summary>
        /// Modified By : Lucky Rathore
        /// Modified On : 16 March 2016
        /// Description :  Chages for updating version id functionality, function SetDealerPriceQuoteDetail(cityId, versionId, dealerId) used.
        /// </summary>
        protected void btnVariant_Command(object sender, CommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.CommandName))
            {
                versionId = Convert.ToUInt32(e.CommandName);
                defaultVariant.Text = Convert.ToString(e.CommandArgument);
                SavePriceQuote();
            }
        }

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
                    //detailedDealer = new DetailedDealerQuotationEntity();
                    detailedDealer = objIPQ.GetDealerQuotation(cityId, versionId, dealerId);

                    if (detailedDealer == null)
                    {
                        Response.Redirect("/pricequote/quotation.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                    else
                    {
                        if (detailedDealer.objMake != null)
                        {
                            makeName = detailedDealer.objMake.MakeName;
                        }

                        if (detailedDealer.objModel != null)
                        {
                            modelName = detailedDealer.objModel.ModelName;
                        }

                        BikeName = String.Format("{0} {1}",makeName,modelName);

                        if (detailedDealer.objVersion != null)
                        {
                            versionName = detailedDealer.objVersion.VersionName;
                        }

                        if (detailedDealer.PrimaryDealer != null)
                        {
                            primarydealer = detailedDealer.PrimaryDealer;
                            IEnumerable<PQ_Price> priceList = primarydealer.PriceList;
                            IEnumerable<OfferEntityBase> offerList = primarydealer.OfferList;
                            if (priceList != null && priceList.Count() > 0)
                            {
                                rptPriceList.DataSource = priceList;
                                rptPriceList.DataBind();
                                foreach (var price in priceList)
                                {
                                    totalPrice += price.Price;
                                }
                            }
                            else
                            {
                                Response.Redirect("/pricequote/quotation.aspx", false);
                            }

                            
                            if (primarydealer.DealerDetails != null)
                            {
                                NewBikeDealers dealerDetails = primarydealer.DealerDetails;
                                dealerName = dealerDetails.Organization;
                                dealerArea = dealerDetails.objArea.AreaName;
                                dealerAddress = dealerDetails.Address;
                                maskingNum = dealerDetails.MaskingNumber;
                                latitude = dealerDetails.objArea.Latitude;
                                longitude = dealerDetails.objArea.Longitude;
                                dealerType = DealerPackageTypes.Premium;
                            }

                            //bind Offer
                            if (primarydealer.OfferList != null && primarydealer.OfferList.Count() > 0)
                            {
                                rptOffers.DataSource = primarydealer.OfferList;
                                rptOffers.DataBind();
                                isoffer = true;
                            }

                            //bind USP benefits.
                            if (primarydealer.Benefits != null && primarydealer.Benefits.Count() > 0)
                            {
                                rptUSPBenefits.DataSource = primarydealer.Benefits;
                                rptUSPBenefits.DataBind();
                                isUSPBenfits = true;
                            }

                            //bind secondary Dealer
                            if (detailedDealer.SecondaryDealerCount > 0)
                            {
                                rptDealers.DataSource = detailedDealer.SecondaryDealers;
                                rptDealers.DataBind();
                            }

                            //booking amount
                            if (primarydealer.IsBookingAvailable)
                            {
                                bookingAmount = Convert.ToUInt16(Utility.Format.FormatPrice(Convert.ToString(primarydealer.BookingAmount)));
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
                Trace.Warn("SetDealerPriceQuoteDetail Ex: ", ex.Message);
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
        /// Summary : To Fill version dropdownlist
        /// </summary>
        protected void BindVersion()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersions<BikeVersionEntity, uint>>();
                    IBikeVersions<BikeVersionEntity, uint> objVersion = container.Resolve<IBikeVersions<BikeVersionEntity, uint>>();

                    objVersionDetails = objVersion.GetById(versionId);
                    versionList = objVersion.GetVersionsByType(EnumBikeType.PriceQuote, objVersionDetails.ModelBase.ModelId, Convert.ToInt32(PriceQuoteQueryString.CityId));
                    if (versionList.Count > 0)
                    {
                        rptVersion.DataSource = versionList;
                        rptVersion.DataBind();
                        defaultVariant.Text = versionList.Find(x => x.VersionId == versionId).VersionName;
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

        private void BindAlternativeBikeControl(String versionId)
        {
            ctrlAlternativeBikes.TopCount = 6;

            if (!String.IsNullOrEmpty(versionId) && versionId != "0")
            {
                ctrlAlternativeBikes.VersionId = Convert.ToInt32(versionId);
                ctrlAlternativeBikes.PQSourceId = (int)PQSourceEnum.Desktop_DPQ_Alternative;
            }
        }

        protected void SavePriceQuote()
        {
            //uint cityId = Convert.ToUInt32(PriceQuoteCookie.CityId), areaId = Convert.ToUInt32(PriceQuoteCookie.AreaId);
            uint cityId = Convert.ToUInt32(PriceQuoteQueryString.CityId), areaId = Convert.ToUInt32(PriceQuoteQueryString.AreaId);
            uint selectedVersionId = Convert.ToUInt32(versionId);
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
                        objPQEntity.ClientIP = CommonOpn.GetClientIP();
                        objPQEntity.SourceId = Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["sourceId"]);
                        objPQEntity.VersionId = selectedVersionId;
                        objPQEntity.PQLeadId = Convert.ToUInt16(PQSourceEnum.Desktop_DPQ_Quotation);
                        objPQEntity.UTMA = Request.Cookies["__utma"] != null ? Request.Cookies["__utma"].Value : "";
                        objPQEntity.UTMZ = Request.Cookies["__utmz"] != null ? Request.Cookies["__utmz"].Value : "";
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
                if (objPQOutput.PQId > 0 && objPQOutput.DealerId > 0)
                {
                    //PriceQuoteCookie.SavePQCookie(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), objPQOutput.DealerId.ToString());
                    Response.Redirect("/pricequote/dealerpricequote.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), Convert.ToString(dealerId))), false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                else if (objPQOutput.PQId > 0)
                {
                    Response.Redirect("/pricequote/quotation.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), "")), false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                else
                {
                    div_ShowErrorMsg.Visible = true;
                    div_ShowErrorMsg.InnerText = "Sorry !! Price Quote for this version is not available.";
                    div_GetPQ.Visible = false;
                }
            }
        }

        #region Set user location from location cookie
        /// <summary>
        /// Created By : Sushil Kumar on 15th March 2016
        /// Description : To set user location
        /// </summary>
        /// <returns></returns>
        protected string GetLocationCookie()
        {
            string location = String.Empty;
            try
            {
                if (this.Context.Request.Cookies.AllKeys.Contains("location") && !string.IsNullOrEmpty(this.Context.Request.Cookies["location"].Value) && this.Context.Request.Cookies["location"].Value != "0")
                {
                    location = this.Context.Request.Cookies["location"].Value;
                    string[] arr = Regex.Split(location, "_");

                    if (arr.Length > 0)
                    {
                        if (arr.Length > 2)
                        {
                            location = String.Format("<span>{0}</span>, <span>{1}</span>", arr[3], arr[1]);
                        }
                        else
                        {
                            location = String.Format("<span>{0}</span>", arr[1]);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("GetLocationCookie Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return location;
        }
        #endregion

        private UInt32 TotalDiscountedPrice()
        {
            UInt32 totalPrice = 0;
            foreach (var priceListObj in objPrice.discountedPriceList)
            {
                totalPrice += priceListObj.Price;
            }
            return totalPrice;
        }



        #region Private Method to process querystring
        /// <summary>
        /// Created By : Suhsil Kumar
        /// Created On : 16th March 2016 
        /// Description : Private Method to proceess mpq queryString and set the values 
        ///               for queried parameters viz. versionId,dealerId,cityId,pqId and areaId
        /// </summary>
        private void ProcessQueryString()
        {
            try
            {
                if (PriceQuoteQueryString.IsPQQueryStringExists() && UInt32.TryParse(PriceQuoteQueryString.PQId, out pqId) && UInt32.TryParse(PriceQuoteQueryString.DealerId, out dealerId) && UInt32.TryParse(PriceQuoteQueryString.VersionId, out versionId))
                {
                    UInt32.TryParse(PriceQuoteQueryString.CityId, out cityId);
                    UInt32.TryParse(PriceQuoteQueryString.AreaId, out areaId);
                    clientIP = Bikewale.Common.CommonOpn.GetClientIP();
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

                Trace.Warn("GetLocationCookie Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }
        #endregion


    }   //End of Class
}   //End of namespace