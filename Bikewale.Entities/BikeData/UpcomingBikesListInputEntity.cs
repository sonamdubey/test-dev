
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 12 May 2014
    /// Summary : Class have list of input parameters required to get the upcoming bikes list.
    /// Start Index and End Index are required. MakeId and ModelId are optional.
    /// modified by :- Subodh Jain 09 march 2017
    /// Summary :- Added BodyStyleId
    /// </summary>
    public class UpcomingBikesListInputEntity
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public uint BodyStyleId { get; set; }
    }
}
