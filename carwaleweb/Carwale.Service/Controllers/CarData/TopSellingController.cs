using Carwale.BL.CarData;
using Carwale.Cache.CarData;
using AEPLCore.Cache;
using Carwale.DAL.CarData;
using Carwale.DTOs.CarData;
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
    public class TopSellingController : ApiController
    {
        private readonly ICarModels _carModels;
        public TopSellingController(ICarModels carModels)
        {
            _carModels = carModels;
        }

        /// <summary>
        /// Controller written by Rohan.S
        /// BL, DAL ,Cache by Ashwini.T
        /// TOP SELLING MODELS api
        /// Takes in Pagination class obj as input ..{pageno,pagesize}
        /// Modified by: Rakesh Yadav On 03 Sep 2015
        /// desc:Resolving dependency injection using UnityBootstraper and UnityResolver and send status code using web api 2
        /// </summary>
        /// <returns></returns>
        [PaginationValidator]
        public IHttpActionResult Get([FromUri]Pagination pagination, int cityId = -1)
        {
            TopSellingModels topSellingModels = new TopSellingModels()
            {
                TopSelling = _carModels.GetTopSellingCarModels(pagination, cityId)
            };

            if (topSellingModels.TopSelling.Count == 0)
                return NotFound();
            return ResponseMessage(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(topSellingModels)) });
        }
    }
}
