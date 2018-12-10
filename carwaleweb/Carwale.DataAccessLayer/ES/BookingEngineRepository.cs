using Carwale.Entity.CarData;
using Carwale.Entity.Customers;
using Carwale.Entity.ES;
using Carwale.Interfaces.ES;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.ES
{
    public class BookingEngineRepository : RepositoryBase, IBookingRepository
    {
        public List<ESVersionColors> GetBookingModelData(int modelId)
        {
            List<ESVersionColors> versionColorsList = new List<ESVersionColors>();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId);

                using (var con = EsMySqlReadConnection)
                {
                    var response = con.QueryMultiple("GetModelColorsData", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("GetModelColorsData");

                    var versionData = response.Read<CarVersionEntity>().ToList();
                    var extColorData = response.Read<ExteriorColor>().ToList();
                    var intColorData = response.Read<InteriorColor>().ToList();
                                     

                    foreach (var version in versionData)
                    {
                        var versionColors = new ESVersionColors();
                        versionColors.ExteriorColor = new List<ExteriorColor>();
                        versionColors.Version = new CarVersionEntity();

                        if (extColorData != null && extColorData.Count > 0)
                            versionColors.ExteriorColor = extColorData.Where(vId => vId.VersionId == version.ID).ToList();
                        versionColors.Version = version;

                        foreach (var data in versionColors.ExteriorColor)
                        {
                            var interiorData = intColorData.Where(id => id.ExtColorId == data.ColorId).ToList();
                            versionColors.ExteriorColor.ForEach(c => ((versionColors.ExteriorColor.Where(x => x.ColorId == data.ColorId).FirstOrDefault())).InteriorColor = interiorData);
                        }
                        versionColorsList.Add(versionColors);
                    }
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return versionColorsList;
        }

        public int GetSetCarCount(int versionId, int extColorId, int intColorId, bool isGetCount)
        {
            int carCount = 0;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_VersionId", versionId);
                param.Add("v_ExtColorId", extColorId);
                param.Add("v_IntColorId", intColorId);
                param.Add("v_GetSetCarCount", isGetCount ? 0 : 1); 

                using (var con = EsMySqlMasterConnection)
                {
                    var response = con.Query("CheckCarAvailability", param, commandType: CommandType.StoredProcedure).AsList();
                    LogLiveSps.LogSpInGrayLog("CheckCarAvailability");
                    if(response.Count > 0)
                        carCount = response.AsList()[0].CarCount;
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return carCount;
        }

        public int SubmitEsCustomerData(ESSurveyCustomerResponse customerResponse)
        {
            int customerId = -1;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_CustomerId", customerResponse.CustomerId != -1 ? customerResponse.CustomerId : -1);
                param.Add("v_Email", customerResponse.BasicInfo.Email != null ? customerResponse.BasicInfo.Email : Convert.DBNull);
                param.Add("v_Mobile", customerResponse.BasicInfo.Mobile != null ? customerResponse.BasicInfo.Mobile : Convert.DBNull);
                param.Add("v_Name", customerResponse.BasicInfo.Name != null ? customerResponse.BasicInfo.Name : Convert.DBNull);
                param.Add("v_CityId", customerResponse.CityId > 0 ? customerResponse.CityId : (int?)null);
                param.Add("v_PlatformId", customerResponse.Platform);
                param.Add("v_Address", String.IsNullOrEmpty(customerResponse.Address) ? "" : customerResponse.Address);
                using (var con = EsMySqlMasterConnection)
                {
                    var response = con.Query("SaveEsCustomerData", param, commandType: CommandType.StoredProcedure).AsList();
                    LogLiveSps.LogSpInGrayLog("SaveEsCustomerData");
                    if (response != null && response.Count > 0)
                        customerId = response.AsList()[0].CustomerId;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return customerId;
        }

        public int SaveEsInquiry(EsInquiry customerInquiry)
        {
            int inquiryId = -1;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_InquiryId", customerInquiry.Id > 0 ? customerInquiry.Id : -1);
                param.Add("v_CustomerId", customerInquiry.CustomerId > 0 ? customerInquiry.CustomerId : (int?)null);
                param.Add("v_VersionId", customerInquiry.VersionId > 0 ? customerInquiry.VersionId : (int?)null);
                param.Add("v_DealerId", customerInquiry.DealerId > 0 ? customerInquiry.DealerId : (int?)null);
                param.Add("v_ExteriorColorId", customerInquiry.ExteriorColorId > 0 ? customerInquiry.ExteriorColorId : (int?)null);
                param.Add("v_InteriorColorId", customerInquiry.InteriorColorId > 0 ? customerInquiry.InteriorColorId : (int?)null);
                param.Add("v_BookingAmount", customerInquiry.BookingAmount > 0 ? customerInquiry.BookingAmount : (int?)null);
                param.Add("v_PaymentType", customerInquiry.PaymentType > 0 ? customerInquiry.PaymentType : (int?)null);
                param.Add("v_TransactionComp", customerInquiry.IsTransactionCompleted ? 1 : 0);
                param.Add("v_TransactionId", customerInquiry.TransactionId);
                using (var con = EsMySqlMasterConnection)
                {
                    var response = con.Query("SaveEsInquiry", param, commandType: CommandType.StoredProcedure).AsList();
                    LogLiveSps.LogSpInGrayLog("SaveEsInquiry");
                    if (response != null && response.Count > 0)
                        inquiryId = response.AsList()[0].InquiryId;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return inquiryId;
        }

        public CustomerMinimal GetEsCustomer(int customerId)
        {
            var customer = new CustomerMinimal();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_CustomerId", customerId, dbType: DbType.Int32);
                using (var con = EsMySqlReadConnection)
                {
                    customer = con.Query<CustomerMinimal>("GetEsCustomer", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch(Exception err)
            {
                Logger.LogException(err);
            }
            return customer;
        }

        public EsBookingSummary GetBookingSummary(int inquiryId)
        {
            var bookingSummary = new EsBookingSummary();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_inquiryId", inquiryId);
                using (var con = EsMySqlReadConnection)
                {
                    bookingSummary = con.Query<EsBookingSummary>("GetBookingSummary", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch(Exception err)
            {
                Logger.LogException(err);
            }
            return bookingSummary;
        }
    }
}
