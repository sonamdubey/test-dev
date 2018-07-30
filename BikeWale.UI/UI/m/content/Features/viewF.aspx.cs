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
        protected PopularBikesMin ctrlPopularBikes;
        protected ModelGallery photoGallery;
        protected HtmlSelect ddlPages;
        protected String baseUrl = String.Empty, pageTitle = String.Empty, ampUrl = string.Empty;
        protected String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = string.Empty, url = string.Empty;
        protected Repeater rptPhotos;
        protected ArticlePageDetails objFeature = null;
        protected FeaturesDetails objArticle;
        private BikeMakeEntityBase _taggedMakeObj;
        protected GlobalCityAreaEntity currentCityArea;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        protected uint taggedModelId;
        protected bool isModelTagged;
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

                if (!objArticle.IsPermanentRedirect && !objArticle.IsPageNotFound)
                {
                    objArticle.GetFeatureDetails();

                    if (objArticle.IsContentFound)
                    {
                        objFeature = objArticle.objFeature;
                        if (objArticle.taggedMakeObj != null)
                            _taggedMakeObj = objArticle.taggedMakeObj;
                        if (objArticle.taggedModelObj != null)
                        {
                            taggedModelId = (uint)objArticle.taggedModelObj.ModelId;
                        }
                        GetFeatureData();
                        BindPages();
                        BindPageWidgets();
                        objImg = objArticle.objImg;

                        if (objImg != null && objImg.Any())
                            BindGallery();
                    }
                    else
                    {
                        Response.Redirect("/m/features/", false);
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
                ErrorClass.LogError(ex, "Bikewale.Mobile.Content.viewF.BindFeaturesDetails");
            }
            finally
            {
                if (objArticle.IsPermanentRedirect)
                {
                    string newUrl = string.Format("/m/features/{0}-{1}/", Request["t"], objArticle.MappedCWId);
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
                photoGallery.Photos = objImg.ToList();
                photoGallery.isModelPage = false;
                photoGallery.articleName = pageTitle;
                rptPhotos.DataSource = objImg;
                rptPhotos.DataBind();
        }

        /// <summary>
        /// Summary : Assign data to page variables
        /// </summary>
        private void GetFeatureData()
        {
            baseUrl = String.Format("/m/features/{0}-{1}/", objFeature.ArticleUrl, objArticle.BasicId);
            //  desktop url for facebook
            url = String.Format("/features/{0}-{1}/", objFeature.ArticleUrl, objArticle.BasicId);
            ampUrl = string.Format("{0}/m/features/{1}-{2}/amp/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objFeature.ArticleUrl, objArticle.BasicId);
            author = objFeature.AuthorName;
            pageTitle = objFeature.Title;
            displayDate = Convert.ToDateTime(objFeature.DisplayDate).ToString("dd MMMM yyyy, hh:mm tt");
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
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Mobile.Content.viewF.BindPageWidgets");
            }
        }
    }
}