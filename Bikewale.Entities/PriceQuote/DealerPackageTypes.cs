namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 15 March 2016
    /// Description : for Dealer Package Types.
    /// Modified by :   Sumit Kate on 04 Aug 2017
    /// Description :   Added CPS Dealer package type
    /// </summary>
    [System.Serializable]
    public enum DealerPackageTypes
    {
        Invalid = 0,
        Standard = 1,
        Deluxe = 2,
        Premium = 3,
        Pricing = 4,
        CPS = 5
    }
}