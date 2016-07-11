using System;
using System.Data;


namespace Bikewale.Common
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 10/8/2012
    ///     Class contains all new bike dealer related functions.
    /// </summary>
    public class LocateDealers
    {
        // Written By : Ashish G. Kamble on 10/8/2012
        //this function returns the dataset containing the name and id of the New Bike Dealer 
        //based on the id of the city as passed
        public DataTable GetNewBikeDealerInCity(string cityId)
        {
            ErrorClass objErr = new ErrorClass(new Exception("Method not used/commented"), "LocateDealers.GetNewBikeDealerInCity");
            objErr.SendMail();
            return null;

            //DataSet ds = null;
            //DataTable dt = null;

            //if (CommonOpn.CheckId(cityId) == false)
            //    return dt;

            //if (cityId == "")
            //    return dt;

            //Database db = new Database();
            //string sql = "";

            //sql = " SELECT CAST(Ma.ID AS  VARCHAR) +  '_' + Ma.MaskingName AS Value , Ma.Name AS Text "
            //    + " FROM BikeMakes Ma With(NoLock) WHERE Ma.IsDeleted = 0 AND Ma.New = 1 AND Ma.Id IN "
            //    + " (SELECT MakeId FROM Dealer_NewBike With(NoLock) WHERE CityId = @cityId AND IsActive = 1) "
            //    + " ORDER BY Text ";

            //try
            //{
            //    SqlParameter[] param = { new SqlParameter("@cityId", cityId) };
            //    ds = db.SelectAdaptQry(sql, param);

            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        dt = ds.Tables[0];
            //    }
            //}
            //catch (SqlException err)
            //{
            //    ErrorClass objErr = new ErrorClass(err, "LocateDealers.GetNewBikeDealerInCity");
            //    objErr.SendMail();
            //}
            //catch (Exception err)
            //{
            //    ErrorClass objErr = new ErrorClass(err, "LocateDealers.GetNewBikeDealerInCity");
            //    objErr.SendMail();
            //}

            //return dt;
        }   // End of GetNewBikeDealerInCity method

        /// <summary>
        /// function will return Make id and name for locat dealer passing parameter city
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        public DataTable GetNewBikeMakes(string cityId, string requestType)
        {
            ErrorClass objErr = new ErrorClass(new Exception("Method not used/commented"), "LocateDealers.GetNewBikeMakes");
            objErr.SendMail();
            return null;

            //SqlCommand cmd = new SqlCommand("GetMakes_DealerLocation");
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;
            //cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = requestType;

            //Database db = new Database();
            //DataTable dt = new DataTable();
            //try
            //{
            //    dt = db.SelectAdaptQry(cmd).Tables[0];
            //}
            //catch (SqlException exSql)
            //{
            //    ErrorClass objErr = new ErrorClass(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception ex)
            //{
            //    //Response.Write(ex.Message);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //return dt;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 24 June 2014
        /// Summary : To get dealer Cities list by make id
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public DataTable GetDealersCitiesListByMakeId(uint makeId)
        {
            ErrorClass objErr = new ErrorClass(new Exception("Method not used/commented"), "LocateDealers.GetDealersCitiesListByMakeId");
            objErr.SendMail();
            return null;

            //DataTable dt = new DataTable();

            //try
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        Database db=new Database();

            //        cmd.CommandText = "GetDealersCitiesListByMakeId";
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;

            //        dt = db.SelectAdaptQry(cmd).Tables[0];
            //    }
            //}
            //catch (Exception err)
            //{
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //return dt;
        }

    }   // End of class
}   // End of namespace