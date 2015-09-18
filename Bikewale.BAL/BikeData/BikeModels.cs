using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using System.Collections;

namespace Bikewale.BAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 Apr 2014
    /// Summary : 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeModels<T,U> : IBikeModels<T,U> where T : BikeModelEntity, new()
    {
        private readonly IBikeModelsRepository<T, U> modelRepository = null;

        public BikeModels()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeModelsRepository<T, U>, BikeModelsRepository<T, U>>();
                modelRepository = container.Resolve<IBikeModelsRepository<T, U>>();
            }
        }

        public List<BikeModelEntityBase> GetModelsByType(EnumBikeType requestType, int makeId)
        {
            List<BikeModelEntityBase> objModelList = null;

            objModelList = modelRepository.GetModelsByType(requestType, makeId);

            return objModelList;
        }

        /// <summary>
        /// Modified By : Sadhana Upadhyay on 20 Aug 
        /// Summary : to retrieve version list for new as well as discontinued bikes
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public List<BikeVersionsListEntity> GetVersionsList(U modelId,bool isNew)
        {
            List<BikeVersionsListEntity> objVersionList = null;

            objVersionList = modelRepository.GetVersionsList(modelId, isNew);

            return objVersionList;
        }

        public BikeDescriptionEntity GetModelSynopsis(U modelId)
        {
            BikeDescriptionEntity objDesc = null;

            objDesc = modelRepository.GetModelSynopsis(modelId);

            return objDesc;
        }

        public UpcomingBikeEntity GetUpcomingBikeDetails(U modelId)
        {
            UpcomingBikeEntity objUpcomingBike = null;

            objUpcomingBike = modelRepository.GetUpcomingBikeDetails(modelId);

            return objUpcomingBike;
        }

        public U Add(T t)
        {
            throw new NotImplementedException();
        }

        public bool Update(T t)
        {
            throw new NotImplementedException();
        }

        public bool Delete(U id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(U id)
        {
            T t = modelRepository.GetById(id);

            return t;
        }

        /// <summary>
        /// Created By : Ashish G. Kamble on 12 May 2014
        /// Summary : 
        /// </summary>
        /// <param name="inputParams">Start Index and End Index are mandetory.</param>
        /// <param name="sortBy">Optional. To get all results set default.</param>
        /// <param name="recordCount">Record count</param>
        /// <returns></returns>
        public List<UpcomingBikeEntity> GetUpcomingBikesList(UpcomingBikesListInputEntity inputParams, EnumUpcomingBikesFilter sortBy, out int recordCount)
        {
            List<UpcomingBikeEntity> objUpcomingBikeList = null;

            objUpcomingBikeList = modelRepository.GetUpcomingBikesList(inputParams, sortBy, out recordCount);

            return objUpcomingBikeList;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 4 June 2014
        /// Summary : To get all recently launched bikes
        /// </summary>
        /// <param name="startIndex">Start Index</param>
        /// <param name="endIndex">End Index</param>
        /// <param name="recordCount">Record Count</param>
        /// <returns></returns>
        public List<NewLaunchedBikeEntity> GetNewLaunchedBikesList(int startIndex, int endIndex, out int recordCount)
        {
            List<NewLaunchedBikeEntity> objNewLaunchedBikeList = null;

            objNewLaunchedBikeList = modelRepository.GetNewLaunchedBikesList(startIndex, endIndex, out recordCount);

            return objNewLaunchedBikeList;
        }


        public Hashtable GetMaskingNames()
        {
            throw new NotImplementedException();
        }

        public Hashtable GetOldMaskingNames()
        {
            throw new NotImplementedException();
        }


        public List<FeaturedBikeEntity> GetFeaturedBikes(uint topRecords)
        {
            throw new NotImplementedException();
        }
    }   // Class
}   // namespace
