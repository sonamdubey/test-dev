
namespace Bikewale.Entities.Location
{

    /// Created By : Vivek Gupta 
    /// Date : 24 june 2016
    /// Desc: get dealer states
    /// </summary>
    public class DealerStateEntity : StateEntityBase
    {
        public string StateLatitude { get; set; }

        public string StateLongitude { get; set; }

        public uint StateCount { get; set; }
    }
}
