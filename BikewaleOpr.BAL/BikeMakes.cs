using Bikewale.Notifications;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.BAL
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 1st Aug 2017
    /// Description : BAL Layer for Bike Makes opr functions.
    /// </summary>
    public class BikeMakes : IBikeMakes
    {
        private readonly IBikeMakesRepository _bikeMakesRepository;

        public BikeMakes(IBikeMakesRepository bikeMakesRepository)
        {
            _bikeMakesRepository = bikeMakesRepository;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 1st Aug 2017
        /// Description : To fetch bike models for given model id from DAL
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<BikeModelEntityBase> GetModelsByMake(EnumBikeType requestType, uint makeId)
        {
            IEnumerable<BikeModelEntityBase> objBikeModelEntityBaseList = null;
            try
            {
                if (makeId > 0)
                {
                    objBikeModelEntityBaseList = _bikeMakesRepository.GetModelsByMake(requestType, makeId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.BikeMakes.GetModelsByMake_{0}_{1}", requestType, makeId));
            }
            return objBikeModelEntityBaseList;
        }


        public IEnumerable<BikeMakeEntityBase> GetMakes(ushort requestType)
        {
            IEnumerable<BikeMakeEntityBase> objMakes = null;
            try
            {
                objMakes = _bikeMakesRepository.GetMakes(requestType);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.BikeMakes.GetMakes_{0}", requestType));
            }
            return objMakes;
        }



        /// <summary>
        /// Gets the make details by identifier.
        /// Author: Sangram Nandkhile on 08 Dec 2017
        /// </summary>
        /// <param name="makeId">The make identifier.</param>
        /// <returns></returns>
        public BikeMakeEntity GetMakeDetailsById(uint makeId)
        {
            BikeMakeEntity objMake = null;
            try
            {
                objMake = _bikeMakesRepository.GetMakeDetailsById(makeId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.BikeMakes.objMake_MakeId:{0}", makeId));
            }
            return objMake;
        }
    }
}
