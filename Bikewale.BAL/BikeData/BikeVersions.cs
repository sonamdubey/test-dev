using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.DAL.BikeData;

namespace Bikewale.BAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 Apr 2014
    /// Summary : Class have all functions related to the bike versions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeVersions<T,U> : IBikeVersions<T,U> where T : BikeVersionEntity, new()
    {
        private IBikeVersions<T, U> versionRepository = null;

        public BikeVersions()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeVersions<T, U>, BikeVersionsRepository<T, U>>();
                versionRepository = container.Resolve<IBikeVersions<T, U>>();
            }
        }

        /// <summary>
        /// Modified By : Sadhana Upadhyay on 25 Aug 2014
        /// Summary : Changed return type to get price
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public List<BikeVersionsListEntity> GetVersionsByType(EnumBikeType requestType, int modelId, int? cityId = null)
        {
            List<BikeVersionsListEntity> objVersionList = null;

            objVersionList = versionRepository.GetVersionsByType(requestType, modelId, cityId);

            return objVersionList;
        }

        public BikeSpecificationEntity GetSpecifications(U versionId)
        {
            BikeSpecificationEntity objVersion = null;

            objVersion = versionRepository.GetSpecifications(versionId);

            return objVersion;
        }

        public List<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId,bool isNew)
        {
            List<BikeVersionMinSpecs> objMVSpecsMin = null;
            objMVSpecsMin = versionRepository.GetVersionMinSpecs(modelId, isNew);
            return objMVSpecsMin;
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
            T t = versionRepository.GetById(id);

            return t;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 5th Aug 2014
        /// Summary : To get list of similar bikes by version id
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="topCount"></param>
        /// <param name="percentDeviation"></param>
        /// <returns></returns>
        public List<SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint percentDeviation)
        {
            List<SimilarBikeEntity> objSimilarBikes = null;

            objSimilarBikes = versionRepository.GetSimilarBikesList(versionId, topCount, percentDeviation);

            return objSimilarBikes;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 4 Dec 2014
        /// Summary : get version color by version id
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public List<VersionColor> GetColorByVersion(U versionId)
        {
            List<VersionColor> objColors = null;

            objColors = versionRepository.GetColorByVersion(versionId);

            return objColors;
        }
    }   // Class
}   // namespace
