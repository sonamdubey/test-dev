using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// created by sajal gupta on 23-05-2017
    /// Description : Entity to hold model's versions details.
    /// </summary>
    public class BikeModelVersionsDetails
    {
        public IEnumerable<BikeVersionSegmentDetails> Versions { get; set; }
        public string MaskingName { get; set; }
        public uint ModelId { get; set; }
        public string ModelName { get; set; }
        public string CCSegment { get; set; }
        public uint TopVersionId { get; set; }
        public string BodyStyle { get; set; }
    }
}
