using System;
using System.Collections.Generic;
using System.Linq;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Entity.Classified.SellCarUsed;
using Dapper;
using System.Data;
using Carwale.Notifications;
using System.Web;
using Carwale.Notifications.Logs;
using Newtonsoft.Json;
using Carwale.Entity;
using MySql.Data.MySqlClient;
using Carwale.Entity.Classified.SellCar;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Enum;

namespace Carwale.DAL.Classified.SellCar
{
    public class SellCarRepository : RepositoryBase, ISellCarRepository
    {
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public int SaveSellCarBasicInfo(SellCarBasicInfo sellCarBasicInfo)
        {
            int newInquiryId = -1;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_SellInquiryId", sellCarBasicInfo.InquiryId);
                param.Add("v_CustomerId", sellCarBasicInfo.CustomerId);
                param.Add("v_CustomerName", sellCarBasicInfo.CustomerName, DbType.String);
                param.Add("v_CustomerEmail", sellCarBasicInfo.CustomerEmail, DbType.String);
                param.Add("v_CustomerMobile", sellCarBasicInfo.CustomerMobile, DbType.String);
                param.Add("v_CityId", sellCarBasicInfo.CityId);
                param.Add("v_CarVersionId", sellCarBasicInfo.CarVersionId);
                param.Add("v_RequestDateTime", DateTime.Now);
                param.Add("v_MakeYear", sellCarBasicInfo.MakeYear);
                param.Add("v_Kms", sellCarBasicInfo.Kms);
                param.Add("v_Price", sellCarBasicInfo.Price);
                param.Add("v_ListInClassifieds", true);
                param.Add("v_IsApproved", false);
                param.Add("v_SourceId", sellCarBasicInfo.SourceId);
                param.Add("v_SendDealers", true);
                param.Add("v_PinCode", sellCarBasicInfo.PinCode);
                param.Add("v_PackageId", sellCarBasicInfo.PackageId);
                param.Add("v_Referrer", sellCarBasicInfo.Referrer, DbType.String);
                param.Add("v_IPAddress", sellCarBasicInfo.IPAddress, DbType.String);
                param.Add("v_AreaName", sellCarBasicInfo.AreaName, DbType.String);
                param.Add("v_ShowContactDetails", sellCarBasicInfo.ShowContactDetails);
                param.Add("v_ShareToCT", sellCarBasicInfo.ShareToCT == null ? -1 : Convert.ToInt32(sellCarBasicInfo.ShareToCT), DbType.Int32);
                param.Add("v_Owners", sellCarBasicInfo.Owners, DbType.Decimal);
                param.Add("v_ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("InsertCustomerSellInquiriesPartial_v17_6_2", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("InsertCustomerSellInquiriesPartial_v17_6_2");
                }
                newInquiryId = param.Get<int>("v_ID");
            }
            catch (MySqlException err)
            {
                Logger.LogException(err);
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return newInquiryId;
        }

        public int GetFreeListingCount(string mobile)
        {
            int listingCount = int.MaxValue;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_mobile", mobile);
                param.Add("v_freeListingCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    con.Execute("GetLisitingCountByMobileNo", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("GetLisitingCountByMobileNo");
                }
                listingCount = param.Get<int>("v_freeListingCount");
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return listingCount;
        }

        public List<Tuple<int, int>> GetListingCount(int inquiryId)
        {
            List<Tuple<int, int>> listingCount = null;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_carid", inquiryId);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    listingCount = con.Query<int, long, Tuple<int, int>>("GetLisitingCountByMobile", (a, b) => Tuple.Create(a, Convert.ToInt32(b)), parameters, splitOn: "*", commandType: CommandType.StoredProcedure).ToList();
                    LogLiveSps.LogSpInGrayLog("GetLisitingCountByMobile");
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return listingCount;
        }

