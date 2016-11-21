using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Memcache;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
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
        protected MUpcomingBikesMin ctrlUpcomingBikes;
        protected PopularBikesMin ctrlPopularBikes;
        protected ModelGallery photoGallery;
        protected HtmlSelect ddlPages;
        protected int BasicId = 0, pageId = 1;
        protected String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty, modelUrl = String.Empty;
        protected String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = string.Empty, url = string.Empty;
        //private CMSPageDetailsEntity pageDetails = null;
        protected Repeater rptPhotos;
        // private IPager objPager = null;
        protected ArticlePageDetails objFeature = null;
        private BikeMakeEntityBase _taggedMakeObj;
        protected GlobalCityAreaEntity currentCityArea;
        private bool _isContentFount = true;
        protected IEnumerable<ModelImage> objImg = null;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            if (!IsPostBack)
            {
                if (ProcessQueryString())
                {
                    if (BasicId > 0)
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
                if (!_basicId.Equals(Request.QueryString["id"]))
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
        /// <summary>
        /// Modified By : Aditi Srivastava on 17 Nov 2016
        /// Summary     : Added photo gallery control
        /// </summary>
        private void GetFeatureDetails()
        {
            try
            {
                //GetFeatureDetailsViaGrpc();

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                          .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                          .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    objFeature = _cache.GetArticlesDetails(Convert.ToUInt32(BasicId));

                    if (objFeature != null)
                    {
                        GetFeatureData();
                        BindPages();
                        BindPageWidgets();
                        objImg = _cache.GetArticlePhotos(BasicId);

                        if (objImg != null && objImg.Count() > 0)
                        {
                            photoGallery.Photos = objImg.ToList();
                            photoGallery.isModelPage = false;
                            photoGallery.articleName = pageTitle;
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
            displayDate = Convert.ToDateTime(objFeature.DisplayDate).ToString("MMMM dd, yyyy hh:mm tt");
            FetchMakeDetails();
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

        /// <summary>
        /// Created by : Aditi Srivastava on 16 Nov 2016
        /// Description: bind upcoming and popular bikes
        /// </summary>
        protected void BindPageWidgets()
        {
            currentCityArea = GlobalCityArea.GetGlobalCityArea();
            try
            {
                ctrlPopularBikes.totalCount = 4;
                ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                ctrlPopularBikes.cityName = currentCityArea.City;
                ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                ctrlUpcomingBikes.pageSize = 4;
                if (_taggedMakeObj != null)
                {
                    ctrlPopularBikes.makeId = _taggedMakeObj.MakeId;
                    ctrlPopularBikes.makeName = _taggedMakeObj.MakeName;
                    ctrlPopularBikes.makeMasking = _taggedMakeObj.MaskingName;
                    ctrlUpcomingBikes.MakeId = _taggedMakeObj.MakeId;
                    ctrlUpcomingBikes.makeMaskingName = _taggedMakeObj.MaskingName;
                    ctrlUpcomingBikes.makeName = _taggedMakeObj.MakeName;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "viewF.BindPageWidgets");
                objErr.SendMail();
            }
        }



        /// <summary>
        /// Created by : Aditi Srivastava on 16 Nov 2016
        /// Description: get make details if a make is tagged
        /// </summary>
        private void FetchMakeDetails()
        {
            try
            {
                if (objFeature.VehiclTagsList != null && objFeature.VehiclTagsList.Count > 0)
                {
                    _taggedMakeObj = objFeature.VehiclTagsList.FirstOrDefault().MakeBase;

                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.newsdetails.FetchMakeDetails");
                objErr.SendMail();
            }
        }
    }
}