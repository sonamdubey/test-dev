using System;

namespace Carwale.Entity.CMS.URIs
{
    public class VideosByIdURI
    {
        public int ModelId { get; set; }
        public ushort ApplicationId { get; set; }
        public uint StartIndex { get; set; }
        public uint EndIndex { get; set; }
        public string SimilarModels { get; set; }
        public string BodyStyleIds { get; set; }
    }
}
