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
        protected Repeater rptPageContent;
        protected int BasicId = 0, pageId = 1;
        protected String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty, modelUrl = String.Empty;
        protected String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = String.Empty, canonicalUrl = String.Empty;
        protected StringBuilder _bikeTested;
        protected Repeater rptPhotos;
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
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IRoadTestCache, RoadTestCache>()
                         .RegisterType<ICacheManager, MemcacheManager>()
                         .RegisterType<IRoadTest, Bikewale.BAL.Content.RoadTest>();
                        IRoadTestCache _roadTest = container.Resolve<IRoadTestCache>();

                        IEnumerable<ModelImage> objImg = _roadTest.BindPhotos(Convert.ToInt32(BasicId));

                        if (objImg != null && objImg.Count() > 0)
                        {
                            rptPhotos.DataSource = objImg;
                            rptPhotos.DataBind();
                        }
                    }
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
                if (objRoadtest.VehiclTagsList.Any(m => (m.MakeBase != null && !String.IsNullOrEmpty(m.MakeBase.MaskingName))))
                {
                    _bikeTested = new StringBuilder();

                    _bikeTested.Append("Bike Tested: ");

                    IEnumerable<int> ids = objRoadtest.VehiclTagsList
                           .Select(e => e.ModelBase.ModelId)
                           .Distinct();

                    foreach (var i in ids)
                    {
                        VehicleTag item = objRoadtest.VehiclTagsList.Where(e => e.ModelBase.ModelId == i).First();
                        if (!String.IsNullOrEmpty(item.MakeBase.MaskingName))
                        {
                            _bikeTested.Append("<a title='" + item.MakeBase.MakeName + " " + item.ModelBase.ModelName + " Bikes' href='/m/" + item.MakeBase.MaskingName + "-bikes/" + item.ModelBase.MaskingName + "/'>" + item.ModelBase.ModelName + "</a>   ");
                        }
                    }
                }
            }
        }

        private void BindPages()
        {
            rptPageContent.DataSource = objRoadtest.PageList;
            rptPageContent.DataBind();
        }

        protected string GetImageUrl(string hostUrl, string imagePath)
        {
            string imgUrl = String.Empty;
            imgUrl = Bikewale.Common.ImagingFunctions.GetPathToShowImages(imagePath, hostUrl);

            return imgUrl;
        }
    }
}