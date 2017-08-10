﻿using Bikewale.Notifications;
using BikewaleOpr.DTO.Dealers;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface;
using BikewaleOpr.Service.AutoMappers.Dealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers.DealerFacility
{
    /// <summary>
    /// Created By : Snehal Dange on 5th August , 2017
    /// Description : Controller for Dealer Facility
    /// </summary>
    public class DealerFacilityController : ApiController
    {
        private readonly IDealers _dealerRepo;
        public DealerFacilityController(IDealers dealer)
        {
            _dealerRepo = dealer;
        }


        /// <summary>
        /// Created By : Snehal Dange on 5th August , 2017
        /// Description : Method for adding dealer Facility
        /// </summary>
        /// <param name="objDTO"></param>
        /// <returns></returns>
        [HttpPost, Route("api/dealerfacility/add/")]
        public IHttpActionResult AddDealerFacility([FromBody] DealerFacilityDTO objDTO)
        {
            FacilityEntity objEntity = null;
          
            uint newId = 0;
            if (objDTO.Id > 0 && objDTO !=null)
            {
                try
                {
                    objEntity = DealerFacilityMapper.Convert(objDTO);
                    if (objEntity != null)
                    {
                        newId = _dealerRepo.SaveDealerFacility(objEntity);
                        if(newId > 0)
                        {
                            return Ok(newId);
                        }
                        else
                        {
                            return InternalServerError();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, " BikewaleOpr.Service.Controllers.DealerFacility.AddDealerFacility");
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
        /// Created By : Snehal Dange on 5th August , 2017
        /// Description : Method for updating dealer Facility
        /// </summary>
        /// <param name="objDTO"></param>
        /// <returns></returns>
        [HttpPost, Route("api/dealerfacility/update/")]
        public IHttpActionResult UpdateDealerFacility([FromBody] DealerFacilityDTO objDTO)
        {
            FacilityEntity objEntity = null;

            bool status = false;
            if (objDTO.FacilityId > 0 && objDTO != null)
            {
                try
                {
                    objEntity = DealerFacilityMapper.Convert(objDTO);


                    if (objEntity != null)
                    {
                        status = _dealerRepo.UpdateDealerFacility(objEntity);
                        if (status)
                        {
                            return Ok();

                        }


                    }

                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, " BikewaleOpr.Service.Controllers.DealerFacility.AddDealerFacility");
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
