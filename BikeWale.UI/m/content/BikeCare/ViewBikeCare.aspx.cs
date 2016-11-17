using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Mobile.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Bikewale.Entities.Location;
using Bikewale.Utility;
namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By:-Subodh Jain 12 Nov 2016
    /// Summary :- Detaile Page for TipsAndAdvice
    /// Modified by : Aditi Srivasatava on 16 Nov 2016
    /// Summary     : Added functions for upcoming and popular bike widgets
    /// </summary>
    public class ViewBikeCare : System.Web.UI.Page
    {
        DetailPageBikeCare objDetailBikeCare;
        private GlobalCityAreaEntity currentCityArea;
        protected MUpcomingBikesMin ctrlUpcomingBikes;
        protected MPopularBikesMin ctrlPopularBikes;
        protected ModelGallery photoGallery;
        protected String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty, modelUrl = String.Empty, pageDescription = String.Empty, pageKeywords = String.Empty;
        protected String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = String.Empty, canonicalUrl = String.Empty;
        protected ArticlePageDetails objTipsAndAdvice;
        public StringBuilder bikeTested;
        protected IEnumerable<ModelImage> objImg = null;
        protected BikeMakeEntityBase _taggedMakeObj = null;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DetailBikeCare();
            GetTaggedBikeList();
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
                if(objImg!=null)
                photoGallery.Photos = objImg.ToList();
                photoGallery.isModelPage = false;
                photoGallery.articleName = pageTitle;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ViewBikeCare.DetailBikeCare");
                objErr.SendMail();
            }


        }
        /// <summary>
        /// Created by : Aditi Srivastava on 16 Nov 2016
        /// Description: Get details of tagged vehicles
        /// </summary>
        private void GetTaggedBikeList()
        {
            if (objTipsAndAdvice != null && objTipsAndAdvice.VehiclTagsList.Count > 0)
            {

                var taggedMakeObj = objTipsAndAdvice.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.MakeBase.MaskingName));
                if (taggedMakeObj != null)
                {
                    _taggedMakeObj = taggedMakeObj.MakeBase;
                }
                else
                {
                    _taggedMakeObj = objTipsAndAdvice.VehiclTagsList.FirstOrDefault().MakeBase;
                    
                }
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 16 Nov 2016
        /// Description: bind upcoming and popular bikes
        /// </summary>
        protected void BindPageWidgets()
        {
            currentCityArea = GlobalCityArea.GetGlobalCityArea();
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

    }
}