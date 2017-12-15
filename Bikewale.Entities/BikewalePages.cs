namespace Bikewale.Entities.Pages
{
    /// <summary>
    /// Created By : Sushil Kumar on 17th Aug 2017
    /// Description : To specify distict pages of bikewale with platform
    /// </summary>
    public enum BikewalePages
    {
        Desktop_HomePage = 1,
        Mobile_HomePage = 2,
        Desktop_MakePage = 3,
        Mobile_MakePage = 4,
        Desktop_ModelPage = 5,
        Mobile_ModelPage = 6,
        Desktop_PriceInCity = 7,
        Mobile_PirceInCity = 8
    }

    /// <summary>
    /// Created by: Vivek Singh Tomar on 23 Aug 2017
    /// Summary: To specify pages of bikewale for GA
    /// Modified by Sajal Gupta on 10-11-2017
    /// Desc : Added Compare_Bikes
    /// </summary>
    public enum GAPages
    {
        Other,
        HP = 1,
        Model_Page = 2,
        Make_Page = 3,
        New_Bikes_Page = 4,
        Search_Page = 5,
        DealerPriceQuote_Page = 6,
        Dealer_Locator_Page = 7,
        Dealer_Locator_Detail_Page = 8,
        Booking_Page = 9,
        PriceInCity_Page = 10,
        Compare_Bikes = 11,
        News_Details = 12,
        ExpertReviews_Details = 13
    }
}
