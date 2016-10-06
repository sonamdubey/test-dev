﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web;

namespace Bikewale.DAL.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 29th August 2016
    /// Description : Used Bikes details repository for used bikes section
    /// </summary>
    public class UsedBikeDetailsRepository : IUsedBikeDetails
    {
        /// <summary>
        /// Created by  : Sushil Kumar on 29th August 2016
        /// Description : To get profile details for the specified inquiry id
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <returns></returns>
        public ClassifiedInquiryDetails GetProfileDetails(uint inquiryId)
        {
            ClassifiedInquiryDetails _objInquiryDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_getprofiledetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.UInt32, inquiryId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        _objInquiryDetails = new ClassifiedInquiryDetails();
                        //used bike details make,model,version
                        if (dr.Read())
                        {
                            _objInquiryDetails.Make = new BikeMakeEntityBase();
                            _objInquiryDetails.Make.MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                            _objInquiryDetails.Make.MakeName = Convert.ToString(dr["Make"]);
                            _objInquiryDetails.Make.MaskingName = Convert.ToString(dr["MakeMaskingName"]);

                            _objInquiryDetails.Model = new BikeModelEntityBase();
                            _objInquiryDetails.Model.ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                            _objInquiryDetails.Model.ModelName = Convert.ToString(dr["Model"]);
                            _objInquiryDetails.Model.MaskingName = Convert.ToString(dr["ModelMaskingName"]);

                            _objInquiryDetails.Version = new BikeVersionEntityBase();
                            _objInquiryDetails.Version.VersionId = SqlReaderConvertor.ToInt32(dr["VersionId"]);
                            _objInquiryDetails.Version.VersionName = Convert.ToString(dr["Version"]);

                            _objInquiryDetails.City = new CityEntityBase();
                            _objInquiryDetails.City.CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]);
                            _objInquiryDetails.City.CityName = Convert.ToString(dr["City"]);
                            _objInquiryDetails.City.CityMaskingName = Convert.ToString(dr["CityMaskingName"]);


                            _objInquiryDetails.State = new StateEntityBase();
                            _objInquiryDetails.State.StateId = SqlReaderConvertor.ToUInt32(dr["StateId"]);
                            _objInquiryDetails.State.StateName = Convert.ToString(dr["State"]);

                            //minimum ad details for inquiry id
                            _objInquiryDetails.MinDetails = new BikeDetailsMin();
                            _objInquiryDetails.MinDetails.AskingPrice = SqlReaderConvertor.ToUInt32(dr["Price"]);
                            _objInquiryDetails.MinDetails.ModelYear = SqlReaderConvertor.ToDateTime(dr["MakeYear"]);
                            _objInquiryDetails.MinDetails.KmsDriven = SqlReaderConvertor.ToUInt32(dr["KmsDriven"]);
                            _objInquiryDetails.MinDetails.OwnerType = Convert.ToString(dr["OwnerType"]);
                            _objInquiryDetails.MinDetails.RegisteredAt = Convert.ToString(dr["RegisteredAt"]);

                            //ad details for inquiry id
                            _objInquiryDetails.OtherDetails = new BikeDetails();
                            _objInquiryDetails.OtherDetails.Id = SqlReaderConvertor.ToUInt32(dr["sellerid"]);
                            _objInquiryDetails.OtherDetails.LastUpdatedOn = SqlReaderConvertor.ToDateTime(dr["LastUpdatedOn"]);
                            _objInquiryDetails.OtherDetails.Seller = Convert.ToString(dr["SellerType"]);
                            _objInquiryDetails.OtherDetails.Insurance = Convert.ToString(dr["insurancetype"]);
                            _objInquiryDetails.OtherDetails.Description = Convert.ToString(dr["Description"]);
                            _objInquiryDetails.OtherDetails.RegisteredAt = Convert.ToString(dr["RegisteredAt"]);
                            _objInquiryDetails.OtherDetails.RegistrationNo = Convert.ToString(dr["RegistrationNo"]);
                            _objInquiryDetails.OtherDetails.Color = new VersionColor() { ColorName = Convert.ToString(dr["Color"]) };

                            if (SqlReaderConvertor.ToBoolean(dr["isspecexists"]))
                            {
                                // bike specifications and features
                                _objInquiryDetails.SpecsFeatures = new BikeSpecifications();
                                #region Specifications
                                _objInquiryDetails.SpecsFeatures.Displacement = SqlReaderConvertor.ToFloat(dr["Displacement"]);
                                _objInquiryDetails.SpecsFeatures.MaxPower = SqlReaderConvertor.ToFloat(dr["MaxPower"]);
                                _objInquiryDetails.SpecsFeatures.MaximumTorque = SqlReaderConvertor.ToFloat(dr["MaximumTorque"]);
                                _objInquiryDetails.SpecsFeatures.NoOfGears = SqlReaderConvertor.ToUInt16(dr["NoOfGears"]);
                                _objInquiryDetails.SpecsFeatures.MaxPowerRPM = SqlReaderConvertor.ToFloat(dr["MaxPowerRPM"]);
                                _objInquiryDetails.SpecsFeatures.MaximumTorqueRPM = SqlReaderConvertor.ToFloat(dr["MaximumTorqueRPM"]);
                                _objInquiryDetails.SpecsFeatures.FuelEfficiencyOverall = SqlReaderConvertor.ToUInt16(dr["FuelEfficiencyOverall"]);
                                _objInquiryDetails.SpecsFeatures.BrakeType = Convert.ToString(dr["BrakeType"]);
                                #endregion

                                #region Features
                                _objInquiryDetails.SpecsFeatures.Speedometer = Convert.ToString(dr["Speedometer"]);
                                _objInquiryDetails.SpecsFeatures.FuelGauge = SqlReaderConvertor.ToBoolean(dr["FuelGauge"]);
                                _objInquiryDetails.SpecsFeatures.TachometerType = Convert.ToString(dr["TachometerType"]);
                                _objInquiryDetails.SpecsFeatures.DigitalFuelGauge = SqlReaderConvertor.ToBoolean(dr["DigitalFuelGauge"]);
                                _objInquiryDetails.SpecsFeatures.ElectricStart = SqlReaderConvertor.ToBoolean(dr["ElectricStart"]);
                                _objInquiryDetails.SpecsFeatures.Tripmeter = SqlReaderConvertor.ToBoolean(dr["Tripmeter"]);
                                #endregion

                            }

                        }

                        if (dr.NextResult())
                        {
                            _objInquiryDetails.Photo = new List<BikePhoto>();
                            while (dr.Read())
                            {
                                _objInquiryDetails.Photo.Add(new BikePhoto
                                {
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    HostUrl = Convert.ToString(dr["HostUrl"]),
                                    IsMain = SqlReaderConvertor.ToBoolean(dr["IsMain"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("{0}_GetProfileDetails_InquiryId :  {1}", HttpContext.Current.Request.ServerVariables["URL"], inquiryId));
                objErr.SendMail();
            }

            return _objInquiryDetails;
        }

        /// <summary>
        /// Created by  : Sangram on 29th August 2016
        /// Description : To get similar used bikes by city id,modelid and inquiry id
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<BikeDetailsMin> GetSimilarBikes(uint inquiryId, uint cityId, uint modelId, ushort topCount)
        {
            IList<BikeDetailsMin> similarBikeDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_similarbikes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryId", DbType.UInt32, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.UInt32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.UInt32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.UInt16, topCount));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            similarBikeDetails = new List<BikeDetailsMin>();
                            while (dr.Read())
                            {
                                similarBikeDetails.Add(new BikeDetailsMin()
                                {
                                    ProfileId = Convert.ToString(dr["ProfileId"]).ToLower(),
                                    BikeName = Convert.ToString(dr["bikename"]),
                                    ModelYear = SqlReaderConvertor.ToDateTime(dr["makeyear"]),
                                    OwnerType = Convert.ToString(dr["Owner"]),
                                    KmsDriven = SqlReaderConvertor.ToUInt32(dr["Kilometers"]),
                                    AskingPrice = SqlReaderConvertor.ToUInt32(dr["Price"]),
                                    RegisteredAt = Convert.ToString(dr["cityname"]),
                                    Photo = new BikePhoto()
                                    {
                                        HostUrl = Convert.ToString(dr["HostUrl"]),
                                        OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                        IsMain = true
                                    }
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("{0}_GetSimilarBikes_InquiryId_{1}_ModelId_{2}_CityId_{3}", HttpContext.Current.Request.ServerVariables["URL"], inquiryId, modelId, cityId));
                objErr.SendMail();
            }

            return similarBikeDetails;
        }

        /// <summary>
        /// Created by  : Sangram on 29th August 2016
        /// Description : To get other used bikes by city id and inquiry id
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<OtherUsedBikeDetails> GetOtherBikesByCityId(uint inquiryId, uint cityId, ushort topCount)
        {
            IList<OtherUsedBikeDetails> similarBikeDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_otherbikesbycity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryId", DbType.UInt32, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.UInt32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.UInt16, topCount));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            similarBikeDetails = new List<OtherUsedBikeDetails>();
                            while (dr.Read())
                            {
                                similarBikeDetails.Add(new OtherUsedBikeDetails()
                                {
                                    ProfileId = Convert.ToString(dr["ProfileId"]).ToLower(),
                                    BikeName = Convert.ToString(dr["bikename"]),
                                    ModelYear = SqlReaderConvertor.ToDateTime(dr["makeyear"]),
                                    OwnerType = Convert.ToString(dr["Owner"]),
                                    KmsDriven = SqlReaderConvertor.ToUInt32(dr["Kilometers"]),
                                    AskingPrice = SqlReaderConvertor.ToUInt32(dr["Price"]),
                                    RegisteredAt = Convert.ToString(dr["cityname"]),
                                    MakeName = Convert.ToString(dr["MakeName"]),
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                    ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                    CityMaskingName = Convert.ToString(dr["CityMaskingName"]),
                                    Photo = new BikePhoto()
                                    {
                                        HostUrl = Convert.ToString(dr["HostUrl"]),
                                        OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                        IsMain = true
                                    }
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("{0}_GetOtherBikesByCityId_InquiryId_{1}_CityId_{2}", HttpContext.Current.Request.ServerVariables["URL"], inquiryId, cityId));
                objErr.SendMail();
            }
            return similarBikeDetails;
        }

        /// <summary>
        /// Created by  : Sangram on 06th oct 2016
        /// Description : To get recent used bikes , if not count is given 6 bikes will be fetched
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<OtherUsedBikeDetails> GetRecentUsedBikesInIndia(ushort topCount)
        {
            IList<OtherUsedBikeDetails> recentBikes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_recentusedbikesinindia"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.UInt16, topCount));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            recentBikes = new List<OtherUsedBikeDetails>();
                            while (dr.Read())
                            {
                                recentBikes.Add(new OtherUsedBikeDetails()
                                {
                                    ProfileId = Convert.ToString(dr["ProfileId"]).ToLower(),
                                    BikeName = Convert.ToString(dr["bikename"]),
                                    ModelYear = SqlReaderConvertor.ToDateTime(dr["makeyear"]),
                                    OwnerType = Convert.ToString(dr["Owner"]),
                                    KmsDriven = SqlReaderConvertor.ToUInt32(dr["Kilometers"]),
                                    AskingPrice = SqlReaderConvertor.ToUInt32(dr["Price"]),
                                    RegisteredAt = Convert.ToString(dr["cityname"]),
                                    MakeName = Convert.ToString(dr["MakeName"]),
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                    ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                    CityMaskingName = Convert.ToString(dr["CityMaskingName"]),
                                    Photo = new BikePhoto()
                                    {
                                        HostUrl = Convert.ToString(dr["HostUrl"]),
                                        OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                        IsMain = true
                                    }
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetRecentUsedBikesInIndia" + HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return recentBikes;
        }
    }
}
