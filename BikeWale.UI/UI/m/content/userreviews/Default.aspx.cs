using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.UserReviews;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.UserReviews;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 9 May 2014
    /// Summary    : Class to manage bike reviews
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected DropDownList ddlMake;
        protected LinkButton btnSubmit;
        protected Repeater rptMostReviewed, rptMostRead, rptMostHelpful, rptMostRecent, rptMostRated;
        private List<ReviewTaggedBikeEntity> objMostReviewed = null;
        private IUserReviewsRepository objUserReviews = null;
        protected IEnumerable<BikeMakeEntityBase> objMakes = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMake();
                RegisterContainer();
                GetMostReviewedBikeList();
                GetMostReadReviews();
                GetMostHelpfulReviews();
                GetMostRecentReviews();
                GetMostRatedReviews();
            }
        }

        private void GetMostRecentReviews()
        {
            List<ReviewsListEntity> objMostRecent = objUserReviews.GetMostRecentReviews(10);
            rptMostRecent.DataSource = objMostRecent;
            rptMostRecent.DataBind();
        }

        private void GetMostHelpfulReviews()
        {
            List<ReviewsListEntity> objMostHelpful = objUserReviews.GetMostHelpfulReviews(10);
            rptMostHelpful.DataSource = objMostHelpful;
            rptMostHelpful.DataBind();
        }

        private void GetMostRatedReviews()
        {
            List<ReviewsListEntity> objMostRated = objUserReviews.GetMostRatedReviews(10);
            rptMostRated.DataSource = objMostRated;
            rptMostRated.DataBind();
        }

        private void GetMostReadReviews()
        {
            List<ReviewsListEntity> objMostRead = objUserReviews.GetMostReadReviews(10);
            rptMostRead.DataSource = objMostRead;
            rptMostRead.DataBind();
        }

        private void RegisterContainer()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IUserReviewsRepository, UserReviewsRepository>();

                objUserReviews = container.Resolve<IUserReviewsRepository>();
            }
        }

        private void GetMostReviewedBikeList()
        {
            objMostReviewed = objUserReviews.GetMostReviewedBikesList(10);
            rptMostReviewed.DataSource = objMostReviewed;
            rptMostReviewed.DataBind();
        }

        private void BindMake()
        {
            try
            {
                MakeModelVersion mmv = new MakeModelVersion();
                mmv.GetMakes(EnumBikeType.UserReviews, ref ddlMake);
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository>();
                    objMakes = objCache.GetMakesByType(EnumBikeType.New);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
        }
    }
}