
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
    public class SellBikesRepository : ISellBikesRepository
    {
        public uint SaveSellBikeData(SellBikeAd ad)
        {
            uint inquiryId = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "saveclassifiedindividualsellbikeinquiries_sp";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int32, ad.VersionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikemakeyear", DbType.DateTime, ad.ManufacturingYear));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeowner", DbType.Byte, ad.Owner));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikecolor", DbType.String, 100, ad.Color));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_kilometers", DbType.Int32, ad.KiloMeters));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int16, ad.CityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_registrationno", DbType.String, 50, ad.OtherInfo.RegistrationNo));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_registrationplace", DbType.String, 50, ad.RegistrationPlace));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_insurancetype", DbType.String, 20, ad.OtherInfo.InsuranceType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_expectedprice", DbType.Int64, ad.Expectedprice));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_comments", DbType.String, 250, ad.OtherInfo.AdDescription));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.Byte, ad.SourceId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbType.String, 50, ad.Name));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 100, ad.EmailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 20, ad.Mobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int64, ParameterDirection.InputOutput));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, ad.CustomerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, ad.ClientIp));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_statusid", DbType.Byte, ad.StatusId));

                    cmd.Parameters["par_inquiryid"].Value = ad.InquiryId;
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    inquiryId = Convert.ToUInt32(cmd.Parameters["par_inquiryid"].Value);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "bikeWale.DAL.Used.SellBikesRepository.SaveSellBikeData");
                objErr.SendMail();
            }
            return inquiryId;
        }

        public uint SaveSellBikeAd()
        {
            throw new NotImplementedException();
        }

        public bool UpdateSellBikeAd(uint inquryId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateOtherInformation()
        {
            throw new NotImplementedException();
        }

        public bool VerifyMobile()
        {
            throw new NotImplementedException();
        }
    }
}
