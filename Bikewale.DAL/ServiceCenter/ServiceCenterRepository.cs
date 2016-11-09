﻿using Bikewale.Entities.service;
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
                            ServiceCenterDetails objServiceCenterDetails = new ServiceCenterDetails();

                            while (dr.Read())
                            {
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


    }
}
