using Carwale.BL.CarData;
using Carwale.DAL.CarData;
using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web;
using System.Collections.Specialized;
using Carwale.Notifications;
using Newtonsoft.Json;
using Carwale.Cache.CarData;
using AEPLCore.Cache;
using Carwale.Entity;
using Carwale.Interfaces.CarData;
using Carwale.Service.Filters;
using Carwale.Interfaces.CompareCars;
using Carwale.DAL.CompareCars;
using Carwale.Cache.CompareCars;
using Carwale.BL.CompareCars;
using Carwale.Interfaces.SponsoredCar;
using Carwale.BL.SponsoredCar;
using Carwale.DAL.SponsoredCar;
using Carwale.Interfaces.Deals;
using Carwale.BL.Deals;
using Carwale.Interfaces.Deals.Cache;
using Carwale.Cache.Deals;
using Carwale.DAL.Deals;
using Carwale.Interfaces.Dealers;
using Carwale.DAL.Dealers;
using AutoMapper;
using System.Net;
using Carwale.Entity.CompareCars;
using Carwale.DTOs.CarData;
using Carwale.Utility;
using Carwale.Interfaces.NewCars;

namespace Carwale.Service.Controllers
{

    public class CompareCarController : ApiController
    {

        private readonly IUnityContainer _unityContainer;
        private readonly ICompareCarsBL _compareCarsBL;

        public CompareCarController(IUnityContainer unityContainer, ICompareCarsBL compareCarsBL)
        {
            _unityContainer = unityContainer;
            _compareCarsBL = compareCarsBL;
        }

        [AuthenticateBasic]
        public IHttpActionResult GetCompareCarDetails()
        {
            try
            {
                var ccarDataDto = new CCarDataDto();
                bool isFeaturedCar = false;
                List<int> versionCount = new List<int>();

                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

                if (nvc.AllKeys.Contains(nvc["fc"]))
                    isFeaturedCar = true;

                if (!string.IsNullOrWhiteSpace(nvc["vids"].ToString()))
                {
                    versionCount = Utility.ExtensionMethods.ConvertStringToList<int>(nvc["vids"].ToString(), ',');
                }

                if (!(versionCount.Count >= 2 && versionCount.Count <= 4))
                {
                    return BadRequest("Bad parameters");
                }

                IServiceAdapterV2 CompareCarAdapter = _unityContainer.Resolve<IServiceAdapterV2>("CompareCarApp");
                ccarDataDto = CompareCarAdapter.Get<CCarDataDto, List<int>>(versionCount);
                if (ccarDataDto != null)
                    return Ok(ccarDataDto);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CompareCarsController.GetCompareCarDetails()");
                objErr.LogException();
            }
            return InternalServerError();
        }


        /*
        Author:Jitendra solanki
        Date Created: 21 mar 2016
        Desc: Return pagewise list of cars for comparision
         * 
        */
        [HttpGet, Route("api/v1/comparecars/"), AuthenticateBasic]
        public IHttpActionResult GetCompareCarList(string pageNo, string pageSize)
        {
            if (Utility.RegExValidations.IsPositiveNumber(pageNo) && Utility.RegExValidations.IsPositiveNumber(pageSize))
            {
                try
                {
                    CompareCarsDetails _topComparison = _compareCarsBL.GetCompareCarList(new Pagination() { PageNo = Convert.ToUInt16(pageNo), PageSize = Convert.ToUInt16(pageSize) });

                    CompareCarsDetailsDTO _topComparisonDTO = null;
                    _topComparisonDTO = Mapper.Map<CompareCarsDetails, CompareCarsDetailsDTO>(_topComparison);

                    return Ok(_topComparisonDTO);
                }
                catch (Exception ex)
                {
                    ExceptionHandler objErr = new ExceptionHandler(ex, "CompareCarController.GetCompareCarList()");
                    objErr.LogException();

                    return InternalServerError();
                }
            }
            else
            {
                return BadRequest("invalid Parameters");
            }
        }
    }
}
