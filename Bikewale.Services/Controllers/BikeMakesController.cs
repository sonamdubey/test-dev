using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.BAL.BikeData;
using Newtonsoft.Json;

namespace Bikewale.Service.Controllers
{
    public class BikeMakesController : ApiController
    {        
        public string Get()
        {
            string makesList = string.Empty;

            using (IUnityContainer container = new UnityContainer())
            { 
                container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakes<BikeMakeEntity, int>>();
                IBikeMakes<BikeMakeEntity, int> objMakes = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();

                List<BikeMakeEntityBase> objList = objMakes.GetMakesByType(EnumBikeType.All);

                makesList = JsonConvert.SerializeObject(objList);
            }

            return makesList;
        }
    }
}
