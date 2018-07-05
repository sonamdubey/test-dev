using PriceQuoteConsumer.Entities;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Consumer;
using MySql.CoreDAL;
using Newtonsoft.Json;

namespace PriceQuoteConsumer
{
	internal class PriceQuoteRepository
	{
		/// <summary>
		/// Method for saving PriceQuote with GUID and other params in newbikepricequotes table
		/// Created By : Rajan Chauhan on 19 June 2018
		/// </summary>
		/// <param name="pqParams">Price Quote parameters entity</param>
		/// <returns>success/failure status of registerPriceQuote</returns>
		public bool RegisterPriceQuote(PriceQuoteParameters pqParams)
		{
			try
			{

				if (pqParams.VersionId > 0)
				{
					using (DbCommand cmd = DbFactory.GetDBCommand())
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandText = "savepricequote_19062018";
						cmd.Parameters.Add(DbFactory.GetDbParam("par_guid", DbType.String, 40, pqParams.GUID));
						cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int32, pqParams.VersionId));
						cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, pqParams.CityId));
						cmd.Parameters.Add(DbFactory.GetDbParam("par_areaid", DbType.Int32, (pqParams.AreaId > 0) ? pqParams.AreaId : Convert.DBNull));
						cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int32, pqParams.CustomerId));
						cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, 50, pqParams.CustomerName));
						cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, 100, pqParams.CustomerEmail));
						cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, 20, pqParams.CustomerMobile));
						cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, !String.IsNullOrEmpty(pqParams.ClientIP) ? pqParams.ClientIP : null));
						cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.Byte, pqParams.SourceId));
						cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, pqParams.DealerId));
						cmd.Parameters.Add(DbFactory.GetDbParam("par_deviceid", DbType.String, 25, (!String.IsNullOrEmpty(pqParams.DeviceId)) ? pqParams.DeviceId : null));
						cmd.Parameters.Add(DbFactory.GetDbParam("par_utma", DbType.String, 500, (!String.IsNullOrEmpty(pqParams.UTMA)) ? pqParams.UTMA : null));
						cmd.Parameters.Add(DbFactory.GetDbParam("par_utmz", DbType.String, 500, (!String.IsNullOrEmpty(pqParams.UTMZ)) ? pqParams.UTMZ : null));
						cmd.Parameters.Add(DbFactory.GetDbParam("par_pqsourceid", DbType.Byte, (pqParams.PQSourceId.HasValue) ? pqParams.PQSourceId : Convert.DBNull));
						cmd.Parameters.Add(DbFactory.GetDbParam("par_refguid", DbType.String, 40,!String.IsNullOrEmpty(pqParams.RefGUID) ? pqParams.RefGUID : Convert.DBNull)); // RefGUID string 40
						MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
						return true; 
					}
				}
			}
			catch (SqlException ex)
			{
				Logger.LogError(String.Format("PQConsumer.PriceQuoteRepository.RegisterPriceQuote : SqlException {0}", JsonConvert.SerializeObject(pqParams)), ex);
			}
			catch (Exception ex)
			{
				Logger.LogError(String.Format("PQConsumer.PriceQuoteRepository.RegisterPriceQuote : {0}", JsonConvert.SerializeObject(pqParams)), ex);
			}
			return false;
		}

	}
}
