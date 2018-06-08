using Bikewale.BAL.EditCMS;
using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Memcache;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Content
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 24 Sept 2014
    /// Modified By : Aditi Srivastava on 10 Nov 2016
    /// Description : Added control for upcoming bikes widget
    /// </summary>
    /// </summary>
    public class ViewF : System.Web.UI.Page
    {
        protected Repeater rptPages, rptPageContent;
        protected ArticlePhotoGallery ctrPhotoGallery;
        protected UpcomingBikesMinNew ctrlUpcomingBikes;
        protected HtmlGenericControl topNav, bottomNav;
        protected string canonicalUrl = String.Empty;
        protected bool ShowGallery = false, IsPhotoGalleryPage = false;
        protected MostPopularBikesMin ctrlPopularBikes;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        protected IEnumerable<ModelImage> objImg = null;
        private BikeMakeEntityBase _taggedMakeObj;
        protected uint taggedModelId;
        protected int makeId;
        protected ModelGallery ctrlModelGallery;
        private GlobalCityAreaEntity currentCityArea;
        protected string articleUrl = string.Empty, articleTitle = string.Empty, authorName = string.Empty, displayDate = string.Empty, ampUrl = string.Empty;
        protected FeaturesDetails objArticle;
        protected ArticlePageDetails objFeature = null;
        protected bool isModelTagged;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified by : Sushil Kumar on 16th Nov 2016
        /// Description : Handle page redirection 
        /// Modified By:  Aditi Srivastava on 30 Jan 2017
        /// Summary    :  Added common view model for fetching data and modified popular widgets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Load(object sender, EventArgs e)
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
            BindFeaturesDetails();
        }



        /// <summary>
        /// Created by : Aditi Srivastava on 30 Jan 2017
        /// Summary    : Bind feature details on page
        /// </summary>
        private void BindFeaturesDetails()
        {
            try
            {
                objArticle = new FeaturesDetails();
                if (!objArticle.IsPermanentRedirect && !objArticle.IsPageNotFound)
                {
                    objArticle.GetFeatureDetails();
                    if (objArticle.IsContentFound)
                    {
                        objFeature = objArticle.objFeature;
                        _taggedMakeObj = objArticle.taggedMakeObj;
                        if (objArticle.taggedModelObj != null)
                            taggedModelId = (uint)objArticle.taggedModelObj.ModelId;
                        GetFeatureData();
                        BindPages();
                        BindPageWidgets();

                        objImg = objArticle.objImg;
                        if (objImg != null && objImg.Any())
                            BindGallery();
                    }
                    else
                    {
                        Response.Redirect("/features/", false);
                        if (HttpContext.Current != null)
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }

                }
                else if (objArticle.IsPageNotFound)
                {
                    UrlRewrite.Return404();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Content.ViewF.BindFeaturesDetails");
            }
            finally
            {
                if (objArticle.IsPermanentRedirect)
                {
                    string newUrl = string.Format("/features/{0}-{1}/", Request["t"], objArticle.MappedCWId);
                    Bikewale.Common.CommonOpn.RedirectPermanent(newUrl);
                }
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 3 Feb 2017
        /// Summary    : Bind image gallery
        /// </summary>
        private void BindGallery()
        {
            ctrPhotoGallery.BasicId = (int)objArticle.BasicId;
            ctrPhotoGallery.ModelImageList = objImg;
            ctrPhotoGallery.BindPhotos();
            ctrlModelGallery.bikeName = objFeature.Title;
            ctrlModelGallery.Photos = objImg.ToList();
        }

        /// <summary>
        /// Summary : Assign feature data to page variables
        /// </summary>
        private void GetFeatureData()
        {
            articleTitle = objFeature.Title;
            authorName = objFeature.AuthorName;
            displayDate = objFeature.DisplayDate.ToString();
            articleUrl = objFeature.ArticleUrl;
            canonicalUrl = string.Format("/features/{0}-{1}/", objFeature.ArticleUrl, objArticle.BasicId);
            ampUrl = string.Format("{0}/m/features/{1}-{2}/amp/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objFeature.ArticleUrl, objArticle.BasicId);
        }

        private void BindPages()
        {
            rptPages.DataSource = objFeature.PageList;
            rptPages.DataBind();

            rptPageContent.DataSource = objFeature.PageList;
            rptPageContent.DataBind();
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 8 Nov 2016
        /// Summary  : Bind popular and upcoming bikes list
        /// </summary>
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
                ErrorClass.LogError(ex, "Bikewale.Content.ViewF.BindPageWidgets");

            }
        }
    }
}