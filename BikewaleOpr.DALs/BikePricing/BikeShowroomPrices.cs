using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Notifications;
using BikewaleOpr.Entity.BikePricing;
using BikewaleOpr.Interface.BikePricing;
using MySql.CoreDAL;

namespace BikewaleOpr.DALs.BikePricing
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 23 Sept 2016
    /// Summary : Class have functions to process pricing in the bikewale opr
    /// </summary>
    public class BikeShowroomPrices : IShowroomPricesRepository
    {
        /// <summary>
        /// Writteny By : Ashish G. Kamble on 23 Sept 2016
        /// Summary : Function to get the existing pricing for the given make and city
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<BikePrice> GetBikePrices(uint makeId, uint cityId)
        {
            IList<BikePrice> objPrices = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetVersionPricesByMakeCity"))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.UInt32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.UInt32, cityId));

                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objPrices = new List<BikePrice>();

                            while (dr.Read())
                            {
                                BikePrice objPrice = new BikePrice();
                                objPrice.MakeName = Convert.ToString(dr["MakeName"]);
                                objPrice.ModelName = Convert.ToString(dr["ModelName"]);
                                objPrice.VersionName = Convert.ToString(dr["VersionName"]);
                                objPrice.VersionId = Convert.ToUInt32(dr["VersionId"]);

                                objPrice.Price = Convert.ToString(dr["Price"]);
                                objPrice.Insurance = Convert.ToString(dr["Insurance"]);
                                objPrice.RTO = Convert.ToString(dr["RTO"]);

                                objPrice.LastUpdatedDate = Convert.ToString(dr["LastUpdatedDate"]);
                                objPrice.LastUpdatedBy = Convert.ToString(dr["LastUpdatedBy"]);

                                objPrices.Add(objPrice);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.BikePricing.BikeShowroomPrices.GetBikePrices");
                objErr.SendMail();
            }
            return objPrices;
        }


        public bool SaveBikePrices(string versionPriceList, string citiesList, int updatedBy)
        {
            bool isUpdated = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("saveversionprices"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionPriceList", DbType.String, versionPriceList));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_citiesList", DbType.String, citiesList));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, updatedBy));

                    cmd.CommandType = CommandType.StoredProcedure;

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.BikePricing.BikeShowroomPrices.GetBikePrices");
                objErr.SendMail();
            }

            return isUpdated;
        }
    }
}
