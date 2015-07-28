using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.BAL.PriceQuote;
using Bikewale.Common;
using System.Web.UI.HtmlControls;
using Bikewale.Interfaces.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.BAL.BikeData;
using System.Data;
using System.Configuration;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.BikeBooking;


namespace Bikewale.Mobile.PriceQuote
{
    public class Quotation : System.Web.UI.Page
    {
        IPriceQuote objPriceQuote = null;
        protected BikeQuotationEntity objQuotation = null;
        protected IBikeVersions<BikeVersionEntity, int> objVersion = null;
        protected BikeVersionEntity objVersionDetails = null;
        protected HtmlGenericControl divVersions;
        protected DropDownList ddlVersion;
        ulong pqId = 0;
        uint versionId = 0;
        //protected Repeater rptAllVersions;

        protected List<BikeVersionsListEntity> versionList = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            ddlVersion.SelectedIndexChanged += new EventHandler(SavePriceQuote);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessPriceQuoteData();
        }

        protected void ProcessPriceQuoteData()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                objPriceQuote = container.Resolve<IPriceQuote>();

                container.RegisterType<IBikeVersions<BikeVersionEntity, int>, BikeVersions<BikeVersionEntity, int>>();
                objVersion = container.Resolve<IBikeVersions<BikeVersionEntity, int>>();
            }

            if (PriceQuoteCookie.IsPQCoockieExist())
            {
                pqId = Convert.ToUInt64(PriceQuoteCookie.PQId);
                objVersionDetails = objVersion.GetById(Convert.ToInt32(PriceQuoteCookie.VersionId));

                Trace.Warn("pq id : " + pqId.ToString());

                if (pqId > 0)
                {
                    if (!IsPostBack)
                    {
                        GetPriceQuoteById();
                        BindVersion(objVersionDetails.ModelBase.ModelId);
                    }
                }
                else
                {
                    Response.Redirect("~/m/pricequote/default.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            else
            {
                Response.Redirect("~/m/pricequote/default.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        /// <summary>
        /// Function to get the price quote on the basis of cvId
        /// </summary>
        protected void GetPriceQuoteById()
        {
            objQuotation = objPriceQuote.GetPriceQuoteById(pqId);
        }   // End of GetPriceQuote

        protected void ProcessPriceQuote()
        {
            PriceQuoteParametersEntity objParams = new PriceQuoteParametersEntity();

            objParams.CityId = Convert.ToUInt16(PriceQuoteCookie.CityId);
            objParams.AreaId = Convert.ToUInt32(PriceQuoteCookie.AreaId);
            objParams.ClientIP = CommonOpn.GetClientIP();
            objParams.SourceId = Convert.ToUInt16(Bikewale.Common.Configuration.MobileSourceId);
            objParams.VersionId = versionId;

            objQuotation = objPriceQuote.GetPriceQuote(objParams);
        }

        /// <summary>
        /// Bind the Versions dropdown based on the Bike Model
        /// </summary>
        /// <param name="modelId"></param>
        protected void BindVersion(int modelId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersions<BikeVersionEntity, uint>>();
                    IBikeVersions<BikeVersionEntity, uint> objVersion = container.Resolve<IBikeVersions<BikeVersionEntity, uint>>();

                    versionList = objVersion.GetVersionsByType(EnumBikeType.PriceQuote, Convert.ToInt32(modelId), Convert.ToInt32(PriceQuoteCookie.CityId));

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
                Trace.Warn("Quotation.BindVersion Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Generate the New Price Quote based on the version selected.
        /// </summary>
        private void SavePriceQuote(object sender, EventArgs e)
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
                    // Save pq cookie
                    PriceQuoteCookie.SavePQCookie(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), objPQOutput.DealerId.ToString());

                    Response.Redirect("/m/pricequote/dealerpricequote.aspx", true);
                }
                else if (objPQOutput.PQId > 0)
                {
                    PriceQuoteCookie.SavePQCookie(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), string.Empty);
                    Response.Redirect("/m/pricequote/quotation.aspx", true);
                }
                else
                {
                    Response.Redirect("/m/pricequote/default.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }
    }   // class
}   // namespace