namespace Bikewale.Entities.NewBikeSearch
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Jan 2018
    /// Description :   Budget Filters
    /// </summary>
    [System.Serializable]
    public class BudgetFilterRanges
    {
        public System.Collections.Generic.IDictionary<string, uint> Budget { set; get; }
        public uint BikesCount { get; set; }
    }
}
