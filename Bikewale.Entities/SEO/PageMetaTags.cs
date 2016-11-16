
namespace Bikewale.Entities.SEO
{
    /// <summary>
    /// Modified By : Sushil Kumar on 10th Nov 2016
    /// Description : Entity for metas related to seo
    /// </summary>
    public class PageMetaTags
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string AlternateUrl { get; set; }
        public string CanonicalUrl { get; set; }
        public string ShareImage { get; set; }
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
    }
}
