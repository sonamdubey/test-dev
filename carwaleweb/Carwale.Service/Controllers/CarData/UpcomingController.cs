using Carwale.BL.CarData;
using Carwale.Cache.CarData;
using Carwale.Cache.Core;
using Carwale.DAL.CarData;
using Carwale.DTOs.CarData;
using Carwale.DTOs.NewCars;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Notifications;
using Carwale.Service.Filters;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Carwale.Service.Controllers
{
    public class UpcomingCarController : ApiController
    {
        /// <summary>
        /// Controller written by Rohan.S
        /// BL, DAL ,Cache by Ashwini.T
        /// New Car Launches api
        /// Takes in Pagination class obj as input ..{pageno,pagesize}
        /// </summary>
        /// <returns></returns>
        [PaginationValidator]
        public HttpResponseMessage Get([FromUri]Pagination pagination)
        {
            var response = new HttpResponseMessage();
            UpcomingModels dto = new UpcomingModels();
            try
            {
                UnityContainer container = new UnityContainer();
                container.RegisterType<ICarModelRepository, CarModelsRepository>()
                        .RegisterType<ICarModelCacheRepository, CarModelsCacheRepository>()
                        .RegisterType<ICarModels, CarModelsBL>()
                        .RegisterType<ICacheProvider, MemcacheManager>();

                var bl = container.Resolve<ICarModels>();

                List<UpcomingCarModel> result = bl.GetUpcomingCarModels(pagination);

                if (result.Count < 1)
                {
                    response.Content = new StringContent("[]");
                    response.StatusCode = HttpStatusCode.NoContent;
                    return response;
                }

                dto.UpcomingCarModels = result;
                response.Content = new StringContent(JsonConvert.SerializeObject(dto));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "UpcomingController.Get");
                objErr.LogException();
            }
            return response;
        }
    }
}
