using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Entities.PriceQuote;
using Bikewale.CoreDAL;
using Bikewale.Notifications;

namespace Bikewale.DAL.PriceQuote
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : class have functions related to price quote.
    /// </summary>
    public class PriceQuoteRepository : IPriceQuote
    {
        /// <summary>
        /// Summary : function to save the price quote.
        /// Modified By : Sadhana Upadhyay on 24th Oct 2014
        /// Summary : Added AreaId varible and removed customerid, customer name, customer email, customer mobile variable
        /// Modified By : Sadhana Upadhyay on 20 July 2015
        /// Summary : Added Dealer id as parameter to save in newbikepricequotes table
        /// </summary>
        /// <param name="pqParams">All necessory parameters to save the price quote</param>
        /// <returns>Returns registered price quote id</returns>
        public ulong RegisterPriceQuote(PriceQuoteParametersEntity pqParams)
        {
            ulong quoteId = 0;
            Database db = null;
            try
            {
                // Modified By : Ashis G. Kamble on 22 Nov 2012.
                // Added : Check whether version id is null or not. If null do not save pricequote.
                if (pqParams.VersionId > 0)
                {
                    db = new Database();
                    using (SqlConnection conn = new SqlConnection(db.GetConString()))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SavePriceQuote_New";
                            cmd.Connection = conn;

                            cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = pqParams.CityId;

                            if (pqParams.AreaId > 0)
                                cmd.Parameters.Add("@AreaId", SqlDbType.Int).Value = pqParams.AreaId;

                            cmd.Parameters.Add("@BikeVersionId", SqlDbType.Int).Value = pqParams.VersionId;
                            cmd.Parameters.Add("@SourceId", SqlDbType.TinyInt).Value = pqParams.SourceId;
                            cmd.Parameters.Add("@ClientIP", SqlDbType.VarChar, 40).Value = pqParams.ClientIP;
                            cmd.Parameters.Add("@QuoteId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@DealerId", SqlDbType.Int).Value = pqParams.DealerId;

                            conn.Open();
                            cmd.ExecuteNonQuery();

                            quoteId = Convert.ToUInt64(cmd.Parameters["@QuoteId"].Value);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("SavePriceQuote sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SavePriceQuote ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return quoteId;
        }

        /// <summary>
        /// Summary : Function to Get the price quote by price quote id.
        /// </summary>
        /// <param name="pqId">price quote id. Only positive numbers are allowed</param>
        /// <returns>Returns price quote object.</returns>
        public BikeQuotationEntity GetPriceQuoteById(ulong pqId)
        {
            BikeQuotationEntity objQuotation = null;
            Database db = null;
            try
            {
                objQuotation = new BikeQuotationEntity();

                db = new Database();

                using (SqlConnection conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "GetPriceQuote_New";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@QuoteId", SqlDbType.BigInt).Value = pqId;
                        cmd.Parameters.Add("@ExShowroomPrice", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@RTO", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Insurance", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@OnRoadPrice", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MakeName", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ModelName", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@VersionName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@City", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@VersionId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@NumOfRows", SqlDbType.Int).Direction = ParameterDirection.Output;

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        int numberOfRecords = Convert.ToInt32(cmd.Parameters["@NumOfRows"].Value);
                        if (numberOfRecords > 0)
                        {
                            objQuotation.ExShowroomPrice = Convert.ToUInt64(cmd.Parameters["@ExShowroomPrice"].Value);
                            objQuotation.RTO = Convert.ToUInt32(cmd.Parameters["@RTO"].Value);
                            objQuotation.Insurance = Convert.ToUInt32(cmd.Parameters["@Insurance"].Value);
                            objQuotation.OnRoadPrice = Convert.ToUInt64(cmd.Parameters["@OnRoadPrice"].Value);
                            objQuotation.MakeName = Convert.ToString(cmd.Parameters["@MakeName"].Value);
                            objQuotation.ModelName = Convert.ToString(cmd.Parameters["@ModelName"].Value);
                            objQuotation.VersionName = Convert.ToString(cmd.Parameters["@VersionName"].Value);
                            objQuotation.City = Convert.ToString(cmd.Parameters["@City"].Value);
                            objQuotation.VersionId = Convert.ToUInt32(cmd.Parameters["@VersionId"].Value);
                            objQuotation.PriceQuoteId = pqId;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetPriceQuote sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetPriceQuote ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objQuotation;
        }

        /// <summary>
        /// Summary : function to get the price quote by providing all the necessory parameters to get the pq.
        /// </summary>
        /// <param name="pqParams">Price quote parameters.</param>
        /// <returns>Returns price qutoe object.</returns>
        public BikeQuotationEntity GetPriceQuote(PriceQuoteParametersEntity pqParams)
        {
            ulong pqId = RegisterPriceQuote(pqParams);
            
            BikeQuotationEntity objQuotation = GetPriceQuoteById(pqId);

            return objQuotation;
        }

        /// <summary>
        /// Summary : Function to get the other versions of the model along with on road prices.
        /// </summary>
        /// <param name="pqId">Price quote id. Only positive numbers are allowed.</param>
        /// <returns>Returns list containing all the versions with on road prices.</returns>
        public List<OtherVersionInfoEntity> GetOtherVersionsPrices(ulong pqId)
        {
            List<OtherVersionInfoEntity> objVersionInfo = null;
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetPriceQuoteVersions_New";

                    cmd.Parameters.Add("@QuoteId", SqlDbType.BigInt).Value = pqId;

                    objVersionInfo = new List<OtherVersionInfoEntity>();



                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        while (dr.Read())
                        {
                            objVersionInfo.Add(new OtherVersionInfoEntity
                            {
                                VersionId = Convert.ToUInt32(dr["VersionId"]),
                                VersionName = Convert.ToString(dr["VersionName"]),
                                OnRoadPrice = Convert.ToUInt64(dr["OnRoadPrice"])
                            });
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetOtherVersionsPrices sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetOtherVersionsPrices ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objVersionInfo;
        }

    }   // Class
}   // namespace
