using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr.Interface.BikePricing;
using BikewaleOpr.Entity.BikePricing;
using Bikewale.Notifications;
using System.Data;
using System.Data.Common;
using MySql.CoreDAL;
using System.Collections.ObjectModel;
using Bikewale.Utility;


namespace BikewaleOpr.DALs.BikePricing
{
    /// <summary>
    /// Created by : Aditi Srivastava on 18 Jan 2017
    /// Summary    : Repository for bike price categories
    /// </summary>
    public class DealerPriceRepository : IDealerPriceRepository
    {
        /// <summary>
        /// Created by : Aditi Srivastava on 18 Jan 2017
        /// Summary    : To get a list of all price categories
        /// </summary>
        /// <returns></returns>
        public ICollection<PriceCategoryEntity> GetAllPriceCategories()
        {
            ICollection<PriceCategoryEntity> priceCatList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikepricecategories"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            priceCatList = new Collection<PriceCategoryEntity>();
                            while (dr.Read())
                            {
                                PriceCategoryEntity objPriceCat = new PriceCategoryEntity();
                                objPriceCat.PriceCategoryId = SqlReaderConvertor.ToInt32(dr["ItemCategoryId"]);
                                objPriceCat.PriceCategoryName = Convert.ToString(dr["ItemName"]);
                                priceCatList.Add(objPriceCat);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.BikePricing.DealerPriceRepository.GetAllPriceCategories");
            }
            return priceCatList;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 18 Jan 2017
        /// Summary    : To add a new bike price category
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public bool SaveBikeCategory(string categoryName)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("addpricecategories"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pricecategory", DbType.String, categoryName));
                    if (Convert.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase)))
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex,String.Format("BikewaleOpr.Dals.BikePricing.DealerPriceRepository.SaveBikeCategory : Category:{0}",categoryName));
            }
            return isSuccess;
        }
    }
}
