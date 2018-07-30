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
        
        private readonly IBikeSeries<BikeSeriesEntity, int> _seriesRepository = null;
        public SeriesListController(IBikeSeries<BikeSeriesEntity, int> seriesRepository)
        {
            _seriesRepository = seriesRepository;
        }
        
        /// <summary>
        ///  To get List of models for the given series id
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns>Series List</returns>
        [ResponseType(typeof(ModelList))]
        public IHttpActionResult Get(int seriesId)
        {
            List<BikeModelEntityBase> objModelsList = null;
            ModelList objDTOSeriesList = null;
            try
            {
                objModelsList = _seriesRepository.GetModelsListBySeriesId(seriesId);

                if (objModelsList != null && objModelsList.Count > 0)
                {
                    objDTOSeriesList = new ModelList();
                    objDTOSeriesList.Model = SeriesListMapper.Convert(objModelsList);

                    objModelsList.Clear();
                    objModelsList = null;

                    return Ok(objDTOSeriesList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Series.SeriesListController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }   // Get Series List

        /// <summary>
        /// To get list of series based on request Type 
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="seriesId"></param>
        /// <returns>Series List Based on RequestType</returns>
        [ResponseType(typeof(SeriesList))]
        public IHttpActionResult Get(EnumBikeType requestType,int seriesId)
        {
            List<BikeModelEntity> objModelsList = null;
            ModelList objDTOSeriesList = null;
            try
            {
                objModelsList = _seriesRepository.GetModelsList(seriesId);

                if (objModelsList != null && objModelsList.Count > 0)
                {
                    objDTOSeriesList = new ModelList();
                    objDTOSeriesList.Model = SeriesListMapper.Convert(objModelsList);

                    return Ok(objDTOSeriesList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Series.SeriesListController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }   // Get Series List by RequestType


    }//class
} //namespace
