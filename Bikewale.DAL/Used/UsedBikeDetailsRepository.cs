using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Used;
using Bikewale.Entities.UsedBikes;
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
        /// Modified By : Sushil Kumar on 17th August 2016
        /// Description : Added AdStatus and CustomerId for sold bikes scenario
        /// Modified by :   Sumit Kate on 25 Oct 2016
        /// Description :   Changed the sp to read lastupdated value
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <returns></returns>
        public ClassifiedInquiryDetails GetProfileDetails(uint inquiryId)
        {
            ClassifiedInquiryDetails _objInquiryDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_getprofiledetails_25102016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.UInt32, inquiryId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        _objInquiryDetails = new ClassifiedInquiryDetails();
                        //used bike details make,model,version
                        if (dr.Read())
                        {
                            _objInquiryDetails.AdStatus = SqlReaderConvertor.ToInt16(dr["adstatus"]);
                            _objInquiryDetails.CustomerId = SqlReaderConvertor.ParseToUInt32(dr["customerid"]);
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
                            _objInquiryDetails.OtherDetails.Seller = Convert.ToByte(dr["SellerType"]).ToString() == "1" ? "D" : "S";
                            _objInquiryDetails.OtherDetails.Insurance = Convert.ToString(dr["insurancetype"]);
                            _objInquiryDetails.OtherDetails.Description = Convert.ToString(dr["Description"]);
                            _objInquiryDetails.OtherDetails.RegisteredAt = Convert.ToString(dr["RegisteredAt"]);
                            _objInquiryDetails.OtherDetails.RegistrationNo = Convert.ToString(dr["RegistrationNo"]);
                            _objInquiryDetails.OtherDetails.Color = new VersionColor() { ColorName = Convert.ToString(dr["Color"]) };
                           
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
                ErrorClass.LogError(ex, string.Format("{0}_GetProfileDetails_InquiryId :  {1}", HttpContext.Current.Request.ServerVariables["URL"], inquiryId));
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
                ErrorClass.LogError(ex, string.Format("{0}_GetSimilarBikes_InquiryId_{1}_ModelId_{2}_CityId_{3}", HttpContext.Current.Request.ServerVariables["URL"], inquiryId, modelId, cityId));
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
                ErrorClass.LogError(ex, string.Format("{0}_GetOtherBikesByCityId_InquiryId_{1}_CityId_{2}", HttpContext.Current.Request.ServerVariables["URL"], inquiryId, cityId));
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
                ErrorClass.LogError(ex, "GetRecentUsedBikesInIndia");
            }
            return recentBikes;
        }
        /// <summary>
        /// Written By : Sajal Gupta on 06-10-2016
        /// Summary : Getting used bike details  by profileId
        /// Modified by : Sajal Gupta on 13-10-2016
        /// Summary : Changed status id for approval pending.
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns>city, make and model name</returns>
        public InquiryDetails GetInquiryDetailsByProfileId(string profileId, string customerId)
        {
            InquiryDetails objInquiryDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getinquirydetailsbyprofileid_12102016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_profileid", DbType.String, 50, profileId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerId", DbType.String, 50, customerId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr.Read())
                        {
                            objInquiryDetails = new InquiryDetails();
                            objInquiryDetails.StatusId = SqlReaderConvertor.ToUInt32(dr["StatusId"]);
                            objInquiryDetails.CityMaskingName = Convert.ToString(dr["CityMaskingName"]);
                            objInquiryDetails.MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]);
                            objInquiryDetails.ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]);
                            objInquiryDetails.IsRedirect = SqlReaderConvertor.ToBoolean(dr["IsRedirect"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception in DAL function GetInquiryDetailsByProfileId for profileId : {0}, customerId : {1}", profileId, customerId));
            }
            return objInquiryDetails;
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 02 Nov 2016
        /// Description :   Returns the used Bike photos
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        public IEnumerable<BikePhoto> GetBikePhotos(uint inquiryId, bool isApproved)
        {
            ICollection<BikePhoto> photos = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_getlistingphotos"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.String, 50, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isaprooved", DbType.SByte, 8, isApproved ? 1 : 0));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            photos = new List<BikePhoto>();

                            while (dr.Read())
                            {
                                photos.Add(
                                    new BikePhoto()
                                    {
                                        IsMain = Utility.SqlReaderConvertor.ToBoolean(dr["ismain"]),
                                        HostUrl = Convert.ToString(dr["hosturl"]),
                                        OriginalImagePath = Convert.ToString(dr["originalimagepath"])
                                    }
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("GetBikePhotos({0},{1})", inquiryId, isApproved));
            }
            return photos;
        }
        /// <summary>
        /// Created By : Subodh Jain on 2 jan 2017 
        /// Description : Get Used Bike By Model Count In City
        /// Modified By : Sangram Nandkhile on 07 Feb 2017 
        /// Description : Changed SP to fetch Minimum price for the model
        /// Modified By :-Subodh Jain on 15 March 2017
        /// Summary :-Added getusedbikesinpopularcitybymodel_14032017
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="totalCount"></param>

        /// <returns></returns>

        public IEnumerable<MostRecentBikes> GetUsedBikeByModelCountInCity(uint makeid, uint cityid, uint topcount)
        {
            IList<MostRecentBikes> objUsedBikesList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikesinpopularcitybymodel_14032017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int16, makeid));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int16, cityid));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, topcount));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objUsedBikesList = new List<MostRecentBikes>();
                            while (dr.Read())
                            {

                                objUsedBikesList.Add(new MostRecentBikes
                                {
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                    AvailableBikes = SqlReaderConvertor.ParseToUInt32(dr["AvailableBikes"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    HostUrl = Convert.ToString(dr["HostUrl"]),
                                    MakeName = Convert.ToString(dr["makename"]),
                                    MakeMaskingName = Convert.ToString(dr["makemaskingname"]),
                                    MinimumPrice = Convert.ToString(dr["price"]),
                                    ModelId = SqlReaderConvertor.ToUInt32(dr["modelid"]),
                                    UsedHostUrl = Convert.ToString(dr["usedHostUrl"]),
                                    UsedOriginalImagePath = Convert.ToString(dr["usedOriginalImagePath"]),
                                    BikePrice = SqlReaderConvertor.ToUInt32(dr["price"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UsedBikesRepository.GetUsedBikeByModelCountInCity_makeId:{0}_cityid:{1}", makeid, cityid));
            }
            return objUsedBikesList;
        }//end of GetUsedBikeByModelCountInCity

        /// <summary>
        /// Created by: Sangram Nandkhile on 03 Feb 2017
        /// Summary: Fetch popular models by Make with bike count
        /// </summary>
        /// <param name="makeid"></param>
        /// <param name="topcount"></param>
        /// <returns></returns>
        public IEnumerable<MostRecentBikes> GetPopularUsedModelsByMake(uint makeid, uint topcount)
        {
            IList<MostRecentBikes> objUsedBikesList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getpopularusedbikesmodelsbymake_14032017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int16, makeid));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, topcount));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objUsedBikesList = new List<MostRecentBikes>();
                            while (dr.Read())
                            {
                                objUsedBikesList.Add(new MostRecentBikes
                                {
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                    AvailableBikes = SqlReaderConvertor.ParseToUInt32(dr["AvailableBikes"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    HostUrl = Convert.ToString(dr["HostUrl"]),
                                    MakeName = Convert.ToString(dr["makename"]),
                                    MakeMaskingName = Convert.ToString(dr["makemaskingname"]),
                                    MinimumPrice = Convert.ToString(dr["price"]),
                                    ModelId = SqlReaderConvertor.ToUInt32(dr["modelid"]),
                                    UsedHostUrl = Convert.ToString(dr["usedHostUrl"]),
                                    UsedOriginalImagePath = Convert.ToString(dr["usedOriginalImagePath"]),
                                    BikePrice = SqlReaderConvertor.ToUInt32(dr["price"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UsedBikesRepository.GetPopularUsedModelsByMake: Make:{0}", makeid));
            }
            return objUsedBikesList;
        }//end of GetUsedBikeByModelCountInCity


        /// <summary>
        ///Created By : Subodh Jain on 2 jan 2017 
        /// Description : Get Used Bike By Model Count In City
        /// Modified By :-Subodh Jain on 15 March 2017
        /// Summary :-Added getusedbikespopularmodelincity_14032017
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="totalCount"></param>
        public IEnumerable<MostRecentBikes> GetUsedBikeCountInCity(uint cityid, uint topcount)
        {
            IList<MostRecentBikes> objUsedBikesList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikespopularmodelincity_14032017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityid));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, topcount));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objUsedBikesList = new List<MostRecentBikes>();
                            while (dr.Read())
                            {

                                objUsedBikesList.Add(new MostRecentBikes
                                {
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                    AvailableBikes = SqlReaderConvertor.ToUInt32(dr["AvailableBikes"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    HostUrl = Convert.ToString(dr["HostUrl"]),
                                    MakeMaskingName = Convert.ToString(dr["makeMaskingName"]),
                                    MakeName = Convert.ToString(dr["makename"]),
                                    ModelId = SqlReaderConvertor.ToUInt32(dr["modelid"]),
                                    UsedHostUrl = Convert.ToString(dr["usedHostUrl"]),
                                    UsedOriginalImagePath = Convert.ToString(dr["usedOriginalImagePath"]),
                                    BikePrice = SqlReaderConvertor.ToUInt32(dr["price"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UsedBikesRepository.GetUsedBikeCountInCity:_cityid:{0}", cityid));
            }
            return objUsedBikesList;
        }
        /// <summary>
        ///Created By : Subodh Jain on 2 jan 2017 
        /// Description : Get Used Bike By Model In India
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="totalCount"></param>
        public IEnumerable<MostRecentBikes> GetUsedBike(uint topcount)
        {
            IList<MostRecentBikes> objUsedBikesList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikespopularmodel"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, topcount));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objUsedBikesList = new List<MostRecentBikes>();
                            while (dr.Read())
                            {

                                objUsedBikesList.Add(new MostRecentBikes
                                {
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                    AvailableBikes = SqlReaderConvertor.ToUInt32(dr["AvailableBikes"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    HostUrl = Convert.ToString(dr["HostUrl"]),
                                    MakeMaskingName = Convert.ToString(dr["makeMaskingName"]),
                                    MakeName = Convert.ToString(dr["makename"]),
                                    ModelId = SqlReaderConvertor.ToUInt32(dr["modelid"]),
                                    UsedHostUrl = Convert.ToString(dr["usedHostUrl"]),
                                    UsedOriginalImagePath = Convert.ToString(dr["usedOriginalImagePath"]),
                                    BikePrice = SqlReaderConvertor.ToUInt32(dr["price"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UsedBikesRepository.GetUsedBike:topcount:{0}", topcount));
            }
            return objUsedBikesList;
        }
        /// <summary>
        /// Created by  :   Sajal Gupta on 30-12-2016
        /// Description :   DAL function to read available used bikes in city by make
        /// </summary>
        public IEnumerable<UsedBikesCountInCity> GetUsedBikeInCityCountByMake(uint makeId, ushort topCount)
        {
            IList<UsedBikesCountInCity> bikesCountList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikeincitycountbymake"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_count", DbType.Int16, topCount));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            bikesCountList = new List<UsedBikesCountInCity>();

                            while (dr.Read())
                            {
                                bikesCountList.Add(
                                    new UsedBikesCountInCity()
                                    {
                                        BikeCount = Utility.SqlReaderConvertor.ToUInt32(dr["bikescount"]),
                                        CityId = Utility.SqlReaderConvertor.ToUInt32(dr["cityid"]),
                                        CityName = Convert.ToString(dr["name"]),
                                        CityMaskingName = Convert.ToString(dr["CityMaskingName"])
                                    }
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UsedBikeDetailsRepository.GetUsedBikeInCityCountByMake({0} {1})", makeId, topCount));
            }
            return bikesCountList;
        }

        /// <summary>
        /// Created by  :   Sajal Gupta on 30-12-2016
        /// Description :   DAL function to read available used bikes in city by model
        /// </summary>
        public IEnumerable<UsedBikesCountInCity> GetUsedBikeInCityCountByModel(uint modelId, ushort topCount)
        {
            IList<UsedBikesCountInCity> bikesCountList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikeincitycountbymodel"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_count", DbType.Int16, topCount));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            bikesCountList = new List<UsedBikesCountInCity>();

                            while (dr.Read())
                            {
                                bikesCountList.Add(
                                    new UsedBikesCountInCity()
                                    {
                                        BikeCount = Utility.SqlReaderConvertor.ToUInt32(dr["bikescount"]),
                                        CityId = Utility.SqlReaderConvertor.ToUInt32(dr["cityid"]),
                                        CityName = Convert.ToString(dr["name"]),
                                        CityMaskingName = Convert.ToString(dr["CityMaskingName"])
                                    }
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UsedBikeDetailsRepository.GetUsedBikeInCityCountByModel({0}, {1})", modelId, topCount));
            }
            return bikesCountList;
        }//end of GetUsedBikeCountInCity
    }
}
