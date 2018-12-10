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
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Drawing;
using System.Security.Principal;
using System.Web.Mail;
using System.IO;
using Ajax;
using Carwale.PaymentWebServicesClient;
using System.Threading;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.UI.Common;
using Carwale.Notifications.Logs;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.Utility;

namespace CarwaleAjax
{
    public class AjaxFunctions
    {
        private char _delimiter = '|';

        //this function returns the dataset containing the name and id of the city
        //based on the id of the state as passed
        [Ajax.AjaxMethod()]
        public DataSet GetCities(string stateId)
        {
            DataSet ds = new DataSet();

            if (stateId == "" || CommonOpn.CheckId(stateId) == false)
                return ds;

            try
            {
                using (var cmd = DbFactory.GetDBCommand("cwmasterdb.GetCitiesByStateId_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StateId", DbType.Int16, CustomParser.parseIntObject(stateId) > 0 ? CustomParser.parseIntObject(stateId) : 0));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }

            return ds;
        }

        //the content is in the form of name value pairs separated with |
        //split the content and then add them to the dropdownlist, and 
        //make the selected item true
        public void UpdateContents(DropDownList drp, string content, string selectedValue)
        {
            UpdateContents(drp, content, selectedValue, "Any");
        }

        //selectName is the value which is to be at the top of the dropdown
        public void UpdateContents(DropDownList drp, string content, string selectedValue, string selectName)
        {
            drp.Items.Clear();
            drp.Enabled = true;

            //add Any at the top
            drp.Items.Add(new ListItem(selectName, "0"));

            if (content != "")
            {
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
    }//class
}//namespace