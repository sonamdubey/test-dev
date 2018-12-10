using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AppWebApi.Models;
using AppWebApi.Common;
using Microsoft.Practices.Unity;

namespace AppWebApi.Controllers
{
    public class ModelDetailsController : ApiController
    {
        private readonly IUnityContainer _container;

        public ModelDetailsController(IUnityContainer container) {
            _container = container;
        }


        /* Author: Rakesh Yadav
           Date Created: 18 June 2013 */
        public HttpResponseMessage Get(HttpRequestMessage request, string budget, string fuelTypes, string bodyTypes, string transmission, string seatingCapacity, string enginePower, string importantFeatures,string modelId)
        {
            if (CommonOpn.ValidateData(budget, fuelTypes, bodyTypes, transmission, seatingCapacity, enginePower, importantFeatures) && CommonOpn.CheckId(modelId))
            {
                if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
                {
                    ModelDetails md = new ModelDetails(_container,budget, fuelTypes, bodyTypes, transmission, seatingCapacity, enginePower, importantFeatures, modelId);

                    if (!md.ServerErrorOccurred)
                    {
                        if (md.CarsFound)
                            return Request.CreateResponse<ModelDetails>(HttpStatusCode.OK, md);
                        else
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Car not found ");
                    }
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong on the server");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
        }
    }
}