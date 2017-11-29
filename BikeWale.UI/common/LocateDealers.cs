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
            ErrorClass.LogError(new Exception("Method not used/commented"), "LocateDealers.GetNewBikeDealerInCity");
            
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
            //    ErrorClass.LogError(err, "LocateDealers.GetNewBikeDealerInCity");
            //    
            //}
            //catch (Exception err)
            //{
            //    ErrorClass.LogError(err, "LocateDealers.GetNewBikeDealerInCity");
            //    
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
            ErrorClass.LogError(new Exception("Method not used/commented"), "LocateDealers.GetNewBikeMakes");
            
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
            //    ErrorClass.LogError(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
            //    
            //}
            //catch (Exception ex)
            //{
            //    //Response.Write(ex.Message);
            //    ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    
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
            ErrorClass.LogError(new Exception("Method not used/commented"), "LocateDealers.GetDealersCitiesListByMakeId");
            
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
            //    ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    
            //}
            //return dt;
        }

    }   // End of class
}   // End of namespace