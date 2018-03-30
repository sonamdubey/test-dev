using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ServiceCenter;
using BikewaleOpr.Interface.ServiceCenter;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace BikewaleOpr.DALs.ServiceCenter
{
    /// <summary>		
    /// Created By:-Snehal Dange 28 July 2017		
    /// Summary:- For service center related operations		
    /// </summary>		
    public class ServiceCenterRepository : IServiceCenterRepository
    {
        /// <summary>		
        /// Created by:-Snehal Dange 28 July 2017		
        /// Summary:- Get make wise list of cities for service center.		
        /// </summary>		
        /// <param name="makeid"></param>		
        /// <returns></returns>		
        public IEnumerable<CityEntityBase> GetServiceCenterCities(uint makeId)
        {

            IEnumerable<CityEntityBase> objCityList = null;
            try
            {
                if (makeId > 0)
                {

                    using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                    {
                        var param = new DynamicParameters();

                        param.Add("par_makeid", makeId);

                        objCityList = connection.Query<CityEntityBase>("getservicecenterscitiesbymakeid_new_16082017", param: param, commandType: CommandType.StoredProcedure);

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.ServiceCenter.GetServiceCenterCities");

            }
            return objCityList;
        }

        /// <summary>		
        /// Created by:-Snehal Dange 31 July 2017		
        /// Summary:- Get all cities present in database		
        /// </summary>		
        /// <param name="makeid"></param>		


        public IEnumerable<CityEntityBase> GetAllCities()
        {

            IList<CityEntityBase> objCityList = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getcitymappingnames"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objCityList = new List<CityEntityBase>();
                            while (dr.Read())
                            {
                                objCityList.Add(new CityEntityBase
                                {
                                    CityId = SqlReaderConvertor.ToUInt16(dr["id"]),
                                    CityMaskingName = Convert.ToString(dr["citymaskingname"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.ServiceCenter.GetAllCities()");

            }
            return objCityList;
        }


        /// <summary>		
        /// Created by:-Snehal Dange 28 July 2017		
        /// Summary:- Get service centers list by city and makeId on main service center page		
        /// </summary>		
        /// <param name="makeid"></param>		
        /// <returns></returns>		


        public ServiceCenterData GetServiceCentersByCityMake(uint cityId, uint makeId, sbyte activeStatus)
        {
            ServiceCenterData serviceCenters = new ServiceCenterData();
            IEnumerable<ServiceCenterDetails> objServiceCenterList = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("par_cityId", cityId);
                    param.Add("par_makeid", makeId);
                    param.Add("par_activeStatus", activeStatus);

                    objServiceCenterList = connection.Query<ServiceCenterDetails>("getservicecentersbycity_01082017", param: param, commandType: CommandType.StoredProcedure);

                    serviceCenters.Count = (UInt32)objServiceCenterList.Count();
                    serviceCenters.ServiceCenters = objServiceCenterList;

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Error in ServiceCentersRepository.GetServiceCentersByCity for paramerters cityId : {0}, makeId : {1}", cityId, makeId));
            }
            return serviceCenters;
        }



        /// <summary>		
        /// Created by:-Snehal Dange 29 July 2017		
        /// Summary:- To add new service center details		
        /// </summary>		
        /// <param name="serviceCenterDetails"></param>		
        /// <returns></returns>		
        public bool AddUpdateServiceCenter(ServiceCenterCompleteData serviceCenterDetails, string updatedBy)
        {
            int status = 0;
            UInt32 updatedById = Convert.ToUInt32(updatedBy);
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())

                {

                    var param = new DynamicParameters();
                    if (serviceCenterDetails.Id > 0)
                    {
                        param.Add("par_id", serviceCenterDetails.Id);

                    }
                    else
                    {
                        param.Add("par_id", 0);

                    }
                    param.Add("par_name", serviceCenterDetails.Name);
                    param.Add("par_address", serviceCenterDetails.Address);
                    param.Add("par_cityid", serviceCenterDetails.Location.CityId);
                    param.Add("par_stateid", serviceCenterDetails.Location.StateId);
                    param.Add("par_mobile", serviceCenterDetails.Mobile);
                    param.Add("par_phone", serviceCenterDetails.Phone);
                    param.Add("par_pincode", serviceCenterDetails.Pincode);
                    param.Add("par_email", serviceCenterDetails.Email);
                    param.Add("par_lattitude", serviceCenterDetails.Lattitude);
                    param.Add("par_longitude", serviceCenterDetails.Longitude);
                    param.Add("par_makeid", serviceCenterDetails.MakeId);
                    param.Add("par_updatedby", updatedById);
                    status = connection.Execute("saveupdateservicecenter", param: param, commandType: CommandType.StoredProcedure);

                }

            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("Error in ServiceCentersRepository.AddServiceCenter()"));

            }
            return status > 0;
        }




        /// <summary>		
        /// Created by:-Snehal Dange 29 July 2017		
        /// Summary:- To mark the service center details as InActive or Active (similar to delete)		
        /// </summary>		
        /// <param name="Id"></param>		
        public bool UpdateServiceCenterStatus(uint serviceCenterId, string updatedBy)
        {
            int status = 0;
            UInt32 updatedById = Convert.ToUInt32(updatedBy);
            try
            {
                if (serviceCenterId > 0)
                {
                    using (IDbConnection connection = DatabaseHelper.GetMasterConnection())

                    {
                        var param = new DynamicParameters();

                        param.Add("par_id", serviceCenterId);
                        param.Add("par_updatedby", updatedBy);

                        status = connection.Execute("updateservicecenterstatus", param: param, commandType: CommandType.StoredProcedure);

                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Error in ServiceCentersRepository.DeleteServiceCenter()"));
            }
            return status > 0;

        }

        /// <summary>		
        /// Created by:-Snehal Dange 29 July 2017		
        /// Summary:- Get service center data by Id		
        /// </summary>		
        /// <param name="serviceCenterId"></param>		
        /// <returns></returns>		
        public ServiceCenterCompleteData GetDataById(uint serviceCenterId)
        {
            ServiceCenterCompleteData objData = null;
            try
            {
                if (serviceCenterId > 0)
                {
                    using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                    {
                        var param = new DynamicParameters();

                        param.Add("par_serviceCenterId", serviceCenterId);

                        objData = connection.Query<ServiceCenterCompleteData, StateCityEntity, ServiceCenterCompleteData>
                            (
                           "getservicecenterdetails_11082017",

                           (ServiceCenterCompleteData, StateCityEntity) =>
                           {
                               ServiceCenterCompleteData.Location = StateCityEntity;
                               return ServiceCenterCompleteData;
                           }, splitOn: "cityId", param: param, commandType: CommandType.StoredProcedure
                       ).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Error in ServiceCentersRepository.GetDataById()"));
            }
            return objData;
        }


        /// <summary>		
        /// <summary>		
        /// Created by:-Snehal Dange 1 August 2017		
        /// Summary:- Get state details by CityId		
        /// </summary>		
        /// </summary>		
        /// <param name="cityId"></param>		
        /// <returns></returns>		
        public StateCityEntity GetStateDetails(uint cityId)
        {
            IEnumerable<StateCityEntity> objData = null;
            try
            {
                if (cityId > 0)
                {
                    using (IDbConnection connection = DatabaseHelper.GetMasterConnection())

                    {

                        var param = new DynamicParameters();

                        param.Add("par_cityId", cityId);

                        objData = connection.Query<StateCityEntity>("getstatedetailsbycity", param: param, commandType: CommandType.StoredProcedure);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Error in ServiceCentersRepository.GetStateDetails()"));

            }
            return objData.FirstOrDefault();
        }
    }
}
