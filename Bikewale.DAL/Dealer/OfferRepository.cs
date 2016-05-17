using Bikewale.CoreDAL;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.DAL.Dealer
{
    public class OfferRepository : IOffer
    {
        public List<Offer> GetOffersByDealerId(uint dealerId, uint modelId)
        {
            List<Offer> offers = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getdealeroffers";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbParamTypeMapper.GetInstance[SqlDbType.Int], 10, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], 5, modelId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null )
                        {
                            offers = new List<Offer>();
                            while (dr.Read())
                            {
                                offers.Add(new Offer
                                {
                                    OfferId = Convert.ToUInt32(dr["Id"]),
                                    OfferText = Convert.ToString(dr["OfferText"]),
                                    OfferValue = Convert.ToUInt32(dr["OfferValue"]),
                                });
                            }
                        }

                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetOffersByDealerId sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetOffersByDealerId ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return offers;
        }
    }
}
