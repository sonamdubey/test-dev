using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.BAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 Apr 2014
    /// Summary : Class have all functions related to the bike versions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeVersions<T, U> : IBikeVersions<T, U> where T : BikeVersionEntity, new()
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

        public List<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew)
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
        /// <param name="cityid"></param>
        /// <returns></returns>
        public IEnumerable<SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint cityid)
        {

            return versionRepository.GetSimilarBikesList(versionId, topCount, cityid);
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
        /// <summary>
        /// Created By: Aditi Srivastava 17 Oct 2016
        /// Description: Get version colors and group hexcodes by color id
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public IEnumerable<BikeColorsbyVersion> GetColorsbyVersionId(uint versionId)
        {
            return versionRepository.GetColorsbyVersionId(versionId);
        }
    }   // Class
}   // namespace
