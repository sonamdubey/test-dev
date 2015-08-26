using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.Make;
using AutoMapper;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Make
{
    public class MakeListController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestType"></param>
        /// <returns></returns>
        [ResponseType(typeof(MakeList))]
        public HttpResponseMessage Get(EnumBikeType requestType)
        {
            List<BikeMakeEntityBase> objMakeList = null;
            MakeList objDTOMakeList = null;
            using (IUnityContainer container = new UnityContainer())
            {
                IBikeMakes<BikeMakeEntity, int> makesRepository = null;

                container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();

                objMakeList = makesRepository.GetMakesByType(requestType);

                if (objMakeList != null && objMakeList.Count > 0)
                {
                    // Auto map the properties
                    objDTOMakeList = new MakeList();
                    
                    Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                    objDTOMakeList.make = Mapper.Map<List<BikeMakeEntityBase>, List<MakeBase>>(objMakeList);

                    return Request.CreateResponse(HttpStatusCode.OK, objDTOMakeList);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                }
            }
        }   // Get

    }    // Class
}   // Namespace
