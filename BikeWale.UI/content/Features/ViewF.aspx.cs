using Bikewale.BAL.Content;
using Bikewale.Cache.Content;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
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

namespace Bikewale.Content
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 24 Sept 2014
    /// </summary>
    public class ViewF : System.Web.UI.Page
    {
        //protected DropDownList drpPages, drpPages_footer;
        protected Repeater rptPages, rptPageContent;
        protected ArticlePhotoGallery ctrPhotoGallery;
        //protected DataList dlstPhoto;
        protected HtmlGenericControl topNav, bottomNav;
        protected string PageId = "1", Str = string.Empty, canonicalUrl = String.Empty;
        protected bool ShowGallery = false, IsPhotoGalleryPage = false;
        protected int StrCount = 0;

        protected string articleUrl = string.Empty, articleTitle = string.Empty, authorName = string.Empty, displayDate = string.Empty;

        protected ArticlePageDetails objFeature = null;

        private bool _isContentFount = true;
        private string _basicId = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            ProcessQS();

            if (!String.IsNullOrEmpty(_basicId))
            {
                GetFeatureDetails();
                //GetArticlePhotos();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessQS()
        {
            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "" && CommonOpn.CheckId(Request.QueryString["id"]))
            {
                _basicId = Request.QueryString["id"].ToString();

                /** Modified By : Ashwini Todkar on 12 Aug 2014 , add when consuming carwale api
               //Check if basic id exists in mapped carwale basic id log **/
                string _mappedBasicId = BasicIdMapping.GetCWBasicId(_basicId);
                Trace.Warn("Carwale basic id : " + _mappedBasicId);

                //if id exists then redirect url to new basic id url
                if (!String.IsNullOrEmpty(_mappedBasicId))
                {
                    string _newUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
                    var _titleStartIndex = _newUrl.IndexOf('/');
                    var _titleEndIndex = _newUrl.LastIndexOf('-');
                    string _newUrlTitle = _newUrl.Substring(_titleStartIndex, _titleEndIndex - _titleStartIndex + 1);
                    _newUrl = _newUrlTitle + _mappedBasicId + "/";
                    CommonOpn.RedirectPermanent(_newUrl);
                    Trace.Warn("_newUrl : " + _newUrl);
                }
            }
            else
            {
                Response.Redirect("/pagenotfound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to fetch feature details from api asynchronously
        /// </summary>
        private void GetFeatureDetails()
        {
            try
            {
                //GetFeatureDetailsViaGrpc();

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IFeatureCache, FeaturesCache>()
                     .RegisterType<ICacheManager, MemcacheManager>()
                     .RegisterType<IFeatures, Features>();
                    IFeatureCache _features = container.Resolve<IFeatureCache>();

                    objFeature = _features.GetFeatureDetailsViaGrpc(Convert.ToInt32(_basicId));

                    if (objFeature != null)
                    {
                        GetFeatureData();
                        BindPages();
                        IEnumerable<ModelImage> objImg = _features.BindPhotos(Convert.ToInt32(_basicId));

                        if (objImg != null && objImg.Count() > 0)
                        {
                            ctrPhotoGallery.BasicId = Convert.ToInt32(_basicId);
                            ctrPhotoGallery.ModelImageList = objImg;
                            ctrPhotoGallery.BindPhotos();
                        }
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
        private void GetFeatureData()
        {
            articleTitle = objFeature.Title;
            authorName = objFeature.AuthorName;
            displayDate = objFeature.DisplayDate.ToString();
            articleUrl = objFeature.ArticleUrl;
            canonicalUrl = "/features/" + articleUrl + "-" + _basicId + "/";
        }

        private void BindPages()
        {
            rptPages.DataSource = objFeature.PageList;
            rptPages.DataBind();

            rptPageContent.DataSource = objFeature.PageList;
            rptPageContent.DataBind();
        }
    }
}