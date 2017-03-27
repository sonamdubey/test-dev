using Bikewale.Entities.CMS;
using Bikewale.Interfaces.CMS;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models.ExpertReviews
{
    /// <summary>
    /// Created by : Aditi Srivastava on 23 Mar 2017
    /// Summary    : Model to get list of bike care articles for partial view
    /// </summary>
    public class RecentExpertReviews
    {
        private readonly ICMSCacheContent _articles = null;

        #region Constructor
        public RecentExpertReviews(ICMSCacheContent articles)
        {
            _articles = articles;
        }
        #endregion

        #region Functions to get data
        /// <summary>
        /// Created by : Aditi Srivastava on 23 Mar 2017
        /// Summary    : To get list of expert review articles
        /// </summary>
        public RecentExpertReviewsVM GetData(uint totalRecords, uint makeId, uint modelId, string makeName, string makeMasking, string modelName, string modelMasking)
        {
            RecentExpertReviewsVM recentReviews = new RecentExpertReviewsVM();

            try
            {
                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.RoadTest);
                categorList.Add(EnumCMSContentType.ComparisonTests);
                string _contentType = CommonApiOpn.GetContentTypesString(categorList);
               
                recentReviews.ArticlesList = _articles.GetMostRecentArticlesByIdList(_contentType, Convert.ToUInt32(totalRecords), makeId, modelId);
                if (makeId > 0)
                {
                    recentReviews.MakeName = makeName;
                    recentReviews.MakeMasking = makeMasking;
                }

                if (modelId > 0)
                {
                    recentReviews.ModelName = modelName;
                    recentReviews.ModelMasking = modelMasking;
                }
                recentReviews.MoreExpertReviewUrl = UrlFormatter.FormatExpertReviewUrl(makeMasking, modelMasking);


                if (!String.IsNullOrEmpty(modelName) && !String.IsNullOrEmpty(makeName))
                {
                    recentReviews.LinkTitle = string.Format("{0} {1} Expert Reviews", makeName, modelName);
                    recentReviews.BikeName = string.Format("{0} {1}",makeName,modelName);
                }
                else if (String.IsNullOrEmpty(modelName) && !String.IsNullOrEmpty(makeName))
                {
                    recentReviews.LinkTitle = string.Format("{0} Expert Reviews", makeName);
                }
                else
                {
                    recentReviews.LinkTitle = "Expert Reviews on Bikes";
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.ExpertReviews.RecentExpertReviews.GetData: TotalRecords {0},MakeId {1}, ModelId {2}", totalRecords, makeId, modelId));
            }
            return recentReviews;
        }
        #endregion
    }
}