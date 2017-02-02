using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
using Bikewale.Utility;
using System;
using System.Web;

namespace Bikewale.News
{
    /// <summary>
    /// Created By : Sushil Kumar on 10th Nov 2016
    /// Description : Bind news details page
    /// Modified By : Aditi Srivastava on 10 Nov 2016
    /// Description : Added control for upcoming bikes widget
    /// Modified By: Aditi Srivastava on 31 Jan 2017
    /// Summary    : Added common view model for fetching data and modified popular widgets
    /// </summary>
    public class news : System.Web.UI.Page
    {
        private string _basicId = string.Empty;
        protected ArticleDetails objArticle = null;
        protected NewsDetails objNews;
        protected UpcomingBikesMinNew ctrlUpcomingBikes;
        protected GenericBikeInfoControl ctrlGenericBikeInfo;
        protected BikeMakeEntityBase _taggedMakeObj;
        protected BikeModelEntityBase _taggedModelObj;
        protected GlobalCityAreaEntity currentCityArea;
        protected PageMetaTags metas;
        protected uint taggedModelId;
        protected MostPopularBikesMin ctrlPopularBikes;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        protected bool isModelTagged;
        protected int makeId;
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

            Form.Action = Request.RawUrl;

            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();


            BindNewsDetails();

        }

        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Bind news details page
        /// Modified by : Sushil Kumar on 16th Nov 2016
        /// Description : Handle page redirection 
        /// Modified By : Sushil Kumar on 2nd Jan 2016
        /// Description : Get tagged model for article 
        /// Modified By : Sajal Gupta on 27 jan 2017
        /// Description : Save tagged model id.
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
                            Response.Redirect("/news/", false);
                            if (HttpContext.Current != null)
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }
                   
                    else
                    {
                        Response.Redirect("/pagenotfound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : Bikewale.News.BindNewsDetails");
                objErr.SendMail();
            }
            finally
            {
                if (objNews.IsPermanentRedirect)
                {
                    string newUrl = string.Format("/news/{0}-{1}.html", objNews.MappedCWId, Request["t"]);
                    Bikewale.Common.CommonOpn.RedirectPermanent(newUrl);
                }
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Bind page level widgets
        /// Modified By : Sushil Kumar on 2nd Jan 2016
        /// Description : Bind ctrlGenericBikeInfo control 
        /// Modified By : Aditi Srivastava on 31 Jan 2017
        /// Summary     : Modified entire widget binding logic
        /// </summary>
        private void BindPageWidgets()
        {
           try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                isModelTagged = (taggedModelId > 0);
                if (ctrlPopularBikes != null)
                {
                    ctrlPopularBikes.totalCount = 3;
                    ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                    ctrlPopularBikes.cityName = currentCityArea.City;
                    if (_taggedMakeObj != null)
                    {
                        ctrlPopularBikes.MakeId = _taggedMakeObj.MakeId;
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
                        ctrlBikesByBodyStyle.topCount = 3;
                        ctrlBikesByBodyStyle.CityId = currentCityArea.CityId;
                    }
                }
                else
                {                  
                    if (ctrlUpcomingBikes != null)
                    {
                        ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                        ctrlUpcomingBikes.pageSize = 9;
                        ctrlUpcomingBikes.topCount = 3;
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : Bikewale.News.BindPageWidgets");

            }

        }

    }
}