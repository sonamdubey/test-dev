using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.BAL.UserReviews;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.UserReviews;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.UserReviews;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Pager;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Mobile.Controls;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 9 May 2014
    /// Summary    : class to list all bike reviews like most rated,most reviewed and most helpful
    /// </summary>
    public class ListReviews : System.Web.UI.Page
    {
        private IUserReviews objUserReviews = null;
        protected List<ReviewEntity> objReviewList = null;
        protected BikeModelEntity objModelEntity = null;
        protected Repeater rptUserReviews;
        protected uint totalReviews = 0;
        protected int modelId = 0;
        protected ReviewRatingEntity objRating = null;
        private IBikeModels<BikeModelEntity, int> objModel = null;
        private IPager objPager = null;
        int startIndex = 0, endIndex = 0, pageSize = 10, curPageNo = 1;
        protected ListPagerControl listPager;
        protected string prevPageUrl = String.Empty, nextPageUrl = String.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (ProcessQS())
                    {
                        RegistorContainer();

                        if (modelId != 0)
                        {
                            GetModelDetails();
                            GetPaging();
                            GetReviewList();
                        }
                    }
                    else
                    {
                        Response.Redirect("/m/pagenotfound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
            }
        }


        private bool ProcessQS()
        {
            bool isSucess = true;

            if (!String.IsNullOrEmpty(Request.QueryString["bikem"]))
            {
                string model = String.Empty;
                ModelMaskingResponse objResponse = null;

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

                    objResponse = objCache.GetModelMaskingResponse(Request.QueryString["bikem"]);

                    if (objResponse != null && objResponse.StatusCode == 200)
                    {
                        model = objResponse.ModelId.ToString();
                    }
                    else
                    {
                        if (objResponse.StatusCode == 301)
                        {
                            //redirect permanent to new page 
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(Request.QueryString["bikem"], objResponse.MaskingName));

                        }
                        else
                        {
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }
                }

                if (!String.IsNullOrEmpty(model))
                {
                    if (!Int32.TryParse(model, out modelId))
                        isSucess = false;
                }
                else
                {
                    isSucess = false;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["pn"]))
                {
                    if (!Int32.TryParse(Request.QueryString["pn"], out curPageNo))
                        isSucess = false;
                }
            }
            else
            {
                isSucess = false;
            }

            return isSucess;
        }

        private void GetPaging()
        {
            objPager.GetStartEndIndex(pageSize, curPageNo, out startIndex, out endIndex);
        }

        private void GetModelDetails()
        {
            //Get Model details
            objModelEntity = objModel.GetById(modelId);
            GetModelRatings();

        }

        private void GetModelRatings()
        {
            objRating = objUserReviews.GetBikeRatings(Convert.ToUInt32(modelId));
        }

        private void GetReviewList()
        {
            ReviewListBase reviews = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IUserReviewsCache, UserReviewsCacheRepository>()
                             .RegisterType<IUserReviews, UserReviewsRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>();

                var cache = container.Resolve<IUserReviewsCache>();
                reviews = cache.GetBikeReviewsList(Convert.ToUInt32(startIndex), Convert.ToUInt32(endIndex), Convert.ToUInt32(modelId), 0, FilterBy.MostRecent);
                objReviewList = reviews.ReviewList;
                totalReviews = reviews.TotalReviews;
            }
            int totalPages = objPager.GetTotalPages(Convert.ToInt32(totalReviews), pageSize);

            if (totalReviews > 0)
            {
                //if current page number exceeded the total pages count i.e. the page is not available 
                if (curPageNo > 0 && curPageNo <= totalPages)
                {
                    if (objReviewList.Count > 0)
                    {
                        rptUserReviews.DataSource = objReviewList;
                        rptUserReviews.DataBind();
                    }

                    PagerEntity pagerEntity = new PagerEntity();
                    pagerEntity.BaseUrl = "/m/" + objModelEntity.MakeBase.MaskingName + "-bikes/" + objModelEntity.MaskingName + "/user-reviews-";
                    pagerEntity.PageNo = curPageNo;
                    pagerEntity.PagerSlotSize = totalPages;
                    pagerEntity.PageUrlType = "p";
                    pagerEntity.TotalResults = Convert.ToInt32(totalReviews);
                    pagerEntity.PageSize = pageSize;

                    PagerOutputEntity pagerOutput = objPager.GetPager<PagerOutputEntity>(pagerEntity);

                    //get next and prev page links for SEO 
                    listPager.PagerOutput = pagerOutput;
                    listPager.TotalPages = totalPages;
                    listPager.CurrentPageNo = curPageNo;
                    listPager.BindPageNumbers();

                    //get next and prev page links for SEO
                    prevPageUrl = pagerOutput.PreviousPageUrl;
                    nextPageUrl = pagerOutput.NextPageUrl;
                }
                else
                {
                    Response.Redirect("/m/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;

                }
            }
        }

        private void RegistorContainer()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                objModel = container.Resolve<IBikeModels<BikeModelEntity, int>>();

                container.RegisterType<IUserReviews, UserReviews>();

                objUserReviews = container.Resolve<IUserReviews>();

                container.RegisterType<IPager, Pager>();
                objPager = container.Resolve<IPager>();
            }
        }
    }
}