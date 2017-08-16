using Bikewale.Notifications;
using BikewaleOpr.DTO.City;
using BikewaleOpr.DTO.ServiceCenter;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ServiceCenter;
using BikewaleOpr.Interface.ServiceCenter;
using BikewaleOpr.Service.AutoMappers.ServiceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace BikewaleOpr.Service.Controllers.ServiceCenter
{
    /// <summary>		
    /// Created By:-Snehal Dange 28 July 2017		
    /// Summary:- For service center details		
    /// </summary>		
    public class ServiceCenterController : ApiController		
    {		
		
        private readonly IServiceCenter _IServiceCenter = null;		
		
		
        public ServiceCenterController(IServiceCenter serviceCenter)
        {		
            _IServiceCenter = serviceCenter;		
        }		
		
        /// <summary>		
        /// Created By:-Snehal Dange 28 July 2017		
        /// Summary:- Return list of cities for particular MakeId		
        /// </summary>		
        /// <param name="makeId"></param>		
        /// <returns></returns>		
        [HttpGet, Route("api/servicecenter/make/{makeId}/cities/"), ResponseType(typeof(IEnumerable<CityBase>))]		
        public IHttpActionResult Get(uint makeId)
        {		
            IEnumerable<CityEntityBase> objcityList = null;		
            if (makeId > 0)		
            {		
                try		
                {		
                    objcityList = _IServiceCenter.GetServiceCenterCities(makeId);		
                    if (objcityList != null)		
                    {		
                        IEnumerable<CityBase> cityList = ServiceCenterMapper.Convert(objcityList);		
                        return Ok(cityList);		
                    }		
		
                }		
                catch (Exception ex)		
                {		
                    Bikewale.Notifications.ErrorClass objErr = new ErrorClass(ex, " BikewaleOpr.Service.Controllers.ServiceCenter.ServiceCenterController.Get");		
                    return InternalServerError();		
                }		
            }		
            else		
            {		
                return BadRequest();		
		
            }		
            return NotFound();		
        }		
		
		
        /// <summary>		
        /// Created By:-Snehal Dange 28 July 2017		
        /// Summary:- Return list of service centers for makeid and cityid with all details.		
        /// </summary>		
        /// <param name="cityId"></param>		
        /// <param name="makeId"></param>		
        /// <returns></returns>		
		
        [HttpGet, Route("api/servicecenter/make/{makeId}/city/{cityId}/active/{activeStatus}/"), ResponseType(typeof(IEnumerable<ServiceCenterBaseDTO>))]		
        public IHttpActionResult GetServiceCenterDetails(uint cityId, uint makeId, sbyte activeStatus)
        {		
            ServiceCenterData objServiceCenter = null;		
            IEnumerable<ServiceCenterDetails> objServiceCenterList = null;		
            IEnumerable<ServiceCenterBaseDTO> serviceCenterList = null;		
		
            if (cityId > 0 && makeId > 0)		
            {		
                try		
                {		
                    objServiceCenter = _IServiceCenter.GetServiceCentersByCityMake(cityId, makeId, activeStatus);		
		
                    if (objServiceCenter.ServiceCenters != null)		
                    {		
                        objServiceCenterList = objServiceCenter.ServiceCenters;		
		
                        if (objServiceCenterList != null)		
                        {		
                            serviceCenterList = ServiceCenterMapper.Convert(objServiceCenterList);		
                            return Ok(serviceCenterList);		
                        }		
		
		
                    }		
		
                }		
                catch (Exception ex)		
                {		
                    ErrorClass objErr = new ErrorClass(ex, " BikewaleOpr.Service.Controllers.ServiceCenter.ServiceCenterController.GetServiceCenterDetailsByCityMake");		
                    return InternalServerError();		
                }		
            }		
            else		
            {		
                return BadRequest();		
            }		
            return NotFound();		
        }		
		
        /// <summary>		
        /// Created By:-Snehal Dange 29 July 2017		
        /// Summary:- Updates the stautus if it is marked as "Active" or "Inactive".		
        /// </summary>		
        /// <param name="serviceCenterId"></param>		
        /// <returns></returns>		
		
        [HttpGet, Route("api/servicecenter/updatestatus/{serviceCenterId}/make/{makeId}/city/{cityId}/")]		
        public IHttpActionResult UpdateServiceCenterStatus(uint cityId, uint makeId, uint serviceCenterId, string currentUserId)
        {		
            		
            bool status = false;
          
            if (serviceCenterId>0)		
            {		
                try		
                {		
                    status = _IServiceCenter.UpdateServiceCenterStatus(cityId, makeId,serviceCenterId, currentUserId);		
                    if(status)		
                    {		
                        return Ok(status);		
                    }		
                }		
                catch (Exception ex)		
                {		
                    ErrorClass objErr = new ErrorClass(ex, " BikewaleOpr.Service.Controllers.ServiceCenter.ServiceCenterController.UpdateServiceCenterStatus");		
                    return InternalServerError();		
                }		
            }		
            else		
            {		
                return BadRequest();		
            }		
            return NotFound();		
        }		
       		
    }
}
