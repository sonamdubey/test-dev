using Bikewale.Common;
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

        private UserReviewByMakeVM objData = null;

        private uint _makeId;

        public bool IsMobile { get; set; }
        public StatusCodes Status { get; set; }
        public string RedirectUrl { get; set; }
        public ushort PopularBikesCount { get; set; }

        public UserReviewByMakePage(IUserReviewsCache userReviewsCache, IBikeMakesCacheRepository bikeMakesCache, string makeMasking)
        {
            _userReviewsCache = userReviewsCache;
            _bikeMakesCache = bikeMakesCache;
            ProcessQueryString(makeMasking);

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

                    objData.Make = objData.PopularBikes.First().Make.MakeName;
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
                    objData.PageMetaTags.Title = "Bike Reviews | Reviews from Owners and Experts- BikeWale";
                    objData.PageMetaTags.Description = "Read reviews about a bike from real owners and experts. Know pros, cons, and tips from real users and experts before buying a bike.";
                    objData.PageMetaTags.CanonicalUrl = "https://www.bikewale.com/reviews/";
                    objData.PageMetaTags.Keywords = "Reviews, Bike reviews, expert review, Bike expert review, Bike user review, owner review, bike owner review, user review, bike user review";
                    objData.Page_H1 = string.Format("Browse {0} bike reviews", objData.Make);

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


        private void ProcessQueryString(string makeMaskingName)
        {
            try
            {
                MakeMaskingResponse objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);
                if (objResponse != null)
                {
                    Status = (StatusCodes)objResponse.StatusCode;
                    if (objResponse.StatusCode == 200)
                    {
                        _makeId = objResponse.MakeId;
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        RedirectUrl = System.Web.HttpContext.Current.Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName);
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("MakePageModel.ProcessQuery() makeMaskingName:{0}", makeMaskingName));
            }
        }
    }
}