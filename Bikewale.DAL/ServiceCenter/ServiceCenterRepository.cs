using Bikewale.Entities.Location;
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
                                        Link = string.Format("{0}/service-centers/{1}/{2}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, makeMaskingname, Convert.ToString(dr["citymaskingname"]))
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
                                    if (curStateCityList != null)
                                    {
                                        st.Cities = curStateCityList.ToList();
                                    }
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
                ErrorClass.LogError(ex, String.Format("GetServiceCenterList, makeId = {0} ", makeId));

            }
            return objStateCityList;
        }
        /// <summary>
        /// Created by:-Subodh Jain 7 nov 2016
        /// Summary:- Get make wise list of cities for service center
        /// </summary>
        /// <param name="makeid"></param>
        /// <returns></returns>
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
                ErrorClass.LogError(ex, "GetServiceCenterCities");

            }
            return objCityList;
        }
        /// <summary>
        /// Created By : Sajal Gupta on 07/11/2016
        /// Description: DAL layer Function for fetching service center data.
        /// Modified by : Sajal Gupta on 03-04-2017
        /// Description : Added CityMaskingName, MakeMaskingName
        /// </summary>     
        public ServiceCenterData GetServiceCentersByCity(uint cityId, int makeId)
        {
            ServiceCenterData serviceCenters = null;
            IList<ServiceCenterDetails> objServiceCenterList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getservicecentersbycity_03042017"))
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
                                objServiceCenterDetails.Lattitude = Convert.ToString(dr["lattitude"]);
                                objServiceCenterDetails.Longitude = Convert.ToString(dr["longitude"]);
                                objServiceCenterDetails.CityMaskingName = Convert.ToString(dr["CityMaskingName"]);
                                objServiceCenterDetails.MakeMaskingName = Convert.ToString(dr["makeMaskingName"]);
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
                ErrorClass.LogError(ex, string.Format("Error in ServiceCentersRepository.GetServiceCentersByCity for paramerters cityId : {0}, makeId : {1}", cityId, makeId));

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
                        if (dr != null && dr.Read())
                        {

                            if (Convert.ToBoolean(dr["isdataavailable"]))
                            {
                                if (dr.NextResult())
                                {
                                    modelSchedules = new List<ModelServiceSchedule>();
                                    while (dr.Read())
                                    {
                                        model = new ModelServiceSchedule();
                                        model.Schedules = new List<ServiceScheduleBase>();
                                        model.ModelId = SqlReaderConvertor.ToInt32(dr["id"]);
                                        model.ModelName = Convert.ToString(dr["bikename"]);
                                        model.HostUrl = Convert.ToString(dr["HostUrl"]);
                                        model.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                        modelSchedules.Add(model);
                                    }
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
                                        schedule.Days = Convert.ToString(dr["days"]);
                                        ModelServiceSchedule selectedModel = modelSchedules.FirstOrDefault(x => x.ModelId == curModel);
                                        if (selectedModel != null)
                                            selectedModel.Schedules.Add(schedule);
                                    }
                                }
                            }

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Error in ServiceCentersRepository.GetServiceScheduleByMake for paramerters makeId : {0}", makeId));

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

                            if (dr.Read())
                            {
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
                                objServiceCenterCompleteData.Lattitude = SqlReaderConvertor.ParseToDouble(dr["lattitude"]);
                                objServiceCenterCompleteData.Longitude = SqlReaderConvertor.ParseToDouble(dr["longitude"]);
                                objServiceCenterCompleteData.MakeId = SqlReaderConvertor.ToUInt32(dr["makeId"]);
                                objServiceCenterCompleteData.DealerId = SqlReaderConvertor.ToUInt32(dr["dealerId"]);
                                objServiceCenterCompleteData.IsActive = SqlReaderConvertor.ToUInt32(dr["isActive"]);

                                dr.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Error in ServiceCenterRepository.GetServiceCenterById for paramerters serviceCenterId : {0}", serviceCenterId));

            }
            return objServiceCenterCompleteData;
        }

        /// <summary>
        /// Created By : Sajal Gupta on 15/11/2016
        /// Description: DAL layer Function for fetching service center details for sending sms.
        /// </summary>     
        public ServiceCenterSMSData GetServiceCenterSMSData(uint serviceCenterId, string mobileNumber)
        {
            ServiceCenterSMSData objSMSData = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getservicecenteraddress"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_servicecenterid", DbType.Int32, serviceCenterId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobilenumber", DbType.String, mobileNumber));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objSMSData = new ServiceCenterSMSData();
                            int status;
                            if (dr.Read())
                            {
                                status = SqlReaderConvertor.ToUInt16(dr["status"]);
                                if (status == 1)
                                {
                                    if (dr.NextResult() && dr.Read())
                                    {
                                        objSMSData.SMSStatus = EnumServiceCenterSMSStatus.Success;
                                        objSMSData.Name = Convert.ToString(dr["name"]);
                                        objSMSData.Address = Convert.ToString(dr["address"]);
                                        objSMSData.Phone = Convert.ToString(dr["phone"]);
                                        objSMSData.CityId = SqlReaderConvertor.ToUInt32(dr["cityId"]);
                                        objSMSData.CityName = Convert.ToString(dr["cityname"]);
                                        dr.Close();
                                    }
                                }
                                else if (status == 2)
                                {
                                    objSMSData.SMSStatus = EnumServiceCenterSMSStatus.Daily_Limit_Exceeded;
                                }
                                dr.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Error in ServiceCenterRepository.GetServiceCenterSMSData for parameters serviceCenterId : {0}, mobileNumber : {1}", serviceCenterId, mobileNumber));

            }
            return objSMSData;
        }
        /// <summary>
        /// Created By  : Aditi Srivastava on 15 Dec 2016
        /// Description : To get number of service centers by brand
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BrandServiceCenters> GetAllServiceCentersByBrand()
        {
            IList<BrandServiceCenters> listServiceCenter = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getallservicecentersbymake"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            BrandServiceCenters objServiceCenters = null;
                            listServiceCenter = new List<BrandServiceCenters>();
                            while (dr.Read())
                            {


                                objServiceCenters = new BrandServiceCenters();
                                objServiceCenters.MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                objServiceCenters.MakeName = Convert.ToString(dr["MakeName"]);
                                objServiceCenters.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objServiceCenters.ServiceCenterCount = SqlReaderConvertor.ToInt32(dr["ServiceCenterCount"]);
                                listServiceCenter.Add(objServiceCenters);
                            }
                            dr.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Error in ServiceCenterRepository.GetAllServiceCentersByBrand");

            }
            return listServiceCenter;

        }
        /// <summary>
        /// Created By  : Aditi Srivastava on 19 Dec 2016
        /// Description : To get number of service centers by brand in nearby cities
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<CityBrandServiceCenters> GetServiceCentersNearbyCitiesByBrand(int cityId, int makeId, int topCount)
        {
            IList<CityBrandServiceCenters> listServiceCenter = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getservicecentersfornearbycities_31032017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, topCount));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            CityBrandServiceCenters objServiceCenters = null;
                            listServiceCenter = new List<CityBrandServiceCenters>();
                            while (dr.Read())
                            {
                                objServiceCenters = new CityBrandServiceCenters();
                                objServiceCenters.CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]);
                                objServiceCenters.CityName = Convert.ToString(dr["CityName"]);
                                objServiceCenters.CityMaskingName = Convert.ToString(dr["CityMaskingName"]);
                                objServiceCenters.ServiceCenterCount = SqlReaderConvertor.ToInt32(dr["ServiceCenterCount"]);
                                objServiceCenters.Lattitude = SqlReaderConvertor.ToFloat(dr["Lattitude"]);
                                objServiceCenters.Longitude = SqlReaderConvertor.ToFloat(dr["Longitude"]);
                                objServiceCenters.GoogleMapImg = Convert.ToString(dr["googlemapimgurl"]);
                                listServiceCenter.Add(objServiceCenters);
                            }
                            dr.Close();

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Error in ServiceCenterRepository.GetServiceCentersNearbyCitiesByBrand for paramerters cityId : {0},makeId: {1},topCount: {2}", cityId, makeId, topCount));

            }
            return listServiceCenter;
        }

    }
}
