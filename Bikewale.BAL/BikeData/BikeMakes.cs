﻿using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.BAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 Apr 2014
    /// Summary : Class have all functions related to the bike makes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeMakes<T, U> : IBikeMakes<T, U> where T : BikeMakeEntity, new()
    {
        private readonly IBikeMakes<T, U> makesRepository = null;

        /// <summary>
        /// Constructor to initialize the required class level parameters.
        /// </summary>
        public BikeMakes()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeMakes<T, U>, BikeMakesRepository<T, U>>();
                makesRepository = container.Resolve<IBikeMakes<T, U>>();
            }
        }

        public List<BikeMakeEntityBase> GetMakesByType(EnumBikeType requestType)
        {
            List<BikeMakeEntityBase> objMakeList = null;

            objMakeList = makesRepository.GetMakesByType(requestType);

            return objMakeList;
        }

        public List<BikeModelsListEntity> GetModelsList(U makeId)
        {
            List<BikeModelsListEntity> objModelList = null;

            objModelList = makesRepository.GetModelsList(makeId);

            return objModelList;
        }

        public BikeDescriptionEntity GetMakeDescription(U makeId)
        {
            BikeDescriptionEntity objDescription = null;
            objDescription = makesRepository.GetMakeDescription(makeId);
            return objDescription;
        }

        public BikeMakeEntityBase GetMakeDetails(string makeId)
        {
            BikeMakeEntityBase objMakeList = null;

            objMakeList = makesRepository.GetMakeDetails(makeId);

            return objMakeList;
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
            T t = makesRepository.GetById(id);

            return t;
        }


        public IEnumerable<BikeMakeEntityBase> UpcomingBikeMakes()
        {
            return makesRepository.UpcomingBikeMakes();
        }

        public IEnumerable<BikeVersionEntity> GetDiscontinuedBikeModelsByMake(uint makeId)
        {
            return makesRepository.GetDiscontinuedBikeModelsByMake(makeId);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 16 Sep 2016
        /// Description :   Calls make repository
        /// </summary>
        /// <returns></returns>
        public System.Collections.Hashtable GetOldMaskingNames()
        {
            return makesRepository.GetOldMaskingNames();
        }        
    }   // Class
}   //Namespace
