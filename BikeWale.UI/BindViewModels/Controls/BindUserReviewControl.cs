using Bikewale.Common;
using Bikewale.DAL.UserReviews;
using Bikewale.Entities.DTO;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                    container.RegisterType<IUserReviews, UserReviewsRepository>();
                    IUserReviews objVersion = container.Resolve<IUserReviews>();
                    uint recCount = Convert.ToUInt16(RecordCount);
                    List<ReviewEntity> userReviewLists = objVersion.GetBikeReviewsList((uint)stratIndex, (uint)endIndex, (uint)ModelId, 0, Filter, out recCount);
                    if (userReviewLists.Count > 0)
                    {
                        FetchedRecordsCount = userReviewLists.Count;
                        MakeMaskingName = userReviewLists.FirstOrDefault().MakeMaskingName;
                        ModelMaskingName = userReviewLists.FirstOrDefault().ModelMaskingName;
                        rptUserReviews.DataSource = userReviewLists;
                        rptUserReviews.DataBind();
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