using Bikewale.Entities.CMS.Articles;

namespace Bikewale.Interfaces.News
{
    /// <summary>
    /// Author : Vivek Gupta
    /// Date : 19-07-2016
    /// Desc : GRPC caching for news
    /// </summary>
    public interface INewsCache
    {
        CMSContent GetNews(int _startIndex, int _endIndex, string contentTypeList, int modelid = 0);
    }
}
