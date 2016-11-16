using BikeWaleOpr.Common;
using MySql.CoreDAL;
/*THIS CLASS contains all the functions which are to be implemented as ajax server side functions.
*/
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

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

            string sql = "";
            uint _stateId = default(uint);
            if (!string.IsNullOrEmpty(stateId) && uint.TryParse(stateId, out _stateId))
            {
                sql = " select id as Value, name as Text from cities where stateid =" + _stateId + " and isdeleted = 0 order by text ";
            }

            try
            {
                if (_stateId > 0)
                {
                    ds = MySqlDatabase.SelectAdapterQuery(sql, ConnectionType.ReadOnly);
                }
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
            string sql = "";

            uint _makeId = default(uint);
            if (!string.IsNullOrEmpty(makeId) && uint.TryParse(makeId, out _makeId))
            {
                sql = " select id AS Value, name AS Text from bikemodels where isdeleted = 0 and bikemakeid =" + _makeId + " order by text ";
            }


            try
            {
                if (_makeId > 0)
                {
                    ds = MySqlDatabase.SelectAdapterQuery(sql, ConnectionType.ReadOnly);
                }
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
            string sql = "";

            uint _modelid = default(uint);
            if (!string.IsNullOrEmpty(modelId) && uint.TryParse(modelId, out _modelid))
            {
                sql = " select id as Value, name as Text from bikeversions where isdeleted = 0 and  bikemodelid =" + modelId + " order by text ";
            }

            try
            {
                if (_modelid > 0)
                {
                    ds = MySqlDatabase.SelectAdapterQuery(sql, ConnectionType.ReadOnly);
                }

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

        //this function returns a string containing the help contents 
        // for any given id
        /// <summary>
        ///     Written By : Ashish G. Kamble on 9/4/2012
        /// </summary>
        /// <param name="bikeVersionId"></param>
        /// <param name="cityId"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(), AjaxPro.AjaxMethod()]
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
        [Ajax.AjaxMethod(), AjaxPro.AjaxMethod()]
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
