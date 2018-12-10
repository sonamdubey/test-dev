using System;
using Dapper;
using System.Data;
using Carwale.Entity.Stock;
using Carwale.Notifications.Logs;

namespace Carwale.DAL.Stock
{
    public class StockRepository : RepositoryBase
    {
        public string Create(SellInquiry entity)
        {
            string profileId = null;
            try
            {                
                var param = new DynamicParameters();
                param.Add("v_dealerid", entity.DealerId, DbType.Int32);
                param.Add("v_sellertype", entity.SellerType, DbType.Int32);
                param.Add("v_carversionid", entity.CarVersionId, DbType.Int32);
                param.Add("v_carregno", entity.CarRegNo, DbType.String);
                param.Add("v_entrydate", entity.EntryDate, DbType.DateTime);
                param.Add("v_price", entity.Price, DbType.Int32);
                param.Add("v_makeyear", entity.MakeYear, DbType.DateTime);
                param.Add("v_kilometers", entity.Kilometers, DbType.Int32);
                param.Add("v_color", entity.Color, DbType.String);
                param.Add("v_ownertype", entity.OwnerType, DbType.Int32);
                param.Add("v_comments", entity.Comments, DbType.String);
                param.Add("v_lastupdated", entity.LastUpdated, DbType.DateTime);
                param.Add("v_additionalfueltype", entity.AdditionalFuelType, DbType.String);
                param.Add("v_tc_stockid", entity.TC_StockId, DbType.Int32);
                param.Add("v_sourceid", entity.SourceId, DbType.Int32);
                param.Add("v_registrationplace", entity.RegistrationPlace, DbType.String);
                param.Add("v_interiorcolor", entity.InteriorColor, DbType.String);
                param.Add("v_onetimetax", entity.OneTimeTax, DbType.String);
                param.Add("v_insurance", entity.Insurance, DbType.String);
                param.Add("v_insuranceexpiry", entity.InsuranceExpiry, DbType.DateTime);
                param.Add("v_citymileage", entity.CityMileage, DbType.Int32);
                param.Add("v_cardriven", entity.CarDriven, DbType.String);
                param.Add("v_accidental", entity.Accidental, DbType.Boolean);
                param.Add("v_floodaffected", entity.FloodAffected, DbType.Boolean);
                param.Add("v_modifications", entity.Modifications, DbType.String);
                param.Add("v_videourl", entity.VideoUrl, DbType.String);
                param.Add("v_profileid", null, DbType.String, ParameterDirection.Output);
                param.Add("v_certprogid", entity.CertProgId, DbType.Int32);
                param.Add("v_ctepacakgeid", entity.CtePackageId, DbType.UInt16);

                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("addcartosellinquiries_v18_9_6", param, commandType: CommandType.StoredProcedure);
                }
                profileId = param.Get<string>("v_profileid");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return profileId;
        }

        public bool Update(SellInquiry entity)
        {
            try
            {                
                var param = new DynamicParameters();
                param.Add("v_dealerid", entity.DealerId, DbType.Int32);
                param.Add("v_sellertype", entity.SellerType, DbType.Int32);
                param.Add("v_carversionid", entity.CarVersionId, DbType.Int32);
                param.Add("v_carregno", entity.CarRegNo, DbType.String);
                param.Add("v_entrydate", entity.EntryDate, DbType.DateTime);
                param.Add("v_price", entity.Price, DbType.Int32);
                param.Add("v_makeyear", entity.MakeYear, DbType.DateTime);
                param.Add("v_kilometers", entity.Kilometers, DbType.Int32);
                param.Add("v_color", entity.Color, DbType.String);
                param.Add("v_ownertype", entity.OwnerType, DbType.Int32);
                param.Add("v_comments", entity.Comments, DbType.String);
                param.Add("v_lastupdated", entity.LastUpdated, DbType.DateTime);
                param.Add("v_additionalfueltype", entity.AdditionalFuelType, DbType.String);
                param.Add("v_tc_stockid", entity.TC_StockId, DbType.Int32);
                param.Add("v_sourceid", entity.SourceId, DbType.Int32);
                param.Add("v_registrationplace", entity.RegistrationPlace, DbType.String);
                param.Add("v_interiorcolor", entity.InteriorColor, DbType.String);
                param.Add("v_onetimetax", entity.OneTimeTax, DbType.String);
                param.Add("v_insurance", entity.Insurance, DbType.String);
                param.Add("v_insuranceexpiry", entity.InsuranceExpiry, DbType.DateTime);
                param.Add("v_citymileage", entity.CityMileage, DbType.Int32);
                param.Add("v_cardriven", entity.CarDriven, DbType.String);
                param.Add("v_accidental", entity.Accidental, DbType.Boolean);
                param.Add("v_floodaffected", entity.FloodAffected, DbType.Boolean);
                param.Add("v_modifications", entity.Modifications, DbType.String);
                param.Add("v_videourl", entity.VideoUrl, DbType.String);
                param.Add("v_certprogid", entity.CertProgId, DbType.Int32);
                param.Add("v_isupdated", 0, DbType.Int32, ParameterDirection.Output);
                param.Add("v_ctepacakgeid", entity.CtePackageId, DbType.UInt16);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("updatecartosellinquiries_v18_9_6", param, commandType: CommandType.StoredProcedure);
                }
                return param.Get<int>("v_isupdated") == 1;
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return false;
        }

        public bool Delete(SellInquiry entity)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_tc_stockid", entity.TC_StockId, DbType.Int32);
                parameters.Add("v_sourceid", entity.SourceId, DbType.Int32);
                parameters.Add("v_dealerid", entity.DealerId, DbType.Int32);
                parameters.Add("v_isdeleted", 0, DbType.Int32, ParameterDirection.Output);

                using (var con = ClassifiedMySqlMasterConnection)
                {                    
                    con.Execute("deletecarfromsellinquiries", parameters, commandType: CommandType.StoredProcedure);
                }
                return parameters.Get<int>("v_isdeleted") == 1;
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return false;
        }

        public bool UpdateFinance(int inquiryId, bool isDealer, decimal? emiAmount, bool isEligible)
        {
            var param = new DynamicParameters();
            param.Add("v_inquiryid", inquiryId, DbType.Int32);
            param.Add("v_isdealer", isDealer, DbType.Boolean);
            param.Add("v_emi", emiAmount, DbType.Decimal);
            param.Add("v_iseligible", isEligible, DbType.Boolean);
            param.Add("v_isupdated", 0, DbType.Int32, ParameterDirection.Output);

            try
            {
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("updatestockfinance", param, commandType: CommandType.StoredProcedure);
                }
                return param.Get<int>("v_isupdated") == 1;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }
    }
}
