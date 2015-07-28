/*THIS CLASS contains all the functions which are to be implemented as ajax server side functions.
*/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Ajax;
using BikeWaleOpr.Common;
using System.Text.RegularExpressions;
using System.Configuration;

namespace BikeWaleOpr
{
	public class AjaxFunctions
	{
        private char _delimiter = '|';

        [Ajax.AjaxMethod()]
        public DataSet GetCities(string stateId)
        {
            DataSet ds = new DataSet();

            if (stateId == "")
                return ds;

            Database db = new Database();
            string sql = "";

            sql = " SELECT ID AS Value, Name AS Text FROM Cities WHERE "
                + " StateId =" + stateId + " AND IsDeleted = 0 ORDER BY Text ";

            try
            {
                ds = db.SelectAdaptQry(sql);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "AjaxFunctions.GetCities");
                objErr.SendMail();
            }

            return ds;
        }


        //this function returns the dataset containing the name and id of the models
        //available
        [Ajax.AjaxMethod()]
        public DataSet GetModels(string makeId)
        {
            DataSet ds = new DataSet();

            if (makeId == "")
                return ds;

            Database db = new Database();
            string sql = "";

            sql = " SELECT ID AS Value, Name AS Text FROM BikeModels WHERE IsDeleted = 0 AND "
                + " BikeMakeId =" + makeId + " ORDER BY Text ";
            try
            {
                ds = db.SelectAdaptQry(sql);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "AjaxFunctions.GetModels");
                objErr.SendMail();
            }

