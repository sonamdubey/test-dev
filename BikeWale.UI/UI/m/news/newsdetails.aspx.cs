using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using System;
using System.Web;

namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 21 May 2014
    /// Modified By : Ashwini Todkar on  on 30 Sept 2014
    /// Modified By: Aditi Srivastava on 31 Jan 2017
    /// Summary    : Added common view model for fetching data and modified popular widgets
    /// </summary>
    public class newsdetails : System.Web.UI.Page
    {
        protected ArticleDetails objArticle = null;
        protected NewsDetails objNews;
        protected PageMetaTags metas;
        protected GenericBikeInfoControl ctrlGenericBikeInfo;
        protected UpcomingBikesMin ctrlUpcomingBikes;
        protected PopularBikesMin ctrlPopularBikes;
        protected GlobalCityAreaEntity currentCityArea;
        protected uint taggedModelId;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        protected bool isModelTagged;
        private BikeMakeEntityBase _taggedMakeObj;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        //Modified By: Aditi Srivastava on 7 Sep 2016
        //SUmmary: Added request rawURL on form action
        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Action = Request.RawUrl;

            if (!IsPostBack)
            {
                BindNewsDetails();
            }
        }

        /// <summary>
        /// Created By : Aditi Srivastava on 31 Jan 2017
        /// Summary    : Bind news details from view model
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
                            if (objNews.TaggedModel != null)
                                taggedModelId = (uint)objNews.TaggedModel.ModelId;
                            currentCityArea = objNews.CityArea;
                            metas = objNews.PageMetas;
                            BindPageWidgets();
                        }
                        else
                        {
                            Response.Redirect("/m/news/", false);
                            if (HttpContext.Current != null)
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }
                    else
                    {
                        UrlRewrite.Return404();

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Mobile.Content.newsdetails.BindNewsDetails");
            }
            finally
            {
                if (objNews.IsPermanentRedirect)
                {
                    string newUrl = string.Format("/m/news/{0}-{1}.html", objNews.MappedCWId, Request["t"]);
                    Bikewale.Common.CommonOpn.RedirectPermanent(newUrl);
                }
            }
        }


        /// <summary>
        /// Created by : Aditi Srivastava on 16 Nov 2016
        /// Description: bind upcoming and popular bikes
        /// Modified By : Sushil Kumar on 2nd Jan 2016
        /// Description : Bind ctrlGenericBikeInfo control 
        /// Modified by : Sajal Gupta on 31-01-2017
        /// Description : Binded popular bikes widget.
        /// Modified by : Aditi Srivastava on 31-01-2017
        /// Description : Added popular bikes widget for tagged models.
        /// Modified  By :- subodh Jain 10 Feb 2017
        /// Summary :- BikeInfo Slug details
        /// </summary>
        protected void BindPageWidgets()
        {
            try
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
                    if (ctrlGenericBikeInfo != null)
                    {
                        ctrlGenericBikeInfo.ModelId = taggedModelId;
                        ctrlGenericBikeInfo.CityId = currentCityArea.CityId;
                        ctrlGenericBikeInfo.PageId = BikeInfoTabType.News;
                        ctrlGenericBikeInfo.TabCount = 3;
                        ctrlGenericBikeInfo.SmallSlug = true;
                    }
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
                ErrorClass.LogError(ex, "Bikewale.Mobile.Content.newsdetails.BindPageWidgets");
            }
        }
    }
}