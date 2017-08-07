using Bikewale.Notifications;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Models.DealerPricing;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerBikePrice
{
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

        public DealerPricingIndexPageVM GetLandingInformation()
        {
            IEnumerable<CityNameEntity> dealerCities = null;
            DealerPricingIndexPageVM dealerPricingLandingInfo = new DealerPricingIndexPageVM();
            dealerPricingLandingInfo.CopyPricingDealers = new DealerCopyPricingVM();
            dealerPricingLandingInfo.CopyPricingCities = new CityCopyPricingVM();
            dealerPricingLandingInfo.ShowPricingCities = new ShowPricingVM();
            dealerPricingLandingInfo.AddCategoryType = new AddCategoryVM();
            dealerPricingLandingInfo.DealerOperationParams = new DealerOperationVM();

            try
            {
                dealerCities = location.GetDealerCities();
                dealerPricingLandingInfo.CopyPricingDealers.Cities = dealerCities;
                dealerPricingLandingInfo.ShowPricingCities.Cities = location.GetAllCities();
                dealerPricingLandingInfo.CopyPricingCities.States = location.GetStates();
                dealerPricingLandingInfo.AddCategoryType.PriceCategories = dealerPriceQuote.GetBikeCategoryItems("");
                dealerPricingLandingInfo.DealerOperationParams.DealerCities = dealerCities;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetLandingInformation");
            }

            return dealerPricingLandingInfo;
        }
    }
}