
using Bikewale.Entities.BikeBooking;
using System;

namespace Bikewale.DAL.AutoBiz
{
    /// <summary>
    /// Written By : Ashwini Todkar on 29 Oct 2014
    /// </summary>
    public class Dealers
    {
        /// <summary>
        /// Written By : Ashwini Todkar on 29 Oct 2014
        /// </summary>
        /// <param name="objParams"></param>
        /// <returns></returns>
        public PQ_DealerDetailEntity GetDealerDetails(PQParameterEntity objParams)
        {
            throw new NotImplementedException();

            //try
            //{
            //using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetDealerDetails"))
            //{
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int64, Convert.ToInt64(objParams.DealerId)));
            //    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionId", DbType.Int64, Convert.ToInt64(objParams.VersionId)));
            //    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityId", DbType.Int64, Convert.ToInt64(objParams.CityId)));

            //    PQ_DealerDetailEntity objDetailPQ = null;

            //    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
            //    {
            //        objDetailPQ = new PQ_DealerDetailEntity();

            //        objDetailPQ.objQuotation = null;

            //        if (dr.Read())
            //        {
            //            objDetailPQ.objQuotation = new PQ_QuotationEntity()
            //            {
            //                objMake = new BikeMakeEntityBase() { MakeName = dr["MakeName"].ToString(), MaskingName = dr["MakeMaskingName"].ToString() },
            //                objModel = new BikeModelEntityBase() { ModelName = dr["ModelName"].ToString(), MaskingName = dr["ModelMaskingName"].ToString() },
            //                objVersion = new BikeVersionEntityBase() { VersionName = dr["VersionName"].ToString() },
            //                LargePicUrl = dr["largePic"].ToString(),
            //                SmallPicUrl = dr["smallPic"].ToString()
            //            };
            //        }

            //        dr.NextResult();

            //        objDetailPQ.objQuotation.PriceList = new List<PQ_Price>();

            //        while (dr.Read())
            //        {
            //            objDetailPQ.objQuotation.PriceList.Add(new PQ_Price() { CategoryName = dr["ItemName"].ToString(), Price = Convert.ToUInt32(dr["Price"]) });
            //        }

            //        dr.NextResult();

            //        if (dr.Read())
            //        {
            //            objDetailPQ.objDealer = new NewBikeDealers();

            //            objDetailPQ.objDealer.Name = dr["FirstName"].ToString() + " " + dr["LastName"].ToString();
            //            objDetailPQ.objDealer.Address = dr["Address"].ToString();
            //            objDetailPQ.objDealer.EmailId = dr["EmailId"].ToString();
            //            objDetailPQ.objDealer.MobileNo = dr["MobileNo"].ToString();
            //            objDetailPQ.objDealer.objArea.Latitude = Convert.ToDouble(dr["Lattitude"]);
            //            objDetailPQ.objDealer.objArea.Longitude = Convert.ToDouble(dr["Longitudes"]);
            //            objDetailPQ.objDealer.objArea.AreaName = dr["AreaName"].ToString();
            //            objDetailPQ.objDealer.objCity.CityName = dr["CityName"].ToString();
            //            objDetailPQ.objDealer.objState.StateName = dr["StateName"].ToString();
            //            objDetailPQ.objDealer.Website = dr["WebsiteUrl"].ToString();
            //            objDetailPQ.objDealer.Organization = dr["Organization"].ToString();

            //        }
            //    }
            //}
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn("Exception at GetDealerDetails : " + ex.Message);
            //}
            //return new PQ_DealerDetailEntity();
        }
    }
}
