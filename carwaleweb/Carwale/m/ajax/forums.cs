using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using AjaxPro;
using MobileWeb.Common;
using System.Configuration;
using MobileWeb.DataLayer;
using Carwale.Entity;
using Carwale.BL.Customers;
using Carwale.Interfaces;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity.Enum;

namespace MobileWeb.Ajax 
{
	public class Forums
	{
		[AjaxPro.AjaxMethod()]
		public string DoLogin(string loginId, string passwdEnter)
		{
			string retVal = "0";		       
			string userId = "";
			string name = "";
			string isEmailVerified = "";		
			try
			{             
                ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
                Customer customer = customerRepo.GetCustomer(loginId, passwdEnter);

                userId = customer.CustomerId;
                name = customer.Name;
                isEmailVerified = customer.IsEmailVerified.ToString();
				
				if(userId == "-1")
				{
					retVal = "0";	
				}
				else
                {
                    CurrentUser.StartSession(name, userId, loginId, Convert.ToBoolean(isEmailVerified));
                    retVal = "1";
				}
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
		
			return retVal + "::::" + name;
		}
		
		[AjaxPro.AjaxMethod()]
		public string UserRegistration(string customerName, string password, string email, string phone, string mobile, string cityId )
		{	
			if( customerName.ToString().Trim() == "" || email.ToString().Trim() == "" || password.ToString() == "" )
			{
				return "";
			}
			
			string retStr = "";	
			string customerId = "";
            CustomerOnRegister customer = new CustomerOnRegister();
            string val = string.Empty;
            try
            {
                ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
                customer = customerRepo.CreateCustomer(new Customer()
                {
                    Name = customerName,
                    Email = email,
                    Password = password,
                    Mobile = !string.IsNullOrEmpty(mobile) ? mobile : phone,
                    CityId = !string.IsNullOrEmpty(cityId) ? Convert.ToInt32(cityId) : -1
                });
                customerId = customer.CustomerId;
                retStr = customerId;
            }
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
			finally
			{
                if (customer.StatusOnRegister == "N") 
				{
                    CommonOpn co=new CommonOpn();
					CurrentUser.StartSession( customerName, customerId, email, false );
					UpdateSourceId(customerId);															
				}
				else if ( customer.StatusOnRegister == "O"  )
					retStr = "-1";
			}
			return retStr;
		}
		
		
		
		private void UpdateSourceId(string id)
		{
            string sourceId = ConfigurationManager.AppSettings["MobileSourceId"].ToString();
			if(sourceId != "1" && id != "")
			{
                ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
                customerRepo.UpdateSourceId((EnumTableType)Convert.ToUInt32(sourceId),id);                
			}
		}
		
		[AjaxPro.AjaxMethod()]
		public string SendPasswordByEmail(string emailId )
		{
			string retVal = "0";       
            ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
            if (customerRepo.GenPasswordChangeAT(emailId))
                retVal = "1";

			return retVal;
		}			
		
		[AjaxPro.AjaxMethod()]
		public string GetSearchId(string searchTerm, string searchType)
		{	
			if (HttpContext.Current.Request.Cookies["ForumSearch"] != null && HttpContext.Current.Request.Cookies["ForumSearch"].Value.ToString().IndexOf(searchTerm + "~" + searchType) != -1)
				return FetchSearchIdResultsFromCookie(searchTerm, searchType);
			else
				return FetchSearchIdResultsFromDatabase(searchTerm, searchType);		
		}
		
		private string FetchSearchIdResultsFromCookie(string searchTerm, string searchType)
		{
			string[] splittedCookieValue = 	HttpContext.Current.Request.Cookies["ForumSearch"].Value.ToString().Split('~');
			
			for (int i=0; i<splittedCookieValue.Length; i=i+4)
			{
				if (splittedCookieValue[i] == searchTerm && splittedCookieValue[i+1] == searchType)
					return splittedCookieValue[i+2] + "|" + splittedCookieValue[i+3];
			}
			
			return "-1|-1";
		}
		
		private string FetchSearchIdResultsFromDatabase(string searchTerm, string searchType)
		{
			string searchId = "-1", totalResults = "-1";				
			try
			{           
                using (DbCommand cmd = DbFactory.GetDBCommand("SearchForums_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_searchterm", DbType.String, 500, searchTerm));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_searchdatetime", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_searchtype", DbType.String, 100, searchType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_searchid", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_results", DbType.Int32, ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                    searchId = cmd.Parameters["v_searchid"].Value.ToString();
                    totalResults = cmd.Parameters["v_results"].Value.ToString();
                }

				AddToForumSearchCookie(searchId, searchTerm, searchType, totalResults);
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}			
			return searchId + "|" + totalResults;
		}
		
		private void AddToForumSearchCookie(string searchId, string searchTerm, string searchType, string totalResults)
		{
			string valueToAdd = searchTerm + "~" + searchType + "~" + searchId + "~" + totalResults;
			if (HttpContext.Current.Request.Cookies["ForumSearch"] == null)
				HttpContext.Current.Response.Cookies["ForumSearch"].Value = valueToAdd;
			else
				HttpContext.Current.Response.Cookies["ForumSearch"].Value = HttpContext.Current.Request.Cookies["ForumSearch"].Value.ToString() + "~" + valueToAdd;		
		}

        [AjaxPro.AjaxMethod()]
        public string ThankThePost(string postId)
        {
            string retVal = "";
            string customerId = "-1", handleExists = "-1", isSaved = "-1";
            try
            {
                customerId = CurrentUser.Id.ToString();

                if (customerId != "-1")
                {
                    if (DoesHandleExists(customerId))
                    {
                        handleExists = "1";
                        isSaved = SaveToPostThanks(customerId, postId);
                    }
                }
                retVal = customerId + "|" + handleExists + "|" + isSaved;
                
            }
            catch (Exception ex)
            {
                retVal = "";
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return retVal;
        }

        private bool DoesHandleExists(string cId)
        {
            bool handleExists = false;
            try
            {
           
                using(DbCommand cmd = DbFactory.GetDBCommand("GetExistingHandleDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_userId", DbType.Int64, cId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.CarDataMySqlReadConnection))
                    {
                             if (dr.Read())
                             handleExists = true;                    
                    }
                }
            }
            catch (Exception ex)
            {
                handleExists = false;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return handleExists;
        }

        private string SaveToPostThanks(string _customerId, string _postId)
        {
            string isSaved = "-1";      
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("PostThanksSave_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerID", DbType.Int64, _customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostID", DbType.Int64, _postId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsSaved",DbType.Boolean, ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    if (Convert.ToBoolean(cmd.Parameters["v_IsSaved"].Value))
                        isSaved = "1";
                    else
                        isSaved = "0";
                }             
            }
            catch (Exception ex)
            {
                isSaved = "-1";
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }        
            return isSaved;
        }

	}
}	