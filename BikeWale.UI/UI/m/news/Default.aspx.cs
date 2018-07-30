using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Location;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace Bikewale.Mobile.News
{
    /// <summary>
    /// Created By : Ashwini Todkar on 21 May 2014
    /// Modified By : Ashwini Todkar on 1 Oct 2014
    /// Modified by : Aditi Srivastava on 18 Nov 2016
    /// Summary     : Replaced drop down page numbers with Link pagination
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected int curPageNo = 1, totalPages = 0;
        protected string prevPageUrl = String.Empty, nextPageUrl = String.Empty;
        protected LinkPagerControl ctrlPager;
        private const int _pageSize = 10, _pagerSlotSize = 5;
        protected int startIndex = 0, endIndex = 0, totalrecords;
        HttpRequest page = HttpContext.Current.Request;
        protected NewsListing objNews = null;
        protected IEnumerable<ArticleSummary> newsArticles = null;
        protected UpcomingBikesMin ctrlUpcomingBikes;
        protected PopularBikesMin ctrlPopularBikes;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        protected string makeName = string.Empty, makeMaskingName = string.Empty;
        protected uint makeId,modelId;
        private GlobalCityAreaEntity currentCityArea;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified By : Sushil Kumar on 26th July 2016
        /// Description : Added Features,expert reviews and autoexpo categories for multiple categories 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetNewsList();
                BindWidgets();
            }
        }

        /// <summary>
        /// Created by : Sajal Gupta on 27-01-2017
        /// Description : Binded upcoming and popular bikes widget.
        /// Modified By : Aditi Srivastava on 2 Feb 2017
        /// Summary     : Modified entire widget binding logic
        /// </summary>
        protected void BindWidgets()
        {
            try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (ctrlPopularBikes != null)
                {
                    ctrlPopularBikes.totalCount = 9;
                    ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                    ctrlPopularBikes.cityName = currentCityArea.City;
                    if (makeId > 0)
                    {
                        ctrlPopularBikes.makeId = Convert.ToInt32(makeId);
                        ctrlPopularBikes.makeName = makeName;
                        ctrlPopularBikes.makeMasking = makeMaskingName;
                    }
                }
                if (modelId > 0)
                {
                    if (ctrlBikesByBodyStyle != null)
                    {
                        ctrlBikesByBodyStyle.ModelId = modelId;
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
                        if (makeId > 0)
                        {
                            ctrlUpcomingBikes.MakeId = Convert.ToInt32(makeId);
                            ctrlUpcomingBikes.makeName = makeName;
                            ctrlUpcomingBikes.makeMaskingName = makeMaskingName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Mobile.News.Default.BindWidgets");
            }
        }

        private void GetNewsList()
        {

            try
            {
                objNews = new NewsListing();
                if (objNews.objMake != null)
                {
                    makeId = (uint)objNews.objMake.MakeId;
                    makeName = objNews.objMake.MakeName;
                    makeMaskingName = objNews.objMake.MaskingName;
                }
                if (objNews.objModel != null)
                {
                    modelId = (uint)objNews.objModel.ModelId;
                }
                objNews.FetchNewsList(ctrlPager, true);
                newsArticles = objNews.objNewsList;
                startIndex = objNews.StartIndex;
                endIndex = objNews.EndIndex;
                totalrecords = (int)objNews.TotalArticles;

            }
            catch (Exception ex)
            {
               ErrorClass.LogError(ex, "Bikewale.Mobile.News.Default.GetNewsList");
            }
            finally
            {
                if (objNews != null && objNews.IsPageNotFound)
                {
                    UrlRewrite.Return404();

                }
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 26th July 2016
        ///  Description  : Function to show category type based on categoryId
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        protected string GetContentCategory(string contentType)
        {
            string _category = string.Empty;
            EnumCMSContentType _contentType = default(EnumCMSContentType);
            try
            {
                if (!string.IsNullOrEmpty(contentType) && Enum.TryParse<EnumCMSContentType>(contentType, true, out _contentType))
                {
                    switch (_contentType)
                    {
                        case EnumCMSContentType.AutoExpo2016:
                        case EnumCMSContentType.News:
                            _category = "NEWS";
                            break;
                        case EnumCMSContentType.Features:
                        case EnumCMSContentType.SpecialFeature:
                            _category = "FEATURES";
                            break;
                        case EnumCMSContentType.ComparisonTests:
                        case EnumCMSContentType.RoadTest:
                            _category = "EXPERT REVIEWS";
                            break;
                        case EnumCMSContentType.TipsAndAdvices:
                            _category = "Bike Care";
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
               ErrorClass.LogError(ex, "Bikewale.Mobile.News.Default.GetContentCategory");
            }
            return _category;
        }
    }
}