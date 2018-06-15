using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Interface.BikeData;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace BikewaleOpr.DALs.Bikedata
{
	/// <summary>
	/// Created By : Ashish G. Kamble
	/// Summary : Class have functions to get the data from DB and microservice
	/// </summary>
	public class BikeVersionsRepository : IBikeVersions
	{
		/// <summary>
		/// Written By : Ashish G. Kamble on 12 June 2018
		/// Summary : Function to get basic data for a given version id
		/// </summary>
		/// <param name="versionId">Bike version id</param>
		/// <returns>Returns BikeVersionEntity</returns>
		public BikeVersionEntity GetVersionDetails(uint versionId)
		{
			BikeVersionEntity objBikeVersion = new BikeVersionEntity();

			#region Get data from database
			try
			{
				string sql = String.Format("select bv.Id as VersionId, bv.Name as VersionName, if (bv.imported,true,false) as IsImported, bv.BikeFuelType, bs.id as BodyStyleId from bikeversions bv inner join bikebodystyles bs on bs.id = bv.bodystyleid and bv.id = {0} and bv.isdeleted = 0;", versionId);

				using (DbCommand cmd = DbFactory.GetDBCommand(sql))
				{
					cmd.CommandType = CommandType.Text;

					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null)
						{							
							while (dr.Read())
							{
								objBikeVersion.VersionId = SqlReaderConvertor.ToInt32(Convert.ToString(dr["VersionId"]));
								objBikeVersion.VersionName = Convert.ToString(dr["VersionName"]);
								objBikeVersion.IsImported = SqlReaderConvertor.ToBoolean(dr["IsImported"]);
								objBikeVersion.BikeFuelType = SqlReaderConvertor.ToUInt16(dr["BikeFuelType"]);
								objBikeVersion.BodyStyleId = SqlReaderConvertor.ToUInt16(dr["BodyStyleId"]);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.Bikedata.BikeVersionsRepository.GetVersionDetails_VersionId_{0} - Database Query Call", versionId));
			}
			#endregion

			#region Get specs from microservice using apigateway
			try
			{
				Bikewale.BAL.ApiGateway.ApiGatewayHelper.IApiGatewayCaller caller = new Bikewale.BAL.ApiGateway.ApiGatewayHelper.ApiGatewayCaller();
				
				GetVersionSpecsSummaryByItemIdAdapter adapt = new GetVersionSpecsSummaryByItemIdAdapter();

				Bikewale.BAL.ApiGateway.Entities.BikeData.VersionsDataByItemIds_Input input = new Bikewale.BAL.ApiGateway.Entities.BikeData.VersionsDataByItemIds_Input();
				input.Items = new List<Bikewale.Entities.BikeData.EnumSpecsFeaturesItems>
							{   Bikewale.Entities.BikeData.EnumSpecsFeaturesItems.Displacement,
								Bikewale.Entities.BikeData.EnumSpecsFeaturesItems.TopSpeed,								
								Bikewale.Entities.BikeData.EnumSpecsFeaturesItems.KerbWeight };

				input.Versions = new List<int> { Convert.ToInt32(versionId) };

				adapt.AddApiGatewayCall(caller, input);

				caller.Call();

				IEnumerable<Bikewale.Entities.BikeData.VersionMinSpecsEntity> specsList = adapt.Output;

				if (specsList != null)
				{					
					foreach (var data in specsList)
					{
						foreach (var spec in data.MinSpecsList)
						{
							switch (spec.Id)
							{
								case (int)Bikewale.Entities.BikeData.EnumSpecsFeaturesItems.Displacement:
									objBikeVersion.Displacement = String.IsNullOrEmpty(spec.Value) ? 0 : Convert.ToDouble(spec.Value);
									break;
								case (int)Bikewale.Entities.BikeData.EnumSpecsFeaturesItems.TopSpeed:
									objBikeVersion.TopSpeed = String.IsNullOrEmpty(spec.Value) ? 0 : Convert.ToInt32(spec.Value);
									break;								
								case (int)Bikewale.Entities.BikeData.EnumSpecsFeaturesItems.KerbWeight:
									objBikeVersion.KerbWeight = String.IsNullOrEmpty(spec.Value) ? 0 : Convert.ToDouble(spec.Value);
									break;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.Bikedata.BikeVersionsRepository.GetVersionDetails_VersionId_{0} - Specs Microservice Call", versionId));
			}
			#endregion

			return objBikeVersion;
		}   // End of GetVersionDetails function


		/// <summary>
		/// Created By : Sushil Kumar on  25th Oct 2016
		/// Description : Getting Versions only by providing ModelId and request type
		/// </summary>
		/// <param name="modelId"></param>
		/// <param name="requestType">Pass value as New or Used or Upcoming or PriceQuote</param>
		/// <returns></returns>
		public IEnumerable<BikeVersionEntityBase> GetVersions(uint modelId, string requestType)
		{

			IList<BikeVersionEntityBase> _objBikeVersions = null;

			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand("getbikeversions"))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, requestType));
					cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));

					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null)
						{
							_objBikeVersions = new List<BikeVersionEntityBase>();
							while (dr.Read())
							{
								BikeVersionEntityBase _objModel = new BikeVersionEntityBase();
								_objModel.VersionName = Convert.ToString(dr["Text"]);
								_objModel.VersionId = SqlReaderConvertor.ToInt32(dr["Value"]);
								_objBikeVersions.Add(_objModel);
							}
						}
					}

				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.Bikedata.GetVersions_Model_{0}_RequestType_{1}", modelId, requestType));

			}
			return _objBikeVersions;
		}   // End of GetVersions function

	}	// class
}	// namespace
