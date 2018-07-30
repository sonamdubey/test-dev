
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using System.Collections.Generic;
namespace Bikewale.Interfaces.Content
{
    /// <summary>
    /// Author : Vivek Gupta on 18-07-2016, 
    /// Desc : to cache features and photos 
    /// </summary>
    public interface IFeatures
    {
        ArticlePageDetails GetFeatureDetails(int basicId);
        IEnumerable<ModelImage> Photos(int basicId);
    }
}
