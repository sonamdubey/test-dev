namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 27th Sep 2017
    /// Summary : Entity to hold price
    /// </summary>
    public class PriceEntityBase
    {
        public ulong MinPrice { get; set; }
        public ulong MaxPrice { get; set; }
        public ulong AvgPrice { get; set; }
    }
}
