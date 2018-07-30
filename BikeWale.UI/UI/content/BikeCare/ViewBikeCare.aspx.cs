using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Location;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace Bikewale.Content
{
    public class ViewBikeCare : System.Web.UI.Page
    {
        /// <summary>
        /// Created By:-Subodh Jain 12 Nov 2016
        /// Summary :- Detaile Page for TipsAndAdvice
        /// Modified by : Aditi Srivastava on 3 Feb 2017
        /// Summary     : Added widget for body style when model is tagged
        /// </summary>
        DetailPageBikeCare objDetailBikeCare;

        protected String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty, modelUrl = String.Empty, pageDescription = String.Empty, pageKeywords = String.Empty;
        protected String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = String.Empty, canonicalUrl = String.Empty;
        protected ArticlePageDetails objTipsAndAdvice;
        public StringBuilder bikeTested;
        protected IEnumerable<ModelImage> objImg = null;
        protected UpcomingBikesMinNew ctrlUpcoming;
        protected MostPopularBikesMin ctrlPopularBikes;
        protected ModelGallery ctrlModelGallery;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        private BikeMakeEntityBase _taggedMakeObj = null;
        private uint taggedModelId;
        protected bool isModelTagged;
        protected ArticlePhotoGallery ctrPhotoGallery;
        public uint basicId;
        HttpContext page = HttpContext.Current;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();
            DetailBikeCare();
            BindPageWidgets();
        }
        /// <summary>
        /// Created By:-Subodh Jain 12 Nov 2016
        /// Summary :- Detaile Page for TipsAndAdvice
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
                page.Response.Redirect("/bike-care/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }


        }
        /// Created By:- Subodh jain 15 Nov 2016
        /// Summary :- Bike Care Landing page Binding for widgets
        /// Modified by : Aditi Srivastava on 3 Feb 2017
        /// Summary     : Added widget for body style when model is tagged
        /// </summary>
        private void BindPageWidgets()
        {

            try
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (objImg != null && objImg.Any())
                {
                    ctrPhotoGallery.BasicId = Convert.ToInt32(basicId);
                    ctrPhotoGallery.ModelImageList = objImg;
                    ctrPhotoGallery.BindPhotos();

                    ctrlModelGallery.bikeName = objTipsAndAdvice.Title;
                    ctrlModelGallery.Photos = objImg.ToList();
                }
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
                    if (ctrlBikesByBodyStyle != null)
                    {
                        ctrlBikesByBodyStyle.ModelId = taggedModelId;
                        ctrlBikesByBodyStyle.topCount = 3;
                        ctrlBikesByBodyStyle.CityId = currentCityArea.CityId;
                    }
                }
                else
                {
                    if (ctrlUpcoming != null)
                    {
                        ctrlUpcoming.sortBy = (int)EnumUpcomingBikesFilter.Default;
                        ctrlUpcoming.pageSize = 9;
                        ctrlUpcoming.topCount = 3;
                        if (_taggedMakeObj != null)
                        {
                            ctrlUpcoming.MakeId = _taggedMakeObj.MakeId;
                            ctrlUpcoming.makeMaskingName = _taggedMakeObj.MaskingName;
                            ctrlUpcoming.makeName = _taggedMakeObj.MakeName;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Content.BikeCare.BindPageWidgets");
                
            }
        }
    }
}