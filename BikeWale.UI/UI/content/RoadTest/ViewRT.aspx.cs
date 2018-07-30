using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Content
{
    /// <summary>
    /// Modified By : Aditi Srivastava on 10 Nov 2016
    /// Description : Added control for upcoming bikes widget
    /// </summary>
    public class ViewRT : System.Web.UI.Page
    {
        protected UpcomingBikesMinNew ctrlUpcomingBikes;
        protected Repeater rptPages, rptPageContent;
        protected string basicId = string.Empty;
        protected ArticlePageDetails objRoadtest;
        protected RoadTestDetails objArticle;
        protected IEnumerable<ModelImage> objImg = null;
        protected StringBuilder _bikeTested;
        protected ArticlePhotoGallery ctrPhotoGallery;
        protected ModelGallery ctrlModelGallery;
        protected MostPopularBikesMin ctrlPopularBikes;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        private GlobalCityAreaEntity currentCityArea;
        private BikeMakeEntityBase _taggedMakeObj;
        protected string articleUrl = string.Empty, articleTitle = string.Empty, authorName = string.Empty, displayDate = string.Empty, ampUrl = string.Empty;
        protected int makeId;
        protected uint taggedModelId;
        protected bool isModelTagged;
        protected MinGenericBikeInfoControl ctrlMinGenericBikeInfo;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }


        /// <summary>
        /// Modified by : Sushil Kumar on 16th Nov 2016
        /// Description : Handle page redirection 
        /// Modified by : Aditi Srivastava on 30 Jan 2017
        /// Summary     : Moved code to common view model. Added baseUrl
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
            BindRoadTestDetails();
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 30 Jan 2017
        /// Summary    : Bind road test details on page
        /// </summary>
        private void BindRoadTestDetails()
        {
            try
            {
                objArticle = new RoadTestDetails();
                if (!objArticle.IsPermanentRedirect && !objArticle.IsPageNotFound)
                {
                    objArticle.GetRoadTestDetails();
                    if (objArticle.IsContentFound)
                    {
                        objRoadtest = objArticle.objRoadtest;
                        BindPages();
                        GetRoadtestData();
                        _taggedMakeObj = objArticle.taggedMakeObj;
                        if (objArticle.taggedModelObj != null)
                            taggedModelId = (uint)objArticle.taggedModelObj.ModelId;
                        objImg = objArticle.objImg;
                        if (objImg != null && objImg.Any())
                            BindGallery();
                        BindPageWidgets();
                    }
                    else
                    {
                        Response.Redirect("/expert-reviews/", false);
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
                ErrorClass.LogError(ex, "Bikewale.Content.ViewRT.BindRoadTestDetails");
            }
            finally
            {
                if (objArticle.IsPermanentRedirect)
                {
                    string newUrl = string.Format("/expert-reviews/{0}-{1}.html", Request["t"], objArticle.MappedCWId);
                    Bikewale.Common.CommonOpn.RedirectPermanent(newUrl);
                }
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : To bind model gallery
        /// Modified by : Aditi Srivastava on 3 Feb 2017
        /// Summary     : Removed unity resolution as it is already resolved in view model
        /// </summary>
        private void BindGallery()
        {
            ctrPhotoGallery.BasicId = Convert.ToInt32(basicId);
            ctrPhotoGallery.ModelImageList = objImg;
            ctrPhotoGallery.BindPhotos();
            ctrlModelGallery.bikeName = articleTitle;
            ctrlModelGallery.Photos = objImg.ToList();
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 8 Nov 2016
        /// Summary  :   Bind upcoming bikes list
        /// Modified By: Aditi Srivastava on 31 Jan 2017
        /// Summary    : Modified widget logic for tagged bikes
        ///  Modified  By :- Sajal Gupta on 13 Feb 2017
        /// Summary :- BikeInfo Slug details
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
                    if (ctrlMinGenericBikeInfo != null)
                    {
                        ctrlMinGenericBikeInfo.ModelId = taggedModelId;
                        ctrlMinGenericBikeInfo.CityId = currentCityArea.CityId;
                        ctrlMinGenericBikeInfo.PageId = BikeInfoTabType.ExpertReview;
                        ctrlMinGenericBikeInfo.TabCount = 3;
                    }

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
                ErrorClass.LogError(ex, "Bikewale.Content.ViewRT.BindPageWidgets");

            }
        }

        private void BindPages()
        {
            rptPages.DataSource = objRoadtest.PageList;
            rptPages.DataBind();

            rptPageContent.DataSource = objRoadtest.PageList;
            rptPageContent.DataBind();
        }


        private void GetRoadtestData()
        {
            articleTitle = objRoadtest.Title;
            authorName = objRoadtest.AuthorName;
            displayDate = objRoadtest.DisplayDate.ToString();
            articleUrl = objRoadtest.ArticleUrl;
            basicId = objRoadtest.BasicId.ToString();
            ampUrl = string.Format("{0}/m/expert-reviews/{1}-{2}/amp/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objRoadtest.ArticleUrl, basicId);
        }

    }
}