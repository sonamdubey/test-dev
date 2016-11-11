﻿using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
using System;
using System.Web;
using System.Linq;
using Bikewale.Entities.BikeData;
using Bikewale.Controls;
using Bikewale.Utility;
using Bikewale.Cache.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.DAL.BikeData;

namespace Bikewale.News
{
    /// <summary>
    /// 
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : Bikewale.News.NewsListing.GetNewsList");
                objErr.SendMail();
            }
        }

        private void BindPageWidgets()
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

            BindUpcoming();

                    ctrlUpcomingBikes.makeMaskingName = objMMV.MaskingName;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 8 Nov 2016
        /// Summary  : Bind upcoming bikes list
        /// </summary>
        /// </summary>
        private void BindUpcoming()
        {
            ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
            ctrlUpcomingBikes.pageSize = 9;
            ctrlUpcomingBikes.MakeId = makeId;
            ctrlUpcomingBikes.topCount = 3;
        }

    }
}