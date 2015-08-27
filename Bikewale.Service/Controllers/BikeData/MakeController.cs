using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.BikeData;
using Bikewale.Common;
using System.Collections.Generic; 
using Bikewale.BAL.BikeData;
using Microsoft.Practices.Unity;

namespace Bikewale.Service.Controllers.BikeData
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class MakeController : ApiController 
    {
        
        //object for the BikeMakeReposistory
            using(IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeMakes<BikeMakeEntity, int>, IBikeMakes<BikeMakeEntity, int>>();
                IBikeMakes<BikeMakeEntity, int> makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
             }
           

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BikeMakeEntityBase> Get()
        {
            return makesRepository.GetMakesByType(EnumBikeType.All);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestType"></param>
        /// <returns></returns>
        public List<BikeMakeEntityBase> Get(EnumBikeType requestType)
        {
            return makeRepo.GetMakesByType(requestType);
        }

  

    }
}
