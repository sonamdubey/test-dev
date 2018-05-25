
using System;
namespace Bikewale.Entities.Finance.CapitalFirst
{
    /// <summary>
    /// Created by : Snehal Dange on 24th May 2017
    /// Desc  : Entity to store city pan mapping for Capital First
    /// </summary>

    [Serializable]
    public class CityPanMapping
    {
        public uint CityId { get; set; }
        public bool PanStatus { get; set; }
    }
}
