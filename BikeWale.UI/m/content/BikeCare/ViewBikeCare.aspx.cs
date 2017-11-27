using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By:-Subodh Jain 12 Nov 2016
    /// Summary :- Detaile Page for TipsAndAdvice
    /// Modified by : Aditi Srivasatava on 16 Nov 2016
    /// Summary     : Added functions for upcoming and popular bike widgets
    /// Modified by : Aditi Srivastava on 3 feb 2017
    /// Summary     : Changed widget logic for tagged bikes
    /// </summary>
    public class ViewBikeCare : System.Web.UI.Page
    {
        DetailPageBikeCare objDetailBikeCare;
        private GlobalCityAreaEntity currentCityArea;
        protected UpcomingBikesMin ctrlUpcomingBikes;
        protected PopularBikesMin ctrlPopularBikes;
        protected ModelGallery photoGallery;
        protected String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty, modelUrl = String.Empty, pageDescription = String.Empty, pageKeywords = String.Empty;
        protected String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = String.Empty, canonicalUrl = String.Empty;
        protected ArticlePageDetails objTipsAndAdvice;
        public StringBuilder bikeTested;
        protected IEnumerable<ModelImage> objImg = null;
        protected BikeMakeEntityBase _taggedMakeObj = null;
        protected uint taggedModelId;
        protected bool isModelTagged;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        public uint basicId;
        HttpContext page = HttpContext.Current;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DetailBikeCare();
            BindPageWidgets();
        }
        /// <summary>
        /// Created By:-Subodh Jain 12 Nov 2016
        /// Summary :- Detaile Page for TipsAndAdvice
        /// Modified by : Aditi Srivastava on 17 Nov 2016
        /// Summary  :   Added model gallery control
        /// </summary>
        private void DetailBikeCare()
        {
            objDetailBikeCare = new DetailPageBikeCare();
            if (objDetailBikeCare != null && !objDetailBikeCare.pageNotFound)
            {


                try
                {
                    objTipsAndAdvice = objDetailBikeCare.objTipsAndAdvice;
                    objImg = objDetailBikeCare.objImg;
                    pageTitle = objDetailBikeCare.title;
                    pageDescription = objDetailBikeCare.description;
                    pageKeywords = objDetailBikeCare.keywords;
                    displayDate = objDetailBikeCare.displayDate;
                    bikeTested = objDetailBikeCare.bikeTested;
                    canonicalUrl = objDetailBikeCare.canonicalUrl;
                    basicId = objDetailBikeCare.BasicId;
                    if (objDetailBikeCare.taggedMakeObj != null)
                    {
                        _taggedMakeObj = objDetailBikeCare.taggedMakeObj;
                    }
                    if (objDetailBikeCare.taggedModelObj != null)
                        taggedModelId = (uint)objDetailBikeCare.taggedModelObj.ModelId;

                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "ViewBikeCare.DetailBikeCare");
                    
                }
            }
            else
            {
                page.Response.Redirect("/m/bike-care/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

        }
        /// <summary>
        /// Created by : Aditi Srivastava on 16 Nov 2016
        /// Description: bind upcoming and popular bikes
        /// Modified by : Aditi Srivastava on 3 feb 2017
        /// Summary     : Changed widget logic for tagged bikes
        /// </summary>
        protected void BindPageWidgets()
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
            if (objImg != null && objImg.Any())
            {
                photoGallery.Photos = objImg.ToList();
                photoGallery.isModelPage = false;
                photoGallery.articleName = pageTitle;
            }
        }

    }
}