
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13 Oct 2016
    /// Summary: Save Sell Bike data inquiry
    /// </summary>
    public class SellBikesRepository<T, U> : ISellBikesRepository<T, U> where T : SellBikeAd, new()
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 14 Oct 2016
        /// Description :   Saves a sell bike ad
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public int Add(T ad)
        {
            int inquiryId = default(int);
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "classified_insertindividualsellbike";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int32, ad.Version.VersionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikemakeyear", DbType.DateTime, ad.ManufacturingYear));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeowner", DbType.Byte, ad.Owner));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikecolor", DbType.String, 100, ad.Color));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikecolorid", DbType.String, 100, ad.ColorId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_kilometers", DbType.Int32, ad.KiloMeters));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int16, ad.CityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_registrationplace", DbType.String, 50, ad.RegistrationPlace));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_expectedprice", DbType.Int64, ad.Expectedprice));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.Byte, ad.SourceId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sellertype", DbType.Byte, Convert.ToByte(ad.Seller.SellerType)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, 50, ad.Seller.CustomerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, 100, ad.Seller.CustomerEmail));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, 20, ad.Seller.CustomerMobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, ad.Seller.CustomerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, CurrentUser.GetClientIP()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_statusid", DbType.Byte, Convert.ToByte(ad.Status)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int64, ParameterDirection.InputOutput));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    inquiryId = Utility.SqlReaderConvertor.ToInt32(cmd.Parameters["par_inquiryid"].Value);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("bikeWale.DAL.Used.SellBikesRepository.Add({0})", Newtonsoft.Json.JsonConvert.SerializeObject(ad)));

            }
            return inquiryId;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 14 Oct 2016
        /// Description :   Updates the sell bike ad
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public bool Update(T ad)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "classified_updateindividualsellbike";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int32, ad.Version.VersionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikemakeyear", DbType.DateTime, ad.ManufacturingYear));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeowner", DbType.Byte, ad.Owner));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikecolor", DbType.String, 100, ad.Color));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikecolorid", DbType.String, 100, ad.ColorId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_kilometers", DbType.Int32, ad.KiloMeters));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int16, ad.CityId));
                    //cmd.Parameters.Add(DbFactory.GetDbParam("par_registrationno", DbType.String, 50, ad.OtherInfo.RegistrationNo));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_registrationplace", DbType.String, 50, ad.RegistrationPlace));
                    //cmd.Parameters.Add(DbFactory.GetDbParam("par_insurancetype", DbType.String, 20, ad.OtherInfo.InsuranceType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_expectedprice", DbType.Int64, ad.Expectedprice));
                    //cmd.Parameters.Add(DbFactory.GetDbParam("par_comments", DbType.String, 250, ad.OtherInfo.AdDescription));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.Byte, ad.SourceId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sellertype", DbType.Int16, (int)ad.Seller.SellerType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, 50, ad.Seller.CustomerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, 100, ad.Seller.CustomerEmail));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, 20, ad.Seller.CustomerMobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, ad.Seller.CustomerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, CurrentUser.GetClientIP()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_statusid", DbType.Byte, Convert.ToByte(ad.Status)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int64, ad.InquiryId));
                    isSuccess = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("bikeWale.DAL.Used.SellBikesRepository.Update({0})", Newtonsoft.Json.JsonConvert.SerializeObject(ad)));

            }
            return isSuccess;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 14 Oct 2016
        /// Description :   Update Other Information of sell ad
        /// </summary>
        /// <param name="otherInfo"></param>
        /// <param name="inquiryId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool UpdateOtherInformation(SellBikeAdOtherInformation otherInfo, U inquiryId, UInt64 customerId)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "classified_updateotherindivdualsellbikeinfo";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_registrationno", DbType.String, 50, Utility.FormatDescription.SanitizeHtml(otherInfo.RegistrationNo)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_insurancetype", DbType.String, 20, otherInfo.InsuranceType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_comments", DbType.String, 250, Utility.FormatDescription.SanitizeHtml(otherInfo.AdDescription)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int32, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerId", DbType.Int32, customerId));
                    isSuccess = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("bikeWale.DAL.Used.SellBikesRepository.UpdateOtherInformation({0},{1},{2})", Newtonsoft.Json.JsonConvert.SerializeObject(otherInfo), inquiryId, customerId));

            }
            return isSuccess;
        }

        /// <summary>
        /// Created by : Sajal Gupta on 22-02-2017
        /// Description: Funcion to update status aginst inquiry id.      
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="status"></param>
        public void ChangeInquiryStatus(uint inquiryId, SellAdStatus status)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("sellbikelistingstatuschange"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int32, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_status", DbType.Int32, status));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("bikeWale.DAL.Used.SellBikesRepository.ChangeInquiryStatus (inquiryId:{0}) => status:{1}", inquiryId, status));
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 14 Oct 2016
        /// Description :   Retrieves sell bike ad
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public T GetById(U inquiryId, UInt64 customerId)
        {
            T objAd = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_getindividualsellbike"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int32, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int32, customerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            objAd = new T();
                            objAd.Make = new Entities.BikeData.BikeMakeEntityBase()
                            {
                                MakeId = Utility.SqlReaderConvertor.ToInt32(dr["makeid"]),
                                MakeName = Convert.ToString(dr["makename"]),
                                MaskingName = Convert.ToString(dr["makemaskingname"])
                            };
                            objAd.Model = new Entities.BikeData.BikeModelEntityBase()
                            {
                                ModelId = Utility.SqlReaderConvertor.ToInt32(dr["modelid"]),
                                ModelName = Convert.ToString(dr["modelname"]),
                                MaskingName = Convert.ToString(dr["modelmaskingname"])
                            };
                            objAd.Version = new Entities.BikeData.BikeVersionEntityBase()
                            {
                                VersionId = Utility.SqlReaderConvertor.ToInt32(dr["versionid"]),
                                VersionName = Convert.ToString(dr["versionname"])
                            };
                            objAd.CityId = Utility.SqlReaderConvertor.ToUInt32(dr["cityid"]);
                            objAd.Color = Convert.ToString(dr["color"]);
                            objAd.ColorId = Utility.SqlReaderConvertor.ToUInt32(dr["colorid"]);
                            objAd.Expectedprice = Utility.SqlReaderConvertor.ToUInt32(dr["price"]);
                            objAd.InquiryId = Utility.SqlReaderConvertor.ToUInt32(dr["id"]);
                            objAd.KiloMeters = Utility.SqlReaderConvertor.ToUInt32(dr["kilometers"]);
                            objAd.ManufacturingYear = Utility.SqlReaderConvertor.ToDateTime(dr["makeyear"]);
                            objAd.OtherInfo = new SellBikeAdOtherInformation()
                            {
                                AdDescription = Convert.ToString(dr["comments"]),
                                InsuranceType = Convert.ToString(dr["insurancetype"]),
                                RegistrationNo = Convert.ToString(dr["bikeregno"])
                            };
                            objAd.Owner = Utility.SqlReaderConvertor.ToUInt16(dr["owner"]);
                            objAd.RegistrationPlace = Convert.ToString(dr["registrationplace"]);
                            objAd.Seller = new SellerEntity()
                            {
                                CustomerId = Utility.SqlReaderConvertor.ToUInt32(dr["customerid"]),
                                CustomerName = Convert.ToString(dr["customername"]),
                                CustomerMobile = Convert.ToString(dr["customermobile"]),
                                CustomerEmail = Convert.ToString(dr["customeremail"]),
                            };
                            SellerType st;
                            SellAdStatus sa;
                            st = (SellerType)Convert.ToByte(dr["sellertype"]);
                            Enum.TryParse<SellAdStatus>(Convert.ToString(dr["statusid"]), out sa);
                            objAd.Seller.SellerType = st;

                            objAd.Status = sa;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("bikeWale.DAL.Used.SellBikesRepository.GetById({0}) => Customerid:{1}", inquiryId, customerId));

            }
            return objAd;
        }


        #region Not implemented methods
        public bool Delete(U id)
        {
            throw new NotImplementedException();
        }

        public T GetById(U id)
        {
            throw new NotImplementedException();
        }
        System.Collections.Generic.List<T> Interfaces.IRepository<T, U>.GetAll()
        {
            throw new NotImplementedException();
        }
        #endregion

        U Interfaces.IRepository<T, U>.Add(T t)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Oct 2016
        /// Description :   Save Bike Photo
        /// </summary>
        /// <param name="isMain"></param>
        /// <param name="isDealer"></param>
        /// <param name="inquiryId"></param>
        /// <param name="originalImageName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public string SaveBikePhotos(bool isMain, bool isDealer, U inquiryId, string originalImageName, string description)
        {
            string photoId = "";
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_bikephotos_insert"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int64, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_description", DbType.String, 200, description));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_directorypath", DbType.String, 200, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isreplicated", DbType.Boolean, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_originalimagepath", DbType.String, 300, ""));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isdealer", DbType.Boolean, isDealer));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ismain", DbType.Boolean, isMain));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbType.String, 100, ""));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_photoid", DbType.Int64, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    photoId = cmd.Parameters["par_photoid"].Value.ToString();
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, String.Format("SaveBikePhotos({0},{1},{2},{3},{4})", isMain, isDealer, inquiryId, originalImageName, description));

            }
            return photoId;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Oct 2016
        /// Description :   Upload Image To Common Database to be processed by Image Consumer
        /// </summary>
        /// <param name="photoId"></param>
        /// <param name="imageName"></param>
        /// <param name="imgC"></param>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public string UploadImageToCommonDatabase(string photoId, string imageName, ImageCategories imgC, string directoryPath)
        {
            string url = string.Empty;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "img_allbikephotosinsert";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_itemid", DbType.Int64, photoId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_origfilename", DbType.String, imageName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_categoryid", DbType.Int32, imgC));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dirpath", DbType.String, directoryPath));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbType.String, Utility.BWConfiguration.Instance.RabbitImgHostURL));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_url", DbType.String, 255, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    url = cmd.Parameters["par_url"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("UploadImageToCommonDatabase({0},{1},{2},{3})", photoId, imageName, imgC, directoryPath));

            } // catch Exception
            return url;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 02 Nov 2016
        /// Description :   Returns the used Bike photos
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        public IEnumerable<BikePhoto> GetBikePhotos(U inquiryId, bool isApproved)
        {
            ICollection<BikePhoto> photos = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_getlistingphotos"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.String, 50, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isaprooved", DbType.SByte, 8, isApproved ? 1 : 0));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
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
                                        OriginalImagePath = Convert.ToString(dr["originalimagepath"]),
                                        Id = Utility.SqlReaderConvertor.ToUInt32(dr["id"])
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
        /// Created by  :   Sumit Kate on 03 Nov 2016
        /// Description :   Marks gived photoid as main image for sell bike inquiry specified by inquiryid
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="photoId"></param>
        /// <param name="isDealer"></param>
        /// <returns></returns>
        public bool MarkMainImage(U inquiryId, uint photoId, bool isDealer)
        {
            bool isMain = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_bikephotos_mainimage"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int32, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_photoid", DbType.Int32, photoId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isdealer", DbType.Boolean, isDealer));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    isMain = true;
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, String.Format("MarkMainImage(inquiryId: {0}, photoId: {1}, isDealer: {2}  )", inquiryId, photoId, isDealer));

            }
            return isMain;
        }
    }
}
