using System;

namespace Bikewale.Entities.GenericBikes
{
    /// <summary>
    /// Created by : Aditi Srivastava on 12 Jan 2017
    /// Description: Entity for bike ranking by category(body style)
    /// </summary>
    [Serializable]
    public class BikeRankingEntity
    {
        public int Rank { get; set; }
        public EnumBikeBodyStyles BodyStyle { get; set; }
    }
}
