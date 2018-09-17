
namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Modified By : Lucky Rathore
    /// Modified On : 29 March 2016
    /// Description : Mobile_DealerLocator_Detail = 41 and Mobile_DealerLocator_Listing = 42 added.
    /// Modified By : Lucky Rathore
    /// Modified On : 28 March 2016
    /// Description : Desktop_DealerLocator_SubmitButton and  Desktop_DealerLocator_GetOfferButton added.
    /// Modified By : Sushil Kumar
    /// Modified On : 6th June 2016
    /// Description : Mobile_PriceInCity_SelectAreas,Mobile_PriceInCity_Alternative and  Mobile_PriceInCity_DealerCard_GetOffers added.
    /// Modified By : Sushil Kumar on 28th Oct 2016
    /// Description : Added pqsources for bikes list on homepage desktop and mobile 
    /// Modified By : Subodh Jain on 28th Nov 2016
    /// Description : Added pqsources for Service Center on Default desktop and mobile 
    /// Modified by :   Sumit Kate on 19 Jan 2017
    /// Description :   Added Desktop_DealerLocator_Detail_AvailableModels
    /// Modified by : Sajal Gupta on 24-01-217
    /// Description : Added Mobile_News_Listing_page
    /// Modified By:- Subodh jain 09 march 2017 
    /// Summary :- Added Desktop_Scooters_Landing_Check_on_road_price  Mobile_Scooters_Landing_Check_on_road_price                      
    /// Modified By : Deepak Israni on 8 May 2018
    /// Description : Added Mobile_ExpertReviews_Listing_Page, Mobile_BikeCare_Listing_Page, Mobile_BikeCare_Details_Page, Mobile_Features_Listing_Page, Mobile_Features_Details_Page, Mobile_ComparisionTest_Listing_Page
    /// Modified by : Sanskar Gupta on 14 May 2018
    /// Description : Added `Mobile_News_Details_Page`
    /// Modified by : Sanskar Gupta on 15 May 2018
    /// Description : Added `Mobile_ExpertReviews_Details_Page`
    /// Modified by : Sanskar Gupta on 17 May 2018
    /// Description : Added `Desktop_ExpertReviews_Details_Page`
    /// </summary>
    public enum PQSourceEnum
    {
        Desktop_HP_PQ_Widget = 1,
        Desktop_New_MostPopular = 2,
        Desktop_New_NewLaunches = 3,
        Desktop_New_PQ_Widget = 4,
        Desktop_MakePage = 5,
        Desktop_ModelPage = 6,
        Desktop_ModelPage_Alternative = 7,
        Desktop_CompareBike = 8,
        Desktop_PQ_Landing = 9,
        Desktop_PQ_Alternative = 10,
        Desktop_DPQ_Alternative = 11,
        Desktop_LocateDealer_NewLaunches = 12,
        Desktop_Upcoming_NewLaunches = 13,
        Desktop_NewLaunchLanding = 14,
        Desktop_UserReview_ModelPage = 15,
        Desktop_NewBikeSearch = 16,
        Mobile_HP_PQ_Widget = 17,
        Mobile_New_MostPopular = 18,
        Mobile_New_NewLaunches = 19,
        Mobile_New_PQ_Widget = 20,
        Mobile_MakePage = 21,
        Mobile_ModelPage = 22,
        Mobile_ModelPage_Alternative = 23,
        Mobile_CompareBike = 24,
        Mobile_PQ_Landing = 25,
        Mobile_PQ_Alternative = 26,
        Mobile_DPQ_Alternative = 27,
        Mobile_LocateDealer_NewLaunches = 28,
        Mobile_Upcoming_NewLaunches = 29,
        Mobile_NewLaunchLanding = 30,
        Mobile_UserReview_ModelPage = 31,
        Mobile_NewBikeSearch = 32,
        Desktop_PQ_Quotation = 33,
        Mobile_PQ_Quotation = 34,
        Desktop_DPQ_Quotation = 35,
        Mobile_DPQ_Quotation = 36,
        DeskTop_AutoSuggest = 37,
        Mobile_AutoSuggest = 38,
        Desktop_BookingListing = 39,
        Mobile_BookingListing = 40,
        Mobile_DealerLocator_Detail = 41,
        Mobile_DealerLocator_Listing = 42,
        Desktop_DealerLocator_SubmitButton = 43,
        Desktop_DealerLocator_GetOfferButton = 44,
        Desktop_ModelPage_FloatingButton = 45,
        Desktop_PriceInCity_SelectAreas = 46,
        Desktop_PriceInCity_Alternative = 47,
        Desktop_PriceInCity_DealersCard_GetOfferButton = 48,
        Desktop_SpecsAndFeaturePage_CheckOnRoadPrice = 49,
        Desktop_SpecsAndFeaturePage_GetOffersFromDealer = 50,
        Mobile_PriceInCity_DealersCard_GetOfferButton = 51,
        Mobile_PriceInCity_AlternateBikes = 52,
        Mobile_PriceInCity_SelectArea = 53,
        Mobile_SpecsAndFeaturePage_CheckOnRoadPrice = 54,
        Mobile_SpecsAndFeaturePage_GetOffersFromDealer = 55,
        Desktop_SpecsAndFeaturePage_OnLoad = 56,
        Mobile_SpecsAndFeaturePage_OnLoad = 57,
        Desktop_MakePage_GetOffersFromDealer = 58,
        Mobile_MakePage_GetOffersFromDealer = 59,
        Desktop_dealer_details_Get_offers = 60,
        Mobile_dealer_details_Get_offers = 61,
        Desktop_HP_MostPopular = 62,
        Desktop_HP_NewLaunches = 63,
        Mobile_HP_MostPopular = 64,
        Mobile_HP_NewLaunches = 65,
        Mobile_ServiceCenter_Listing_CityPage = 66,
        Mobile_ServiceCenter_DetailsPage = 67,
        Desktop_ServiceCenter_DefaultPage = 68,
        Mobile_ServiceCenter_DefaultPage = 69,
        Desktop_DPQ_SecondaryDealers = 70,
        Mobile_DPQ_SecondaryDealers = 71,
        Desktop_Generic_Bikes = 72,
        Desktop_Generic_Scooters = 73,
        Desktop_Generic_MileageBikes = 74,
        Desktop_Generic_SportsBikes = 75,
        Desktop_Generic_CruiserBikes = 76,
        Mobile_GenricBikeInfo_Widget = 77,
        Mobile_Dealerpricequote_DealersCard_GetOfferButton = 78,
        Desktop_Dealerpricequote_DealersCard_GetOfferButton = 79,
        Desktop_PriceInCity_Dealer_Detail_Click = 80,
        Mobile_PriceInCity_Dealer_Detail_Click = 81,
        Desktop_DealerLocator_Detail_AvailableModels = 82,
        Mobile_DealerLocator_Detail_AvailableModels = 83,
        Mobile_News_Listing_page = 84,
        Desktop_Photos_page = 85,
        Desktop_Scooters_Landing_Check_on_road_price = 86,
        Mobile_Scooters_Landing_Check_on_road_price = 87,
        Desktop_DealerLocator_Landing_Check_on_road_price = 88,
        Mobile_DealerLocator_Landing_Check_on_road_price = 89,
        Desktop_UpcomiingBikes_NewLaunchesWidget = 90,
        Mobile_UpcomiingBikes_NewLaunchesWidget = 91,
        Desktop_PriceInCity_Manufacturer_LeadCapture = 92,
        Mobile_PriceInCity_Manufacturer_LeadCapture = 93,
        Desktop_HomePage_BestBikes = 94,
        Desktop_NewPage_BestBikes = 95,
        Mobile_HomePage_BestBikes = 96,
        Mobile_NewPage_BestBikes = 97,
        Desktop_Scooter_MakePage_PopularBikes = 98,
        Mobile_Scooter_MakePage_PopularBikes = 99,
        Mobile_Popup_LeadCapture_ForAMP = 100,
        Mobile_Photos_Page = 101,
        Desktop_Series_Page = 102,
        Mobile_Series_Page = 103,
        Desktop_ElectricBike = 104,
        Mobile_ElectricBikes = 105,
        Desktop_Videos_Page_PopularSeries = 106,
        Mobile_Videos_Page_PopularSeries = 107,
        Desktop_NewsDetailsPage = 108,
        Mobile_ExpertReviews_Listing_Page = 109,
        Mobile_BikeCare_Listing_Page = 110,
        Mobile_BikeCare_Details_Page = 111,
        Mobile_Features_Listing_Page = 112,
        Mobile_Features_Details_Page = 113,
        Mobile_ComparisionTest_Listing_Page = 114,
        Mobile_News_Details_Page = 115,
        Mobile_ExpertReviews_Details_Page = 116,
        Desktop_ExpertReviews_Details_Page = 117,
        Desktop_DealerLocator_Detail = 118,
        Desktop_DealerLocator_Listing = 119,
        Desktop_PriceInCity = 120,
        Mobile_PriceInCity = 121
    }

}
