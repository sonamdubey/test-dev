using Bikewale.DTO.PriceQuote;
using Bikewale.Entities.BikeData;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Bikewale.Interfaces.BikeData;
using Bikewale.BAL.BikeData;
namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Bike Makes Controller
    /// </summary>
    public class MakesController : ApiController
    {
        /// <summary>
        /// List of Bike Makes
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<Make>))]
        public IHttpActionResult Get()
        {
            IEnumerable<Make> ctoMakes = null;
            IEnumerable<BikeMakeEntityBase> makes = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakes<BikeMakeEntity, int>>();
                    IBikeMakes<BikeMakeEntity, int> objMake = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                    makes = objMake.GetMakesByType(EnumBikeType.PriceQuote);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (makes != null)
            {
                ctoMakes = from make in makes
                           select new Make() 
                           {
                               MakeId = make.MakeId,
                               MakeName = make.MakeName,
                               MaskingName = make.MaskingName
                           };
                return Ok(ctoMakes);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
