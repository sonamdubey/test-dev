using Carwale.Entity.Enum;
using Carwale.Entity.Stock;
using Carwale.Interfaces.Stock;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Stock
{
    public class StockCitiesRepository : RepositoryBase, IStockCitiesRepository
    {
        public bool DeleteStockCities(int inquiryId, SellerType sellerType)
        {
            string updateQuery = GetUpdateStockTableQuery(sellerType, false, inquiryId);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute(GetDeleteFromStockCitiesQuery(inquiryId, sellerType), commandType: CommandType.Text);
                    con.Execute(updateQuery, commandType: CommandType.Text);
                }
                return true;
        }

        public bool AddStockCities(int inquiryId, SellerType sellerType, IEnumerable<int> cityIds)
        {
            string insertQuery = "insert into stockcities (inquiryid, sellertype, cityid) values (@InquiryId, @SellerType, @CityId)";
            string updateQuery = GetUpdateStockTableQuery(sellerType, true, inquiryId);
            int seller = (int) sellerType;
            using(var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute(GetDeleteFromStockCitiesQuery(inquiryId, sellerType), commandType: CommandType.Text );
                con.Execute(insertQuery,param: cityIds.Select(cityId => new {
                    InquiryId = inquiryId,
                    SellerType = seller,
                    CityId = cityId
                }), commandType: CommandType.Text);
                con.Execute(updateQuery,commandType: CommandType.Text);
            }
            return true;
        }

        private static string GetDeleteFromStockCitiesQuery(int inquiryId, SellerType sellerType)
        {
            return $"delete from stockcities where inquiryid = {inquiryId} and sellerType = {(int)sellerType}";
        }

        private static string GetUpdateStockTableQuery(SellerType sellerType, bool isMultiCityStock, int inquiryId)
        {
            return $"update {GetStockTable(sellerType)} set isMultiCityStock = {isMultiCityStock} where id = {inquiryId}";
        }

        private static string GetStockTable(SellerType sellerType)
        {
            return  sellerType == SellerType.Dealer ? "sellinquiries" : "customersellinquiries";
        }

        public IEnumerable<StockCity> GetStockCities(int inquiryId, SellerType sellerType)
        {
            using (var con = ClassifiedMySqlMasterConnection)
            {
                string query = @"select sc.cityid, ci.name as cityname 
                                 from stockcities sc inner join cwmasterdb.cities ci on ci.id = sc.cityid 
                                 where sc.inquiryid = @v_inquiryid and sc.sellerType = @v_sellerType";
                return con.Query<StockCity>(query, new { v_inquiryid = inquiryId, v_sellerType = (int)sellerType }, commandType: CommandType.Text);
            }
        }
    }
}