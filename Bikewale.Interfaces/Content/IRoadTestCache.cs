
using Bikewale.Entities.CMS.Photos;
using System.Collections.Generic;
namespace Bikewale.Interfaces.Content
{
    /// <summary>
    /// Author : Vivek Gupta on 18-07-2016
    /// Desc: for caching and used to fetch photos
    /// </summary>
    public interface IRoadTestCache
    {
        IEnumerable<ModelImage> BindPhotos(int basicId);
    }
}
