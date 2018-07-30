using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.PhotoGallery;
using Bikewale.Interfaces.PhotoGallery;
using Bikewale.DAL.PhotoGallery;
using Microsoft.Practices.Unity;
using Bikewale.Entities.CMS;

namespace Bikewale.BAL.PhotoGallery
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 2 July 2014
    /// Summary : Bussiness Layer gor madel photo gallery
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class ModelPhotos<T, U> : IModelPhotos<T, U> where T : ModelPhotoEntity, new()
    {
        private readonly IModelPhotos<T, U> photoRepository = null;

        public ModelPhotos()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IModelPhotos<T, U>, ModelPhotosRespository<T, U>>();
                photoRepository = container.Resolve<IModelPhotos<T, U>>();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 27 June 2014
        /// Summary : To get image list by modelid and categoryid
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        List<ModelPhotoEntity> IModelPhotos<T, U>.GetModelPhotosList(U modelId, List<EnumCMSContentType> categoryIdList)
        {
            List<ModelPhotoEntity> objPhotosList = null;

            objPhotosList = photoRepository.GetModelPhotosList(modelId, categoryIdList);

            return objPhotosList;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 3 July 2014
        /// Summary : to get other model photos list by category id list
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="categoryIdList"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<ModelPhotoEntity> GetOtherModelPhotosList(int startIndex, int endIndex, int makeId, int modelId, List<EnumCMSContentType> categoryIdList, out int recordCount)
        {
            List<ModelPhotoEntity> objPhotosList = null;

            objPhotosList = photoRepository.GetOtherModelPhotosList(startIndex, endIndex, makeId, modelId, categoryIdList, out recordCount);

            return objPhotosList;
        }
    }
}
