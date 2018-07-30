using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.PhotoGallery;
using Bikewale.Entities.CMS;

namespace Bikewale.Interfaces.PhotoGallery
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 2 July 2014
    /// Summary : interface for model photo gallery
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface IModelPhotos<T, U>
    {
        List<ModelPhotoEntity> GetModelPhotosList(U modelId, List<EnumCMSContentType> categoryIdList);
        List<ModelPhotoEntity> GetOtherModelPhotosList(int startIndex, int endIndex, int makeId, int modelId,List<EnumCMSContentType> categoryIdList, out int recordCount);
    }
}
