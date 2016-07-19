using Bikewale.Cache.Content;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Content;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 22 May 2014
    /// </summary>
    public class viewF : System.Web.UI.Page
    {
        //protected Repeater rptPages, rptPageContent;
        protected Repeater rptPageContent;
        protected HtmlSelect ddlPages;
        protected int BasicId = 0, pageId = 1;
        protected String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty, modelUrl = String.Empty;
        protected String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = string.Empty, url = string.Empty;
        //private CMSPageDetailsEntity pageDetails = null;
        protected Repeater rptPhotos;
        // private IPager objPager = null;
        protected ArticlePageDetails objFeature = null;
        private bool _isContentFount = true;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ProcessQueryString())
                {
                    GetFeatureDetails();
                }
                else
                {
                    Response.Redirect("/m/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        private bool ProcessQueryString()
        {
            bool isSucess = true;

            if (Request.QueryString["id"] != null && !String.IsNullOrEmpty(Request.QueryString["id"]) && CommonOpn.CheckId(Request.QueryString["id"]))
            {
                /** Modified By : Ashwini Todkar on 12 Aug 2014 , add when consuming carwale api
                //Check if basic id exists in mapped carwale basic id log **/
                string _basicId = BasicIdMapping.GetCWBasicId(Request.QueryString["id"]);

                //if id exists then redirect url to new basic id url
                if (!String.IsNullOrEmpty(_basicId))
                {
                    string _newUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
                    var _titleStartIndex = _newUrl.IndexOf('/');
                    var _titleEndIndex = _newUrl.LastIndexOf('-');
                    string _newUrlTitle = _newUrl.Substring(_titleStartIndex, _titleEndIndex - _titleStartIndex + 1);
                    _newUrl = _newUrlTitle + _basicId + "/";
                    CommonOpn.RedirectPermanent(_newUrl);
                    Trace.Warn("_newUrl : " + _newUrl);
                }

                if (!Int32.TryParse(Request.QueryString["id"].ToString(), out BasicId))
                    isSucess = false;

                if (Request.QueryString["pn"] != null && !String.IsNullOrEmpty(Request.QueryString["pn"]) && CommonOpn.CheckId(Request.QueryString["pn"]))
                {
                    if (!Int32.TryParse(Request.QueryString["pn"], out pageId))
                        isSucess = false;
                }
            }
            else
            {
                isSucess = false;
            }

            return isSucess;
        }

        private void GetFeatureDetails()
        {
            try
            {
                //GetFeatureDetailsViaGrpc();

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IFeatureCache, FeaturesCache>()
                    .RegisterType<ICacheManager, MemcacheManager>()
                    .RegisterType<IFeatures, Bikewale.BAL.Content.Features>();
                    IFeatureCache _features = container.Resolve<IFeatureCache>();

                    objFeature = _features.GetFeatureDetailsViaGrpc(BasicId);

                    if (objFeature != null)
                    {
                        GetFeatureData();
                        BindPages();
                        IEnumerable<ModelImage> objImg = _features.BindPhotos(BasicId);

                        if (objImg != null && objImg.Count() > 0)
                        {
                            rptPhotos.DataSource = objImg;
                            rptPhotos.DataBind();
                        }
                    }
                    else
                    {
                        _isContentFount = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Trace.Warn("Ex Message: " + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFount)
                {
                    Response.Redirect("/m/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        private void GetFeatureData()
        {

            baseUrl = "/m/features/" + objFeature.ArticleUrl + '-' + BasicId.ToString() + "/";

            //  desktop url for facebook
            url = "/features/" + objFeature.ArticleUrl + '-' + BasicId.ToString() + "/";

            //data = objFeature.Description;
            author = objFeature.AuthorName;
            pageTitle = objFeature.Title;
            displayDate = Convert.ToDateTime(objFeature.DisplayDate).ToString("dd-MMM-yyyy");

            if (objFeature.TagsList != null && objFeature.TagsList.Count > 0)
            {
                if (objFeature.VehiclTagsList != null && objFeature.VehiclTagsList.Count > 0)
                {
                    modelName = objFeature.VehiclTagsList[0].ModelBase.ModelName;
                    modelUrl = "/m/" + UrlRewrite.FormatSpecial(objFeature.VehiclTagsList[0].MakeBase.MakeName) + "-bikes/" + objFeature.VehiclTagsList[0].ModelBase.MaskingName + "/";
                }
            }
        }

        private void BindPages()
        {
            if (objFeature.PageList != null)
            {
                rptPageContent.DataSource = objFeature.PageList;
                rptPageContent.DataBind();
            }
        }

        protected string GetImageUrl(string hostUrl, string imagePath)
        {
            string imgUrl = String.Empty;
            imgUrl = Bikewale.Common.ImagingFunctions.GetPathToShowImages(imagePath, hostUrl);

            return imgUrl;
        }

        protected string GetImageUrl(string hostUrl, string imagePath, string size)
        {
            string imgUrl = String.Empty;
            imgUrl = Bikewale.Utility.Image.GetPathToShowImages(imagePath, hostUrl, size);

            return imgUrl;
        }
    }
}