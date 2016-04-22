using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BikewaleOpr.CommuteDistance
{
    /// <summary>
    /// Created By : Lucky Rathore On 12 Apr 2016
    /// Description : To Handle communitcation with DB.
    /// </summary>
    internal class CommuteDistanceRepository
    {

        protected string connectionString = String.Empty;

        /// <summary>
        /// Created By : Lucky Rathore On 12 Apr 2016
        /// Description : Set connectionString
        /// </summary>
        public CommuteDistanceRepository()
        {
            this.connectionString = ConfigurationManager.AppSettings["connectionstring"];
        }

        /// <summary>
        /// Created By : Lucky Rathore On 12 Apr 2016
        /// Description : To Get List of Delaers  whoes area mapping needed to be done.
        /// </summary>
        /// <returns>List of Delaers</returns>
        public IEnumerable<GeoLocationEntity> GetDealer()
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader objReader = null;
            GeoLocationEntity location = null;
            IList<GeoLocationEntity> dealers = null;
            try
            {
                using (connection = new SqlConnection(this.connectionString))
                {
                    using (command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "GetCommuteDistanceDealers";
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        objReader = command.ExecuteReader();
                        if (objReader != null && objReader.HasRows)
                        {
                            dealers = new List<GeoLocationEntity>();
                            while (objReader.Read())
                            {
                                location = new GeoLocationEntity();
                                location.Id = DBNull.Value == objReader["DealerId"] ? default(UInt16) : Convert.ToUInt16(objReader["DealerId"]);
                                location.Latitude = DBNull.Value == objReader["Lattitude"] ? 0 : Convert.ToDouble(objReader["Lattitude"]);
                                location.Longitude = DBNull.Value == objReader["Longitude"] ? 0 : Convert.ToDouble(objReader["Longitude"]);
                                dealers.Add(location);
                            }
                            Debug.WriteLine(String.Format("Total Records fetched : {0}", dealers.Count));
                        }
                        else
                        {
                            Debug.WriteLine("No Records fetched");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetDealer");
                objErr.SendMail();
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return dealers;
        }

        /// <summary>
        /// Created By : Lucky Rathore On 12 Apr 2016
        /// Description : To Get Area of city from which specfied dealer Belong.
        /// </summary>
        /// <param name="dealerId">e.g. 4</param>
        /// <returns>List of Area</returns>
        public IEnumerable<GeoLocationEntity> GetAreaByDealer(UInt16 dealerId, UInt16 leadServingDistance, out GeoLocationEntity dealerLocation)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader objReader = null;
            GeoLocationEntity location = null;
            IList<GeoLocationEntity> areas = null;
            GeoLocationEntity dealerLocationEntity = null;
            try
            {
                using (connection = new SqlConnection(this.connectionString))
                {
                    using (command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "GetAreasByDealer";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@DealerId", SqlDbType.Int).Value = dealerId;
                        command.Parameters.Add("@LeadServingDistance", SqlDbType.TinyInt).Value = Convert.ToByte(leadServingDistance);
                        connection.Open();
                        objReader = command.ExecuteReader();
                        if (objReader != null && objReader.HasRows)
                        {

                            if (objReader.Read())
                            {
                                dealerLocationEntity = new GeoLocationEntity();
                                dealerLocationEntity.Id = DBNull.Value == objReader["DealerId"] ? default(UInt16) : Convert.ToUInt16(objReader["DealerId"]);
                                dealerLocationEntity.Latitude = DBNull.Value == objReader["Lattitude"] ? 0 : Convert.ToDouble(objReader["Lattitude"]);
                                dealerLocationEntity.Longitude = DBNull.Value == objReader["Longitude"] ? 0 : Convert.ToDouble(objReader["Longitude"]);
                            }


                            if (objReader.NextResult())
                            {
                                areas = new List<GeoLocationEntity>();
                                while (objReader.Read())
                                {
                                    location = new GeoLocationEntity();
                                    location.Id = DBNull.Value == objReader["AreaId"] ? default(UInt16) : Convert.ToUInt16(objReader["AreaId"]);
                                    location.Latitude = DBNull.Value == objReader["Lattitude"] ? 0 : Convert.ToDouble(objReader["Lattitude"]);
                                    location.Longitude = DBNull.Value == objReader["Longitude"] ? 0 : Convert.ToDouble(objReader["Longitude"]);
                                    areas.Add(location);
                                }
                                Debug.WriteLine(String.Format("Total Records fetched : {0}", areas.Count));
                            }
                        }
                        else
                        {
                            Debug.WriteLine("No Records fetched");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetAreaByDealer");
                objErr.SendMail();
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                dealerLocation = dealerLocationEntity;
            }
            return areas;
        }

        /// <summary>
        /// Created By : Lucky Rathore On 12 Apr 2016
        /// Description : To update distance.
        /// </summary>
        /// <param name="dealerId">e.g. 4</param>
        /// <param name="areaDistance">e.g. '18890:51,18249:89,17768:79' (areaId:CityId comma seprated pair)</param>
        /// <returns></returns>
        public bool UpdateArea(UInt16 dealerId, string areaDistance)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            int resp = 0;
            try
            {
                using (connection = new SqlConnection(this.connectionString))
                {
                    using (command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "UpdateCommuteDistance";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@DealerId", SqlDbType.Int).Value = dealerId;
                        command.Parameters.Add("@AreaDistance", SqlDbType.VarChar, -1).Value = areaDistance;
                        connection.Open();
                        resp = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UpdateArea");
                objErr.SendMail();
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return resp > 0 ? true : false;
        }
    }
}