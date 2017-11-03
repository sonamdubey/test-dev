
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 12 May 2014
    /// Summary : Class have list of input parameters required to get the upcoming bikes list.
    /// Start Index and End Index are required. MakeId and ModelId are optional.
    /// modified by :- Subodh Jain 09 march 2017
    /// Summary :- Added BodyStyleId
    /// Modified by : Ashutosh Sharma on 03 Nov 2017
    /// Description : Added DeviatedPriceMin, DeviatedPriceMax.
    /// </summary>
    public class UpcomingBikesListInputEntity 
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public uint BodyStyleId { get; set; }
        public uint Year { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public ulong DeviatedPriceMin { get; set; }
        public ulong DeviatedPriceMax { get; set; }
    }
}
