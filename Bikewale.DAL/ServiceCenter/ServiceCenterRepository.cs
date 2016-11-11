using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
namespace Bikewale.DAL.ServiceCenter
{

    /// <summary>
    /// Created By:-Subodh jain 7 nov 2016
    /// Summary:- For service center locator 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class ServiceCenterRepository<T, U> : IServiceCenterRepository<T, U> where T : ServiceCenterLocatorList, new()
    {
        public U Add(T t)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(T t)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(U id)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<T> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public T GetById(U id)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Created By :-Subodh Jain 7 nov 2016
        /// Summary :- To get cities and state with count of service center
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public ServiceCenterLocatorList GetServiceCenterList(uint makeId)
        {
            IList<ServiceCityEntity> objCityList = null;
            IList<ServiceCenterLocator> objStateList = null;
            ServiceCenterLocatorList objStateCityList = null;
            string makeMaskingname = string.Empty;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getservicecenterscitylist"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.UInt32, makeId));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objCityList = new List<ServiceCityEntity>();
                            if (dr.Read())
                            {
                                makeMaskingname = Convert.ToString(dr["maskingname"]);
                            }

                            if (dr.NextResult())
                            {
                                while (dr.Read())
                                {
                                    objCityList.Add(new ServiceCityEntity
                                    {
                                        CityId = SqlReaderConvertor.ToUInt16(dr["cityid"]),
                                        CityName = Convert.ToString(dr["name"]),
                                        CityMaskingName = Convert.ToString(dr["citymaskingname"]),
                                        Lattitude = Convert.ToString(dr["lattitude"]),
                                        Longitude = Convert.ToString(dr["longitude"]),
                                        ServiceCenterCountCity = SqlReaderConvertor.ToUInt16(dr["servicecnt"]),
                                        stateId = SqlReaderConvertor.ToUInt16(dr["stateid"]),
                                        Link = string.Format("{0}/{1}-service-centers-in-{2}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, makeMaskingname, Convert.ToString(dr["citymaskingname"]))
                                    });
                                }
                            }

                            if (dr.NextResult())
                            {
                                objStateList = new List<ServiceCenterLocator>();
                                while (dr.Read())
                                {
                                    objStateList.Add(new ServiceCenterLocator
                                    {
                                        Id = SqlReaderConvertor.ToUInt16(dr["Id"]),
                                        Lat = Convert.ToString(dr["statelattitude"]),
                                        Long = Convert.ToString(dr["statelongitude"]),
                                        Name = Convert.ToString(dr["name"]),
                                        stateMaskingName = Convert.ToString(dr["statemaskingname"]),
                                        ServiceCenterCountState = SqlReaderConvertor.ToUInt16(dr["statedealercnt"])
                                    });

                                }

                            }

                            // Now statelist and city list is ready
                            // Now iterate on states and find cities for that state
                            // Add the cities for that state
                            if (objStateList != null)
                            {
                                objStateCityList = new ServiceCenterLocatorList();
                                objStateCityList.ServiceCenterDetailsList = objStateList;
                                foreach (var st in objStateCityList.ServiceCenterDetailsList)
                                {
                                    var curStateCityList = from curCity in objCityList
                                                           where curCity.stateId == st.Id
                                                           select curCity;
                                    st.Cities = curStateCityList;
                                }

                                objStateCityList.CityCount = objCityList.Count();
                                objStateCityList.ServiceCenterCount = Convert.ToInt32(objStateCityList.ServiceCenterDetailsList.Sum(m => m.ServiceCenterCountState));
                            }
                            dr.Close();
                        }

                    }
                }


            }


            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("GetServiceCenterList, makeId = {0} ", makeId));
                objErr.SendMail();
            }
            return objStateCityList;
        }

        /// <summary>
        /// Created By : Sajal Gupta on 07/11/2016
        /// Description: DAL layer Function for fetching service center data.
        /// </summary>     
        public ServiceCenterData GetServiceCentersByCity(uint cityId, int makeId)
        {
            ServiceCenterData serviceCenters = null;
            IList<ServiceCenterDetails> objServiceCenterList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getservicecentersbycity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityId", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            serviceCenters = new ServiceCenterData();
                            objServiceCenterList = new List<ServiceCenterDetails>();
                            ServiceCenterDetails objServiceCenterDetails = null;

                            while (dr.Read())
                            {
                                objServiceCenterDetails = new ServiceCenterDetails();
                                objServiceCenterDetails.ServiceCenterId = SqlReaderConvertor.ToUInt32(dr["id"]);
                                objServiceCenterDetails.Name = Convert.ToString(dr["name"]);
                                objServiceCenterDetails.Address = Convert.ToString(dr["address"]);
                                objServiceCenterDetails.Phone = Convert.ToString(dr["phone"]);
                                objServiceCenterDetails.Mobile = Convert.ToString(dr["mobile"]);
                                objServiceCenterList.Add(objServiceCenterDetails);
                            }

                            serviceCenters.ServiceCenters = objServiceCenterList;

                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    serviceCenters.Count = SqlReaderConvertor.ToUInt32(dr["totalServiceCenters"]);
                                }
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Error in ServiceCentersRepository.GetServiceCentersByCity for paramerters cityId : {0}, makeId : {1}", cityId, makeId));
                objErr.SendMail();
            }
            return serviceCenters;
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 09/11/2016
        /// Description: DAL layer Function for fetching Service Schedule data
        /// </summary>
        public IEnumerable<ModelServiceSchedule> GetServiceScheduleByMake(uint makeId)
        {
            ICollection<ModelServiceSchedule> modelSchedules = null;
            ModelServiceSchedule model = null;
            ICollection<ServiceScheduleBase> scheduleList = null;
            ServiceScheduleBase schedule = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getserviceschedulebymake"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            modelSchedules = new List<ModelServiceSchedule>();
                            while (dr.Read())
                            {
                                model = new ModelServiceSchedule();
                                model.Schedules = new List<ServiceScheduleBase>();
                                model.ModelId = SqlReaderConvertor.ToInt32(dr["id"]);
                                model.ModelName = Convert.ToString(dr["bikename"]);
                                modelSchedules.Add(model);
                            }
                            scheduleList = new List<ServiceScheduleBase>();
                            if (dr.NextResult())
                            {
                                while (dr.Read())
                                {
                                    schedule = new ServiceScheduleBase();
                                    ushort curModel = SqlReaderConvertor.ToUInt16(dr["bikemodelid"]);
                                    schedule.ServiceNo = SqlReaderConvertor.ToUInt32(dr["serviceno"]);
                                    schedule.Kms = Convert.ToString(dr["kms"]).Trim();
                                    schedule.Days = SqlReaderConvertor.ToUInt32(dr["days"]);
                                    ModelServiceSchedule selectedModel = modelSchedules.FirstOrDefault(x => x.ModelId == curModel);
                                    if (selectedModel != null)
                                        selectedModel.Schedules.Add(schedule);
                                }
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Error in ServiceCentersRepository.GetServiceScheduleByMake for paramerters makeId : {0}", makeId));
                objErr.SendMail();
            }
            return modelSchedules;
        }

        /// <summary>
        /// Created By : Sajal Gupta on 09/11/2016
        /// Description: DAL layer Function for fetching service center complete data.
        /// </summary>     
        public ServiceCenterCompleteData GetServiceCenterDataById(uint serviceCenterId)
        {
            ServiceCenterCompleteData objServiceCenterCompleteData = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getservicecenterdetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_serviceCenterId", DbType.Int32, serviceCenterId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objServiceCenterCompleteData = new ServiceCenterCompleteData();

                            dr.Read();

                            objServiceCenterCompleteData.Id = serviceCenterId;
                            objServiceCenterCompleteData.Name = Convert.ToString(dr["name"]);
                            objServiceCenterCompleteData.Address = Convert.ToString(dr["address"]);
                            objServiceCenterCompleteData.Phone = Convert.ToString(dr["phone"]);
                            objServiceCenterCompleteData.Mobile = Convert.ToString(dr["mobile"]);
                            objServiceCenterCompleteData.CityId = SqlReaderConvertor.ToUInt32(dr["cityId"]);
                            objServiceCenterCompleteData.CityName = Convert.ToString(dr["cityname"]);
                            objServiceCenterCompleteData.CityMaskingName = Convert.ToString(dr["citymaskingname"]);
                            objServiceCenterCompleteData.StateId = SqlReaderConvertor.ToUInt32(dr["stateId"]);
                            objServiceCenterCompleteData.AreaId = SqlReaderConvertor.ToUInt32(dr["areaId"]);
                            objServiceCenterCompleteData.Pincode = Convert.ToString(dr["pincode"]);
                            objServiceCenterCompleteData.Email = Convert.ToString(dr["email"]);
                            objServiceCenterCompleteData.Lattitude = Convert.ToString(dr["lattitude"]);
                            objServiceCenterCompleteData.Longitude = Convert.ToString(dr["longitude"]);
                            objServiceCenterCompleteData.MakeId = SqlReaderConvertor.ToUInt32(dr["makeId"]);
                            objServiceCenterCompleteData.DealerId = SqlReaderConvertor.ToUInt32(dr["dealerId"]);
                            objServiceCenterCompleteData.IsActive = SqlReaderConvertor.ToUInt32(dr["isActive"]);

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Error in ServiceCenterRepository.GetServiceCenterById for paramerters serviceCenterId : {0}", serviceCenterId));
                objErr.SendMail();
            }
            return objServiceCenterCompleteData;
        }

    }
}
