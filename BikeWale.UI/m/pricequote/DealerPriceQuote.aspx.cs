using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Bikewale.Entities.BikeBooking;
using Bikewale.Common;
using Bikewale.Mobile.PriceQuote;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.Customer;
using Bikewale.Entities.Customer;
using Bikewale.BAL.Customer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.BAL.BikeData;
using Bikewale.Mobile.Controls;

namespace Bikewale.Mobile.BikeBooking
{
    public class DealerPriceQuote : System.Web.UI.Page
    {
        protected Repeater rptPriceList, rptColors, rptDisclaimer, rptOffers;
        protected DropDownList ddlVersion;

        protected PQ_QuotationEntity objPrice = null;
        protected UInt64 totalPrice = 0;
        protected string pqId = string.Empty, areaId = string.Empty, MakeModel = string.Empty, BikeName = string.Empty;
        protected UInt32 dealerId = 0, cityId = 0, versionId = 0;
        private bool isPriceAvailable = false;
        protected List<VersionColor> objColors = null;
        protected UInt32 insuranceAmount = 0;
        protected CustomerEntity objCustomer = new CustomerEntity();
        protected BikeVersionEntity objVersionDetails = null;
        protected List<BikeVersionsListEntity> versionList = null;
        protected bool IsInsuranceFree = false;
        protected AlternativeBikes ctrlAlternateBikes;
        protected string cityArea = string.Empty;
        protected uint bookingAmount = 0;
        protected String clientIP = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (PriceQuoteCookie.IsPQCoockieExist())
            {
                if (!String.IsNullOrEmpty(PriceQuoteCookie.DealerId))
                    dealerId = Convert.ToUInt32(PriceQuoteCookie.DealerId);
                else
                {
                    Response.Redirect("/m/pricequote/quotation.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }

                areaId = PriceQuoteCookie.AreaId;
                cityId = Convert.ToUInt32(PriceQuoteCookie.CityId);

                if (!IsPostBack)
                {
                    pqId = PriceQuoteCookie.PQId;
                    versionId = Convert.ToUInt32(PriceQuoteCookie.VersionId);

                    BindVersion();

                    GetDealerPriceQuote(cityId, versionId, dealerId);
                    GetVersionColors(versionId);
                    PriceQuoteCookie.SavePQCookie(cityId.ToString(), pqId, areaId, versionId.ToString(), dealerId.ToString());
                    BindAlternativeBikeControl(versionId.ToString());
                    clientIP = CommonOpn.GetClientIP();
                }
                else
                    SavePriceQuote();

                PreFillCustomerDetails();

                cityArea = GetLocationCookie();
            }
            else
            {
                Response.Redirect("/m/pricequote/default.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        protected async void GetDealerPriceQuote(uint cityId, uint versionId, uint dealerId)
        {
            try
            {                
                string api = "/api/DealerPriceQuote/GetDealerPriceQuote/?cityid=" + cityId + "&versionid=" + versionId + "&dealerid=" + dealerId;

                using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objPrice = await objClient.GetApiResponse<PQ_QuotationEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, api, objPrice);
                }
                
                if (objPrice != null)
                {
                    BikeName = objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName;
                    //Added By : Ashwini Todkar on 1 Dec 2014
                    if (objPrice.PriceList != null && objPrice.PriceList.Count > 0)
                    {
                        MakeModel = objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName;

                        rptPriceList.DataSource = objPrice.PriceList;
                        rptPriceList.DataBind();

                        foreach (var price in objPrice.PriceList)
                        {
                            totalPrice += price.Price;
                        }

                        dealerId = objPrice.PriceList[0].DealerId;
                        foreach (var price in objPrice.PriceList)
                        {
                            Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(), objPrice.objModel.ModelId.ToString(), price.CategoryName, price.Price, ref insuranceAmount);
                        }
                        if (insuranceAmount > 0)
                        {
                            IsInsuranceFree = true;
                        }
                        isPriceAvailable = true;
                    }

                    if (objPrice.Disclaimer != null && objPrice.Disclaimer.Count > 0)
                    {
                        rptDisclaimer.DataSource = objPrice.Disclaimer;
                        rptDisclaimer.DataBind();
                    }

                    if (objPrice.objOffers != null && objPrice.objOffers.Count > 0)
                    {

                        rptOffers.DataSource = objPrice.objOffers;
                        rptOffers.DataBind();

                    }

                    if (objPrice.Varients != null && objPrice.Varients.Count() > 0)
                    {

                       //Capture Lead
                        foreach (var i in objPrice.Varients)
                        {
                            if (i.objVersion.VersionId == versionId)
                            {
                                bookingAmount = i.BookingAmount;
                                break;
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
            finally
            {
                if (!isPriceAvailable)
                {
                    Response.Redirect("/m/pricequote/quotation.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// Created BY : Sadhana Upadhyay on 14 Nov 2014
        /// Summary : To fill Customer detail when customer is loged in
        /// </summary>
        protected void PreFillCustomerDetails()
        {
            try
            {
                if (CurrentUser.Id != "-1")
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<ICustomer<CustomerEntity, UInt32>, Customer<CustomerEntity, UInt32>>();
                        ICustomer<CustomerEntity, UInt32> objCust = container.Resolve<ICustomer<CustomerEntity, UInt32>>();

                        objCustomer = objCust.GetById(Convert.ToUInt32(CurrentUser.Id));
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
                    versionList = objVersion.GetVersionsByType(EnumBikeType.PriceQuote, objVersionDetails.ModelBase.ModelId, Convert.ToInt32(PriceQuoteCookie.CityId));

                    if (versionList.Count > 0)
                    {
                        ddlVersion.DataSource = versionList;
                        ddlVersion.DataValueField = "VersionId";
                        ddlVersion.DataTextField = "VersionName";
                        ddlVersion.DataBind();

                        ddlVersion.SelectedValue = PriceQuoteCookie.VersionId;
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

        protected void SavePriceQuote()
        {
            PQOutputEntity objPQOutput = null;
            uint cityId = Convert.ToUInt32(PriceQuoteCookie.CityId), areaId = Convert.ToUInt32(PriceQuoteCookie.AreaId);
            uint selectedVersionId = Convert.ToUInt32(ddlVersion.SelectedValue);
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
                        objPQEntity.SourceId = Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["mobileSourceId"]);
                        objPQEntity.VersionId = selectedVersionId;
                        objPQEntity.PQLeadId = Convert.ToUInt16(PQSourceEnum.Mobile_DPQ_Quotation);
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
                    PriceQuoteCookie.SavePQCookie(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), objPQOutput.DealerId.ToString());
                    Response.Redirect("/m/pricequote/dealerpricequote.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                else if (objPQOutput.PQId > 0)
                {
                    // Save pq cookie
                    PriceQuoteCookie.SavePQCookie(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), "");
                    Response.Redirect("/m/pricequote/quotation.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Sorry, Price for this Version is not available.');", true);
                }
            }
        }
        public void GetVersionColors(uint versionId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersions<BikeVersionEntity, uint>>();
                    IBikeVersions<BikeVersionEntity, uint> objVersions = container.Resolve<IBikeVersions<BikeVersionEntity, uint>>();

                    objColors = objVersions.GetColorByVersion(versionId);

                    if (objColors.Count > 0)
                    {
                        rptColors.DataSource = objColors;
                        rptColors.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private string GetLocationCookie()
        {
            string location = String.Empty;
            if (this.Context.Request.Cookies.AllKeys.Contains("location") && this.Context.Request.Cookies["location"].Value != "0")
            {
                location = this.Context.Request.Cookies["location"].Value;
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
            }
        }

    }   //End of class
}   //End of namespace