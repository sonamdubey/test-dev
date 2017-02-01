using Bikewale.BAL.BikeData;
using Bikewale.BAL.EditCMS;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Memcache;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web;

namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 21 May 2014
    /// Modified By : Ashwini Todkar on  on 30 Sept 2014
    /// </summary>
    public class newsdetails : System.Web.UI.Page
    {
        //private CMSPageDetailsEntity pageDetails = null;
        protected String nextPageUrl = String.Empty, prevPageUrl = String.Empty, prevPageTitle = String.Empty, nextPageTitle = String.Empty, pageUrl = String.Empty, ampUrl = string.Empty;
        protected String newsContent = String.Empty, newsTitle = String.Empty, author = String.Empty, displayDate = String.Empty, mainImg = String.Empty;
        protected uint pageViews = 0;
        protected String _newsId = String.Empty;
        private ArticleDetails objNews = null;
        private bool _isContentFound = true;
        protected MinGenericBikeInfoControl ctrlGenericBikeInfo;
        protected UpcomingBikesMin ctrlUpcomingBikes;
        protected PopularBikesMin ctrlPopularBikes;
        private BikeMakeEntityBase _taggedMakeObj;
        private BikeModelEntityBase _taggedModelObj;
        protected GlobalCityAreaEntity currentCityArea;
        static bool _useGrpc = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.UseGrpc);
        static bool _logGrpcErrors = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.LogGrpcErrors);
        static readonly ILog _logger = LogManager.GetLogger(typeof(newsdetails));
        protected uint taggedModelId;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        protected bool showBodyStyleWidget;
        protected EnumBikeBodyStyles bodyStyle;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        //Modified By: Aditi Srivastava on 7 Sep 2016
        //SUmmary: Added request rawURL on form action
        protected void Page_Load(object sender, EventArgs e)
        {

            Form.Action = Request.RawUrl;

            if (!IsPostBack)
            {
                ProcessQS();

                int _basicId = 0;

                if (Int32.TryParse(_newsId, out _basicId))
                {
                    GetNewsDetails(_basicId);
                    BindPageWidgets();

                }
                else
                {
                    Response.Redirect("/m/pagenotfound.aspx", true);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// PopulateWhere to process query string and get carwale new basicid against bikewale basicid
        /// </summary>
        private void ProcessQS()
        {
            if (Request.QueryString["id"] != null && !String.IsNullOrEmpty(Request.QueryString["id"]))
            {

                /** Modified By : Ashwini Todkar on 19 Aug 2014 , add when consuming carwale api
               //Check if basic id exists in mapped carwale basic id log **/
                string basicid = BasicIdMapping.GetCWBasicId(Request["id"]);
                Trace.Warn("basicid" + basicid);
                //if id exists then redirect url to new basic id url
                if (!basicid.Equals(Request.QueryString["id"]))
                {
                    string newUrl = "/m/news/" + basicid + "-" + Request["t"] + ".html";
                    Trace.Warn("New url : " + newUrl);
                    CommonOpn.RedirectPermanent(newUrl);
                }
                else
                {
                    if (CommonOpn.CheckId(Request.QueryString["id"]) == true)
                        _newsId = Request.QueryString["id"];
                }
            }
            else
            {
                Response.Redirect("/m/news/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        /// <summary>
        ///  PopulateWhere to set news details from carwale api asynchronously
        /// </summary>
        /// <param name="_basicId"></param>
        private void GetNewsDetails(int _basicId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                            .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                            .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    objNews = _cache.GetNewsDetails(Convert.ToUInt32(_basicId));

                    if (objNews != null)
                        GetNewsData();

                    else
                        _isContentFound = false;
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
                if (!_isContentFound)
                {
                    Response.Redirect("/m/news/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// PopulateWhere to set news details
        /// Modified By : Sushil Kumar on 2nd Jan 2016
        /// Description : Get tagged model for article 
        /// Modified By : Sajal Gupta on 27-01-2017
        /// Description : Saved taggedModelId in the variable.  
        /// </summary>
        private void GetNewsData()
        {
            newsTitle = objNews.Title;
            author = objNews.AuthorName;
            displayDate = objNews.DisplayDate.ToString();
            newsContent = objNews.Content;
            nextPageTitle = objNews.NextArticle.Title;
            prevPageTitle = objNews.PrevArticle.Title;
            pageViews = objNews.Views;
            pageUrl = _newsId + '-' + objNews.ArticleUrl + ".html";
            ampUrl = String.Format("{0}/m/news/{1}-{2}/amp/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objNews.ArticleUrl, _newsId);

            if (objNews != null && objNews.VehiclTagsList.Count > 0)
            {
                _taggedMakeObj = objNews.VehiclTagsList.FirstOrDefault().MakeBase;
                _taggedMakeObj = new Bikewale.Common.MakeHelper().GetMakeNameByMakeId((uint)_taggedMakeObj.MakeId);
                _taggedModelObj = objNews.VehiclTagsList.FirstOrDefault().ModelBase;
                if (_taggedModelObj != null)
                    taggedModelId = (uint)_taggedModelObj.ModelId;
                _taggedModelObj = new Bikewale.Common.ModelHelper().GetModelDataById((uint)_taggedModelObj.ModelId);

            }
            if (!String.IsNullOrEmpty(objNews.NextArticle.ArticleUrl))
                nextPageUrl = objNews.NextArticle.BasicId + "-" + objNews.NextArticle.ArticleUrl + ".html";

            if (!String.IsNullOrEmpty(objNews.PrevArticle.ArticleUrl))
                prevPageUrl = objNews.PrevArticle.BasicId + "-" + objNews.PrevArticle.ArticleUrl + ".html";
        }

        protected String GetMainImagePath()
        {
            String mainImgUrl = String.Empty;
            //mainImgUrl = ImagingFunctions.GetPathToShowImages( objNews.LargePicUrl, objNews.HostUrl);
            mainImgUrl = Bikewale.Utility.Image.GetPathToShowImages(objNews.OriginalImgUrl, objNews.HostUrl, Bikewale.Utility.ImageSize._640x348);
            return mainImgUrl;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 16 Nov 2016
        /// Description: bind upcoming and popular bikes
        /// Modified By : Sushil Kumar on 2nd Jan 2016
        /// Description : Bind ctrlGenericBikeInfo control 
        /// Modified by : Sajal Gupta on 31-01-2017
        /// Description : Binded popular bikes widget.
        /// </summary>
        protected void BindPageWidgets()
        {
            try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (ctrlPopularBikes != null)
                {
                    ctrlPopularBikes.totalCount = 4;
                    ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                    ctrlPopularBikes.cityName = currentCityArea.City;
                    if (ctrlUpcomingBikes != null)
                    {
                        ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                        ctrlUpcomingBikes.pageSize = 4;
                    }
                    if (_taggedMakeObj != null)
                    {
                        ctrlUpcomingBikes.MakeId = _taggedMakeObj.MakeId;
                        ctrlUpcomingBikes.makeMaskingName = _taggedMakeObj.MaskingName;
                        ctrlUpcomingBikes.makeName = _taggedMakeObj.MakeName;
                        ctrlPopularBikes.makeId = _taggedMakeObj.MakeId;
                        ctrlPopularBikes.makeName = _taggedMakeObj.MakeName;
                        ctrlPopularBikes.makeMasking = _taggedMakeObj.MaskingName;
                    }
                    if (_taggedModelObj != null)
                    {
                        ctrlGenericBikeInfo.ModelId = (uint)_taggedModelObj.ModelId;
                    }

                }
                if (taggedModelId > 0)
                {
                    ctrlBikesByBodyStyle.ModelId = taggedModelId;
                    ctrlBikesByBodyStyle.topCount = 9;
                    ctrlBikesByBodyStyle.CityId = currentCityArea.CityId;

                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                            .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                            .RegisterType<ICacheManager, MemcacheManager>()
                            .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();

                        IBikeModelsCacheRepository<int> modelCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                        bodyStyle = modelCache.GetBikeBodyType(taggedModelId);

                    }

                    if (bodyStyle == EnumBikeBodyStyles.Scooter || bodyStyle == EnumBikeBodyStyles.Cruiser || bodyStyle == EnumBikeBodyStyles.Sports)
                        showBodyStyleWidget = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "newsdetails.BindPageWidgets");
                objErr.SendMail();
            }

        }
    }
}