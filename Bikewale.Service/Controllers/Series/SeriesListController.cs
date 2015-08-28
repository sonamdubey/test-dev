using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.Series;
using Bikewale.DTO.Model;
using AutoMapper;
using System.Web.Http.Description;
using Bikewale.Service.AutoMappers.Series;
using Bikewale.Notifications;

namespace Bikewale.Service.Controllers.Series
{
    /// <summary>
    /// To Get List of Series 
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class SeriesListController : ApiController
    {
        /// <summary>
        ///  To get List of Series 
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns>Series List</returns>
        [ResponseType(typeof(SeriesList))]
        public HttpResponseMessage Get(int seriesId)
        {
            List<BikeModelEntityBase> objModelsList = null;
            ModelList objDTOSeriesList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IBikeSeries<BikeSeriesEntity, int> seriesRepository = null;

                    container.RegisterType<IBikeSeries<BikeSeriesEntity, int>, BikeSeriesRepository<BikeSeriesEntity, int>>();
                    seriesRepository = container.Resolve<IBikeSeries<BikeSeriesEntity, int>>();


                    objModelsList = seriesRepository.GetModelsListBySeriesId(seriesId);

                    if (objModelsList != null && objModelsList.Count > 0)
                    {
                        objDTOSeriesList = new ModelList();
                        objDTOSeriesList.Model = SeriesEntityToDTO.ConvertModelList(objModelsList);
                        return Request.CreateResponse(HttpStatusCode.OK, objDTOSeriesList);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Series.SeriesListController");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "OOps ! Some error occured.");
            }
        }   // Get Series List

        /// <summary>
        /// To get list of series based on request Type 
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="seriesId"></param>
        /// <returns>Series List Based on RequestType</returns>
        [ResponseType(typeof(SeriesList))]
        public HttpResponseMessage Get(EnumBikeType requestType,int seriesId)
        {
            List<BikeModelEntity> objModelsList = null;
            ModelList objDTOSeriesList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IBikeSeries<BikeSeriesEntity, int> seriesRepository = null;

                    container.RegisterType<IBikeSeries<BikeSeriesEntity, int>, BikeSeriesRepository<BikeSeriesEntity, int>>();
                    seriesRepository = container.Resolve<IBikeSeries<BikeSeriesEntity, int>>();


                    objModelsList = seriesRepository.GetModelsList(seriesId);

                    if (objModelsList != null && objModelsList.Count > 0)
                    {
                        objDTOSeriesList = new ModelList();
                        objDTOSeriesList.Model = SeriesEntityToDTO.ConvertModelList(objModelsList);

                        return Request.CreateResponse(HttpStatusCode.OK, objDTOSeriesList);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Series.SeriesListController");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "OOps ! Some error occured.");
            }
        }   // Get Series List by RequestType


    }//class
} //namespace
