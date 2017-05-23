
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by Sajal Gupta on 23-05-2017
    /// Descruiption : EntityTo hold version segment dertails.
    /// </summary>
    public class BikeVersionsSegment
    {
        public string BodyStyle { get; set; }
        public string ModelMaskingName { get; set; }
        public uint ModelId { get; set; }
        public string ModelName { get; set; }
        public string Segment { get; set; }
        public string VersionName { get; set; }
        public uint VersionId { get; set; }
        public string CCSegment { get; set; }
        public uint TopVersionId { get; set; }
    }
}
