using Bikewale.Entities.CMS;
using System.Collections.Generic;

namespace Bikewale.Interfaces.CMS
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Interface for News, Road test, features , Tips and Advice , AutoExpo.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    public interface ICMSContentRepository<T,V>
    {        
        IList<T> GetContentList(int startIndex, int endIndex, out int recordCount, ContentFilter filters);
        
        V GetContentDetails(int contentId, int pageId);

        void UpdateViews(int contentId);
        List<CMSFeaturedArticlesEntity> GetFeaturedArticles(List<EnumCMSContentType> contentTypes, ushort totalRecords);
        List<CMSFeaturedArticlesEntity> GetMostRecentArticles(List<EnumCMSContentType> contentTypes, ushort totalRecords);
    }
}
