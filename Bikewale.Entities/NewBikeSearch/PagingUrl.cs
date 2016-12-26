using System;

namespace Bikewale.Entities.NewBikeSearch
{
    [Serializable]
    public class PagingUrl
    {
        public string PrevPageUrl { get; set; }
        public string NextPageUrl { get; set; }
    }
}
