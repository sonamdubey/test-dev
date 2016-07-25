using Bikewale.Entities.CMS.Photos;
using System.Collections.Generic;
namespace Bikewale.Interfaces.Content
{
    /// <summary>
    /// Author : Vivek Gupta on 18-07-2016   
    /// </summary>
    public interface IRoadTest
    {
        IEnumerable<ModelImage> GetPhotos(int basicId);
    }
}
