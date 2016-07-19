using Bikewale.Entities.CMS.Articles;

namespace Bikewale.Interfaces.News
{
    /// <summary>
    /// Author : Vivek Gupta
    /// Date : 19-07-2016
    /// Desc : GRPC caching for news
    /// </summary>
    public interface INews
    {
        CMSContent GetNews(int _pageSize, int _pageNumber, string contentTypeList, int modelid = 0);
    }
}
