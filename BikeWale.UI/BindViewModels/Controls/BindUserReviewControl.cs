using Bikewale.Cache.Core;
using Bikewale.Cache.UserReviews;
using Bikewale.Common;
using Bikewale.DAL.UserReviews;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Author:rakesh yadav On 08 Sep 2015
    /// Desc: Call UserReviewsList service and bind review list to repeater
    /// </summary>
    /// <summary>
    /// Author:Lucky Rathore On 16 oct 2015
    /// Desc: Add ModelMaskingName and MakeMaskingName
    /// 
    /// Author: Sangram Nandkhile on 16 Dec 2015
    /// Desc: Removed API call
    /// </summary>
    public class BindUserReviewControl
    {
        public int ModelId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int VersionId { get; set; }
        public FilterBy Filter { get; set; }
        public int RecordCount { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public int ReviewId { get; set; }
        /// <summary>
        /// Modified By :- Subodh Jain 17 Jan 2017
        /// Summary :- Review list other than reviewid current review
        /// Modified By:- subodh Jain 30 Jan 2017
        /// Summary :- Added Fetch Count check for repeater binding
        /// </summary>
        /// <param name="rptUserReviews"></param>
        public void BindUserReview(Repeater rptUserReviews)
        {
            FetchedRecordsCount = 0;
            try
            {
                int stratIndex = 1;
                int endIndex = 4;
                Paging.GetStartEndIndex(PageSize, PageNo, out stratIndex, out endIndex);
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUserReviewsCache, UserReviewsCacheRepository>()
                    .RegisterType<ICacheManager, MemcacheManager>()
                    .RegisterType<IUserReviewsRepository, UserReviewsRepository>();
                    IUserReviewsCache objVersion = container.Resolve<IUserReviewsCache>();
                    uint recCount = Convert.ToUInt16(RecordCount);
                    ReviewListBase reviews = objVersion.GetBikeReviewsList((uint)stratIndex, (uint)endIndex, (uint)ModelId, 0, Filter);

                    if (reviews != null && reviews.ReviewList != null && reviews.ReviewList.Count() > 0)
                    {
                        reviews.ReviewList = reviews.ReviewList.Where(x => x.ReviewId != ReviewId);
                        FetchedRecordsCount = reviews.ReviewList.Count();
                        if (FetchedRecordsCount > 0)
                        {
                            MakeMaskingName = reviews.ReviewList.FirstOrDefault().MakeMaskingName;
                            ModelMaskingName = reviews.ReviewList.FirstOrDefault().ModelMaskingName;

                            rptUserReviews.DataSource = reviews.ReviewList;
                            rptUserReviews.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }
    }
}