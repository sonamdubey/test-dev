using Bikewale.Common;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Memcache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 22 May 2014
    /// </summary>
    public class viewRT : System.Web.UI.Page
    {
        protected HtmlSelect ddlPages;
        //protected Repeater rptPages, rptPageContent;
        protected Repeater rptPageContent;
        protected int BasicId = 0, pageId = 1;
        protected String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty, modelUrl = String.Empty;
        protected String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = String.Empty, canonicalUrl = String.Empty;
        //private CMSPageDetailsEntity pageDetails = null;
        protected StringBuilder _bikeTested;
        protected Repeater rptPhotos;
        // private IPager objPager = null;
        protected ArticlePageDetails objRoadtest;
        private bool _isContentFound = true;

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
                    GetRoadTestDetails();
                }
                else
                {
                    Response.Redirect("/m/road-tests/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        private bool ProcessQueryString()
        {
            bool isSuccess = true;

            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && CommonOpn.CheckId(Request.QueryString["id"]))
            {
                /** Modified By : Ashwini Todkar on 19 Aug 2014 , add when consuming carwale api
                 Check if basic id exists in mapped carwale basic id log **/

                string basicId = BasicIdMapping.GetCWBasicId(Request["id"]);
                //Trace.Warn("basicid" + basicId);
                //if id exists then redirect url to new basic id url
                //Trace.Warn("news host : " + Request.ServerVariables["HTTP_X_ORIGINAL_URL"]);
                if (!String.IsNullOrEmpty(basicId))
                {
                    string _newUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];

                    var _titleStartIndex = _newUrl.LastIndexOf('/') + 1;
                    var _titleEndIndex = _newUrl.LastIndexOf('-');
                    string _newUrlTitle = _newUrl.Substring(_titleStartIndex, _titleEndIndex - _titleStartIndex + 1);
                    _newUrl = "/m/road-tests/" + _newUrlTitle + basicId + ".html";
                    CommonOpn.RedirectPermanent(_newUrl);
                }

                BasicId = Convert.ToInt32(Request.QueryString["id"]);

                // if (!Int32.TryParse(Request.QueryString["id"].ToString(), out BasicId))
                // isSuccess = false;

                //if (Request.QueryString["pn"] != null && !String.IsNullOrEmpty(Request.QueryString["pn"]) && CommonOpn.CheckId(Request.QueryString["pn"]))
                //{
                //    if (!Int32.TryParse(Request.QueryString["pn"], out pageId))
                //        isSuccess = false;
                //}
            }
            else
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        private async void GetRoadTestDetails()
        {
            try
            {
                string _apiUrl = "webapi/article/contentpagedetail/?basicid=" + BasicId;

                // Send HTTP GET requests 
                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objRoadtest = await objClient.GetApiResponse<ArticlePageDetails>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objRoadtest);
                }

                if (objRoadtest != null)
                {
                    GetRoadTestData();
                    BindPages();
                    BindPhotos();
                }
                else
                {
                    _isContentFound = false;
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("ex.Message: " + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFound)
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        private async void BindPhotos()
        {
            try
            {
                string _apiUrl = "webapi/image/GetArticlePhotos/?basicid=" + BasicId;
                List<ModelImage> objImg = null;

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
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

        //private void GetNavigationLinks()
        //{
        //    PagerEntity pagerEntity = new PagerEntity();
        //    pagerEntity.BaseUrl = "/m/road-tests/";
        //    pagerEntity.PageNo = pageId;
        //    pagerEntity.PagerSlotSize = 5;
        //    pagerEntity.PageUrlType = objRoadtest.ArticleUrl + "-" + BasicId.ToString() + "/p";
        //    pagerEntity.TotalResults = objRoadtest.PageList.Count;// pageDetails.PageList.Count;
        //    pagerEntity.PageSize = 1;

        //    PagerOutputEntity pagerOutput = objPager.GetPager<PagerOutputEntity>(pagerEntity);

        //    //get next and prev page links for SEO
        //   // prevPageUrl = pagerOutput.PreviousPageUrl;
        //   // nextPageUrl = pagerOutput.NextPageUrl;
        //}

        private void GetRoadTestData()
        {

            baseUrl = "/m/road-tests/" + objRoadtest.ArticleUrl + '-' + BasicId.ToString() + "/";
            canonicalUrl = "http://www.bikewale.com/road-tests/" + objRoadtest.ArticleUrl + '-' + BasicId.ToString() + ".html";
            data = objRoadtest.Description;
            author = objRoadtest.AuthorName;
            pageTitle = objRoadtest.Title;
            displayDate = Convert.ToDateTime(objRoadtest.DisplayDate).ToString("dd-MMM-yyyy");

            if (objRoadtest.VehiclTagsList != null && objRoadtest.VehiclTagsList.Count > 0)
            {

                _bikeTested = new StringBuilder();

                _bikeTested.Append("Bike Tested: ");

                IEnumerable<int> ids = objRoadtest.VehiclTagsList
                       .Select(e => e.ModelBase.ModelId)
                       .Distinct();

                foreach (var i in ids)
                {
                    VehicleTag item = objRoadtest.VehiclTagsList.Where(e => e.ModelBase.ModelId == i).First();
                    _bikeTested.Append("<a title='" + item.MakeBase.MakeName + " " + item.ModelBase.ModelName + " Bikes' href='/m/" + item.MakeBase.MaskingName + "-bikes/" + item.ModelBase.MaskingName + "/'>" + item.ModelBase.ModelName + "</a>   ");
                }
            }

            //if (pageDetails.ImageList != null)
            //    BindPhotos();
        }

        //private void BindPhotos()
        //{
        //    if (pageDetails.ImageList != null)
        //    {
        //        rptPhotos.DataSource = pageDetails.ImageList;
        //        rptPhotos.DataBind();
        //    }
        //}

        private void BindPages()
        {
            //Trace.Warn("pageDetails.PageList : ", pageDetails.PageList.Count.ToString());

            //rptPages.DataSource = objRoadtest.PageList;
            //rptPages.DataBind();

            rptPageContent.DataSource = objRoadtest.PageList;
            rptPageContent.DataBind();
            //ddlPages.Items.FindByValue(pageId.ToString()).Selected = true;
        }

        protected string GetImageUrl(string hostUrl, string imagePath)
        {
            string imgUrl = String.Empty;
            imgUrl = Bikewale.Common.ImagingFunctions.GetPathToShowImages(imagePath, hostUrl);

            return imgUrl;
        }
    }
}