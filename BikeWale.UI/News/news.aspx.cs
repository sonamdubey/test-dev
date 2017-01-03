using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
using System;
using System.Web;

namespace Bikewale.News
{
    /// <summary>
    /// Created By : Sushil Kumar on 10th Nov 2016
    /// Description : Bind news details page
    /// Modified By : Aditi Srivastava on 10 Nov 2016
    /// Description : Added control for upcoming bikes widget
    /// </summary>
    public class news : System.Web.UI.Page
    {
        private string _basicId = string.Empty;
        protected ArticleDetails objArticle = null;
        protected NewsDetails objNews;
        protected UpcomingBikesMinNew ctrlUpcomingBikes;
        protected GenericBikeInfoControl ctrlGenericBikeInfo;
        private BikeMakeEntityBase _taggedMakeObj;
        private BikeModelEntityBase _taggedModelObj;
        protected GlobalCityAreaEntity currentCityArea;
        protected PageMetaTags metas;

        protected MostPopularBikesMin ctrlPopularBikes;
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
                        objArticle = objNews.ArticleDetails;
                        _taggedMakeObj = objNews.TaggedMake;
                        _taggedModelObj = objNews.TaggedModel;
                        currentCityArea = objNews.CityArea;
                        metas = objNews.PageMetas;
                        BindPageWidgets();
                    }
                    else if (!objNews.IsContentFound)
                    {
                        Response.Redirect("/news/", false);
                        if (HttpContext.Current != null)
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : Bikewale.News.NewsListing.BindNewsDetails");
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
        /// </summary>
        private void BindPageWidgets()
        {
            ctrlPopularBikes.totalCount = 3;
            ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
            ctrlPopularBikes.cityName = currentCityArea.City;

            ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
            ctrlUpcomingBikes.pageSize = 9;
            ctrlUpcomingBikes.topCount = 3;

            if (_taggedMakeObj != null)
            {
                ctrlPopularBikes.makeId = _taggedMakeObj.MakeId;
                ctrlPopularBikes.makeName = _taggedMakeObj.MakeName;
                ctrlPopularBikes.makeMasking = _taggedMakeObj.MaskingName;
                ctrlUpcomingBikes.makeMaskingName = _taggedMakeObj.MaskingName;
                ctrlUpcomingBikes.MakeId = _taggedMakeObj.MakeId;
                ctrlUpcomingBikes.makeName = _taggedMakeObj.MakeName;

            }
            if (_taggedModelObj != null)
            {
                ctrlGenericBikeInfo.ModelId = (uint)_taggedModelObj.ModelId;
            }

        }

    }
}