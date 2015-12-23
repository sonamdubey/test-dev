using Bikewale.BAL.CMS;
using Bikewale.BAL.Pager;
using Bikewale.Common;
using Bikewale.Entities.CMS;
using Bikewale.Entities.Pager;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
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
        protected String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty , modelUrl = String.Empty;
        protected String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = string.Empty , url = string.Empty;
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
                    string _newUrl = Request.ServerVariables["HTTP_X_REWRITE_URL"];
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

        private async void GetFeatureDetails()
        {
            try
            {                
                string _apiUrl = "webapi/article/contentpagedetail/?basicid=" + BasicId;

                using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objFeature = await objClient.GetApiResponse<ArticlePageDetails>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objFeature);
                }                

                if (objFeature != null)
                {
                    GetFeatureData();
                    BindPages();
                    BindPhotos();
                }
                else
                {
                    _isContentFount = false;
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

        //private void GetNavigationLinks()
        //{
        //    PagerEntity pagerEntity = new PagerEntity();
        //    pagerEntity.BaseUrl = "/m/features/";
        //    pagerEntity.PageNo = pageId;
        //    pagerEntity.PagerSlotSize = 5;
        //    pagerEntity.PageUrlType = pageDetails.Url + "-" + BasicId.ToString() + "/p";
        //    pagerEntity.TotalResults = pageDetails.PageList.Count;
        //    pagerEntity.PageSize = 1;

        //    PagerOutputEntity pagerOutput = objPager.GetPager<PagerOutputEntity>(pagerEntity);

        //    //get next and prev page links for SEO
        //    prevPageUrl = pagerOutput.PreviousPageUrl;
        //    nextPageUrl = pagerOutput.NextPageUrl;
        //}

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

        /// <summary>
        /// 
        /// </summary>
        private async void BindPhotos()
        {
            try
            {                
                string _apiUrl = "webapi/image/GetArticlePhotos/?basicid=" + BasicId;                
                List<ModelImage> objImg = null;

                using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objImg = await objClient.GetApiResponse<List<ModelImage>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objImg);
                }
                

                if (objImg != null && objImg.Count > 0)
                {
                    rptPhotos.DataSource = objImg;
                    rptPhotos.DataBind();
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }          
        }

        private void BindPages()
        {
            //ddlPages.DataSource = pageDetails.PageList;
            //ddlPages.DataTextField = "PageName";
            //ddlPages.DataValueField = "Priority";
            //ddlPages.DataBind();

            //ddlPages.Items.FindByValue(pageId.ToString()).Selected = true;

          //  rptPages.DataSource = objFeature.PageList;
          //  rptPages.DataBind();

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

        protected string GetImageUrl(string hostUrl, string imagePath,string size)
        {
            string imgUrl = String.Empty;
            imgUrl = Bikewale.Utility.Image.GetPathToShowImages(imagePath, hostUrl, size);

            return imgUrl;
        }
    }
}