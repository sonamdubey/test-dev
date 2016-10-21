
using Bikewale.CoreDAL;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, ad.ClientIp));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_statusid", DbType.Byte, Convert.ToByte(ad.Status)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int64, ParameterDirection.InputOutput));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    inquiryId = Utility.SqlReaderConvertor.ToInt32(cmd.Parameters["par_inquiryid"].Value);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("bikeWale.DAL.Used.SellBikesRepository.Add({0})", Newtonsoft.Json.JsonConvert.SerializeObject(ad)));
                objErr.SendMail();
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sellertype", DbType.Byte, Convert.ToByte(ad.Seller.SellerType)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, 50, ad.Seller.CustomerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, 100, ad.Seller.CustomerEmail));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, 20, ad.Seller.CustomerMobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, ad.Seller.CustomerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, CommonOpn.GetClientIP()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_statusid", DbType.Byte, Convert.ToByte(ad.Status)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int64, ad.InquiryId));
                    isSuccess = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("bikeWale.DAL.Used.SellBikesRepository.Update({0})", Newtonsoft.Json.JsonConvert.SerializeObject(ad)));
                objErr.SendMail();
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

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_registrationno", DbType.String, 50, otherInfo.RegistrationNo));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_insurancetype", DbType.String, 20, otherInfo.InsuranceType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_comments", DbType.String, 250, otherInfo.AdDescription));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int32, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerId", DbType.Int32, customerId));
                    isSuccess = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("bikeWale.DAL.Used.SellBikesRepository.UpdateOtherInformation({0},{1},{2})", Newtonsoft.Json.JsonConvert.SerializeObject(otherInfo), inquiryId, customerId));
                objErr.SendMail();
            }
            return isSuccess;
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

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
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
                            Enum.TryParse<SellerType>(Convert.ToString(dr["sellertype"]), out st);
                            Enum.TryParse<SellAdStatus>(Convert.ToString(dr["statusid"]), out sa);
                            objAd.Seller.SellerType = st;

                            objAd.Status = sa;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("bikeWale.DAL.Used.SellBikesRepository.GetById({0})", inquiryId, customerId));
                objErr.SendMail();
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


        /// <summary>
        /// Created By : Sangram Nandkhile Upadhyay on 13 Oct 2014
        /// Summary : To get isfake flag by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool IsFakeCustomer(ulong customerId)
        {
            bool isFake = false;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "checkfakecustomerbyid";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.UInt64, customerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            isFake = Convert.ToBoolean(dr["IsFake"]);
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("bikeWale.DAL.Used.SellBikesRepository.IsFakeCustomer({0})", customerId));
                objErr.SendMail();
            }

            return isFake;
        }

        U Interfaces.IRepository<T, U>.Add(T t)
        {
            throw new NotImplementedException();
        }
    }
}
