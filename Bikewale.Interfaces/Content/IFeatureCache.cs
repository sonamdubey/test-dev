
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using System.Collections.Generic;
namespace Bikewale.Interfaces.Content
{
    /// <summary>
    /// Author : Vivek Gupta
    /// Created Date: 18-07-2016
    /// </summary>
    public interface IFeatureCache
    {
        ArticlePageDetails GetFeatureDetailsViaGrpc(int basicId);
        IEnumerable<ModelImage> BindPhotos(int basicId);
    }
}
