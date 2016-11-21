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
        protected ModelGallery photoGallery;
        protected GlobalCityAreaEntity currentCityArea;
        protected MUpcomingBikesMin ctrlUpcomingBikes;
        protected PopularBikesMin ctrlPopularBikes;
        protected uint BasicId = 0, pageId = 1;
        protected String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty, modelUrl = String.Empty;
        protected String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = String.Empty, canonicalUrl = String.Empty;
        protected StringBuilder _bikeTested;
        protected Repeater rptPhotos;
        protected ArticlePageDetails objRoadtest;
        protected IEnumerable<ModelImage> objImg = null;
        private bool _isContentFound = true;
        private BikeMakeEntityBase _taggedMakeObj;
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
                    BindPageWidgets();
                }
                else
                {
                    Response.Redirect("/m/expert-reviews/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        private bool ProcessQueryString()
        {
            bool isSuccess = true;
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && CommonOpn.CheckId(Request.QueryString["id"]))
            {
                /** Modified By : Ashwini Todkar on 19 Aug 2014 , add when consuming carwale api
                 Check if basic id exists in mapped carwale basic id log **/

                string basicId = BasicIdMapping.GetCWBasicId(Request["id"]);

                if (!basicId.Equals(Request.QueryString["id"]))
                {
                    string _newUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];

                    var _titleStartIndex = _newUrl.LastIndexOf('/') + 1;
                    var _titleEndIndex = _newUrl.LastIndexOf('-');
                    string _newUrlTitle = _newUrl.Substring(_titleStartIndex, _titleEndIndex - _titleStartIndex + 1);
                    _newUrl = "/m/expert-reviews/" + _newUrlTitle + basicId + ".html";
                    CommonOpn.RedirectPermanent(_newUrl);
                }

                BasicId = Convert.ToUInt32(Request.QueryString["id"]);
            }
            else
            {
                isSuccess = false;
            }

            return isSuccess;
        }
        /// <summary>
        /// Modified By : Aditi Srivastava on 17 Nov 2016
        /// Summary     : Added photo gallery control
        /// </summary>
        private void GetRoadTestDetails()
        {
            try
            {

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                       .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                       .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    objRoadtest = _cache.GetArticlesDetails(BasicId);

                    if (objRoadtest != null)
                    {
                        GetRoadTestData();

                        if (objRoadtest.PageList != null)
                        {
                            rptPageContent.DataSource = objRoadtest.PageList;
                            rptPageContent.DataBind();
                        }


                        objImg = _cache.GetArticlePhotos(Convert.ToInt32(BasicId));

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
                        _isContentFound = false;
                    }

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
            try
            {
                baseUrl = "/m/expert-reviews/" + objRoadtest.ArticleUrl + '-' + BasicId.ToString() + "/";
                canonicalUrl = "http://www.bikewale.com/expert-reviews/" + objRoadtest.ArticleUrl + '-' + BasicId.ToString() + ".html";
                data = objRoadtest.Description;
                author = objRoadtest.AuthorName;
                pageTitle = objRoadtest.Title;
                displayDate = objRoadtest.DisplayDate.ToString();

                if (objRoadtest.VehiclTagsList != null && objRoadtest.VehiclTagsList.Count > 0)
                {
                    _taggedMakeObj = objRoadtest.VehiclTagsList.FirstOrDefault().MakeBase;

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
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "viewRT.GetRoadTestData");
                objErr.SendMail();
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 16 Nov 2016
        /// Description: Bind upcoming and popular bikes
        /// </summary>
        protected void BindPageWidgets()
        {
            currentCityArea = GlobalCityArea.GetGlobalCityArea();
            try
            {
                if (ctrlPopularBikes != null)
                {
                    ctrlPopularBikes.totalCount = 4;
                    ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                    ctrlPopularBikes.cityName = currentCityArea.City;
                    if (_taggedMakeObj != null)
                    {
                        ctrlPopularBikes.makeId = _taggedMakeObj.MakeId;
                        ctrlPopularBikes.makeName = _taggedMakeObj.MakeName;
                        ctrlPopularBikes.makeMasking = _taggedMakeObj.MaskingName;

                    }
                }
                if (ctrlUpcomingBikes != null)
                {
                    ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                    ctrlUpcomingBikes.pageSize = 4;
                    if (_taggedMakeObj != null)
                    {
                        ctrlUpcomingBikes.MakeId = _taggedMakeObj.MakeId;
                        ctrlUpcomingBikes.makeMaskingName = _taggedMakeObj.MaskingName;
                        ctrlUpcomingBikes.makeName = _taggedMakeObj.MakeName;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "viewRT.BindPageWidgets");
                objErr.SendMail();
            }
        }
    }
}