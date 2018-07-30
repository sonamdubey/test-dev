using Bikewale.Entities.GenericBikes;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 28 Mar 2017
    /// Description :   BikeModelRank ViewModel
    /// </summary>
    public class BikeModelRankVM
    {
        public BikeRankingEntity Rank { get; set; }
        public string StyleName { get; set; }
        public string BikeType { get; set; }
        public string RankText { get; set; }
    }
}
