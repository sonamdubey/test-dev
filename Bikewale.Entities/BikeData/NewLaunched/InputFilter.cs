
namespace Bikewale.Entities.BikeData.NewLaunched
{
    /// <summary>
    /// Created by  :   Sumit Kate on 13 feb 2017
    /// Description :   New Launched bikes API InputFilter entity
    /// </summary>
    public class InputFilter
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public uint Make { get; set; }
        public uint YearLaunch { get; set; }
        public uint CityId { get; set; }
    }
}
