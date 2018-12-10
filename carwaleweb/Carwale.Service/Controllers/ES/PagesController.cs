using AutoMapper;
using Carwale.DTOs.ES;
using Carwale.Entity.ES;
using Carwale.Interfaces.ES;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Carwale.Service.Controllers.ES
{
    [EnableCors(origins: "http://opr.carwale.com, https://opr.carwale.com, https://oprst.carwale.com, http://webserver:8082", headers: "*", methods: "*")]
     public class PagesController : ApiController
     {
         private readonly IPagesCache _pageCache;
         public PagesController(IPagesCache pageCache)
         {
             _pageCache = pageCache;
         }
 
         [HttpGet, Route("api/pages/properties/")]
         public IHttpActionResult GetPagesAndProperties(int applicationId, int platformId)
         {
             try
             {
                 if (applicationId < 1 || platformId < 1)
                     return BadRequest("Either applicationId or platformId is invalid");
 
                 var response = _pageCache.GetPagesAndPropertiesCache(applicationId, platformId);
 
                 var finalResponse = Mapper.Map<List<Pages>, List<PagesDto>>(response);
                 return Ok(finalResponse);
             }
             catch (Exception ex)
             {
                 Logger.LogException(ex);
                 return InternalServerError();
             }
         }
 
     }
 }