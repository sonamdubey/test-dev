using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.GenericBikes;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.GenericBikes
{
    /// <summary>
    /// Created By : Sushil Kumar on 2nd Jan 2016
    /// Description :  Generic Bike repository
    /// Modified By : Sushil Kumar on 5th Jan 2016
    /// Description : To get generic bike info with min specs
    /// Modified by : Aditi Srivastava on 12 Jan 2017
    /// Description : Added method to get bike ranking by category
    /// </summary>
    public class GenericBikeRepository : IGenericBikeRepository
    {

        /// <summary>
        /// Created By : Sushil Kumar on 2nd Jan 2016
        /// Summary :  To get generic bike info by modelid
        /// Modified By : Sushil Kumar on 5th Jan 2016
        /// Description : To get generic bike info with min specs
        /// </summary>
        /// <returns></returns>
        public Entities.GenericBikes.GenericBikeInfo GetGenericBikeInfo(uint modelId)
        {
            GenericBikeInfo genericBikeInfo = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getgenericbikeinfo";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                            {
                                genericBikeInfo = new GenericBikeInfo();
                                genericBikeInfo.Make = new Entities.BikeData.BikeMakeEntityBase();
                                genericBikeInfo.Model = new Entities.BikeData.BikeModelEntityBase();
                                genericBikeInfo.MinSpecs = new MinSpecsEntity();
                                genericBikeInfo.OriginalImagePath = Convert.ToString(dr["originalimagepath"]);
                                genericBikeInfo.HostUrl = Convert.ToString(dr["hosturl"]);
                                genericBikeInfo.VideosCount = SqlReaderConvertor.ToUInt32(dr["videoscount"]);
                                genericBikeInfo.NewsCount = SqlReaderConvertor.ToUInt32(dr["newscount"]);
                                genericBikeInfo.PhotosCount = SqlReaderConvertor.ToUInt32(dr["photoscount"]);
                                genericBikeInfo.ExpertReviewsCount = SqlReaderConvertor.ToUInt32(dr["expertreviewscount"]);
                                genericBikeInfo.FeaturesCount = SqlReaderConvertor.ToUInt32(dr["featurescount"]);
                                genericBikeInfo.IsSpecsAvailable = SqlReaderConvertor.ToBoolean(dr["isspecsavailable"]);
                                genericBikeInfo.BikePrice = SqlReaderConvertor.ToUInt32(dr["price"]);
                                genericBikeInfo.Make.MakeName = Convert.ToString(dr["makename"]);
                                genericBikeInfo.Make.MaskingName = Convert.ToString(dr["makemaskingname"]);
                                genericBikeInfo.Model.ModelName = Convert.ToString(dr["modelname"]);
                                genericBikeInfo.Model.MaskingName = Convert.ToString(dr["modelmaskingname"]);
                                genericBikeInfo.MinSpecs.Displacement = SqlReaderConvertor.ToNullableFloat(dr["displacement"]);
                                genericBikeInfo.MinSpecs.FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(dr["fuelefficiencyoverall"]);
                                genericBikeInfo.MinSpecs.MaxPower = SqlReaderConvertor.ToNullableFloat(dr["maxpower"]);
                                genericBikeInfo.MinSpecs.MaximumTorque = SqlReaderConvertor.ToNullableFloat(dr["maxpowerrpm"]);
                                genericBikeInfo.MinSpecs.KerbWeight = SqlReaderConvertor.ToNullableUInt16(dr["kerbweight"]);


                            }
                            dr.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, "GenericBikeRepository.GetGenericBikeInfo");
            }
            return genericBikeInfo;
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 12 Jan 2017
        /// Description : To get bike rankings by category
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeRankingEntity GetBikeRankingByCategory(uint modelId)
        {
            BikeRankingEntity bikeRankObj = null;
            EnumBikeBodyStyles bodyStyle;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getbikerankingbymodel";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                            {
                                bikeRankObj = new BikeRankingEntity();
                                bikeRankObj.Rank = SqlReaderConvertor.ToInt32(dr["Rank"]);
                                Enum.TryParse(Convert.ToString(dr["CategoryId"]),out bodyStyle);
                                bikeRankObj.BodyStyle = bodyStyle;
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, String.Format("GenericBikeRepository.GetBikeRankingByCategory: ModelId:{0}",modelId));
            }
            return bikeRankObj;
        }
        
    }
}
