using Carwale.Interfaces.Classified.SellCar;
using Dapper;
using System;
using System.Data;
using AEPLCore.Logging;
using Carwale.Entity.Classified.SellCarUsed;
using System.Linq;
using System.Collections.Generic;

namespace Carwale.DAL.Classified.SellCar
{
    public class ListingRepository : RepositoryBase, IListingRepository
    {
		private static Logger Logger = LoggerFactory.GetLogger();
		public bool ListingDelete(int inquiryId, int status)
        {
            bool isSuccess = false;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_inquiryid", inquiryId);
                param.Add("v_status", status);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    isSuccess = con.Execute("updateinquirystatus_v1", param, commandType: CommandType.StoredProcedure) > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error while deleting listing :" + inquiryId);
            }
            return isSuccess;
        }

        public bool PatchListings(int inquiryId, SellCarInfo sellCarInfo)
        {
            bool isSuccess = false;
            try
            {
                if (sellCarInfo != null)
                {
                    var param = GetPatchListingParameters(inquiryId, sellCarInfo);
                    using (var con = ClassifiedMySqlMasterConnection)
                    {
                        con.Execute("updatecustomersellinquiry_patch_v18_2_6", param, commandType: CommandType.StoredProcedure);
                    }
                    isSuccess = param.Get<short>("v_isupdated") > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error while updating listing in patch request :" + inquiryId);
                throw;
            }
            return isSuccess;
        }

        public bool PatchListingsV1(int inquiryId, SellCarInfo sellCarInfo)
        {
            bool isSuccess = false;
            try
            {
                if (sellCarInfo != null)
                {
                    var param = GetPatchListingParameters(inquiryId, sellCarInfo);
                    param.Add("v_regtype", (int)sellCarInfo.RegType != 0 ? sellCarInfo.RegType.ToString() : null);
                    using (var con = ClassifiedMySqlMasterConnection)
                    {
                        con.Execute("updatecustomersellinquiry_patch_v18_4_4", param, commandType: CommandType.StoredProcedure);
                    }
                    isSuccess = param.Get<short>("v_isupdated") > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error while updating listing in patch request :" + inquiryId);
                throw;
            }
            return isSuccess;
        }

        private static DynamicParameters GetPatchListingParameters(int inquiryId, SellCarInfo sellCarInfo)
        {
            var param = new DynamicParameters();
            DateTime? insuranceExpiryDate = null;
            DateTime? manufactureYear = null;
            if (sellCarInfo != null)
            {
                if (sellCarInfo.InsuranceExpiryYear.HasValue && sellCarInfo.InsuranceExpiryMonth.HasValue)
                {
                    insuranceExpiryDate = new DateTime(sellCarInfo.InsuranceExpiryYear.Value, sellCarInfo.InsuranceExpiryMonth.Value, 01);
                }
                if (sellCarInfo.ManufactureYear > 0 && sellCarInfo.ManufactureMonth > 0)
                {
                    manufactureYear = new DateTime(sellCarInfo.ManufactureYear, sellCarInfo.ManufactureMonth, 1);
                }
                param.Add("v_carversionid", sellCarInfo.VersionId != 0 ? sellCarInfo.VersionId : (int?)null, DbType.Int32);
                param.Add("v_makeyear", manufactureYear);
                param.Add("v_kms", sellCarInfo.KmsDriven != 0 ? sellCarInfo.KmsDriven : (int?)null, DbType.Int32);
                param.Add("v_price", sellCarInfo.ExpectedPrice != 0 ? sellCarInfo.ExpectedPrice : (int?)null, DbType.Int32);
                param.Add("v_owners", sellCarInfo.Owners != 0 ? sellCarInfo.Owners : (int?)null, DbType.Int16);
                param.Add("v_regno", !string.IsNullOrEmpty(sellCarInfo.RegistrationNumber) ? sellCarInfo.RegistrationNumber : null, DbType.String);
                param.Add("v_color", !string.IsNullOrEmpty(sellCarInfo.Color) ? sellCarInfo.Color : null, DbType.String);
                param.Add("v_additionalfuel", !string.IsNullOrEmpty(sellCarInfo.AlternateFuel) ? sellCarInfo.AlternateFuel : null, DbType.String);
                param.Add("v_insurance", sellCarInfo.Insurance != null ? Convert.ToString(sellCarInfo.Insurance) : null, DbType.String);
                param.Add("v_insuranceexpiry", insuranceExpiryDate, DbType.DateTime);
                param.Add("v_inquiryid", inquiryId, DbType.Int32);
                param.Add("v_status", sellCarInfo.StatusId ?? null);
                param.Add("v_isarchived", sellCarInfo.IsArchived ?? null);
                param.Add("v_ispremium", sellCarInfo.IsPremium ?? null);
                param.Add("v_maskingnumber", !string.IsNullOrEmpty(sellCarInfo.MaskingNumber) ? sellCarInfo.MaskingNumber : null);
                param.Add("v_is_customer_editable", sellCarInfo.CustomerEditable ?? null);
                param.Add("v_isupdated", 0, DbType.Int16, direction: ParameterDirection.Output);
            }
            return param;
        }


        public IList<SellCarBasicInfo> GetExpiredListings()
        {
            IList<SellCarBasicInfo> expiredListings = null;
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    string searchQuery = @"select id as inquiryid,packagetype from customersellinquiries where statusid=@status and date(classifiedexpirydate) < @expiryDate";
                    expiredListings = con.Query<SellCarBasicInfo>(searchQuery, new { status = 1, expiryDate = DateTime.Now }).ToList<SellCarBasicInfo>();
                }
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }
            return expiredListings;
        }
    }
}