        public int GetSellCarStepsCompleted(int inquiryId)
        {
            int sellCarStepsCompleted = 0;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_inquiryid", inquiryId);
                parameters.Add("v_sellcarstepscompleted", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("getsellcarstepscompleted", parameters, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("getsellcarstepscompleted");
                }
                sellCarStepsCompleted = parameters.Get<int>("v_sellcarstepscompleted");
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return sellCarStepsCompleted;
        }

        public bool UpdateSellCarCurrentStep(int carId, int currentStep)
        {
            bool ret = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_carid", carId);
                parameters.Add("v_currentstep", currentStep);
                parameters.Add("v_affectedrows", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("sellcarupdatecurrentstep", parameters, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("sellcarupdatecurrentstep");
                }
                ret = parameters.Get<int>("v_affectedrows") > 0;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ret;
        }

        public SellInquiriesOtherDetails GetSellCarOtherDetails(int inquiryId)
        {
            SellInquiriesOtherDetails otherDetails = null;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_InquiryId", inquiryId, dbType: DbType.Int32);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    otherDetails = con.Query<SellInquiriesOtherDetails>("GetCustomerSellInquiriesOtherDetails", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault<SellInquiriesOtherDetails>();
                    LogLiveSps.LogSpInGrayLog("GetCustomerSellInquiriesOtherDetails");
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "SellCarRepository.GetSellCarOtherDetails");
                objErr.SendMail();
            }
            return otherDetails;
        }

        public int SaveTempSellCarInquiryDetails(SellCarInfo inquiry, SellCarCustomer customer = null)
        {
            var param = new DynamicParameters();
            param.Add("v_customerId", customer?.Id, DbType.Int32);
            param.Add("v_contactDetails", customer != null ? JsonConvert.SerializeObject(customer, _serializerSettings) : null, DbType.String);
            param.Add("v_inquiryDetails", inquiry != null ? JsonConvert.SerializeObject(inquiry, _serializerSettings) : null, DbType.String);
            param.Add("v_tempinquiryid", inquiry != null ? (inquiry.TempInquiryId) : (customer != null ? customer.TempInquiryId : 0), DbType.Int32, direction: ParameterDirection.InputOutput);
            try
            {
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("InsertTempCustomerSellInquiry", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("InsertTempCustomerSellInquiry");
                    return param.Get<int>("v_tempinquiryid");
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return -1;
        }

        public bool SaveSellCarOtherDetails(SellInquiriesOtherDetails details)
        {
            bool ret = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_sellinquiryid", details.Id, DbType.Int32);
                parameters.Add("v_regplace", details.RegistrationPlace, DbType.String);
                parameters.Add("v_regno", details.RegistrationNumber, DbType.String);
                parameters.Add("v_color", details.Color, DbType.String);
                parameters.Add("v_colorcode", details.ColorCode, DbType.String);
                parameters.Add("v_comments", details.Comments, DbType.String);
                parameters.Add("v_insurance", details.Insurance, DbType.String);
                parameters.Add("v_insuranceexpiry", details.InsuranceExpiry == DateTime.MinValue ? Convert.DBNull : details.InsuranceExpiry, DbType.DateTime);
                parameters.Add("v_onetimetax", details.OneTimeTax, DbType.String);
                parameters.Add("v_affectedrows", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("v_regtype", details.RegType.ToString());
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("UpdateCustomerSellInquiriesOtherDetails_v18_3_3", parameters, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("UpdateCustomerSellInquiriesOtherDetails_v18_3_3");
                }
                ret = parameters.Get<int>("v_affectedrows") > 0;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "SellCarRepository.SaveSellCarOtherDetails");
                objErr.SendMail();
            }
            return ret;
        }

        public SellCarConditions GetSellCarCondition(int inquiryId)
        {
            SellCarConditions carCondition = null;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_InquiryId", inquiryId, dbType: DbType.Int32);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    carCondition = con.Query<SellCarConditions>("GetSellCarCarCondition", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault<SellCarConditions>();
                    LogLiveSps.LogSpInGrayLog("GetSellCarCarCondition");
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "SellCarRepository.GetSellCarCarCondition");
                objErr.SendMail();
            }
            return carCondition;
        }

