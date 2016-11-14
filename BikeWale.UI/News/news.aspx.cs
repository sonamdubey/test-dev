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
        private BikeMakeEntityBase _taggedMakeObj;
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
        /// </summary>
        private void BindNewsDetails()
        {
            try
            {
                objNews = new NewsDetails();
                if (!objNews.IsPageNotFound)
                {
                    objArticle = objNews.ArticleDetails;
                    _taggedMakeObj = objNews.TaggedMake;
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
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : Bikewale.News.NewsListing.BindNewsDetails");
                objErr.SendMail();
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Bind page level widgets
        /// </summary>
        private void BindPageWidgets()
        {
            if (ctrlPopularBikes != null)
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
            }

        }

    }
}