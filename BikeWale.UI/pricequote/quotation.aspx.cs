using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Mobile.PriceQuote;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Configuration;
using Bikewale.Interfaces.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.BAL.BikeData;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.BikeBooking;

namespace Bikewale.PriceQuote
{
    /// <summary>
    ///     Created By : Ashish G. Kamble
    ///     Summary : Class to show the price quote. If other versions of the same model are available then customer can take price quote of those versions.
    /// </summary>
    public class Quotation : Page
    {
        //protected Repeater rptAllVersions;
        protected UserReviewsMin ucUserReviewsMin;
        protected UpcomingBikesMin ucUpcoming;
        protected LocateDealer ucLocateDealer;
        protected NewsMin newsMin;
        protected SimilarBikes ctrl_similarBikes;
        protected HtmlGenericControl divAllVersions, div_ShowPQ, divUserReviews;

        protected string cityId = string.Empty, city = string.Empty, priceQuoteId = string.Empty, make = string.Empty, imgPath = String.Empty, dealerId = string.Empty;
        protected MakeModelVersion mmv = null;
        protected BikeQuotationEntity objQuotation = null;
        protected List<BikeVersionsListEntity> versionList = null;
        protected DropDownList ddlVersion;
        IPriceQuote objPriceQuote = null;
        protected UInt32 versionId = 0;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            ddlVersion.SelectedIndexChanged += new EventHandler(SavePriceQuote);
        }

        /// <summary>
        /// Bind the Versions dropdown based on the Bike Model
        /// </summary>
        /// <param name="modelId"></param>
        protected void BindVersion(string modelId)
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

        protected void Page_Load(object sender, EventArgs e)
        {
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            ProcessPriceQuoteData();
            if (PriceQuoteCookie.PQId != null)
            {
                priceQuoteId = PriceQuoteCookie.PQId;

                mmv = new MakeModelVersion();
                if (versionId > 0)
                {
                    mmv.GetVersionDetails(versionId.ToString());
                    //imgPath = ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/" + mmv.LargePic, mmv.HostUrl);
                    imgPath = Bikewale.Utility.Image.GetPathToShowImages(mmv.OriginalImagePath, mmv.HostUrl, Bikewale.Utility.ImageSize._210x118);

                    // Added By Sadhana Upadhyay on 6th Aug 2014 to show news and similar bike widget
                    ctrl_similarBikes.VersionId = mmv.VersionId;

                    ucUpcoming.HeaderText = "Upcoming Bikes from " + mmv.Make;
                    ucUpcoming.MakeId = mmv.MakeId;
                    // newsMin.MakeId = mmv.MakeId;
                    ucLocateDealer.Make = mmv.MakeId + '_' + mmv.MakeMappingName;

                    BindVersion(mmv.ModelId);
                }
            }
            else
            {
                Response.Redirect("default.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

        }

        /// <summary>
        ///     Function will show price quote for the current version id 
        ///     Modified By : Sadhana Upadhyay on 28 Oct 2014
        ///     Summary : called method from Bikewale BAL
        /// </summary>
        protected void ShowPriceQuote()
        {
            try
            {
                objQuotation = objPriceQuote.GetPriceQuoteById(Convert.ToUInt64(priceQuoteId));
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("ShowPriceQuote ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   // End of ShowPriceQuote method

        /// <summary>
        /// Generate the New Price Quote based on the version selected.
        /// </summary>
        protected void SavePriceQuote(object sender, EventArgs e)
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
                if (objPQOutput.PQId > 0 && objPQOutput.DealerId>0)
                {
                    // Save pq cookie
                    PriceQuoteCookie.SavePQCookie(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), objPQOutput.DealerId.ToString());

                    Response.Redirect("/pricequote/dealerpricequote.aspx", true);
                }
                else if(objPQOutput.PQId>0)
                {
                    PriceQuoteCookie.SavePQCookie(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), string.Empty);
                    Response.Redirect("/pricequote/quotation.aspx", true);
                }
                else
                {
                    Response.Redirect("~/pricequote/default.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        protected void ProcessPriceQuote()
        {
            PriceQuoteParametersEntity objParams = new PriceQuoteParametersEntity();

            objParams.CityId = Convert.ToUInt16(PriceQuoteCookie.CityId);
            objParams.AreaId = Convert.ToUInt32(PriceQuoteCookie.AreaId);
            objParams.ClientIP = CommonOpn.GetClientIP();
            objParams.SourceId = Convert.ToUInt16(Bikewale.Common.Configuration.SourceId);
            objParams.VersionId = Convert.ToUInt32(versionId);

            objQuotation = objPriceQuote.GetPriceQuote(objParams);

            // save new pq cookie
            PriceQuoteCookie.SavePQCookie(objQuotation.Area.ToString(), objQuotation.PriceQuoteId.ToString(), objQuotation.Area.ToString(), objQuotation.VersionId.ToString(), "");
        }

        protected void ProcessPriceQuoteData()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                objPriceQuote = container.Resolve<IPriceQuote>();
            }

            // Validate price quote cookie and process pricequote.
            if (PriceQuoteCookie.IsPQCoockieExist())
            {
                if (!IsPostBack)
                {
                    priceQuoteId = PriceQuoteCookie.PQId;

                    Trace.Warn("pq id : " + priceQuoteId);

                    if (priceQuoteId != "0")
                    {

                        versionId = Convert.ToUInt32(PriceQuoteCookie.VersionId);
                        ShowPriceQuote();
                    }
                    else
                    {
                        Response.Redirect("/pricequote/default.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
            }
            else
            {
                Response.Redirect("/pricequote/default.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }
    }   // End of class
}   // End of namespacert