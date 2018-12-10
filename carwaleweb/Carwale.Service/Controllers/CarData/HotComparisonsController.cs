using Carwale.BL.CarData;
using Carwale.BL.CompareCars;
using Carwale.BL.SponsoredCar;
using Carwale.Cache.CarData;
using Carwale.Cache.CompareCars;
using AEPLCore.Cache;
using Carwale.DAL.CarData;
using Carwale.DAL.CompareCars;
using Carwale.DAL.SponsoredCar;
using Carwale.DTOs.CarData;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.SponsoredCar;
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
    public class HotComparisonsController : ApiController
    {
        private readonly ICompareCarsBL _compareCar;

        public HotComparisonsController(ICompareCarsBL compareCar)
        {
            _compareCar = compareCar;
        }

        /// <summary>
        /// Controller written by Rohan.S
        /// BL, DAL ,Cache by Ashwini.T
        /// HOT CAR COMPARE LISTING api
        /// Takes in Pagination class obj as input ..{pageno,pagesize}
        /// Modified by: Rakesh Yadav 
        /// desc:Resolving dependency injection using UnityBootstraper and UnityResolver and send status code using web api 2
        /// </summary>
        /// <returns></returns>
        [PaginationValidator]
        public IHttpActionResult Get([FromUri]Pagination pagination)
        {
            var hotCarComparison = _compareCar.GetHotComaprisons(pagination);

            HotCarComparisons dto = new HotCarComparisons();
            dto.HotComparisons = hotCarComparison;

            if (hotCarComparison.Count == 0)
                return NotFound();

            return ResponseMessage(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(dto)) });
        }
    }
}
