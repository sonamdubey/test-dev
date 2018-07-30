
using Bikewale.DTO.Make;
namespace Bikewale.DTO.BikeData.NewLaunched
{
    /// <summary>
    /// Created by  :   Sumit Kate on 13 Feb 2017
    /// Description :   BikesCountByMake DTO
    /// </summary>
    public class BikesCountByMakeDTO
    {
        public MakeBase Make { get; set; }
        public ushort Count { get; set; }
    }
}
