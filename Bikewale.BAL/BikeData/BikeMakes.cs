using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UpcomingNotification;
using Bikewale.Interfaces.BikeData;
using Bikewale.Utility;
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

        public BikeDescriptionEntity GetMakeDescription(U makeId)
        {
            BikeDescriptionEntity objDescription = null;
            objDescription = makesRepository.GetMakeDescription(makeId);
            return objDescription;
        }

        public BikeMakeEntityBase GetMakeDetails(uint makeId)
        {
            return makesRepository.GetMakeDetails(makeId);

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
        /// Created by  :   Sumit Kate on 13 Sep 2016
        /// Description :   Gets Makes and their models by calling DAL
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeMakeModelBase> GetAllMakeModels()
        {
            return makesRepository.GetAllMakeModels();
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


        public IEnumerable<BikeMakeEntityBase> GetScooterMakes()
        {
            return makesRepository.GetScooterMakes();
        }
        /// <summary>
        /// Created by  :   Aditi Srivastava on 15 Mar 2017
        /// Description :   Calls make repository
        /// </summary>
        /// <returns></returns>
        public BikeDescriptionEntity GetScooterMakeDescription(uint makeId)
        {
            return makesRepository.GetScooterMakeDescription(makeId);
        }

        /// <summary>
        /// Created by : Snehal Dange on 22nd Nov 2017
        /// Description: Calls GetMakeFooterCategoriesandPrice()
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public MakeSubFooterEntity GetMakeFooterCategoriesandPrice(uint makeId)
        {
            return makesRepository.GetMakeFooterCategoriesandPrice(makeId);
        }

        /// <summary>
        /// Created by : Snehal Dange on 13th Dec 2017
        /// Description : Calls GetDealerBrandsInCity to get list of brands where showroom is present for a particular city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<BikeMakeEntityBase> GetDealerBrandsInCity(uint cityId)
        {
            return makesRepository.GetDealerBrandsInCity(cityId);
        }

        /// <summary>
        /// Created by : Snehal Dange on 14th Dec 2017
        /// Description : Calls GetServiceCenterBrandsInCity to get list of brands where service center is present for a particular city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<BikeMakeEntityBase> GetServiceCenterBrandsInCity(uint cityId)
        {
            return makesRepository.GetServiceCenterBrandsInCity(cityId);
        }

        /// <summary>
        /// Created by : 
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public ResearchMoreAboutMake ResearchMoreAboutMake(uint makeId)
        {
            return makesRepository.ResearchMoreAboutMake(makeId);
        }

        public ResearchMoreAboutMake ResearchMoreAboutMakeByCity(uint makeId, uint cityId)
        {
            return makesRepository.ResearchMoreAboutMakeByCity(makeId, cityId);
        }

        public void ProcessNotification(UpcomingNotificationEntity entityNotif)
        {
            makesRepository.ProcessNotification(entityNotif);

        }

    }   // Class
}   //Namespace
