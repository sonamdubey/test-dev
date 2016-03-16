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
using Bikewale.Mobile.PriceQuote;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
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
        protected string pqId = string.Empty, areaId = string.Empty, BikeName = string.Empty;
        protected UInt32 dealerId = 0, cityId = 0, versionId = 0;
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

            div_ShowErrorMsg.Visible = false;

            if (PriceQuoteQueryString.IsPQQueryStringExists())
            {
                if (!String.IsNullOrEmpty(PriceQuoteQueryString.DealerId))
                    dealerId = Convert.ToUInt32(PriceQuoteQueryString.DealerId);
                else
                {
                    Response.Redirect("/pricequote/quotation.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                areaId = PriceQuoteQueryString.AreaId;
                cityId = Convert.ToUInt32(PriceQuoteQueryString.CityId);

                pqId = PriceQuoteQueryString.PQId;

                if (!String.IsNullOrEmpty(hdnVariant.Value) && hdnVariant.Value != "0")
                {
                    versionId = Convert.ToUInt32(hdnVariant.Value);
                }
                else
                {
                    versionId = Convert.ToUInt32(PriceQuoteQueryString.VersionId);
                    hdnVariant.Value = Convert.ToString(versionId);
                }
                BindVersion();                   
                BindAlternativeBikeControl(versionId.ToString());
                clientIP = CommonOpn.GetClientIP();
                PreFillCustomerDetails();
                cityArea = GetLocationCookie();
                SetDealerPriceQuoteDetail(cityId, versionId, dealerId);
                mpqQueryString = EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(Convert.ToString(cityId), Convert.ToString(pqId), Convert.ToString(areaId), Convert.ToString(versionId), Convert.ToString(dealerId)));
            }
            else
            {
                Response.Redirect("/pricequote/default.aspx", false);
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
                //SavePriceQuote();
            }
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created on : 15 March 2016
        /// Description : for Dealer Basics details.
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

                    BikeName = makeName + " " + modelName;

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

                        //set primary dealer Detail
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
                        else
                        {
                            Response.Redirect("/pricequote/quotation.aspx", false);
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
                        //EMI deatails
                        if (primarydealer.EMIDetails == null)
                        {
                                primarydealer.EMIDetails = setEMIDetails();
                        }
                            
                    }
                }
            }
      }
            catch(Exception ex)
            {
                Trace.Warn("getEMIDetails Ex: ", ex.Message);
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
                _objEMI.MaxDownPayment = 100;
                _objEMI.MinDownPayment = 25;
                _objEMI.MaxTenure = 48;
                _objEMI.MinTenure = 6;
                _objEMI.MaxRateOfInterest = 12;
                _objEMI.MinRateOfInterest = 7;
                _objEMI.ProcessingFee = 2000;
            }
            catch(Exception ex)
            {
                Trace.Warn("getEMIDetails Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return _objEMI;
        }

        /// <summary>
        /// Created BY : Sadhana Upadhyay on 14 Nov 2014
        /// Summary : To fill Customer detail when customer is loged in
        /// </summary>
        protected void PreFillCustomerDetails()
        {
            try
            {
                if (Bikewale.Common.CurrentUser.Id != "-1")
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<ICustomer<CustomerEntity, UInt32>, Customer<CustomerEntity, UInt32>>();
                        ICustomer<CustomerEntity, UInt32> objCust = container.Resolve<ICustomer<CustomerEntity, UInt32>>();

                        objCustomer = objCust.GetById(Convert.ToUInt32(Bikewale.Common.CurrentUser.Id));
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("PreFillCustomerDetails Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
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
                if (this.Context.Request.Cookies.AllKeys.Contains("location") && this.Context.Request.Cookies["location"].Value != "0")
                {
                    location = this.Context.Request.Cookies["location"].Value;
                    string[] arr = location.Split('_');

                    if (arr.Length > 0)
                    {
                        if (arr.Length > 2)
                        {
                            location = String.Format("<span>{0}</span>, <span>{1}</span>", arr[3], arr[1]);
                        }

                        location = String.Format("<span>{0}</span>", arr[1]);
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

        private UInt32 TotalDiscountedPrice()
        {
            UInt32 totalPrice = 0;
            foreach (var priceListObj in objPrice.discountedPriceList)
            {
                totalPrice += priceListObj.Price;
            }
            return totalPrice;
        }


    }   //End of Class
}   //End of namespace