using Bikewale.BAL.BikeData;
using Bikewale.BAL.Customer;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entity.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Mobile.PriceQuote;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
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
        protected Repeater rptPriceList, rptColors, rptDisclaimer, rptOffers;
        protected DropDownList ddlVersion;
        protected HtmlGenericControl div_GetPQ, div_ShowErrorMsg;
       
        protected PQ_QuotationEntity objPrice = null;
        protected List<VersionColor> objColors = null;
        protected BikeVersionEntity objVersionDetails = null;
        protected List<BikeVersionsListEntity> versionList = null;

        protected SimilarBikesHorizontal ctrl_similarBikes;
        protected UInt64 totalPrice = 0;
        protected string pqId = string.Empty, areaId = string.Empty, BikeName = string.Empty;
        protected UInt32 dealerId = 0, cityId = 0, versionId = 0;

        protected CustomerEntity objCustomer = new CustomerEntity();

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();
            div_ShowErrorMsg.Visible = false;

            if (PriceQuoteCookie.IsPQCoockieExist())
            {
                if (!String.IsNullOrEmpty(PriceQuoteCookie.DealerId))
                    dealerId = Convert.ToUInt32(PriceQuoteCookie.DealerId);
                else
                    Response.Redirect("/pricequote/quotation.aspx", true);

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
                }
                else
                    SavePriceQuote();

                PreFillCustomerDetails();
                
                ctrl_similarBikes.VersionId = versionId.ToString();
            }
            else
            {
                Response.Redirect("/pricequote/default.aspx", true);
            }
        }

        protected void GetDealerPriceQuote(uint cityId, uint versionId,uint dealerId)
        {
            bool isPriceAvailable = false;
            try
            {
                totalPrice = 0;
                string abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string requestType = "application/json";
                string api = "/api/DealerPriceQuote/GetDealerPriceQuote/?cityid=" + cityId + "&versionid=" + versionId + "&dealerid=" + dealerId;


                objPrice = BWHttpClient.GetApiResponseSync<PQ_QuotationEntity>(abHostUrl, requestType, api, objPrice);

                if (objPrice != null )
                {
                    BikeName = objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName;
                    //Added By : Ashwini Todkar on 1 Dec 2014
                    if (objPrice.PriceList != null && objPrice.PriceList.Count > 0)
                    {
                        dealerId = objPrice.PriceList[0].DealerId;

                        rptPriceList.DataSource = objPrice.PriceList;
                        rptPriceList.DataBind();

                        foreach (var price in objPrice.PriceList)
                        {
                            totalPrice += price.Price;
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
                    Response.Redirect("/pricequote/quotation.aspx", true);
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
            uint cityId = Convert.ToUInt32(PriceQuoteCookie.CityId), areaId = Convert.ToUInt32(PriceQuoteCookie.AreaId);
            uint selectedVersionId = Convert.ToUInt32(ddlVersion.SelectedValue);
            PQOutputEntity objPQOutput = null; 
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    // save price quote
                    container.RegisterType<IDealerPriceQuote,BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objIPQ = container.Resolve<IDealerPriceQuote>();

                    PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                    if (cityId > 0)
                    {
                        objPQEntity.CityId = cityId;
                        objPQEntity.AreaId = areaId;
                        objPQEntity.ClientIP = CommonOpn.GetClientIP();
                        objPQEntity.SourceId = Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["sourceId"]);
                        objPQEntity.VersionId = selectedVersionId;

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
                if(objPQOutput.PQId>0 && objPQOutput.DealerId>0)
                {
                    PriceQuoteCookie.SavePQCookie(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), objPQOutput.DealerId.ToString());
                    Response.Redirect("/pricequote/dealerpricequote.aspx", true);
                }
                else if (objPQOutput.PQId > 0)
                {
                    // Save pq cookie
                    PriceQuoteCookie.SavePQCookie(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), "");
                    Response.Redirect("/pricequote/quotation.aspx", true);
                }
                else
                {
                    div_ShowErrorMsg.Visible = true;
                    div_ShowErrorMsg.InnerText = "Sorry !! Price Quote for this version is not available.";
                    div_GetPQ.Visible = false;
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
                    IBikeVersions<BikeVersionEntity, uint> objVersion = container.Resolve<IBikeVersions<BikeVersionEntity, uint>>();

                    objColors = objVersion.GetColorByVersion(versionId);

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
    }   //End of Class
}   //End of namespace