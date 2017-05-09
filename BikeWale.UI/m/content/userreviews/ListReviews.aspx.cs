﻿using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.UserReviews;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.UserReviews;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Pager;
using Bikewale.Entities.SEO;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Mobile.Controls;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private IUserReviewsRepository objUserReviews = null;
        protected List<ReviewEntity> objReviewList;
        protected BikeModelEntity objModelEntity = null;
        protected Repeater rptUserReviews;
        protected uint totalReviews = 0;
        protected int modelId = 0;
        protected ReviewRatingEntity objRating = null;
        private IBikeModels<BikeModelEntity, int> objModel = null;
        private IPager objPager = null;
        protected int startIndex = 0, endIndex = 0, pageSize = 10, curPageNo = 1;
        public LinkPagerControl ctrlPager;
        protected UserReviewSimilarBike ctrlUserReviewSimilarBike;
        protected string prevPageUrl = String.Empty, nextPageUrl = String.Empty;
        protected PageMetaTags pageMetas;
        protected bool isReviewAvailable;
        protected string returnUrl=string.Empty;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        //Modified By: Aditi Srivastava on 7 Sep 2016
        //SUmmary: Added request rawURL on form action
        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Action = Request.RawUrl;
            if (!IsPostBack)
            {
                if (ProcessQS())
                {
                    try
                    {
                        RegistorContainer();

                        if (modelId != 0)
                        {
                            GetModelDetails();
                            GetPaging();
                            GetReviewList();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                        objErr.SendMail();
                    }
                }
                else
                {
                    Response.Redirect("/m/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            BindControl();
            CreatMetas();
            SetReturnUrl();
        }
        /// <summary>
        /// Created By :- Subodh Jain 17 Jan 2017
        /// Summary :- Bind metas
        /// </summary>
        private void CreatMetas()
        {
            pageMetas = new PageMetaTags();
            pageMetas.Title = string.Format("User Reviews: {0} {1} | Bikes Reviews.", objModelEntity.MakeBase.MakeName, objModelEntity.ModelName);
            pageMetas.Description = string.Format("{0} {1} User Reviews - Read first-hand reviews of actual {0} {1} owners. Find out what buyers of {0} {1} have to say about the bike.", objModelEntity.MakeBase.MakeName, objModelEntity.ModelName);
            pageMetas.Keywords = string.Format("{0} {1} reviews, {0} {1} Users Reviews, {0} {1} customer reviews, {0} {1} customer feedback, {0} {1} owner feedback, user bike reviews, owner feedback, consumer feedback, buyer reviews", objModelEntity.MakeBase.MakeName, objModelEntity.ModelName);
            pageMetas.AlternateUrl = (curPageNo > 1) ? string.Format("{0}/m/{1}-bikes/{2}/user-reviews-p{3}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objModelEntity.MakeBase.MaskingName, objModelEntity.MaskingName, curPageNo) : string.Format("{0}/m/{1}-bikes/{2}/user-reviews/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objModelEntity.MakeBase.MaskingName, objModelEntity.MaskingName);
            pageMetas.CanonicalUrl = (curPageNo > 1) ? string.Format("{0}/{1}-bikes/{2}/user-reviews-p{3}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objModelEntity.MakeBase.MaskingName, objModelEntity.MaskingName, curPageNo) : string.Format("{0}/{1}-bikes/{2}/user-reviews/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objModelEntity.MakeBase.MaskingName, objModelEntity.MaskingName);

        }
        /// <summary>
        /// Created By :- Subodh Jain 2017
        /// Created By :- Bind User Control
        /// </summary>
        private void BindControl()
        {
            ctrlUserReviewSimilarBike.ModelId = Convert.ToUInt16(modelId);
            ctrlUserReviewSimilarBike.TopCount = 6;
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 17 Oct 2016
        /// Description :   Added try-catch and 301 redirection
        /// </summary>
        /// <returns></returns>
        private bool ProcessQS()
        {
            bool isSucess = true;
            string modelMaskingName = Request.QueryString["bikem"];
            string model = String.Empty;
            ModelMaskingResponse objResponse = null;
            try
            {
                if (!String.IsNullOrEmpty(modelMaskingName))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                        var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

                        objResponse = objCache.GetModelMaskingResponse(modelMaskingName);
                    }
                }
                else
                {
                    isSucess = false;
                }
            }
            catch (Exception)
            {
                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
                isSucess = false;
            }
            finally
            {
                if (objResponse != null && objResponse.StatusCode == 200)
                {
                    model = objResponse.ModelId.ToString();

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
                    if (objResponse.StatusCode == 301)
                    {
                        //redirect permanent to new page 
                        CommonOpn.RedirectPermanent(Request.RawUrl.Replace(modelMaskingName, objResponse.MaskingName));
                        isSucess = false;

                    }
                    else
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                        isSucess = false;
                    }
                }
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
            if (objModelEntity.Futuristic)
            {
                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;

            }
            GetModelRatings();

        }

        private void GetModelRatings()
        {
            objRating = objUserReviews.GetBikeRatings(Convert.ToUInt32(modelId));
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 26 Apr 2017
        /// Description :   Call ToList function
        /// </summary>
        private void GetReviewList()
        {
            ReviewListBase reviews = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IUserReviewsCache, UserReviewsCacheRepository>()
                             .RegisterType<IUserReviewsRepository, UserReviewsRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>();

                var cache = container.Resolve<IUserReviewsCache>();
                reviews = cache.GetBikeReviewsList(Convert.ToUInt32(startIndex), Convert.ToUInt32(endIndex), Convert.ToUInt32(modelId), 0, FilterBy.MostRecent);
                if (reviews != null && reviews.TotalReviews > 0)
                {
                    objReviewList = reviews.ReviewList.ToList();
                    totalReviews = reviews.TotalReviews;
                }
            }
            int totalPages = objPager.GetTotalPages(Convert.ToInt32(totalReviews), pageSize);

            if (totalReviews > 0)
            {
                //if current page number exceeded the total pages count i.e. the page is not available 
                if (curPageNo > 0 && curPageNo <= totalPages)
                {


                    PagerEntity pagerEntity = new PagerEntity();
                    pagerEntity.BaseUrl = "/m/" + objModelEntity.MakeBase.MaskingName + "-bikes/" + objModelEntity.MaskingName + "/user-reviews-";
                    pagerEntity.PageNo = curPageNo;
                    pagerEntity.PagerSlotSize = totalPages;
                    pagerEntity.PageUrlType = "p";
                    pagerEntity.TotalResults = Convert.ToInt32(totalReviews);
                    pagerEntity.PageSize = pageSize;

                    PagerOutputEntity pagerOutput = objPager.GetPager<PagerOutputEntity>(pagerEntity);

                    //get next and prev page links for SEO 
                    ctrlPager.PagerOutput = pagerOutput;
                    ctrlPager.TotalPages = totalPages;
                    ctrlPager.CurrentPageNo = curPageNo;
                    ctrlPager.BindPagerList();

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
        private void SetReturnUrl()
        {
            returnUrl = Utils.Utils.EncryptTripleDES(string.Format("returnUrl=/{0}-bikes/{1}/user-reviews/",objModelEntity.MakeBase.MaskingName,objModelEntity.MaskingName));
        }

        private void RegistorContainer()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                objModel = container.Resolve<IBikeModels<BikeModelEntity, int>>();

                container.RegisterType<IUserReviewsRepository, UserReviewsRepository>();
                objUserReviews = container.Resolve<IUserReviewsRepository>();

                container.RegisterType<IPager, Pager>();
                objPager = container.Resolve<IPager>();
            }
        }
    }
}