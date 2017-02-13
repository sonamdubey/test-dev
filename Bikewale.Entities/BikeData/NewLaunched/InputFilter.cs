
namespace Bikewale.Entities.BikeData.NewLaunched
{
    public class InputFilter
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public uint Make { get; set; }
        public uint YearLaunch { get; set; }
        public uint CityId { get; set; }
    }
}
