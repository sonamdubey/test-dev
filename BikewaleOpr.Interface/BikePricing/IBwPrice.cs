namespace BikewaleOpr.Interface.BikePricing
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 10 Nov 2017
    /// Description : Provide BAL methods for Bikewale pricing.
    /// </summary>
    public interface IBwPrice
    {
        bool SaveBikePrices(string versionAndPriceList, string citiesList, uint makeId, string modelIds, int updatedBy);
        void UpdateModelPriceDocument(string versionIds, string cityIds);
        void CreateModelPriceDocument(string modelIds, string cityIds);
    }
}
