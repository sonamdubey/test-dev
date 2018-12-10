using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AppWebApi.Models;
using AppWebApi.Common;

namespace AppWebApi.Controllers
{
    public class NewCarSearchResultController : ApiController
    {
        
        /* Author: Rakesh Yadav
           Date Created: 14 June 2013 */
        public HttpResponseMessage Get(HttpRequestMessage request, string makes, string budget, string fuelTypes, string bodyTypes, string transmission, string seatingCapacity, string enginePower, string importantFeatures, string pageNo, string pageSize, string sortCriteria, string sortOrder)
        {
            if (CommonOpn.ValidateData(makes, budget, fuelTypes, bodyTypes, transmission, seatingCapacity, enginePower, importantFeatures, sortCriteria, sortOrder) && CommonOpn.CheckId(pageNo) && CommonOpn.CheckId(pageSize) && pageNo.Trim() != "0" && pageSize.Trim() != "0")
            {
                if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
                {
                    string carModelUrl = "";
                    //CarModelUrl = Common.ApiHost + "/api/modeldetails?Budget=" + budget + "&FuelTypes=" + fuelTypes + "&BodyTypes=" + bodyTypes + "&Transmission=" + transmission + "&SeatingCapacity=" + seatingCapacity + "&EnginePower=" + enginePower + "&ImportantFeatures=" + importantFeatures;
                    carModelUrl = CommonOpn.ApiHostUrl + "modeldetails?budget=" + budget + "&fuelTypes=" + fuelTypes + "&bodyTypes=" + bodyTypes + "&transmission=" + transmission + "&seatingCapacity=" + seatingCapacity + "&enginePower=" + enginePower + "&importantFeatures=" + importantFeatures;
                    NewCarSearchResult newCarSearchResult = new NewCarSearchResult(makes, budget, fuelTypes.Replace('4','5'), bodyTypes, transmission, seatingCapacity, enginePower, importantFeatures, pageNo, pageSize, sortCriteria, sortOrder, carModelUrl);

                    if (!newCarSearchResult.ServerErrorOccurred)
                    {
                        if (newCarSearchResult.CarsFound)
                        {
                            return Request.CreateResponse<NewCarSearchResult>(HttpStatusCode.OK, newCarSearchResult);
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cars not found");
                        }
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong on the server");
                    }
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

        [Route("api/v2/NewCarSearchResult/{cityId:int:min(-1)}")]
        public HttpResponseMessage GetV2(HttpRequestMessage request, int cityId, string makes, string budget, string fuelTypes, string bodyTypes, string transmission, string seatingCapacity, string enginePower, string importantFeatures, string pageNo, string pageSize, string sortCriteria, string sortOrder)
        {
            if (CommonOpn.ValidateData(makes, budget, fuelTypes, bodyTypes, transmission, seatingCapacity, enginePower, importantFeatures, sortCriteria, sortOrder) && CommonOpn.CheckId(pageNo) && CommonOpn.CheckId(pageSize) && pageNo.Trim() != "0" && pageSize.Trim() != "0")
            {
                if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
                {
                    NewCarSearchResultV2 newCarSearchResult = new NewCarSearchResultV2(cityId, makes, budget, fuelTypes.Replace('4', '5'), bodyTypes, transmission, seatingCapacity, enginePower, importantFeatures, pageNo, pageSize, sortCriteria, sortOrder);

                    if (!newCarSearchResult.ServerErrorOccurred)
                    {
                        if (newCarSearchResult.CarsFound)
                        {
                            return Request.CreateResponse<NewCarSearchResultV2>(HttpStatusCode.OK, newCarSearchResult);
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cars not found");
                        }
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong on the server");
                    }
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