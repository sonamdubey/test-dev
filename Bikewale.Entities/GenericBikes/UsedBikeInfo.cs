using System;

namespace Bikewale.Entities.GenericBikes
{
    /// <summary>
    /// Created By : Sanjay George on 27 Sept 2018
    /// </summary>
    [Serializable]
    public class UsedBikeInfo
    {
        public UInt32 UsedBikeCount { get; set; }
        public uint UsedBikeMinPrice { get; set; }
    }
}
