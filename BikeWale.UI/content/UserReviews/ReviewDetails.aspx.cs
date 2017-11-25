using Bikewale.BAL.Pager;
using Bikewale.BAL.UserReviews.Search;
using Bikewale.Cache.Core;
using Bikewale.Cache.UserReviews;
using Bikewale.Common;
using Bikewale.DAL.UserReviews;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Microsoft.Practices.Unity;
using System;
using System.Collections;

namespace Bikewale.Content
{
    public class ReviewDetails : System.Web.UI.Page
    {
        protected int reviewId = 0;
        private string _makeMasking, _modelMasking;
        private IUserReviewsCache _userReviewsCache = null;

        protected string RedirectUrl;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Int32.TryParse(Request.QueryString["rid"], out reviewId);
                _makeMasking = Request.QueryString["makeMaskingName"];
                _modelMasking = Request.QueryString["modelMaskingName"];

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUserReviewsCache, UserReviewsCacheRepository>()
                                 .RegisterType<IUserReviewsRepository, UserReviewsRepository>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<IUserReviewsSearch, UserReviewsSearch>()
                                 .RegisterType<IPager, Pager>();
                    container.RegisterType<IPager, BAL.Pager.Pager>();

                    _userReviewsCache = container.Resolve<IUserReviewsCache>();

                    Hashtable ht = _userReviewsCache.GetUserReviewsIdMapping();

                    if (ht.ContainsKey(reviewId))
                    {
                        uint newReviewId = 0;

                        newReviewId = Convert.ToUInt32(ht[reviewId]);

                        RedirectUrl = string.Format("/{0}-bikes/{1}/reviews/{2}/", _makeMasking, _modelMasking, newReviewId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
            }
            finally
            {
                if (!string.IsNullOrEmpty(RedirectUrl))
                    Bikewale.Common.CommonOpn.RedirectPermanent(RedirectUrl);
            }
        }      
    }//class
}//namespace