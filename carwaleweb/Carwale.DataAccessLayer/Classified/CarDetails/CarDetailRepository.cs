using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Enum;
using Carwale.Entity.Stock;
using Carwale.Interfaces.Classified.CarDetail;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Carwale.DAL.Classified.CarDetails
{
    public class CarDetailRepository : RepositoryBase, IListingDetails
    {
        private static string _imgHostUrl = Carwale.Utility.CWConfiguration._imgHostUrl;
        protected CarDetailsEntity objDetails = new CarDetailsEntity();

        #region GetIndividualListingDetails
        public CarDetailsEntity GetIndividualListingDetails(uint inquiryId, bool isMasterConnection)
        {
            string connectionString = isMasterConnection ? DbConnections.ClassifiedMySqlMasterConnection : DbConnections.ClassifiedMySqlReadConnection;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getindividuallistingsdetails_v18_4_4"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_inquiryid", DbType.Int32, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_issold", DbType.Int16, ParameterDirection.Output));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, connectionString))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                PopulateBasicInfo(dr);
                                PopulateIndividualWaranty(dr);
                                PopulateNonAbsureCarCondition(dr);
                                PopulateModifications(dr);
                                PopulateFinance(dr);
                                PopulatePackageInfo(dr);
                                dr.NextResult();
                                PopulateImages(dr);
                            }
                        }
                    }
                    objDetails.IsSold = cmd.Parameters["v_issold"].Value != null ? Convert.ToInt32(cmd.Parameters["v_issold"].Value) == 1 : false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return objDetails;
        }
        #endregion

        #region GetDealerListingDetails
        /// <summary>
        /// Created By : Sadhana Upadhyay on 29 MAy 2015
        /// Summary : To get Dealer Listing details
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <returns></returns>
        public CarDetailsEntity GetDealerListingDetails(uint inquiryId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getdealerlistingdetails_v17_4_1"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_inquiryid", DbType.Int32, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_issold", DbType.Int16, ParameterDirection.Output));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.ClassifiedMySqlReadConnection))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                PopulateBasicInfo(dr);
                                PopulateNonAbsureCarCondition(dr);
                                PopulateModifications(dr);
                                PopulateDealerInfo(dr);
                                PopulateFinance(dr);
                                dr.NextResult();
                                PopulateImages(dr);
                            }
                        }
                    }
                    objDetails.IsSold = cmd.Parameters["v_issold"].Value != null ? Convert.ToInt32(cmd.Parameters["v_issold"].Value) == 1 : false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return objDetails;
        }   //End of GetDealerListingDetails
        #endregion

        public bool PhotoRequestDone(int sellInquiryId, int buyerId, int consumerType)
        {
            bool isDone = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_inquiry", sellInquiryId, DbType.Int32);
                parameters.Add("v_buyerid", buyerId, DbType.Int32);
                parameters.Add("v_consumertype", consumerType, DbType.Int32);
                parameters.Add("v_isdone", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    con.Execute("photorequestdone", parameters, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("photorequestdone");
                }
                isDone = parameters.Get<int>("v_isdone") == 1 ? true : false;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return isDone;
        }

        public bool UploadPhotosRequest(int sellInquiryId, int buyerId, int consumerType, string buyerMessage)
        {
            bool ret = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_sellinquiryid", sellInquiryId, DbType.Int32);
                parameters.Add("v_buyerid", buyerId, DbType.Int32);
                parameters.Add("v_consumertype", consumerType, DbType.Int32);
                parameters.Add("v_buyermessage", buyerMessage, dbType: DbType.String);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    ret = con.Execute("classified_uploadphotosrequest_sp", parameters, commandType: CommandType.StoredProcedure) > 0;
                    LogLiveSps.LogSpInGrayLog("classified_uploadphotosrequest_sp");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return ret;
        }

        public int ReportListing(int inquiryId, int inquiryType, int reasonId, string description, string email)
        {
            int complaintId = -1;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_inquiryid", inquiryId, DbType.Int32);
                parameters.Add("v_inquirytype", inquiryType, DbType.Int32);
                parameters.Add("v_reasonid", reasonId, DbType.Int32);
                parameters.Add("v_description", description, DbType.String);
                parameters.Add("v_emailid", email, DbType.String);
                parameters.Add("v_complaintid", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("classified_reportlisting_sp", parameters, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("classified_reportlisting_sp");
                }
                complaintId = parameters.Get<int>("v_complaintid");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return complaintId;
        }

        public List<ReportListingReasons> GetReportListingReasons(bool isDealer)
        {
            List<ReportListingReasons> reasons = null;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_isdealer", isDealer, DbType.Boolean);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    reasons = con.Query<ReportListingReasons>("getreportlistingreasons", parameters, commandType: CommandType.StoredProcedure).AsList();
                    LogLiveSps.LogSpInGrayLog("getreportlistingreasons");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return reasons;
        }

        private void PopulateFinance(IDataReader dr)
        {
            objDetails.Finance = new Entity.Classified.CarDetails.Finance()
            {
                IsEligibleForFinance =
                dr["IsEligibleForFinance"] != DBNull.Value && Convert.ToBoolean(dr["IsEligibleForFinance"]) && objDetails.BasicCarInfo.CwBasePackageId != CwBasePackageId.Franchise,
                Emi = dr["EMI"] != DBNull.Value ? Convert.ToDecimal(dr["EMI"]) : (decimal?)null
            };
        }

        #region PopulateBasicInfo
        void PopulateBasicInfo(IDataReader dr)
        {
            try
            {
                objDetails.BasicCarInfo = new BasicCarInfo()
                {
                    ProfileId = dr["ProfileId"] != DBNull.Value ? dr["ProfileId"].ToString() : "",
                    InquiryId = dr["InquiryId"] != DBNull.Value ? Convert.ToUInt32(dr["InquiryId"]) : 0,
                    Price = dr["Price"] != DBNull.Value ? dr["Price"].ToString() : "",
                    Color = dr["Color"] != DBNull.Value ? Format.UppercaseWords(dr["Color"].ToString()) : "",
                    InteriorColor = dr["InteriorColor"] != DBNull.Value ? Format.UppercaseWords(dr["InteriorColor"].ToString()) : "",
                    Kilometers = dr["Kilometers"] != DBNull.Value ? Format.Numeric(dr["Kilometers"].ToString()) : "",
                    FuelName = dr["FuelType"] != DBNull.Value ? dr["FuelType"].ToString() : "",
                    FuelType = dr["FuelTypeId"] != DBNull.Value ? Convert.ToUInt16(dr["FuelTypeId"]) : Convert.ToUInt16(0),
                    TransmissionType = dr["CarTransmission"] != DBNull.Value ? dr["CarTransmission"].ToString() : "",
                    TransmissionTypeId = dr["CarTransmissionId"] != DBNull.Value ? Convert.ToUInt16(dr["CarTransmissionId"]) : Convert.ToUInt16(0),
                    SellerType = dr["Seller"] != DBNull.Value ? dr["Seller"].ToString() : "",
                    SellerId = dr["SellerType"] != DBNull.Value ? Convert.ToUInt16(dr["SellerType"]) : Convert.ToUInt16(0),
                    Insurance = dr["Insurance"] != DBNull.Value ? dr["Insurance"].ToString() : "",
                    InsuranceExpiry = dr["InsuranceExpiry"] != DBNull.Value ? Convert.ToDateTime(dr["InsuranceExpiry"]).ToString("dd MMM yyyy") : "",
                    AdditionalFuel = dr["AdditionalFuel"] != DBNull.Value ? dr["AdditionalFuel"].ToString() : "",
                    FuelEconomy = dr["CityMileage"] != DBNull.Value ? dr["CityMileage"].ToString() : "",
                    OwnerNumber = dr["Owners"] != DBNull.Value ? Convert.ToInt16(dr["Owners"].ToString()) : default(short?),
                    VideoCount = dr["VideoCount"] != DBNull.Value ? Convert.ToInt32(dr["VideoCount"]) : 0,
                    VideoURL = dr["YoutubeVideo"] != DBNull.Value ? dr["YoutubeVideo"].ToString() : "",
                    MakeYear = Convert.ToDateTime(dr["MakeYear"]),
                    LastUpdatedDate = dr["LastUpdated"] != DBNull.Value ? Convert.ToString(dr["LastUpdated"]) : "",
                    MakeName = dr["MakeName"] != DBNull.Value ? dr["MakeName"].ToString() : "",
                    MakeId = dr["MakeId"] != DBNull.Value ? Convert.ToUInt32(dr["MakeId"]) : 0,
                    ModelId = dr["ModelId"] != DBNull.Value ? Convert.ToUInt32(dr["ModelId"]) : 0,
                    ModelName = dr["ModelName"] != DBNull.Value ? dr["ModelName"].ToString() : "",
                    RootId = dr["RootId"] != DBNull.Value ? Convert.ToUInt32(dr["RootId"]) : 0,
                    RootName = dr["RootName"] != DBNull.Value ? dr["RootName"].ToString() : "",
                    VersionId = dr["VersionId"] != DBNull.Value ? Convert.ToUInt32(dr["VersionId"]) : 0,
                    VersionName = dr["VersionName"] != DBNull.Value ? dr["VersionName"].ToString() : "",
                    CityName = dr["CityName"] != DBNull.Value ? dr["CityName"].ToString() : "",
                    CityId = dr["CityId"] != DBNull.Value ? Convert.ToUInt32(dr["CityId"]) : 0,
                    AreaId = dr["AreaId"] != DBNull.Value ? Convert.ToUInt32(dr["AreaId"]) : 0,
                    AreaName = dr["AreaName"] != DBNull.Value ? dr["AreaName"].ToString() : "",
                    RegisterCity = dr["RegistrationPlace"] != DBNull.Value ? dr["RegistrationPlace"].ToString() : "",
                    RegistrationNumber = dr["CarRegNo"] != DBNull.Value ? dr["CarRegNo"].ToString() : "",
                    LifeTimeTax = dr["Tax"] != DBNull.Value ? dr["Tax"].ToString() : "",
                    IsPremium = dr["IsPremium"] != DBNull.Value ? Convert.ToBoolean(dr["IsPremium"]) : false,
                    MaskingName = dr["MaskingName"] != DBNull.Value ? dr["MaskingName"].ToString() : "",
                    VersionSubSegmentID = dr["SubSegmentId"] != DBNull.Value ? dr["SubSegmentId"].ToString() : "",
                    BodyStyleId = dr["BodyStyleId"] != DBNull.Value ? dr["BodyStyleId"].ToString() : "",
                    PriceNumeric = dr["Price"] != DBNull.Value ? dr["Price"].ToString() : "",
                    KmNumeric = dr["Kilometers"] != DBNull.Value ? dr["Kilometers"].ToString() : "",
                    IsNew = dr["isnew"] != DBNull.Value ? Convert.ToInt32(dr["isnew"]) != 0 : false,
                    CertificationId = dr["CertificationId"] != DBNull.Value ? dr["CertificationId"].ToString() : "",
                    CertificationScore = dr["CertificationScore"] != DBNull.Value ? Convert.ToDecimal(dr["CertificationScore"]) : default(decimal?),
                    CityMaskingName = dr["CityMaskingName"].ToString()
                };
                if (objDetails.BasicCarInfo.SellerId == 2)
                {
                    objDetails.BasicCarInfo.SellerName = dr["CustomerName"] != DBNull.Value ? dr["CustomerName"].ToString() : "";
                    CarRegistrationType regType;
                    Enum.TryParse(dr["regtype"].ToString(), out regType);
                    objDetails.BasicCarInfo.RegType = regType;
                }
                else
                {
                    objDetails.BasicCarInfo.IsChatAvailable = !string.IsNullOrEmpty(dr["ChatUserToken"].ToString());
                    objDetails.BasicCarInfo.TCStockId = dr["TC_StockId"] != DBNull.Value ? Convert.ToInt32(dr["TC_StockId"]) : 0;
                    objDetails.BasicCarInfo.CtePackageId = dr["CtePackageId"] != DBNull.Value ? Convert.ToInt32(dr["CtePackageId"]) : 0;
                    if (dr["sourceid"] != DBNull.Value)
                    {
                        ClassifiedStockSource stockSource;
                        Enum.TryParse(dr["sourceid"].ToString(), out stockSource);
                        objDetails.BasicCarInfo.StockSource = stockSource;
                    }
                    if (dr["CwBasePackageId"] != DBNull.Value)
                    {
                        CwBasePackageId cwBasePackageId;
                        Enum.TryParse(dr["CwBasePackageId"].ToString(), out cwBasePackageId);
                        objDetails.BasicCarInfo.CwBasePackageId = cwBasePackageId;
                    }
                }
                PopulateOwnersComments(dr);
                PopulateUsedCarFeatures(dr);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

        }
        #endregion

        #region PopulateIndividualWaranty
        void PopulateIndividualWaranty(IDataReader dr)
        {
            try
            {
                objDetails.IndividualWarranty = new IndividualWarranty()
                {
                    WarrantyDescription = dr["Warranties"] != DBNull.Value ? dr["Warranties"].ToString() : ""
                };
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region PopulateNonAbsureCarCondition
        void PopulateNonAbsureCarCondition(IDataReader dr)
        {
            try
            {
                objDetails.NonAbsureCarCondition = new NonAbsureCarCondition()
                {
                    AC = dr["ACCondition"] != DBNull.Value ? dr["ACCondition"].ToString() : "",
                    Tyres = dr["TyresCondition"] != DBNull.Value ? dr["TyresCondition"].ToString() : "",
                    Battery = dr["BatteryCondition"] != DBNull.Value ? dr["BatteryCondition"].ToString() : "",
                    Brakes = dr["BrakesCondition"] != DBNull.Value ? dr["BrakesCondition"].ToString() : "",
                    Electricals = dr["ElectricalsCondition"] != DBNull.Value ? dr["ElectricalsCondition"].ToString() : "",
                    Engine = dr["EngineCondition"] != DBNull.Value ? dr["EngineCondition"].ToString() : "",
                    Exterior = dr["ExteriorCondition"] != DBNull.Value ? dr["ExteriorCondition"].ToString() : "",
                    Interior = dr["InteriorCondition"] != DBNull.Value ? dr["InteriorCondition"].ToString() : "",
                    Seats = dr["SeatsCondition"] != DBNull.Value ? dr["SeatsCondition"].ToString() : "",
                    Suspensions = dr["SuspensionsCondition"] != DBNull.Value ? dr["SuspensionsCondition"].ToString() : "",
                    OverAll = dr["OverallCondition"] != DBNull.Value ? dr["OverallCondition"].ToString() : ""
                };
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region PopulateModifications
        void PopulateModifications(IDataReader dr)
        {
            try
            {
                objDetails.Modifications = new Modifications()
                {
                    Comments = dr["Modifications"] != DBNull.Value ? dr["Modifications"].ToString() : ""
                };
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region PopulateFeatures


        void PopulateUsedCarFeatures(IDataReader dr)
        {
            try
            {
                objDetails.UsedCarFeatures = new UsedCarFeatures()
                {
                    Features_SafetySecurity = dr["Features_SafetySecurity"] == DBNull.Value ? "" : dr["Features_SafetySecurity"].ToString(),
                    Features_Comfort = dr["Features_Comfort"] == DBNull.Value ? "" : dr["Features_Comfort"].ToString(),
                    Features_Others = dr["Features_Others"] == DBNull.Value ? "" : dr["Features_Others"].ToString()
                };
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region PopulateImages
        void PopulateImages(IDataReader dr)
        {
            try
            {

                string imageUrlFull;
                string mainImageUrl;
                objDetails.ImageList = new CarDetailsImageGallery();
                objDetails.ImageList.ImageUrls = new List<string>();
                objDetails.ImageList.ImageUrlAttributes = new List<ImageUrl>();
                while (dr.Read())
                {
                    if ((objDetails.BasicCarInfo.SellerId == 2 && Convert.ToBoolean(dr["isapproved"]).Equals(true)) || objDetails.BasicCarInfo.SellerId == 1)
                    {
                        objDetails.ImageList.ImageUrlAttributes.Add(new ImageUrl { HostUrl = _imgHostUrl, OriginalImgPath = dr["OriginalImgPath"].ToString(), IsMain = Convert.ToBoolean(dr["IsMain"]) });
                        mainImageUrl = ImageSizes.CreateImageUrl(_imgHostUrl, ImageSizes._640X348, dr["OriginalImgPath"].ToString());
                        imageUrlFull = ImageSizes.CreateImageUrl(_imgHostUrl, ImageSizes._891X501, dr["OriginalImgPath"].ToString());
                        if (Convert.ToBoolean(dr["IsMain"]))
                        {
                            objDetails.ImageList.MainImageUrl = mainImageUrl;
                        }
                        objDetails.ImageList.ImageUrls.Add(imageUrlFull);
                    }
                    objDetails.ImageList.TotalPhotosUploaded++;
                }
                objDetails.BasicCarInfo.PhotoCount = objDetails.ImageList.ImageUrlAttributes.Count;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region PopulateDealerInfo
        void PopulateDealerInfo(IDataReader dr)
        {
            try
            {
                objDetails.DealerInfo = new DealerInfo();
                objDetails.DealerInfo.CertifiedUrl = dr["DealerLogoUrl"] != DBNull.Value ? dr["DealerLogoUrl"].ToString() : "";
                objDetails.DealerInfo.IsBookOnline = dr["IsPremium"] != DBNull.Value ? Convert.ToBoolean(dr["IsPremium"]) : false;
                objDetails.DealerInfo.IsTestDrive = dr["IsPremium"] != DBNull.Value ? Convert.ToBoolean(dr["IsPremium"]) : false;
                objDetails.DealerInfo.DealerAddress = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : "";
                objDetails.DealerInfo.DealerId = dr["DealerId"] != DBNull.Value ? dr["DealerId"].ToString() : "";
                objDetails.DealerInfo.DealerName = dr["DealerName"] != DBNull.Value ? dr["DealerName"].ToString() : "";
                objDetails.DealerInfo.OrganizationName = dr["Organization"] != DBNull.Value ? dr["Organization"].ToString() : "";
                objDetails.DealerInfo.Lattitude = dr["Lattitude"] != DBNull.Value ? Convert.ToDouble(dr["Lattitude"]) : 0;
                objDetails.DealerInfo.Longitude = dr["Longitude"] != DBNull.Value ? Convert.ToDouble(dr["Longitude"]) : 0;
                objDetails.DealerInfo.DealerProfileHostUrl = dr["DealerProfileHostUrl"] != DBNull.Value ? dr["DealerProfileHostUrl"].ToString() : "";
                objDetails.DealerInfo.DealerProfileImagePath = dr["DealerProfileImagePath"] != DBNull.Value ? dr["DealerProfileImagePath"].ToString() : "";
                objDetails.DealerInfo.DealerShowroomUrl = "";
                objDetails.DealerInfo.MaskingNumber = String.Empty;
                objDetails.DealerInfo.RatingText = dr["RatingText"].ToString() != string.Empty ? dr["RatingText"].ToString() : null;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region PopulateOwnersComments
        void PopulateOwnersComments(IDataReader dr)
        {
            try
            {
                objDetails.OwnerComments = new OwnersComment();
                objDetails.OwnerComments.SellerNote = dr["Comments"] != DBNull.Value ? dr["Comments"].ToString() : "";
                if (objDetails.BasicCarInfo.SellerId == 2)
                    objDetails.OwnerComments.ReasonForSell = dr["ReasonForSelling"] != DBNull.Value ? dr["ReasonForSelling"].ToString() : "";
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        #region SendErrorMail
        void SendErrorMail(Exception ex, string methodName)
        {
            var objErr = new ExceptionHandler(ex, methodName);
            objErr.LogException();
        }
        #endregion

        #region PopulatePackageInfo
        private void PopulatePackageInfo(IDataReader dataReader)
        {
            objDetails.StockPackageInfo = new StockPackageInfo()
            {
                PackageId = dataReader["PackageId"] != DBNull.Value ? Convert.ToInt32(dataReader["PackageId"]) : 0,
                PackageStartDate = dataReader["PackageStartDate"] != DBNull.Value ? Convert.ToDateTime(dataReader["PackageStartDate"]) : (DateTime?)null
            };
        }
        #endregion
    }   //End of class
}   //End of namespace
