﻿using Bikewale.Entities.BikeData;
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_currentuserid", DbType.Int32, -1));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        _objInquiryDetails = new ClassifiedInquiryDetails();
                        //used bike details make,model,version
                        if (dr.Read())
                        {
                            _objInquiryDetails.Make = new BikeMakeEntityBase();
                            _objInquiryDetails.Make.MakeId = Convert.ToInt32(dr["MakeId"]);
                            _objInquiryDetails.Make.MakeName = Convert.ToString(dr["Make"]);
                            _objInquiryDetails.Make.MaskingName = Convert.ToString(dr["MakeMaskingName"]);

                            _objInquiryDetails.Model = new BikeModelEntityBase();
                            _objInquiryDetails.Model.ModelId = Convert.ToInt32(dr["ModelId"]);
                            _objInquiryDetails.Model.ModelName = Convert.ToString(dr["Model"]);
                            _objInquiryDetails.Model.MaskingName = Convert.ToString(dr["ModelMaskingName"]);

                            _objInquiryDetails.Version = new BikeVersionEntityBase();
                            _objInquiryDetails.Version.VersionId = Convert.ToInt32(dr["VersionId"]);
                            _objInquiryDetails.Version.VersionName = Convert.ToString(dr["Version"]);

                            _objInquiryDetails.City = new CityEntityBase();
                            _objInquiryDetails.City.CityId = Convert.ToUInt32(dr["CityId"]);
                            _objInquiryDetails.City.CityName = Convert.ToString(dr["City"]);
                            _objInquiryDetails.City.CityMaskingName = Convert.ToString(dr["CityMaskingName"]);


                            _objInquiryDetails.State = new StateEntityBase();
                            _objInquiryDetails.State.StateId = Convert.ToUInt32(dr["StateId"]);
                            _objInquiryDetails.State.StateName = Convert.ToString(dr["State"]);

                            //minimum ad details for inquiry id
                            _objInquiryDetails.MinDetails = new BikeDetailsMin();
                            _objInquiryDetails.MinDetails.AskingPrice = Convert.ToUInt32(dr["Price"]);
                            _objInquiryDetails.MinDetails.ModelYear = Convert.ToDateTime(dr["MakeYear"]);
                            _objInquiryDetails.MinDetails.KmsDriven = Convert.ToUInt32(dr["KmsDriven"]);
                            _objInquiryDetails.MinDetails.OwnerType = Convert.ToString(dr["OwnerType"]);
                            _objInquiryDetails.MinDetails.RegisteredAt = Convert.ToString(dr["RegisteredAt"]);

                            //ad details for inquiry id
                            _objInquiryDetails.OtherDetails = new BikeDetails();
                            _objInquiryDetails.OtherDetails.Id = Convert.ToUInt32(dr["sellerid"]);
                            _objInquiryDetails.OtherDetails.LastUpdatedOn = Convert.ToDateTime(dr["LastUpdatedOn"]);
                            _objInquiryDetails.OtherDetails.Seller = Convert.ToString(dr["SellerType"]);
                            _objInquiryDetails.OtherDetails.Insurance = Convert.ToString(dr["insurancetype"]);
                            _objInquiryDetails.OtherDetails.Description = Convert.ToString(dr["Description"]);
                            _objInquiryDetails.OtherDetails.RegisteredAt = Convert.ToString(dr["RegisteredAt"]);
                            _objInquiryDetails.OtherDetails.RegistrationNo = Convert.ToString(dr["RegistrationNo"]);
                            _objInquiryDetails.OtherDetails.Color = new VersionColor() { ColorName = Convert.ToString(dr["Color"]) };

                            // bike specifications and features
                            _objInquiryDetails.SpecsFeatures = new BikeSpecifications();
                            #region Specifications
                            _objInquiryDetails.SpecsFeatures.Displacement = Convert.ToSingle(dr["Displacement"]);
                            _objInquiryDetails.SpecsFeatures.MaxPower = Convert.ToSingle(dr["MaxPower"]);
                            _objInquiryDetails.SpecsFeatures.MaximumTorque = Convert.ToSingle(dr["MaximumTorque"]);
                            _objInquiryDetails.SpecsFeatures.NoOfGears = Convert.ToUInt16(dr["NoOfGears"]);
                            _objInquiryDetails.SpecsFeatures.MaxPowerRPM = Convert.ToSingle(dr["MaxPowerRPM"]);
                            _objInquiryDetails.SpecsFeatures.MaximumTorqueRPM = Convert.ToSingle(dr["MaximumTorqueRPM"]);
                            _objInquiryDetails.SpecsFeatures.FuelEfficiencyOverall = Convert.ToUInt16(dr["FuelEfficiencyOverall"]);
                            _objInquiryDetails.SpecsFeatures.BrakeType = Convert.ToString(dr["BrakeType"]);
                            #endregion

                            #region Features
                            _objInquiryDetails.SpecsFeatures.Speedometer = Convert.ToString(dr["Speedometer"]);
                            _objInquiryDetails.SpecsFeatures.FuelGauge = Convert.ToBoolean(dr["FuelGauge"]);
                            _objInquiryDetails.SpecsFeatures.TachometerType = Convert.ToString(dr["TachometerType"]);
                            _objInquiryDetails.SpecsFeatures.DigitalFuelGauge = Convert.ToBoolean(dr["DigitalFuelGauge"]);
                            _objInquiryDetails.SpecsFeatures.ElectricStart = Convert.ToBoolean(dr["ElectricStart"]);
                            _objInquiryDetails.SpecsFeatures.Tripmeter = Convert.ToBoolean(dr["Tripmeter"]);
                            #endregion

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
        public IEnumerable<BikeDetailsMin> GetSimilarBikes(uint inquiryId, uint cityId, uint modelId)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<BikeDetailsMin> GetBikesByCityId(uint inquiryId, uint cityId)
        {
            throw new System.NotImplementedException();
        }
    }
}
