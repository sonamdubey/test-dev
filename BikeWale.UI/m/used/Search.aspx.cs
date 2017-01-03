using Bikewale.BindViewModels.Webforms.Used;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Used;
using Bikewale.Mobile.Controls;
using Bikewale.Utility.UsedCookie;
using System;
using System.Collections.Generic;

namespace Bikewale.Mobile.Used
{
    /// <summary>
    /// Created by: Ashish Kamble on 10 Sep 2016
    /// </summary>
    public class Search : System.Web.UI.Page
    {
        #region variables
        SearchUsedBikes objUsedBikesPage = null;
        protected string pageTitle = string.Empty, pageDescription = string.Empty, pageKeywords = string.Empty, pageCanonical = string.Empty
                 , heading = string.Empty, nextUrl = string.Empty, prevUrl = string.Empty, redirectUrl = string.Empty, alternateUrl = string.Empty,
                 cityName = string.Empty, currentQueryString = string.Empty, makeMaskingName = string.Empty, modelMaskingName = string.Empty,
                 cityMaskingName = string.Empty, makeName = string.Empty, modelName = string.Empty;
        protected IEnumerable<UsedBikeBase> usedBikesList = null;
        protected IEnumerable<CityEntityBase> citiesList = null;
        protected IEnumerable<BikeMakeModelBase> makeModelsList = null;
        public LinkPagerControl ctrlPager;
        protected ushort makeId;
        protected uint modelId, cityId, totalListing, PageIdentifier;
        protected CityEntityBase objCity = null;
        protected int _startIndex = 0, _endIndex = 0;
        protected UsedBikesCityCountByBrand ctrlUsedBikesCityCount = null;
        protected UsedBikeByModels ctrlUsedBikeByModels;
        protected UsedBikesCityCountByModel ctrlUsedBikesCityCountByModel = null;

        #endregion

        #region events

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        /// <summary>
        /// Modified By :Subodh Jain 2 jan 2017
        /// Description:- Added cookie and widget binding function 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadUsedBikesList();
            UsedCookie.SetUsedCookie();
            BindBrandIndiaWidget();
            BindModelIndiaWidget();

            if (makeId > 0 && cityId > 0 && modelId == 0 && Bikewale.Utility.UsedCookie.UsedCookie.BrandCity)
                BindBrandCityWidget();
        }

        #endregion

        #region methods
        /// <summary>
        /// Created By : Sushil Kumar on 23rd Sep 2016 
        /// Description : Bind Used bikes search page with listing,cities and makemodels
        /// Modified by :   Sumit Kate on 28 Sep 2016
        /// Description :   Use property RedirectionUrl
        /// Modiefied By:Subodh Jain 2 jan 2017
        /// Description :- Addded makeMaskingName modelMaskingName cityMaskingName  
        /// Modified by : Sajal Gupta on 03-01-2017
        /// Desc : Read makeName, modelname from view model
        /// </summary>
        private void LoadUsedBikesList()
        {
            objUsedBikesPage = new SearchUsedBikes();
            if (!objUsedBikesPage.IsPageNotFound && !objUsedBikesPage.IsPermanentRedirection)
            {
                objUsedBikesPage.BindSearchPageData();
                objUsedBikesPage.CreateMetas();
                objUsedBikesPage.BindLinkPager(ctrlPager);
                totalListing = objUsedBikesPage.TotalBikes;
                prevUrl = objUsedBikesPage.prevUrl;
                nextUrl = objUsedBikesPage.nextUrl;
                pageTitle = objUsedBikesPage.pageTitle;
                pageDescription = objUsedBikesPage.pageDescription;
                pageKeywords = objUsedBikesPage.pageKeywords;
                pageCanonical = objUsedBikesPage.pageCanonical;
                alternateUrl = objUsedBikesPage.alternateUrl;
                heading = objUsedBikesPage.heading;
                citiesList = objUsedBikesPage.Cities;
                makeId = objUsedBikesPage.MakeId;
                modelId = objUsedBikesPage.ModelId;
                cityId = objUsedBikesPage.CityId;
                cityName = objUsedBikesPage.City;
                _startIndex = objUsedBikesPage.startIndex;
                _endIndex = objUsedBikesPage.endIndex;
                objCity = objUsedBikesPage.SelectedCity;
                makeModelsList = objUsedBikesPage.MakeModels;
                usedBikesList = objUsedBikesPage.UsedBikes.Result;
                currentQueryString = objUsedBikesPage.CurrentQS;
                makeMaskingName = objUsedBikesPage.makeMaskingName;
                modelMaskingName = objUsedBikesPage.modelMaskingName;
                cityMaskingName = objUsedBikesPage.cityMaskingName;
                makeName = objUsedBikesPage.Make;
                modelName = objUsedBikesPage.Model;
            }
            else
            {
                // Redirection
                if (objUsedBikesPage.IsPermanentRedirection)
                {
                    CommonOpn.RedirectPermanent(objUsedBikesPage.RedirectionUrl);
                }
                else if (objUsedBikesPage.IsPageNotFound)
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                }


            }

        }
        /// <summary>
        /// Created By : Subodh Jain on 2 jan 2017 
        /// Description : Bind Used bikes Widget
        /// </summary>
        private void BindBrandCityWidget()
        {
            try
            {
                ctrlUsedBikeByModels.CityId = cityId;
                ctrlUsedBikeByModels.MakeId = makeId;
                ctrlUsedBikeByModels.TopCount = 6;
                ctrlUsedBikeByModels.MakeMaskingName = makeMaskingName;
                ctrlUsedBikeByModels.ModelMaskingName = modelMaskingName;
                ctrlUsedBikeByModels.CityMaskingName = cityMaskingName;
                ctrlUsedBikeByModels.CityName = cityName;
                PageIdentifier = 1;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Search.BindBrandCityWidget");
            }


        }

        /// <summary>
        /// Created by : Sajal Gupta on 2-01-2017
        /// Desc : Bind brand india widget if makeid is not null and city id is null;
        /// </summary>
        private void BindBrandIndiaWidget()
        {
            try
            {
                if (makeId != 0 && modelId == 0 && cityId == 0 && Bikewale.Utility.UsedCookie.UsedCookie.BrandIndia)
                {
                    ctrlUsedBikesCityCount.MakeId = makeId;
                    ctrlUsedBikesCityCount.MakeMaskingName = makeMaskingName;
                    ctrlUsedBikesCityCount.MakeName = makeName;
                    PageIdentifier = 0;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Search.BindBrandIndiaWidget");
            }
        }

        /// <summary>
        /// Created by : Sajal Gupta on 03-01-2017
        /// Desc : Bind model india widget if makeid is not null and modelindia is not null and city id is null;
        /// </summary>
        private void BindModelIndiaWidget()
        {
            try
            {
                if (makeId != 0 && modelId != 0 && cityId == 0 && Bikewale.Utility.UsedCookie.UsedCookie.ModelIndia)
                {
                    ctrlUsedBikesCityCountByModel.ModelName = modelName;
                    ctrlUsedBikesCityCountByModel.MakeMaskingName = makeMaskingName;
                    ctrlUsedBikesCityCountByModel.ModelId = modelId;
                    ctrlUsedBikesCityCountByModel.ModelMaskingName = modelMaskingName;
                    PageIdentifier = 2;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Search.BindModelIndiaWidget");
            }
        }
        #endregion

    } // class
}   // namespace