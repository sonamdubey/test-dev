using Carwale.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Interfaces;
using System.Data.SqlClient;
using Carwale.DAL.CoreDAL;
using System.Data;
using Carwale.Notifications;
using System.Web;
using Carwale.Utility;
using Carwale.Entity.PaymentGateway;
using Carwale.Entity.Dealers;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Offers;
using Carwale.Entity.Geolocation;
using Dapper;
using Carwale.Entity.Deals;
using Carwale.Interfaces.Deals;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;


namespace Carwale.DAL.Deals
{
    public class DealsInquiryRepository : RepositoryBase, IRepository<DealsInquiryDetail>, IDealsUserInquiry<DropOffInquiryDetailEntity> ,IDealInquiriesRepository
    {
        public IEnumerable<DealsInquiryDetail> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DealsInquiryDetail> GetAllById(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<DealsInquiryDetail> Find(SearchQuery<DealsInquiryDetail> query, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public DealsInquiryDetail GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Create(DealsInquiryDetail entity)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_id", -1);
                param.Add("v_customername", entity.CustomerName.Trim());
                param.Add("v_customeremail", entity.CustomerEmail.Trim());
                param.Add("v_customermobile", entity.CustomerMobile.Trim());
                param.Add("v_stockid", entity.StockId);
                param.Add("v_cityid", entity.CityId);
                param.Add("v_entrydatetime", DateTime.Now);
                param.Add("v_source", entity.Source);
                param.Add("v_platformid", entity.PlatformId);
                param.Add("v_eagerness", entity.Eagerness);
                param.Add("v_mastercityid", entity.MasterCityId > 0 ? (int?)entity.MasterCityId : null);
                param.Add("v_abtestvalue", entity.ABTestValue > 0 ? (int?)entity.ABTestValue : null);
                param.Add("v_recordid", direction: ParameterDirection.Output);

                using (var con = AdvantageMySqlMasterConnection)
                {
                    con.Execute("savedealinquiry-v17_1_2", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("savedealinquiry-v17_1_2");
                     return param.Get<int>("v_recordid");
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return default(int);
        }

        public bool Update(DealsInquiryDetail entity)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_id", entity.RecordId);
                param.Add("v_pushstatus", entity.ResponseId);
                param.Add("v_ispaid", entity.IsPaid);

                using (var con = AdvantageMySqlMasterConnection)
                {
                    LogLiveSps.LogSpInGrayLog("updatedealinquiry");
                    return con.Execute("updatedealinquiry", param, commandType: CommandType.StoredProcedure) > 0 ? true : false;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return false;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<DropOffInquiryDetailEntity> GetDealsDroppedUsers()
        {
            List<DropOffInquiryDetailEntity> DroppedOffUserList = new List<DropOffInquiryDetailEntity>();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getdropofusers"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.AdvantageMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            DroppedOffUserList.Add(new DropOffInquiryDetailEntity
                            {
                                CustomerDetail = new TransactionDetails
                                {
                                    CustomerName = CustomParser.parseStringObject(dr["UserName"]),
                                    CustMobile = CustomParser.parseStringObject(dr["UserMobile"]),
                                    CustEmail = CustomParser.parseStringObject(dr["UserEmail"]),
                                },
                                CustLocation = new CustLocation{ 
                                    CityName = CustomParser.parseStringObject(dr["UserCity"]),
                                    CityId = CustomParser.parseIntObject(dr["CityId"])
                                },
                                CreatedOn = CustomParser.parseDateObject(dr["CreatedOn"]),
                                LastCallTime = DateTime.Now,
                                FollowUpTime = DateTime.Now,
                                Comments = "",
                                OfferPrice = CustomParser.parseIntObject(dr["OfferPrice"]),
                                ActualPrice = CustomParser.parseIntObject(dr["ActualPrice"]),
                                ReferenceId = CustomParser.parseIntObject(dr["RefId"]),
                                DealerDetail = new DealerSummary
                                {
                                    Name = CustomParser.parseStringObject(dr["DealerName"]),
                                    DealerId = CustomParser.parseIntObject(dr["DealerId"]),
                                    Address = CustomParser.parseStringObject(dr["DealerAddress"]),
                                    EmailId = CustomParser.parseStringObject(dr["DealerEmail"]),
                                    PhoneNo = CustomParser.parseStringObject(dr["DealerMobile"])
                                },
                                CarDetail = new CarEntity
                                {
                                    MakeName = CustomParser.parseStringObject(dr["Make"]),
                                    ModelName = CustomParser.parseStringObject(dr["Model"]),
                                    VersionName = CustomParser.parseStringObject(dr["Version"]),
                                    ModelId = CustomParser.parseIntObject(dr["ModelId"]),
                                    VersionId = CustomParser.parseIntObject(dr["VersionId"])
                                },
                                ManufacturingMonth = CustomParser.parseStringObject(dr["ManufacturingDate"]),
                                StockDetail = new BasicCarInfo
                                {
                                    Color = CustomParser.parseStringObject(dr["Color"]),
                                    InteriorColor = "",
                                    TCStockId = CustomParser.parseIntObject(dr["StockId"])
                                },
                                Offer = new OffersEntity { ShortDescription = CustomParser.parseStringObject(dr["Offers"]) },
                                Disposition = "",
                                DispositionId = 0,
                                PushStatus = CustomParser.parseIntObject(dr["PushStatus"])
                            });
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return DroppedOffUserList;
        }

        public bool PushMultipleLeads(DealsInquiryDetail dealsInquiry)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_customername", dealsInquiry.CustomerName.Trim());
                param.Add("v_customeremail", dealsInquiry.CustomerEmail.Trim());
                param.Add("v_customermobile", dealsInquiry.CustomerMobile.Trim());
                param.Add("v_stockids", dealsInquiry.MultipleStockId);
                param.Add("v_cityid", dealsInquiry.CityId);
                param.Add("v_source", dealsInquiry.Source);
                param.Add("v_platformid", dealsInquiry.PlatformId);
                param.Add("v_eagerness", dealsInquiry.Eagerness);
                param.Add("v_mastercityid", dealsInquiry.MasterCityId > 0 ? (int?)dealsInquiry.MasterCityId : null);
                param.Add("v_abtestvalue", dealsInquiry.ABTestValue > 0 ? (int?)dealsInquiry.ABTestValue : null);
                param.Add("v_isinserted", direction: ParameterDirection.Output);
                
                using (var con = AdvantageMySqlMasterConnection)
                {
                    con.Execute("savemultipledealinquiries-v17_1_2", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("savemultipledealinquiries");
                    return param.Get<int>("v_isinserted") == 0 ? false : true;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return false;
        }

        public bool Delete(DealsInquiryDetail entity)
        {
            throw new NotImplementedException();
        }        
    }
}
