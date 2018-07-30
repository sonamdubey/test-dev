using Bikewale.BAL.UserReviews.Search;
using Bikewale.Cache.Core;
using Bikewale.Cache.UserReviews;
using Bikewale.Common;
using Bikewale.DAL.UserReviews;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Bikewale.Mobile.Controls
{
    public class TopUserReviews : System.Web.UI.UserControl
    {
        protected List<ReviewEntity> objReviewList = null;
        protected string modelMaskingName = String.Empty, makeMaskingName = String.Empty;
        protected uint totalReviews = 0;
        protected Repeater rptUserReviews;
        protected string _headerText = String.Empty;

        public uint ModelId { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public uint TopCount { get; set; }
        public FilterBy Filter { get; set; }

        public string HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; }
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                    GetTopUserReviews();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 26 Apr 2017
        /// Description :   Call ToList function
        /// </summary>
        private void GetTopUserReviews()
        {
            IUserReviewsCache objUserReviews = null;

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IUserReviewsCache, UserReviewsCacheRepository>()
                    .RegisterType<ICacheManager, MemcacheManager>()
                    .RegisterType<IUserReviewsRepository, UserReviewsRepository>()
                 .RegisterType<IUserReviewsSearch, UserReviewsSearch>();
                container.RegisterType<IPager, BAL.Pager.Pager>();

                objUserReviews = container.Resolve<IUserReviewsCache>();

                var userReviews = objUserReviews.GetBikeReviewsList(1, TopCount, ModelId, 0, Filter);

                if (totalReviews > 0 && userReviews.TotalReviews > 0)
                {
                    modelMaskingName = ModelMaskingName;
                    makeMaskingName = MakeMaskingName;

                    rptUserReviews.DataSource = userReviews.ReviewList;
                    rptUserReviews.DataBind();
                }
            }
        }
    }
}