        public bool SaveSellCarCondition(SellCarConditions sellCarCondition)
        {
            bool ret = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_inquiryid", sellCarCondition.InquiryId, DbType.Int32);
                parameters.Add("v_interiorcolor", sellCarCondition.InteriorColor, DbType.String);
                parameters.Add("v_interiorcolorcode", sellCarCondition.InteriorColorCode, DbType.String);
                parameters.Add("v_mileage", sellCarCondition.CityMileage, DbType.String);
                parameters.Add("v_fuel", sellCarCondition.AdditionalFuel, DbType.String);
                parameters.Add("v_driven", sellCarCondition.CarDriven, DbType.String);
                parameters.Add("v_accidental", sellCarCondition.Accidental ? 1 : 0);
                parameters.Add("v_floodaffected", sellCarCondition.FloodAffected ? 1 : 0);
                parameters.Add("v_accessories", sellCarCondition.Accessories, DbType.String);
                parameters.Add("v_warranties", sellCarCondition.Warranties, DbType.String);
                parameters.Add("v_affectedrows", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("SaveSellCarCondition_v17_6_3", parameters, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("SaveSellCarCondition_v17_6_3");
                }
                ret = parameters.Get<int>("v_affectedrows") > 0;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "SellCarRepository.SaveSellCarCondition");
                objErr.SendMail();
            }
            return ret;
        }

        public CustomerSellInquiryData GetCustomerSellInquiryData(int inquiryId)
        {
            CustomerSellInquiryData details = null;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_inquiryId", inquiryId, DbType.Int32);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    details = con.Query<CustomerSellInquiryData>("GetCustomerSellInquiryData", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault<CustomerSellInquiryData>();
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return details;
        }

        public CustomerSellInquiryVehicleData GetCustomerSellInquiryVehicleDetails(int inquiryId)
        {
            CustomerSellInquiryVehicleData vehicleData = null;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_inquiryid", inquiryId, DbType.Int32);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    vehicleData = con.Query<CustomerSellInquiryVehicleData>("GetCustomerSellInquiryVehicleDetails_v17_6_2", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault<CustomerSellInquiryVehicleData>();
                    LogLiveSps.LogSpInGrayLog("GetCustomerSellInquiryVehicleDetails_v17_6_2");
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "SellCarRepository.GetCustomerSellInquiryVehicleDetails");
                objErr.SendMail();
            }
            return vehicleData;
        }

        public List<CustomerSellInquiry> GetCustomerSellInquiries(string customerEmail, int defaultPackageId)
        {
            List<CustomerSellInquiry> custSellInquiries = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_customerEmail", customerEmail);
                param.Add("v_defaultpackageid", defaultPackageId, DbType.Int32);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    custSellInquiries = con.Query<CustomerSellInquiry>("GetCustomerSellInquiries_18_6_2", param, commandType: CommandType.StoredProcedure).ToList();
                    LogLiveSps.LogSpInGrayLog("GetCustomerSellInquiries_18_6_2");
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return custSellInquiries;
        }

        public bool UpdateCarIsArchived(int inquiryId)
        {
            var param = new DynamicParameters();
            param.Add("v_inquiryid", inquiryId, DbType.Int32);
            param.Add("v_affectedrows", 0, DbType.Int32, direction: ParameterDirection.Output);
            try
            {
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("UpdateCarIsArchived", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("UpdateCarIsArchived");
                    return param.Get<int>("v_affectedrows") > 0;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "SellCarRepository.UpdateCarIsArchived");
                objErr.SendMail();
            }
            return false;
        }

        public bool IsCustomerAuthorizedToManageCar(int customerId, int inquiryId)
        {
            bool isAuthorized = false;
            var param = new DynamicParameters();
            param.Add("v_customerid", customerId, DbType.Int32);
            param.Add("v_inquiryid", inquiryId, DbType.Int32);
            param.Add("v_isauthorized", 0, DbType.Int16, ParameterDirection.Output);
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    con.Execute("iscustomerauthorizedtomanagecar", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("iscustomerauthorizedtomanagecar");
                    isAuthorized = param.Get<Int16>("v_isauthorized") == 1;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "SellCarRepository.IsCustomerAuthorizedToManageCar");
                objErr.SendMail();
            }
            return isAuthorized;
        }

        public void GetSellCarExpiry(int customerId, int inquiryId, out DateTime? expiryDate, out string carName)
        {
            expiryDate = null;
            carName = null;
            var param = new DynamicParameters();
            param.Add("v_customerid", customerId, DbType.Int32);
            param.Add("v_inquiryid", inquiryId, DbType.Int32);
            param.Add("v_carname", null, DbType.String, ParameterDirection.Output);
            param.Add("v_expiry", null, DbType.DateTime, ParameterDirection.Output);
            try
            {
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("getsellcarexpiry", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("getsellcarexpiry");
                    expiryDate = param.Get<DateTime>("v_expiry");
                    carName = param.Get<string>("v_carname");
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "SellCarRepository.GetSellCarExpiry");
                objErr.SendMail();
            }
        }

        public bool RenewSellCarListing(int inquiryId, int customerId)
        {
            var param = new DynamicParameters();
            param.Add("v_customerid", customerId, DbType.Int32);
            param.Add("v_inquiryid", inquiryId, DbType.Int32);
            param.Add("v_affectedrows", 0, DbType.Int32, direction: ParameterDirection.Output);
            try
            {
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("renewlisting", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("renewlisting");
                    return param.Get<int>("v_affectedrows") > 0;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "SellCarRepository.RenewSellCarListing");
                objErr.SendMail();
            }
            return false;
        }

        public TempCustomerSellInquiry GetTempCustomerSellInquiry(int tempInquiryId)
        {
            TempCustomerSellInquiry tempCustomerSellInquiry = new TempCustomerSellInquiry();
            var param = new DynamicParameters();
            param.Add("v_tempInquiryId", tempInquiryId, DbType.Int32);
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    var result = con.Query("GetTempInquiryCustomerDetails", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result != null)
                    {
                        if (result.ContactDetails != null)
                        {
                            tempCustomerSellInquiry.sellCarCustomer = new SellCarCustomer();
                            tempCustomerSellInquiry.sellCarCustomer = JsonConvert.DeserializeObject<SellCarCustomer>(result.ContactDetails);
                            tempCustomerSellInquiry.sellCarCustomer.Id = result.CustomerId;
                        }

                        if (result.InquiryDetails != null)
                        {
                            tempCustomerSellInquiry.sellCarInfo = new SellCarInfo();
                            tempCustomerSellInquiry.sellCarInfo = JsonConvert.DeserializeObject<SellCarInfo>(result.InquiryDetails);
                            tempCustomerSellInquiry.sellCarInfo.TempInquiryId = tempInquiryId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "tempId : " + tempInquiryId);
            }
            return tempCustomerSellInquiry;
        }

        public int SaveSellCarDetails(SellCarInfo sellCarInfo, SellCarCustomer customer)
        {
            try
            {
                var param = GetSaveCarDetailsParameters(sellCarInfo, customer);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("insertcustomersellinquiry", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("insertcustomersellinquiry");
                    return param.Get<int>("v_id");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return -1;
        }

        public int SaveSellCarDetailsV1(SellCarInfo sellCarInfo, SellCarCustomer customer)
        {

            try
            {
                var param = GetSaveCarDetailsParameters(sellCarInfo, customer);
                param.Add("v_regtype", sellCarInfo.RegType.ToString());
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("insertcustomersellinquiry_v1", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("insertcustomersellinquiry_v1");
                    return param.Get<int>("v_id");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return -1;
        }

        private static DynamicParameters GetSaveCarDetailsParameters(SellCarInfo sellCarInfo, SellCarCustomer customer)
        {
            DateTime? insuranceExpiryDate = null;
            if (sellCarInfo.InsuranceExpiryYear.HasValue && sellCarInfo.InsuranceExpiryMonth.HasValue)
            {
                insuranceExpiryDate = new DateTime(sellCarInfo.InsuranceExpiryYear.Value, sellCarInfo.InsuranceExpiryMonth.Value, 01);
            }
            var param = new DynamicParameters();
            param.Add("v_customerid", customer.Id, DbType.Int32);
            param.Add("v_cityid", customer.CityId, DbType.Int32);
            param.Add("v_carversionid", sellCarInfo.VersionId, DbType.Int32);
            param.Add("v_makeyear", new DateTime(sellCarInfo.ManufactureYear, sellCarInfo.ManufactureMonth, 1), DbType.DateTime);
            param.Add("v_kms", sellCarInfo.KmsDriven, DbType.Int32);
            param.Add("v_price", sellCarInfo.ExpectedPrice, DbType.Int32);
            param.Add("v_sourceid", sellCarInfo.SourceId, DbType.Int16);
            param.Add("v_pincode", sellCarInfo.PinCode, DbType.Int32);
            param.Add("v_referrer", sellCarInfo.Referrer, DbType.String);
            param.Add("v_ipaddress", sellCarInfo.IPAddress, DbType.String);
            param.Add("v_areaid", sellCarInfo.AreaId, DbType.Int32);
            param.Add("v_customername", customer.Name, DbType.String);
            param.Add("v_customeremail", customer.Email, DbType.String);
            param.Add("v_customermobile", customer.Mobile, DbType.String);
            param.Add("v_sharetoct", sellCarInfo.ShareToCT, DbType.Boolean);
            param.Add("v_owners", sellCarInfo.Owners, DbType.Int16);
            param.Add("v_regno", sellCarInfo.RegistrationNumber, DbType.String);
            param.Add("v_color", sellCarInfo.Color, DbType.String);
            param.Add("v_additionalfuel", sellCarInfo.AlternateFuel, DbType.String);
            param.Add("v_insurance", Convert.ToString(sellCarInfo.Insurance), DbType.String);
            param.Add("v_insuranceexpiry", insuranceExpiryDate, DbType.DateTime);
            param.Add("v_tempinquiryid", sellCarInfo.TempInquiryId, DbType.Int32);
            param.Add("v_id", 0, DbType.Int32, direction: ParameterDirection.Output);
            return param;
        }

        public C2BStockDetailsV2 GetTempSellCarDetails(int TempInquiryId)
        {
            C2BStockDetailsV2 c2bStockDetails = new C2BStockDetailsV2();

            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_tempinquiryid", TempInquiryId);
                    c2bStockDetails = con.Query<C2BStockDetailsV2, string, string, C2BStockDetailsV2>("gettempsellcardetails",
                        (tempC2BStockDetails, customerDetails, carDetails)
                        =>
                        {
                            tempC2BStockDetails.Customer = customerDetails != null ? JsonConvert.DeserializeObject<SellCarCustomer>(customerDetails) : null;
                            tempC2BStockDetails.SellCarDetails = carDetails != null ? JsonConvert.DeserializeObject<SellCarInfo>(carDetails) : null;
                            return tempC2BStockDetails;
                        }, param, commandType: CommandType.StoredProcedure, splitOn: "contactDetails,inquiryDetails").First();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error in Fetching TempSellCarDetails from Database");
            }
            return c2bStockDetails;
        }

        public void InsertSellCarC2BLead(int? tempInquiryId, int? inquiryId, int? ctTempId, int? lastAction, string status)
        {
            try
            {
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_inquiryid", inquiryId);
                    param.Add("v_tempinquiryid", tempInquiryId);
                    param.Add("v_cttempid", ctTempId);
                    param.Add("v_lastaction", lastAction);
                    param.Add("v_status", status);
                    con.Execute("insertsellcarc2blead", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error in inserting SellcarC2BLead in database");
            }
        }

        public bool SaveOtherDetails(SellCarInfo sellCarInfo, int inquiryId)
        {
            if (sellCarInfo == null)
            {
                return false;
            }
            try
            {
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_mileage", sellCarInfo.Mileage, DbType.Int32);
                    param.Add("v_warranties", sellCarInfo.Warranties, DbType.String);
                    param.Add("v_comments", sellCarInfo.Comments, DbType.String);
                    param.Add("v_inquiryid", inquiryId, DbType.Int32);
                    return con.Execute("savesellcarotherdetails", param, commandType: CommandType.StoredProcedure) > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }

        public IEnumerable<int> C2BCities()
        {
            IEnumerable<int> cities = null;
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    cities = con.Query<int>("getc2bcities", commandType: CommandType.StoredProcedure);
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return cities;
        }

        public bool InsertVerifiedMobileEmailPair(string mobile, string email, int sourceId)
        {
            try
            {
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    string searchQuery = "select 1 from cwmasterdb.cv_mobileemailpair where emailid=@email and mobileno=@mobile;";
                    string insertQuery = "insert into cwmasterdb.cv_mobileemailpair (emailid,mobileno,sourceid,createdon) values (@email,@mobile,@sourceId,@createdOn);";
                    string rowsFound = con.Query<string>(searchQuery, new { email, mobile }).FirstOrDefault();
                    if (string.IsNullOrEmpty(rowsFound))
                    {
                        DateTime createdOn = DateTime.Now;
                        con.Execute(insertQuery, new { email, mobile, createdOn, sourceId });
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }
    }
}
