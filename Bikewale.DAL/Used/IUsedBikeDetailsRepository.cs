using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
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
        /// 
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <returns></returns>
        public ClassifiedInquiryDetails GetProfileDetails(uint inquiryId)
        {
            ClassifiedInquiryDetails _objInquiryDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_getprofiledetails_dev"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.UInt32, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_currentuserid", DbType.Int32, -1));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        _objInquiryDetails = new ClassifiedInquiryDetails();
                        //used bike details make,model,version
                        if (dr.Read())
                        {
                            _objInquiryDetails.Make = new BikeMakeEntityBase()
                             {
                                 MakeId = Convert.ToInt32(dr["MakeId"]),
                                 MakeName = Convert.ToString(dr["Make"]),
                                 MaskingName = Convert.ToString(dr["MakeMaskingName"])
                             };

                            _objInquiryDetails.Model = new BikeModelEntityBase()
                            {
                                ModelId = Convert.ToInt32(dr["ModelId"]),
                                ModelName = Convert.ToString(dr["Model"]),
                                MaskingName = Convert.ToString(dr["ModelMaskingName"])
                            };
                            _objInquiryDetails.Version = new BikeVersionEntityBase()
                            {
                                VersionId = Convert.ToInt32(dr["VersionId"]),
                                VersionName = Convert.ToString(dr["Version"])
                            };

                            _objInquiryDetails.City = new CityEntityBase()
                            {
                                CityId = Convert.ToUInt32(dr["CityId"]),
                                CityName = Convert.ToString(dr["City"]),
                                CityMaskingName = Convert.ToString(dr["CityMaskingName"])
                            };

                            _objInquiryDetails.State = new StateEntityBase()
                            {
                                StateId = Convert.ToUInt32(dr["StateId"]),
                                StateName = Convert.ToString(dr["State"])
                            };
                            //minimum ad details for inquiry id
                            _objInquiryDetails.MinDetails = new BikeDetailsMin()
                            {
                                AskingPrice = Convert.ToUInt32(dr["Price"]),
                                ModelYear = Convert.ToDateTime(dr["MakeYear"]),
                                KmsDriven = Convert.ToUInt32(dr["KmsDriven"]),
                                OwnerType = Convert.ToString(dr["OwnerType"]),
                                RegisteredAt = Convert.ToString(dr["RegisteredAt"])
                            };
                            //ad details for inquiry id
                            _objInquiryDetails.OtherDetails = new BikeDetails()
                            {
                                Id = Convert.ToUInt32(dr["sellerid"]),
                                LastUpdatedOn = Convert.ToDateTime(dr["LastUpdatedOn"]),
                                Seller = Convert.ToString(dr["SellerType"]),
                                Insurance = Convert.ToString(dr["insurancetype"]),
                                Description = Convert.ToString(dr["Description"]),
                                RegisteredAt = Convert.ToString(dr["RegisteredAt"]),
                                RegistrationNo = Convert.ToString(dr["RegistrationNo"]),
                                Color = new VersionColor()
                                {
                                    ColorName = Convert.ToString(dr["Color"])
                                }
                            };
                            // bike specifications and features
                            _objInquiryDetails.SpecsFeatures = new BikeSpecifications()
                            {
                                #region Specifications
                                Displacement = Convert.ToSingle(dr["Displacement"]),
                                MaxPower = Convert.ToSingle(dr["MaxPower"]),
                                MaximumTorque = Convert.ToSingle(dr["MaximumTorque"]),
                                NoOfGears = Convert.ToUInt16(dr["NoOfGears"]),
                                MaxPowerRPM = Convert.ToSingle(dr["MaxPowerRPM"]),
                                MaximumTorqueRPM = Convert.ToSingle(dr["MaximumTorqueRPM"]),
                                FuelEfficiencyOverall = Convert.ToUInt16(dr["FuelEfficiencyOverall"]),
                                BrakeType = Convert.ToString(dr["BrakeType"]),
                                #endregion

                                #region Features
                                Speedometer = Convert.ToString(dr["Speedometer"]),
                                FuelGauge = Convert.ToBoolean(dr["FuelGauge"]),
                                TachometerType = Convert.ToString(dr["TachometerType"]),
                                DigitalFuelGauge = Convert.ToBoolean(dr["DigitalFuelGauge"]),
                                ElectricStart = Convert.ToBoolean(dr["ElectricStart"]),
                                Tripmeter = Convert.ToBoolean(dr["Tripmeter"])
                                #endregion
                            };

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
                                    IsMain = Convert.ToBoolean(dr["IsMain"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetProfileDetails ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return _objInquiryDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<BikeDetailsMin> GetSimilarBikes(uint inquiryId, uint cityId, uint modelId, ushort topCount)
        {
            List<BikeDetailsMin> similarBikeDetails = null;
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
                                    ProfileId = Convert.ToString(dr["ProfileId"]),
                                    ModelYear = Convert.ToString(dr["makeyear"]),
                                    OwnerType = Convert.ToString(dr["Owner"]),
                                    KmsDriven = Convert.ToUInt32(dr["Kilometers"]),
                                    AskingPrice = Convert.ToUInt32(dr["Price"]),
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
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return similarBikeDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<OtherUsedBikeDetails> GetOtherBikesByCityId(uint inquiryId, uint cityId, ushort topCount)
        {
            List<OtherUsedBikeDetails> similarBikeDetails = null;
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
                                    ProfileId = Convert.ToString(dr["ProfileId"]),
                                    ModelYear = Convert.ToString(dr["makeyear"]),
                                    OwnerType = Convert.ToString(dr["Owner"]),
                                    KmsDriven = Convert.ToUInt32(dr["Kilometers"]),
                                    AskingPrice = Convert.ToUInt32(dr["Price"]),
                                    RegisteredAt = Convert.ToString(dr["cityname"]),
                                    MakeName = Convert.ToString(dr["MakeName"]),
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                    ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                    CityMaskingName = Convert.ToString(dr["CityMaskingName"]),
                                    BikeName = string.Format("{0} {1}", Convert.ToString(dr["MakeName"]), Convert.ToString(dr["ModelName"])),
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
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return similarBikeDetails;
        }
    }
}
