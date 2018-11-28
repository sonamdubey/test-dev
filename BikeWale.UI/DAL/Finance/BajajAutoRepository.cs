using Bikewale.Entities.Finance.BajajAuto;
using Bikewale.Interfaces.Finance.BajajAuto;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.Finance.BajajAuto
{
    public class BajajAutoRepository : IBajajAutoRepository
    {
        public uint SqlReaderConverter { get; private set; }

        /// <summary>
        /// Modified by  : Rajan Chauhan on 29 August 2018
        /// Description  : Added Truncate to Address fields for data too long error
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        public uint SaveBasicDetails(UserDetails userDetails)
        {
            uint bajajAutoId = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "savebajajfinancebasicdetails";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_salutation", DbType.Byte, userDetails.Salutation));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_firstname", DbType.String, 26, userDetails.FirstName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lastname", DbType.String, 26, userDetails.LastName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_gender", DbType.String, 10, userDetails.Gender));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 10, userDetails.MobileNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 50, userDetails.EmailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dob", DbType.DateTime, userDetails.DateOfBirth));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_addressline1", DbType.String, 24, String.IsNullOrEmpty(userDetails.AddressLine1) ? "" : userDetails.AddressLine1.Truncate(23)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_addressline2", DbType.String, 24, String.IsNullOrEmpty(userDetails.AddressLine2) ? "" : userDetails.AddressLine2.Truncate(23)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_landmark", DbType.String, 24, String.IsNullOrEmpty(userDetails.LandMark) ? "" : userDetails.LandMark.Truncate(23)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pincode", DbType.String, 6, userDetails.PinCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_residencestatus", DbType.String, 50, userDetails.ResidenceStatus));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_residingsince", DbType.String, 5, userDetails.ResidingSince));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.UInt32, userDetails.CityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.UInt32, userDetails.VersionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.UInt32, ParameterDirection.InputOutput, userDetails.BajajAutoId));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    bajajAutoId = SqlReaderConvertor.ToUInt32(cmd.Parameters["par_id"].Value);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.Finance.BajajAuto.BajajAutoRepository.SaveBasicDetails_userDetails_{0}", Newtonsoft.Json.JsonConvert.SerializeObject(userDetails)));
            }
            return bajajAutoId;
        }

        public void SaveEmployeeDetails(UserDetails userDetails)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "savebajajfinanceemploymentdetails";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_employmenttype", DbType.Byte, userDetails.EmploymentType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_workingsince", DbType.Byte, userDetails.WorkingSince));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_companyid", DbType.UInt32, userDetails.CompanyId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_othercompany", DbType.String, 50, userDetails.OtherCompany));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_primaryincome", DbType.String, 10, userDetails.PrimaryIncome));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dependents", DbType.UInt16, userDetails.Dependents));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.UInt32, userDetails.BajajAutoId));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.Finance.BajajAuto.BajajAutoRepository.SaveBasicDetails_userDetails_{0}", userDetails));
            }
        }

        public void SaveOtherDetails(UserDetails userDetails)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "savebajajfinanceotherdetails";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobilenumber", DbType.String, 10, userDetails.MobileNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_supplierid", DbType.UInt32, userDetails.BajajSupplierId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_likelypurchasedate", DbType.DateTime, userDetails.LikelyPurchaseDate));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_repaymentmode", DbType.String, 30, userDetails.RepaymentMode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_idproof", DbType.Byte, userDetails.IdProof));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_idproofno", DbType.String, 20, userDetails.IdProofNo));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bankaccountno", DbType.String, 20, userDetails.BankAccountNo));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_accountvintage", DbType.Byte, userDetails.AccountVintage));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_responsetext", DbType.String, 1000, userDetails.ResponseJson));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_refenqnumber", DbType.UInt64, userDetails.RefEnqNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_leadid", DbType.UInt32, userDetails.LeadId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ismobileverified", DbType.Boolean, userDetails.IsMobileVerified));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.UInt32, userDetails.BajajAutoId));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.Finance.BajajAuto.BajajAutoRepository.SaveBasicDetails_userDetails_{0}", userDetails));
            }
        }
        public BajajBikeMappingEntity GetBajajFinanceBikeMappingInfo(uint versionId, uint pincodeId)
        {
            BajajBikeMappingEntity bikeMappingEntity = null;
            try
            {
                bikeMappingEntity = new BajajBikeMappingEntity();
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getbajajfinancebikemapping";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.UInt32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pincodeid", DbType.UInt32, pincodeId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            bikeMappingEntity.MakeId = SqlReaderConvertor.ToUInt32(dr["F1_Makeid"]);
                            bikeMappingEntity.ModelId = SqlReaderConvertor.ToUInt32((dr["F1_Model_Id"]));
                            bikeMappingEntity.CityId = SqlReaderConvertor.ToUInt32((dr["F1_CityId"]));
                            bikeMappingEntity.StateId = SqlReaderConvertor.ToUInt32((dr["F1_State_code"]));
                            bikeMappingEntity.PinCode = Convert.ToString(dr["pincode"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.Finance.BajajAuto.BajajAutoRepository.GetBajajFinanceBikeMappingInfo_versionId_{0}_pincodeId_{1}", versionId, pincodeId));
            }
            return bikeMappingEntity;
        }
    }
}
