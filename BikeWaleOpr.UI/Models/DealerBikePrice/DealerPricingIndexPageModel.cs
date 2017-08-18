using Bikewale.Notifications;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Models.DealerPricing;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerBikePrice
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   Populates DealerPricingIndexPageVW
    /// </summary>
    public class DealerPricingIndexPageModel
    {
        private readonly ILocation location = null;
        private readonly IDealerPriceQuote dealerPriceQuote = null;

        public DealerPricingIndexPageModel(
            ILocation locationObject, IDealerPriceQuote dealerPriceQuoteObject)
        {
            location = locationObject;
            dealerPriceQuote = dealerPriceQuoteObject;
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
        /// Description :   Populates all the models need for landing page of dealer pricing management page
        /// </summary>
        /// <returns></returns>
        public DealerPricingIndexPageVM GetLandingInformation()
        {
            IEnumerable<CityNameEntity> dealerCities = null;
            DealerPricingIndexPageVM dealerPricingLandingInfo = new DealerPricingIndexPageVM();
            dealerPricingLandingInfo.CopyPricingDealers = new DealerCopyPricingVM();
            dealerPricingLandingInfo.CopyPricingCities = new CityCopyPricingVM();
            dealerPricingLandingInfo.ShowPricingCities = new ShowPricingVM();
            dealerPricingLandingInfo.AddCategoryType = new AddCategoryVM();
            dealerPricingLandingInfo.DealerOperationParams = new DealerOperationPricingVM();

            try
            {
                dealerCities = location.GetDealerCities();
                dealerPricingLandingInfo.PageTitle = "Dealers Operations";
                dealerPricingLandingInfo.CopyPricingDealers.Cities = dealerCities;
                dealerPricingLandingInfo.ShowPricingCities.Cities = location.GetAllCities();
                dealerPricingLandingInfo.CopyPricingCities.States = location.GetStates();
                dealerPricingLandingInfo.AddCategoryType.PriceCategories = dealerPriceQuote.GetBikeCategoryItems("");
                dealerPricingLandingInfo.DealerOperationParams.DealerCities = dealerCities;
                dealerPricingLandingInfo.DealerOperationParams.IsOpen = true;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetLandingInformation");
            }

            return dealerPricingLandingInfo;
        }
    }
}