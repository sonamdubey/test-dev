using Bikewale.BAL.BikeData;
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
    /// <summary>
    /// Modified By : Lucky Rathore
    /// Modified On : 31 March 2016
    /// Description : Removed rptColors function.
    /// </summary>
    public class DealerPriceQuote : System.Web.UI.Page
    {
        protected Repeater rptPriceList, rptDisclaimer, rptOffers, rptDiscount, rptVersion, rptUSPBenefits, rptDealers;
        protected DropDownList ddlVersion;
        protected HtmlGenericControl div_GetPQ;
        protected PQ_QuotationEntity objPrice = null;
        protected List<VersionColor> objColors = null;
        protected BikeVersionEntity objVersionDetails = null;
        protected List<BikeVersionsListEntity> versionList = null;
        protected AlternativeBikes ctrlAlternativeBikes;
        protected string BikeName = string.Empty, pageUrl = string.Empty, clientIP = string.Empty, cityArea = string.Empty, city = string.Empty, area = string.Empty;
        protected uint totalPrice = 0, bookingAmount, dealerId = 0, cityId = 0, versionId = 0, pqId = 0, areaId = 0, insuranceAmount = 0, totalDiscount = 0;
        protected bool IsInsuranceFree, isUSPBenfits, isoffer, isEMIAvailable, IsDiscount;
        protected CustomerEntity objCustomer = new CustomerEntity();
        protected DetailedDealerQuotationEntity detailedDealer = null;
        protected string dealerName, dealerArea, maskingNum, dealerAddress, makeName, modelName, versionName, mpqQueryString;
        protected double latitude, longitude;
        protected HiddenField hdnVariant, hdnDealerId;
        protected Label defaultVariant;
        protected DealerPackageTypes dealerType;
        protected DealerQuotationEntity primarydealer = null;
        IPriceQuote objPriceQuote = null;
        protected BikeQuotationEntity objQuotation = null;
        protected IEnumerable<PQ_Price> primaryPriceList = null;
        protected bool isSecondaryDealerAvailable = false;



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
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            ProcessQueryString();

            if (versionId > 0)
            {
                BindVersion();
                hdnVariant.Value = versionId.ToString();
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

        #region Version Change Event
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
        #endregion


        #region Dealer PriceQuote Details Binding
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
                    detailedDealer = objIPQ.GetDealerQuotation(cityId, versionId, dealerId);

                    if (detailedDealer != null)
                    {
                        if (detailedDealer.objMake != null)
                        {
                            makeName = detailedDealer.objMake.MakeName;
                        }

                        if (detailedDealer.objModel != null)
                        {
                            modelName = detailedDealer.objModel.ModelName;
                        }

                        BikeName = String.Format("{0} {1}", makeName, modelName);

                        if (detailedDealer.objVersion != null)
                        {
                            versionName = detailedDealer.objVersion.VersionName;
                        }

                        if (detailedDealer.PrimaryDealer != null)
                        {
                            primarydealer = detailedDealer.PrimaryDealer;
                            primaryPriceList = primarydealer.PriceList;
                            IEnumerable<OfferEntityBase> offerList = primarydealer.OfferList;
                            if (primaryPriceList != null && primaryPriceList.Count() > 0)
                            {
                                rptPriceList.DataSource = primaryPriceList;
                                rptPriceList.DataBind();
                                foreach (var price in primaryPriceList)
                                {
                                    totalPrice += price.Price;
                                }
                            }
                            else
                            {
                                container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                                objPriceQuote = container.Resolve<IPriceQuote>();
                                objQuotation = objPriceQuote.GetPriceQuoteById(Convert.ToUInt64(pqId));
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
                                dealerType = dealerDetails.DealerPackageType;
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
                                isSecondaryDealerAvailable = true;
                                rptDealers.DataSource = detailedDealer.SecondaryDealers;
                                rptDealers.DataBind();
                            }

                            //booking amount
                            if (primarydealer.IsBookingAvailable)
                            {
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
                Trace.Warn("SetDealerPriceQuoteDetail Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            finally
            {
                if (dealerId == 0 && !isSecondaryDealerAvailable && pqId > 0)
                {
                    Response.Redirect("/pricequote/quotation.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(cityId.ToString(), pqId.ToString(), areaId.ToString(), versionId.ToString(), Convert.ToString(dealerId))), false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }
        #endregion

        #region Set Default EMI details
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
                Trace.Warn("setEMIDetails Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return _objEMI;
        }
        #endregion

        #region Bind Versions
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
                    if (versionList != null && versionList.Count > 0)
                    {
                        rptVersion.DataSource = versionList;
                        rptVersion.DataBind();
                        var defVar = versionList.Find(x => x.VersionId == versionId);
                        if (defVar != null)
                            defaultVariant.Text = defVar.VersionName;
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
        #endregion

        /// <summary>
        /// Description : To bind alternative bikes controle.
        /// </summary>
        /// <param name="versionId"></param>
        private void BindAlternativeBikeControl(String versionId)
        {
            ctrlAlternativeBikes.TopCount = 6;

            if (!String.IsNullOrEmpty(versionId) && versionId != "0")
            {
                ctrlAlternativeBikes.VersionId = Convert.ToInt32(versionId);
                ctrlAlternativeBikes.PQSourceId = (int)PQSourceEnum.Desktop_DPQ_Alternative;
            }
        }

        #region Save PriceQuote on Version Change
        /// <summary>
        /// Modified By : Sushil Kumar on 18th March 2016
        /// Description : Changed finally section from code as no check was made for objPQOutput == null
        /// Modified By : Vivek Gupta on 29-04-2016
        /// Desc : In case of dealerId=0 and isDealerAvailable = true , while redirecting to pricequotes ,don't redirect to BW PQ redirect to dpq
        /// </summary>
        protected void SavePriceQuote()
        {
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
                if (objPQOutput != null && objPQOutput.PQId > 0)
                {
                    if (objPQOutput.DealerId > 0)
                    {
                        // Save pq cookie
                        //PriceQuoteCookie.SavePQCookie(objPQEntity.CityId.ToString(), objPQOutput.PQId.ToString(), objPQEntity.AreaId.ToString(), objPQOutput.VersionId.ToString(), objPQOutput.DealerId.ToString());                          
                        Response.Redirect("/pricequote/dealerpricequote.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), Convert.ToString(dealerId))), false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }

                    else if (objPQOutput.DealerId == 0 && objPQOutput.IsDealerAvailable)
                    {
                        // Save pq cookie
                        //PriceQuoteCookie.SavePQCookie(objPQEntity.CityId.ToString(), objPQOutput.PQId.ToString(), objPQEntity.AreaId.ToString(), objPQOutput.VersionId.ToString(), objPQOutput.DealerId.ToString());                        
                        Response.Redirect("/pricequote/dealerpricequote.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), Convert.ToString(dealerId))), false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                    else if (objPQOutput.DealerId == 0 && !objPQOutput.IsDealerAvailable)
                    {
                        //PriceQuoteCookie.SavePQCookie(objPQEntity.CityId.ToString(), objPQOutput.PQId.ToString(), objPQEntity.AreaId.ToString(), objPQOutput.VersionId.ToString(), "");                        
                        Response.Redirect("/pricequote/quotation.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), Convert.ToString(dealerId))), false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect("/pricequote/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }
        #endregion

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

        #region Private Method to process querystring
        /// <summary>
        /// Created By : Suhsil Kumar
        /// Created On : 16th March 2016 
        /// Description : Private Method to proceess mpq queryString and set the for queried parameters viz. versionId,dealerId,cityId,pqId and areaId
        /// Modified By : Lucky Rathore
        /// Description : DealerId Assingment moved in "if" condition
        /// </summary>
        private void ProcessQueryString()
        {
            try
            {
                if (PriceQuoteQueryString.IsPQQueryStringExists() && UInt32.TryParse(PriceQuoteQueryString.PQId, out pqId) && UInt32.TryParse(PriceQuoteQueryString.VersionId, out versionId))
                {
                    UInt32.TryParse(PriceQuoteQueryString.DealerId, out dealerId);
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