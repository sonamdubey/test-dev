using Bikewale.CoreDAL;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
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
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealerOffers";
                    cmd.Parameters.Add("@dealerId", SqlDbType.Int, 10).Value = dealerId;
                    cmd.Parameters.Add("@modelId", SqlDbType.Int, 5).Value = modelId;
                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null && dr.HasRows)
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
            finally
            {
                db.CloseConnection();
            }
            return offers;
        }
    }
}
