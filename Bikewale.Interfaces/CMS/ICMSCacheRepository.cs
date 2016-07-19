using Bikewale.Entities.CMS.Articles;

namespace Bikewale.Interfaces.CMS
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICMSCacheContent
    {
        ArticleDetails GetNewsDetails(uint basicId);
    }
}
