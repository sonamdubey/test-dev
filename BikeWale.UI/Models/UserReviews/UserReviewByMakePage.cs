﻿using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.UserReviews
{
    public class UserReviewByMakePage
    {
        private readonly IUserReviewsCache _userReviewsCache = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCache;
        private readonly uint _makeId;
        private readonly string _makeMaskingName;

        private UserReviewByMakeVM objData = null;

        public bool IsMobile { get; set; }
        public StatusCodes Status { get; set; }
        public string RedirectUrl { get; set; }
        public ushort PopularBikesCount { get; set; }

        public UserReviewByMakePage(IUserReviewsCache userReviewsCache, IBikeMakesCacheRepository bikeMakesCache, string makeMasking)
        {
            _userReviewsCache = userReviewsCache;
            _bikeMakesCache = bikeMakesCache;
            _makeMaskingName = makeMasking;
            _makeId = ProcessQueryString();

        }

        public UserReviewByMakeVM GetData()
        {
            try
            {
                objData = new UserReviewByMakeVM();
                var objBikes = _userReviewsCache.GetPopularBikesWithUserReviewsByMake(_makeId);
                if (objBikes != null && objBikes.Any())
                {
                    objData.PopularBikes = objBikes.Take(PopularBikesCount);
                    objData.OtherBikes = objBikes.Skip(PopularBikesCount);

                    objData.Make = objData.PopularBikes.First().Make;
                }
                BindPageMetas();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewLandingPage.GetData");
            }
            return objData;
        }

        public void BindPageMetas()
        {
            try
            {
                if (objData != null)
                {
                    objData.PageMetaTags.Title = string.Format("{0} Bikes Reviews | Reviews of {0} Models- BikeWale", objData.Make.MakeName);
                    objData.PageMetaTags.Description = string.Format("Explore reviews of all models of {0} Bikes. Read 5000+ unbiased and verified reviews from real owners on BikeWale", objData.Make.MakeName);
                    objData.PageMetaTags.CanonicalUrl = string.Format("{0}/{1}-bikes/reviews/", BWConfiguration.Instance.BwHostUrl, objData.Make.MaskingName);
                    objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/{1}-bikes/reviews/", BWConfiguration.Instance.BwHostUrl, objData.Make.MaskingName);
                    objData.PageMetaTags.Keywords = string.Format("{0} reviews, {0} bike reviews, {0} expert review, {0} bike expert review, {0} Bike user review, {0} owner review, {0} bike owner review, {0} user review, {0} bike user review", objData.Make.MakeName.ToLower());
                    objData.Page_H1 = string.Format("Reviews on bikes from {0}", objData.Make.MakeName);

                    SetBreadcrumList();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewLandingPage.BindPageMetas()");
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList()
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;
            if (IsMobile)
            {
                url += "m/";
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url + "reviews/", "Reviews"));
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objData.Page_H1));


            objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

        }


        private uint ProcessQueryString()
        {
            uint makeId = 0;
            try
            {
                MakeMaskingResponse objResponse = _bikeMakesCache.GetMakeMaskingResponse(_makeMaskingName);
                if (objResponse != null)
                {
                    Status = (StatusCodes)objResponse.StatusCode;
                    if (objResponse.StatusCode == 200)
                    {
                        makeId = objResponse.MakeId;
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        RedirectUrl = System.Web.HttpContext.Current.Request.RawUrl.Replace(_makeMaskingName, objResponse.MaskingName);
                        Status = StatusCodes.RedirectPermanent;
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("MakePageModel.ProcessQuery() makeMaskingName:{0}", _makeMaskingName));
            }
            return makeId;
        }
    }
}