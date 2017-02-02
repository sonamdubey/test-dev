using Bikewale.BAL.BikeData;
using Bikewale.BAL.EditCMS;
using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
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
    /// Modified By: Aditi Srivastava on 31 Jan 2017
    /// Summary    : Added common view model for fetching data and modified popular widgets
    /// </summary>
    public class newsdetails : System.Web.UI.Page
    {
        protected String _newsId = String.Empty;
        protected ArticleDetails objArticle = null;
        protected NewsDetails objNews;
        protected PageMetaTags metas;
        protected MinGenericBikeInfoControl ctrlGenericBikeInfo;
        protected UpcomingBikesMin ctrlUpcomingBikes;
        protected PopularBikesMin ctrlPopularBikes;
        protected BikeMakeEntityBase _taggedMakeObj;
        protected BikeModelEntityBase _taggedModelObj;
        protected GlobalCityAreaEntity currentCityArea;
        static bool _useGrpc = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.UseGrpc);
        static bool _logGrpcErrors = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.LogGrpcErrors);
        static readonly ILog _logger = LogManager.GetLogger(typeof(newsdetails));
        protected uint taggedModelId;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        protected bool isModelTagged;
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
             BindNewsDetails();
            }
        }

       /// <summary>
       /// Created By : Aditi Srivastava on 31 Jan 2017
       /// Summary    : Bind news details from view model
       /// </summary>
        private void BindNewsDetails()
        {
            try
            {
                objNews = new NewsDetails();
                if (!objNews.IsPermanentRedirect)
                {
                    if (!objNews.IsPageNotFound)
                    {
                        objNews.GetNewsArticleDetails();
                        if (objNews.IsContentFound)
                        {
                            objArticle = objNews.ArticleDetails;
                            _taggedMakeObj = objNews.TaggedMake;
                            _taggedModelObj = objNews.TaggedModel;
                            if (_taggedModelObj != null)
                                taggedModelId = (uint)_taggedModelObj.ModelId;
                            currentCityArea = objNews.CityArea;
                            metas = objNews.PageMetas;
                            BindPageWidgets();
                        }
                        else
                        {
                            Response.Redirect("/m/news/", false);
                            if (HttpContext.Current != null)
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }                    
                    else
                    {
                        Response.Redirect("/m/pagenotfound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }


            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : Bikewale.Mobile.Content.newsdetails.BindNewsDetails");
                objErr.SendMail();
            }
            finally
            {
                if (objNews.IsPermanentRedirect)
                {
                    string newUrl = string.Format("/m/news/{0}-{1}.html", objNews.MappedCWId, Request["t"]);
                    Bikewale.Common.CommonOpn.RedirectPermanent(newUrl);
                }
            }
        }

               
        /// <summary>
        /// Created by : Aditi Srivastava on 16 Nov 2016
        /// Description: bind upcoming and popular bikes
        /// Modified By : Sushil Kumar on 2nd Jan 2016
        /// Description : Bind ctrlGenericBikeInfo control 
        /// Modified by : Sajal Gupta on 31-01-2017
        /// Description : Binded popular bikes widget.
        /// Modified by : Aditi Srivastava on 31-01-2017
        /// Description : Added popular bikes widget for tagged models.
        /// </summary>
        protected void BindPageWidgets()
        {
           try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                isModelTagged = (taggedModelId > 0);
                if (ctrlPopularBikes != null)
                {
                    ctrlPopularBikes.totalCount = 9;
                    ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                    ctrlPopularBikes.cityName = currentCityArea.City;
                    if (_taggedMakeObj != null)
                    {
                        ctrlPopularBikes.makeId = _taggedMakeObj.MakeId;
                        ctrlPopularBikes.makeName = _taggedMakeObj.MakeName;
                        ctrlPopularBikes.makeMasking = _taggedMakeObj.MaskingName;
                    }
                }

                if (isModelTagged)
                {
                    ctrlGenericBikeInfo.ModelId = taggedModelId;
                    if (ctrlBikesByBodyStyle != null)
                    {
                        ctrlBikesByBodyStyle.ModelId = taggedModelId;
                        ctrlBikesByBodyStyle.topCount = 9;
                        ctrlBikesByBodyStyle.CityId = currentCityArea.CityId;
                    }
                }
                else
                {
                    if (ctrlUpcomingBikes != null)
                    {
                        ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                        ctrlUpcomingBikes.pageSize = 9;
                         if (_taggedMakeObj != null)
                        {
                            ctrlUpcomingBikes.MakeId = _taggedMakeObj.MakeId;
                            ctrlUpcomingBikes.makeMaskingName = _taggedMakeObj.MaskingName;
                            ctrlUpcomingBikes.makeName = _taggedMakeObj.MakeName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : Bikewale.Mobile.Content.newsdetails.BindPageWidgets");
            }
        }
    }
}