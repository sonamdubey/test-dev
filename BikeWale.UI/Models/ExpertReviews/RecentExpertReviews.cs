using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.CMS;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 23 Mar 2017
    /// Summary    : Model to get list of bike care articles for partial view
    /// </summary>
    public class RecentExpertReviews
    {
        private readonly ICMSCacheContent _articles = null;
        private string Title { get; set; }
        private readonly uint _totalRecords;
        private readonly uint _makeId;
        private readonly string _makeName;
        private readonly string _makeMasking;
        private readonly uint _modelId;
        private readonly string _modelIdList;
        private readonly string _modelName;
        private readonly string _modelMasking;
        public bool IsScooter { get; set; }
        public bool IsViewAllLink { get; set; }

        #region Constructor
        public RecentExpertReviews(uint totalRecords, ICMSCacheContent articles)
        {
            _totalRecords = totalRecords;
            _articles = articles;
        }

        public RecentExpertReviews(uint totalRecords, uint makeId, string makeName, string makeMasking, ICMSCacheContent articles, string title)
        {
            _totalRecords = totalRecords;
            _makeId = makeId;
            _makeName = makeName;
            _makeMasking = makeMasking;
            Title = title;
            _articles = articles;
        }

        public RecentExpertReviews(uint totalRecords, uint makeId, uint modelId, ICMSCacheContent articles)
        {
            _totalRecords = totalRecords;
            _makeId = makeId;
            _modelId = modelId;
            _articles = articles;
        }

        public RecentExpertReviews(uint totalRecords, string modelIdList, ICMSCacheContent articles)
        {
            _totalRecords = totalRecords;
            _modelIdList = modelIdList;
            _articles = articles;
        }

        public RecentExpertReviews(uint totalRecords, uint makeId, uint modelId, string makeName, string makeMasking, string modelName, string modelMasking, ICMSCacheContent articles, string title)
        {
            _totalRecords = totalRecords;
            _makeId = makeId;
            _modelId = modelId;
            _makeName = makeName;
            _makeMasking = makeMasking;
            _modelName = modelName;
            _modelMasking = modelMasking;
            _articles = articles;
            Title = title;
        }

		public RecentExpertReviews(uint totalRecords, uint makeId, string modelIdList, ICMSCacheContent articles)
		{
			_totalRecords = totalRecords;
			_makeId = makeId;
			_modelIdList = modelIdList;
			_articles = articles;
		}
        #endregion

        /// <summary>
        /// Created By : Ashish G. Kamble on 21 Dec 2017
        /// Summary : Function to get the recent comparison tests
        /// </summary>
        /// <returns></returns>
        public RecentExpertReviewsVM GetComparisonTests()
        {
            RecentExpertReviewsVM recentReviews = new RecentExpertReviewsVM();

            try
            {
                recentReviews = CallAPI(Convert.ToString((ushort)EnumCMSContentType.ComparisonTests));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.ExpertReviews.RecentExpertReviews.GetComparisonTests: TotalRecords {0},MakeId {1}, ModelId {2}", _totalRecords, _makeId, _modelId));
            }
            return recentReviews;
        }

        #region Functions to get data
        /// <summary>
        /// Created by : Aditi Srivastava on 23 Mar 2017
        /// Summary    : To get list of expert review articles
        /// Modified by: Vivek Singh Tomar on 18th Aug 2017
        /// Summary: Change the view all reviews url if make is scooter
        /// </summary>
        public RecentExpertReviewsVM GetData()
        {
            RecentExpertReviewsVM recentReviews = new RecentExpertReviewsVM();
            try
            {
                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.RoadTest);
                categorList.Add(EnumCMSContentType.ComparisonTests);

                string _contentType = CommonApiOpn.GetContentTypesString(categorList);

                recentReviews = CallAPI(_contentType);                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.ExpertReviews.RecentExpertReviews.GetData: TotalRecords {0},MakeId {1}, ModelId {2}", _totalRecords, _makeId, _modelId));
            }
            return recentReviews;
        }

        /// <summary>
        /// Created By : Ashish G. Kamble on 21 Dec 2017
        /// Summary : Function to call the api and get data from GRPC service. Logic separted from GetData method
        /// </summary>
        /// <param name="contentType">comma separated category ids</param>
        /// <returns></returns>
        private RecentExpertReviewsVM CallAPI(string contentType)
        {
            RecentExpertReviewsVM recentReviews = new RecentExpertReviewsVM();

            try
            {                
                if (!string.IsNullOrEmpty(_modelIdList))
                {
                    recentReviews.ArticlesList = _articles.GetMostRecentArticlesByIdList(contentType, _totalRecords, _makeId, _modelIdList);
                }
                else if (IsScooter)
                {
                    string bodyStyleId = "5";
                    recentReviews.ArticlesList = _articles.GetMostRecentArticlesByIdList(contentType, _totalRecords, bodyStyleId, _makeId, _modelId);
                }
                else
                {
                    recentReviews.ArticlesList = _articles.GetMostRecentArticlesByIdList(contentType, _totalRecords, _makeId, _modelId);
                }

                if (recentReviews.ArticlesList != null)
                {
                    recentReviews.FetchedCount = recentReviews.ArticlesList.Count();
                }
                if (_makeId > 0)
                {
                    recentReviews.MakeName = _makeName;
                    recentReviews.MakeMasking = _makeMasking;
                }

                if (_modelId > 0)
                {
                    recentReviews.ModelName = _modelName;
                    recentReviews.ModelMasking = _modelMasking;
                }
                if (IsScooter)
                {
                    recentReviews.MoreExpertReviewUrl = UrlFormatter.FormatScootersExpertReviewUrl(_makeMasking);
                }
                else
                {
                    recentReviews.MoreExpertReviewUrl = UrlFormatter.FormatExpertReviewUrl(_makeMasking, _modelMasking);
                }

                if (!String.IsNullOrEmpty(_modelName) && !String.IsNullOrEmpty(_makeName))
                {
                    recentReviews.LinkTitle = string.Format("{0} {1} Expert Reviews", _makeName, _modelName);
                    recentReviews.BikeName = string.Format("{0} {1}", _makeName, _modelName);
                }
                else if (String.IsNullOrEmpty(_modelName) && !String.IsNullOrEmpty(_makeName))
                {
                    recentReviews.LinkTitle = string.Format("{0} Expert Reviews", _makeName);
                }
                else
                {
                    if (IsScooter)
                    {
                        recentReviews.LinkTitle = "Expert Reviews on Scooters";
                    }
                    else
                    {
                        recentReviews.LinkTitle = "Expert Reviews on Bikes";
                    }
                }
                recentReviews.Title = Title;
                recentReviews.IsViewAllLink = IsViewAllLink;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.ExpertReviews.RecentExpertReviews.CallAPI: TotalRecords {0},MakeId {1}, ModelId {2}", _totalRecords, _makeId, _modelId));
            }
            return recentReviews;
        }

		public RecentExpertReviewsVM GetData(List<EnumCMSContentType> categoryList, List<EnumCMSContentSubCategoryType> subCategoryList)
		{
			RecentExpertReviewsVM recentReviews = new RecentExpertReviewsVM();
			try
			{
				uint startIndex = 1, endIndex = _totalRecords;

				string _categoryType = CommonApiOpn.GetContentTypesString(categoryList);
				string _subCategoryIdList = CommonApiOpn.GetContentTypesString(subCategoryList);

				CMSContent cmsContent =  _articles.GetContentListBySubCategoryId(startIndex, endIndex, _categoryType, _subCategoryIdList, (int)_makeId, (int)_modelId);

				if (cmsContent != null)
				{
					recentReviews.ArticlesList = cmsContent.Articles;
				}

				if (recentReviews.ArticlesList != null)
				{
					recentReviews.FetchedCount = recentReviews.ArticlesList.Count();
				}
				if (_makeId > 0)
				{
					recentReviews.MakeName = _makeName;
					recentReviews.MakeMasking = _makeMasking;
				}

				if (_modelId > 0)
				{
					recentReviews.ModelName = _modelName;
					recentReviews.ModelMasking = _modelMasking;
				}
				if (IsScooter)
				{
					recentReviews.MoreExpertReviewUrl = UrlFormatter.FormatScootersExpertReviewUrl(_makeMasking);
				}
				else
				{
					recentReviews.MoreExpertReviewUrl = UrlFormatter.FormatExpertReviewUrl(_makeMasking, _modelMasking);
				}

				if (!String.IsNullOrEmpty(_modelName) && !String.IsNullOrEmpty(_makeName))
				{
					recentReviews.LinkTitle = string.Format("{0} {1} Expert Reviews", _makeName, _modelName);
					recentReviews.BikeName = string.Format("{0} {1}", _makeName, _modelName);
				}
				else if (String.IsNullOrEmpty(_modelName) && !String.IsNullOrEmpty(_makeName))
				{
					recentReviews.LinkTitle = string.Format("{0} Expert Reviews", _makeName);
				}
				else
				{
					if (IsScooter)
					{
						recentReviews.LinkTitle = "Expert Reviews on Scooters";
					}
					else
					{
						recentReviews.LinkTitle = "Expert Reviews on Bikes";
					}
				}
				recentReviews.Title = Title;
				recentReviews.IsViewAllLink = IsViewAllLink;
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, string.Format("Bikewale.Models.ExpertReviews.RecentExpertReviews.GetData: TotalRecords {0},MakeId {1}, ModelId {2}", _totalRecords, _makeId, _modelId));
			}
			return recentReviews;
		}
		#endregion
	}
}