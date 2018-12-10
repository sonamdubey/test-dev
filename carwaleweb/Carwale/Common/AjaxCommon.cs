using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Carwale.UI.Common;
using AjaxPro;
using Ajax;
using Newtonsoft.Json;
using Carwale.Entity;
using Carwale.BL.Customers;
using Carwale.Interfaces;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications.Logs;
using Carwale.DAL;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.Utility;

namespace CarwaleAjax
{
    public class AjaxCommon : RepositoryBase
    {
        // write all the Common Ajax functions come here
        // These functions will be common for entire website

        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;

        /* 
            This function returns the dataset containing the name and id of the city
            based on the id of the state
        */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public string GetCities(string stateId)
        {
            DataSet ds = new DataSet();

            if (stateId == "" || CommonOpn.CheckId(stateId) == false)
                return "";

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
                ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return JSON.GetJSONString(ds.Tables[0]);
        }
        /*****************************************************************************************************************************
		
            Ajax functions related to the customers like..
                -- Send password if user forgot.
                -- Check login status
                -- Register user through ajax
                -- Login user through ajax
                -- Get current looged in user name
                --
        ********************************************************************************************************************************/
        /*
            Finction to check customer login status through AJAX.
            This function true if user is logged in else false;
        */
        
        // function to login through ajax
       
        
        /*******************************************************************************************************************************************
            -- Ajax function realated to customers ends here
        **********************************************************************************************************************************************/
        
        ///// <summary>
        ///// Function for Fetching image pending in rabbit mq 
        ///// </summary>
        ///// <param name="makeId"></param>
        ///// <param name="modelId"></param>
        ///// <returns></returns>
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public string FetchProcessedImagesList(string imageList, string imgCategory)
        {
            if (imageList.Length > 0)
                imageList = imageList.Substring(0, imageList.Length - 1);

            CommonRQ imgP = new CommonRQ();

            DataSet ds = imgP.FetchProcessedImagesList(imageList, imgCategory);

            //return ds.Tables[0].Rows.Count.ToString();
            try
            {
                if (ds != null && ds.Tables.Count > 0)                    
                    return JsonConvert.SerializeObject(ds.Tables[0]);
                else
                    return "";
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "AjaxFunctions.FetchProcessedImagesList");
                objErr.SendMail();
            }
            return "";
        }

        [AjaxPro.AjaxMethod()]
        public bool CustomerPasswordChangeRequest(string email)
        {
            bool retVal = false;

            ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
            retVal = customerRepo.GenPasswordChangeAT(email);
            return retVal;
        }
    }
}