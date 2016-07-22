using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.PriceQuote
{
    /// <summary>
    ///     Created By : Ashish G. Kamble
    ///     Summary : Class to show the price quote. If other versions of the same model are available then customer can take price quote of those versions.
    ///     Modified by :   Sumit Kate on 05 Jan 2016
    ///     Description :   Added hasAlternateBikes, hasUpcomingBikes class variables.
    /// </summary>
    public class Quotation : Page
    {
        //protected Repeater rptAllVersions;
        protected UpcomingBikes_new ctrlUpcomingBikes;
        protected AlternativeBikes ctrlAlternativeBikes;
        protected HtmlGenericControl divAllVersions, div_ShowPQ, divUserReviews;

        protected string city = string.Empty, priceQuoteId = string.Empty, make = string.Empty, imgPath = String.Empty, dealerId = string.Empty;
        protected uint cityId = 0, areaId = 0;
        protected MakeModelVersion mmv = null;
        protected BikeQuotationEntity objQuotation = null;
        protected List<BikeVersionsListEntity> versionList = null;
        protected DropDownList ddlVersion;
        IPriceQuote objPriceQuote = null;
        protected UInt32 versionId = 0;
        protected bool hasAlternateBikes = false, hasUpcomingBikes = false;
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

                    container.RegisterType<IBikeVersionCacheRepository<BikeVersionEntity, uint>, BikeVersionsCacheRepository<BikeVersionEntity, uint>>()
                            .RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersions<BikeVersionEntity, uint>>()
                            .RegisterType<ICacheManager, MemcacheManager>()
                            ;
                    var objCache = container.Resolve<IBikeVersionCacheRepository<BikeVersionEntity, uint>>();


                    versionList = objCache.GetVersionsByType(EnumBikeType.PriceQuote, Convert.ToInt32(modelId), Convert.ToInt32(PriceQuoteQueryString.CityId));

                    if (versionList.Count > 0)
                    {
                        ddlVersion.DataSource = versionList;
                        ddlVersion.DataValueField = "VersionId";
                        ddlVersion.DataTextField = "VersionName";
                        ddlVersion.DataBind();

                        ddlVersion.SelectedValue = PriceQuoteQueryString.VersionId;
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
        /// Modified by :   Sumit Kate on 05 Jan 2016
        /// Description :   Set hasAlternateBikes, hasUpcomingBikes class variables.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            ProcessPriceQuoteData();

            if (PriceQuoteQueryString.PQId != null)
            {
                priceQuoteId = PriceQuoteQueryString.PQId;

                mmv = new MakeModelVersion();
                if (versionId > 0)
                {
                    mmv.GetVersionDetails(versionId.ToString());
                    //imgPath = ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/" + mmv.LargePic, mmv.HostUrl);
                    imgPath = Bikewale.Utility.Image.GetPathToShowImages(mmv.OriginalImagePath, mmv.HostUrl, Bikewale.Utility.ImageSize._310x174);


                    BindVersion(mmv.ModelId);

                    BindAlternativeBikeControl(Convert.ToString(PriceQuoteQueryString.VersionId));

                    //To get Upcoming Bike List Details 
                    ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                    ctrlUpcomingBikes.pageSize = 6;
                    ctrlUpcomingBikes.MakeId = Convert.ToInt32(mmv.MakeId);

                    hasAlternateBikes = ctrlAlternativeBikes.FetchedRecordsCount > 0;
                    hasUpcomingBikes = ctrlUpcomingBikes.FetchedRecordsCount > 0;
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
        /// Modified By : Lucky Rathore on 27 June 2016
        /// Description : replace cookie __utmz with _bwutmz
        /// </summary>
        protected void SavePriceQuote(object sender, EventArgs e)
        {
            PQOutputEntity objPQOutput = null;
            uint selectedVersionId = default(uint);

            try
            {
                selectedVersionId = Convert.ToUInt32(ddlVersion.SelectedValue);
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
                        objPQEntity.PQLeadId = Convert.ToUInt16(PQSourceEnum.Desktop_PQ_Quotation);
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
                if (objPQOutput.PQId > 0 && objPQOutput.DealerId > 0)
                {
                    // Save pq cookie
                    //PriceQuoteCookie.SavePQCookie(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), objPQOutput.DealerId.ToString());                    
                    Response.Redirect("/pricequote/dealerpricequote.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), objPQOutput.DealerId.ToString())), false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                else if (objPQOutput.PQId > 0)
                {
                    //PriceQuoteCookie.SavePQCookie(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), string.Empty);                    
                    Response.Redirect("/pricequote/quotation.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), selectedVersionId.ToString(), string.Empty)), false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
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

            objParams.CityId = Convert.ToUInt16(PriceQuoteQueryString.CityId);
            objParams.AreaId = Convert.ToUInt32(PriceQuoteQueryString.AreaId);
            objParams.ClientIP = CommonOpn.GetClientIP();
            objParams.SourceId = Convert.ToUInt16(Bikewale.Common.Configuration.SourceId);
            objParams.VersionId = Convert.ToUInt32(versionId);

            objQuotation = objPriceQuote.GetPriceQuote(objParams);

            // save new pq cookie
            //PriceQuoteCookie.SavePQCookie(objQuotation.Area.ToString(), objQuotation.PriceQuoteId.ToString(), objQuotation.Area.ToString(), objQuotation.VersionId.ToString(), "");            
        }

        protected void ProcessPriceQuoteData()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                objPriceQuote = container.Resolve<IPriceQuote>();
            }


            // Validate price quote cookie and process pricequote.
            //if (PriceQuoteCookie.IsPQCoockieExist())
            if (PriceQuoteQueryString.IsPQQueryStringExists())
            {
                cityId = Convert.ToUInt32(PriceQuoteQueryString.CityId);
                areaId = Convert.ToUInt32(PriceQuoteQueryString.AreaId);
                if (!IsPostBack)
                {
                    priceQuoteId = PriceQuoteQueryString.PQId;

                    Trace.Warn("pq id : " + priceQuoteId);
                    if (priceQuoteId != "0")
                    {

                        versionId = Convert.ToUInt32(PriceQuoteQueryString.VersionId);
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

        private void BindAlternativeBikeControl(String versionId)
        {
            ctrlAlternativeBikes.TopCount = 6;

            if (!String.IsNullOrEmpty(versionId) && versionId != "0")
            {
                ctrlAlternativeBikes.VersionId = Convert.ToInt32(versionId);
                ctrlAlternativeBikes.PQSourceId = (int)PQSourceEnum.Desktop_PQ_Alternative;
            }
        }
    }   // End of class
}   // End of namespacert