            return ds;
        }

        //this function returns the dataset containing the name and id of the versions
        //available
        [Ajax.AjaxMethod()]
        public DataSet GetVersions(string modelId)
        {
            DataSet ds = new DataSet();

            if (modelId == "")
                return ds;

            Database db = new Database();
            string sql = "";

            sql = " SELECT ID AS Value, Name AS Text FROM BikeVersions WHERE IsDeleted = 0 AND "
                + " BikeModelId =" + modelId + " ORDER BY Text ";
            try
            {
                ds = db.SelectAdaptQry(sql);
                HttpContext.Current.Trace.Warn("dscount" + ds.Tables[0].Rows.Count.ToString());
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "AjaxFunctions.GetVersions");
                HttpContext.Current.Trace.Warn("err" + err.Message);
                objErr.SendMail();
            }

            return ds;
        }

        //the content is in the form of name value pairs separated with |
        //split the content and then add them to the dropdownlist, and 
        //make the selected item true
        public void UpdateContents(DropDownList drp, string content, string selectedValue)
        {
            if (content != "")
            {
                drp.Items.Clear();
                drp.Enabled = true;

                //add Any at the top
                drp.Items.Add(new ListItem("Any", "0"));

                string[] listItems = content.Split(_delimiter);

                for (int i = 0; i < listItems.Length - 1; i++)
                {
                    drp.Items.Add(new ListItem(listItems[i], listItems[i + 1]));
                    i++;
                }

                ListItem selectedListItem = drp.Items.FindByValue(selectedValue);
                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                }
            }
        }

        [Ajax.AjaxMethod()]
        public static string GetSpecificationsUrl(string make, string model, string version, string versionId)
        {
            return FormatSpecialNoSpaces(make) + "-bikes/" + FormatSpecialNoSpaces(model) + "/" +
                    FormatSpecialNoSpaces(version) + "-specifications-" + versionId + ".html";
        }

        public static string FormatSpecialNoSpaces(string url)
        {
            string reg = @"[^/\-0-9a-zA-Z\s]*"; // everything except a-z, 0-9, / and -

            url = Regex.Replace(url, reg, "", RegexOptions.IgnoreCase);

            return url.Replace(" ", "").Replace("-", "").Replace("/", "").ToLower();
        }

        [Ajax.AjaxMethod()]
        public bool UpdateLaunchDate(string Id, string minPrice, string maxPrice, string expLaunch, string newLaunch, string modelId)
        {
            string[] tempDate = newLaunch.Split('-');
            DateTime newLaunchDate = new DateTime(int.Parse(tempDate[0]), int.Parse(tempDate[1]), int.Parse(tempDate[2]), int.Parse(tempDate[3]), int.Parse(tempDate[4]), 0);

            bool retVal = false;
            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();
            CommonOpn op = new CommonOpn();

            string conStr = db.GetConString();
            con = new SqlConnection(conStr);

            try
            {
                cmd = new SqlCommand("CON_UpdateExpectedBikeLaunches", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@Id", SqlDbType.BigInt);
                prm.Value = Id;

                prm = cmd.Parameters.Add("@ExpectedLaunch", SqlDbType.VarChar, 250);
                prm.Value = expLaunch;

                prm = cmd.Parameters.Add("@LaunchDate", SqlDbType.DateTime);
                prm.Value = newLaunchDate;

                prm = cmd.Parameters.Add("@EstimatedPriceMin", SqlDbType.Decimal);
                prm.Value = minPrice;
                prm.Precision = 5;
                prm.Scale = 2;

                prm = cmd.Parameters.Add("@EstimatedPriceMax", SqlDbType.Decimal);
                prm.Value = maxPrice;
                prm.Precision = 5;
                prm.Scale = 2;

                prm = cmd.Parameters.Add("@ModelId", SqlDbType.VarChar, 10);
                prm.Value = modelId;

                prm = cmd.Parameters.Add("@Url", SqlDbType.VarChar, 100);
                prm.Value = ConfigurationManager.AppSettings["imgHostURL"];

                con.Open();
                //run the command
                cmd.ExecuteNonQuery();

                retVal = true;

            }
            catch (SqlException err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                HttpContext.Current.Trace.Warn(err.Message.ToString());
                ErrorClass objErr = new ErrorClass(err, "AjaxFunctions.UpdateLaunchDet");
                objErr.SendMail();
                retVal = false;
            } // catch SqlException
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message.ToString());
                ErrorClass objErr = new ErrorClass(err, "AjaxFunctions.UpdateLaunchDet");
                objErr.SendMail();
                retVal = false;
            } // catch Exception
            finally
            {
                //close the connection	
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return retVal;
        }

        //this function returns the dataset containing the name and id of the new models
        //available
        /*Summary   : Get Model from Make
          Author    : 
          Modifier  : Dilip V. 13-Jun-2012 (Parametrized and removed constrain of New)*/
        [Ajax.AjaxMethod()]
        public DataSet GetNewModels(string makeId)
        {
            return CommonOpn.GetModelFromMake(makeId);
        }

        //this function returns the dataset containing the name and id of the new versions
        //available
        [Ajax.AjaxMethod()]
        public DataSet GetNewVersions(string modelId)
        {
            DataSet ds = new DataSet();

            if (modelId == "")
                return ds;

            Database db = new Database();
            string sql = "";

            sql = " SELECT ID AS Value, Name AS Text FROM BikeVersions WHERE IsDeleted = 0 AND "
                + " BikeModelId =" + modelId + " AND New = 1 ORDER BY Text ";

            try
            {
                ds = db.SelectAdaptQry(sql);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "AjaxFunctions.GetVersions");
                objErr.SendMail();
            }

            return ds;
        }

        //this function returns a string containing the help contents 
        // for any given id
        /// <summary>
        ///     Written By : Ashish G. Kamble on 9/4/2012
        /// </summary>
        /// <param name="bikeVersionId"></param>
        /// <param name="cityId"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod()]
        public string CalculateInsurancePremium(string bikeVersionId, string cityId, double price)
        {
            return CommonOpn.GetInsurancePremium(bikeVersionId, cityId, price).ToString();
        }

        //this function returns a string containing the help contents 
        // for any given id
        /// <summary>
        ///     Written By : Ashish G. Kamble on 9/4/2012
        /// </summary>
        /// <param name="bikeVersionId"></param>
        /// <param name="cityId"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod()]
        public string CalculateRegistrationCharges(string bikeVersionId, string cityId, double price)
        {
            return CommonOpn.GetRegistrationCharges(bikeVersionId, cityId, price).ToString();
        }

        [Ajax.AjaxMethod()]
		public string MapOemCities(string cityName, string cityId, string cwCityName)
		{
			string retVal = "Some Prob";
			ContentCommon.MapCities(cityName.Trim(), cityId.Trim(), cwCityName.Trim()).ToString();
			
			return retVal;
		}
		
		[Ajax.AjaxMethod()]
		public string MapUOemCities(string cityName, string cityId, string cwCityName)
		{
			//update the city
			string retVal = "Some Prob";
			ContentCommon.UpdateCities(cityName.Trim(), cityId.Trim(), cwCityName.Trim()).ToString();
			
			return retVal;
		}
		
		[Ajax.AjaxMethod()]
		public string MapOemBikes(string bikeName, string bikeId, string cwBikeName)
		{
			string retVal = "Some Prob";
			ContentCommon.MapBikes(bikeName.Trim(), bikeId.Trim(), cwBikeName.Trim()).ToString();
			
			return retVal;
		}
		
		[Ajax.AjaxMethod()]
		public string MapUOemBikes(string bikeName, string bikeId, string cwBikeName)
		{
			string retVal = "Some Prob";
			ContentCommon.UpdateBikes(bikeName.Trim(), bikeId.Trim(), cwBikeName.Trim()).ToString();
			
			return retVal;
		}


    }//class
}//namespace
