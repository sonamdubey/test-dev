using AutoMapper;
using Carwale.DTOs.Campaigns;
using Carwale.Entity.Campaigns;
using Carwale.Interfaces.Campaigns;
using Carwale.Notifications;
using Carwale.Service.Filters.Campaigns;
using Carwale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Carwale.BL.AppSiteAssociation;
using Carwale.Entity.AppSiteAssociation;
using Microsoft.Practices.Unity;
using Carwale.Interfaces.IAppSiteAssociation;
using Carwale.Service;

 /// <summary>
 /// Author: Ajay Singh(on 16 feb 2016)
 /// Description : For IOS purpose
 /// </summary>
      

namespace Carwale.Service.Controllers.AppSiteAssociation
{
    public class AppSiteAssociationController : ApiController
    {
        private readonly IAppSiteAssociationBL _iAppSiteAssociation;
        public AppSiteAssociationController(IAppSiteAssociationBL iAppSiteAssociationBL)
        {
            _iAppSiteAssociation = iAppSiteAssociationBL;
        }
       
        //[HttpGet]
        //[Route("apple-app-site-association")]
        //public IHttpActionResult Get()
        //{
        //    var response = new HttpResponseMessage();
        //    AppSiteAssociationEntity objAppSiteAssociationEntity = new AppSiteAssociationEntity();
        //    try
        //    {
               
        //            objAppSiteAssociationEntity = _iAppSiteAssociation.Get();
        //            return Json(objAppSiteAssociationEntity);
        //    }
        //    catch(Exception ex)
        //    {
        //        ExceptionHandler objErr = new ExceptionHandler(ex, "AppSiteAssociation.Get()");
        //        objErr.LogException();               
        //    }
        //    return InternalServerError();
            
        //}
    }
}
