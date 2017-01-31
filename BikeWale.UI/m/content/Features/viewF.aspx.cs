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
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Memcache;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 22 May 2014
    /// Modified By: Aditi Srivastava on 30 Jan 2017
    /// Summary    : Added common view model for fetching data and modified popular widgets
    /// </summary>
    public class viewF : System.Web.UI.Page
    {
        protected Repeater rptPageContent;
        protected UpcomingBikesMin ctrlUpcomingBikes;
        protected PopularBikesMin ctrlPopularBikes, ctrlPopularMakeBikes, ctrlPopularBikesModelTagged;
        protected ModelGallery photoGallery;
        protected HtmlSelect ddlPages;
        protected int BasicId = 0, pageId = 1;
        protected String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty, modelUrl = String.Empty;
        protected String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = string.Empty, url = string.Empty;
        protected Repeater rptPhotos;
        protected ArticlePageDetails objFeature = null;
        protected FeaturesDetails objArticle;
        private BikeMakeEntityBase _taggedMakeObj;
        private BikeModelEntityBase _taggedModelObj;
        protected GlobalCityAreaEntity currentCityArea;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        protected uint taggedModelId;
        protected EnumBikeBodyStyles bodyStyle;
        protected bool showBodyStyleWidget, isModelTagged;
        private bool _isContentFount = true;
        protected IEnumerable<ModelImage> objImg = null;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            if (!IsPostBack)
            {
                BindFeaturesDetails();
            }
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
                if (!objArticle.IsPermanentRedirect)
                {
                    if (!objArticle.IsPageNotFound)
                    {
                        objFeature = objArticle.objFeature;
                        _taggedMakeObj = objArticle.taggedMakeObj;
                        _taggedModelObj = objArticle.taggedModelObj;
                        taggedModelId = (uint)_taggedModelObj.ModelId;
                        GetFeatureData();
                        BindPages();
                        bodyStyle = objArticle.BodyStyle;
                        BindPageWidgets();
                        objImg = objArticle.objImg;

                        if (objImg != null && objImg.Count() > 0)
                        {
                            photoGallery.Photos = objImg.ToList();
                            photoGallery.isModelPage = false;
                            photoGallery.articleName = pageTitle;
                            rptPhotos.DataSource = objImg;
                            rptPhotos.DataBind();
                        }
                    }
                    else if (!objArticle.IsContentFound)
                    {
                        Response.Redirect("/m/features/", false);
                        if (HttpContext.Current != null)
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
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
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
            }
        }
        private void GetFeatureData()
        {


            baseUrl = String.Format("/m/features/{0}-{1}/", objFeature.ArticleUrl, Convert.ToString(objArticle.BasicId));
            //  desktop url for facebook
            url = String.Format("/features/{0}-{1}/", objFeature.ArticleUrl, Convert.ToString(objArticle.BasicId));

            author = objFeature.AuthorName;
            pageTitle = objFeature.Title;
            displayDate = Convert.ToDateTime(objFeature.DisplayDate).ToString("MMMM dd, yyyy hh:mm tt");

            if (_taggedModelObj != null)
            {
                modelName = _taggedModelObj.ModelName;
                modelUrl = String.Format("/m/{0}-bikes/{1}/", _taggedMakeObj.MaskingName, _taggedModelObj.MaskingName);

            }

        }

        private void BindPages()
        {
            if (objFeature.PageList != null)
            {
                rptPageContent.DataSource = objFeature.PageList;
                rptPageContent.DataBind();
            }
        }

        protected string GetImageUrl(string hostUrl, string imagePath)
        {
            string imgUrl = String.Empty;
            imgUrl = Bikewale.Common.ImagingFunctions.GetPathToShowImages(imagePath, hostUrl);

            return imgUrl;
        }

        protected string GetImageUrl(string hostUrl, string imagePath, string size)
        {
            string imgUrl = String.Empty;
            imgUrl = Bikewale.Utility.Image.GetPathToShowImages(imagePath, hostUrl, size);

            return imgUrl;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 16 Nov 2016
        /// Description: Bind upcoming and popular bikes
        /// </summary>
        protected void BindPageWidgets()
        {
            currentCityArea = GlobalCityArea.GetGlobalCityArea();
            try
            {
                if (taggedModelId > 0)
                {
                    isModelTagged = true;
                    if (ctrlPopularMakeBikes != null)
                    {
                        ctrlPopularMakeBikes.totalCount = 9;
                        ctrlPopularMakeBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                        ctrlPopularMakeBikes.cityName = currentCityArea.City;
                        if (_taggedMakeObj != null)
                        {
                            ctrlPopularMakeBikes.makeId = _taggedMakeObj.MakeId;
                            ctrlPopularMakeBikes.makeName = _taggedMakeObj.MakeName;
                            ctrlPopularMakeBikes.makeMasking = _taggedMakeObj.MaskingName;
                        }
                    }
                    showBodyStyleWidget = (bodyStyle == EnumBikeBodyStyles.Scooter || bodyStyle == EnumBikeBodyStyles.Cruiser || bodyStyle == EnumBikeBodyStyles.Sports);
                    if (showBodyStyleWidget && ctrlBikesByBodyStyle != null)
                    {
                        ctrlBikesByBodyStyle.ModelId = taggedModelId;
                        ctrlBikesByBodyStyle.topCount = 9;
                        ctrlBikesByBodyStyle.CityId = currentCityArea.CityId;
                    }
                    else
                    {
                        if (ctrlPopularBikesModelTagged != null)
                        {
                            ctrlPopularBikesModelTagged.totalCount = 9;
                            ctrlPopularBikesModelTagged.CityId = Convert.ToInt32(currentCityArea.CityId);
                            ctrlPopularBikesModelTagged.cityName = currentCityArea.City;
                        }
                    }
                }
                else
                {
                    isModelTagged = false;
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
                ErrorClass objErr = new ErrorClass(ex, "viewRT.BindPageWidgets");

            }
        }
    }
}