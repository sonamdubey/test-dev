using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Content
{
    public class ViewRT : System.Web.UI.Page
    {
        protected Repeater rptPages, rptPageContent;
        private string _basicId = string.Empty;
        protected ArticlePageDetails objRoadtest;
        protected StringBuilder _bikeTested;
        protected ArticlePhotoGallery ctrPhotoGallery;
        private bool _isContentFount = true;

        protected string articleUrl = string.Empty, articleTitle = string.Empty, basicId = string.Empty, authorName = string.Empty, displayDate = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();


            ProcessQS();
            GetRoadtestDetails();

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IArticles, Articles>()
                           .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                           .RegisterType<ICacheManager, MemcacheManager>();
                ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                IEnumerable<ModelImage> objImg = _cache.GetArticlePhotos(Convert.ToInt32(_basicId));

                if (objImg != null && objImg.Count() > 0)
                {
                    ctrPhotoGallery.BasicId = Convert.ToInt32(_basicId);
                    ctrPhotoGallery.ModelImageList = objImg;
                    ctrPhotoGallery.BindPhotos();
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        private void ProcessQS()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && CommonOpn.CheckId(Request.QueryString["id"]))
            {
                //id = Request.QueryString["id"].ToString();
                _basicId = Request.QueryString["id"];

                string basicId = BasicIdMapping.GetCWBasicId(_basicId);

                Trace.Warn("Carwale basic id : " + basicId);

                if (!String.IsNullOrEmpty(basicId))
                {
                    // Modified By :Lucky Rathore on 12 July 2016.
                    Form.Action = Request.RawUrl;
                    string _newUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
                    var _titleStartIndex = _newUrl.LastIndexOf('/') + 1;
                    var _titleEndIndex = _newUrl.LastIndexOf('-');
                    string _newUrlTitle = _newUrl.Substring(_titleStartIndex, _titleEndIndex - _titleStartIndex + 1);
                    _newUrl = "/road-tests/" + _newUrlTitle + basicId + ".html";
                    CommonOpn.RedirectPermanent(_newUrl);
                }
            }
            else
            {
                Response.Redirect("/road-tests/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        private void GetRoadtestDetails()
        {
            try
            {

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                           .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                           .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _objArticles = container.Resolve<ICMSCacheContent>();

                    objRoadtest = _objArticles.GetArticlesDetails(Convert.ToUInt32(_basicId));

                    if (objRoadtest != null)
                    {
                        BindPages();
                        GetRoadtestData();
                        if (objRoadtest.VehiclTagsList.Count > 0)
                            GetTaggedBikeList();
                    }
                    else
                    {
                        _isContentFount = false;
                    }
                }


            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFount)
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetTaggedBikeList()
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
                        _bikeTested.Append("<a title='" + item.MakeBase.MakeName + " " + item.ModelBase.ModelName + " Bikes' href='/" + item.MakeBase.MaskingName + "-bikes/" + item.ModelBase.MaskingName + "/'>" + item.ModelBase.ModelName + "</a>   ");
                    }
                }
                Trace.Warn("biketested", _bikeTested.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindPages()
        {
            rptPages.DataSource = objRoadtest.PageList;
            rptPages.DataBind();

            rptPageContent.DataSource = objRoadtest.PageList;
            rptPageContent.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetRoadtestData()
        {
            articleTitle = objRoadtest.Title;
            authorName = objRoadtest.AuthorName;
            displayDate = objRoadtest.DisplayDate.ToString();
            articleUrl = objRoadtest.ArticleUrl;
            basicId = objRoadtest.BasicId.ToString();
        }
    }
}