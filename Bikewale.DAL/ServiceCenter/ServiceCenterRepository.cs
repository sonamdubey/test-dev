using Bikewale.Entities.Location;
using Bikewale.Entities.service;
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
        public IEnumerable<CityEntityBase> GetServiceCenterCities(uint makeId)
        {
            IList<CityEntityBase> objCityList = null;
            try
            {
                if (makeId > 0)
                {

                    using (DbCommand cmd = DbFactory.GetDBCommand("getservicecenterscitiesbymakeid"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, Convert.ToInt32(makeId)));

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (dr != null)
                            {
                                objCityList = new List<CityEntityBase>();
                                while (dr.Read())
                                {
                                    objCityList.Add(new CityEntityBase
                                    {
                                        CityId = SqlReaderConvertor.ToUInt16(dr["CityId"]),
                                        CityName = Convert.ToString(dr["City"]),
                                        CityMaskingName = Convert.ToString(dr["CityMaskingName"])
                                    });
                                }
                                dr.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetServiceCenterCities");
                objErr.SendMail();
            }

            return objCityList;
        }


    }
}
