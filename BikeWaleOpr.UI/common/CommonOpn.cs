/*THIS CLASS HOLDS ALL TH EFUNCTION FOR BINDING GRID, FILLING DROPDOWN LIST AND OTHER SORTS OF
COMMON OPERATIONS.
*/

using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using BikeWaleOPR.Utilities;
using Microsoft.Practices.Unity;
using MySql.CoreDAL;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.Common
{
	public class CommonOpn
	{

		//used for writing the debug messages
		private HttpContext objTrace = HttpContext.Current;		

		//this function binds the grid with the datareader
		//takes as input the sql string and the datagridname
		public void BindGridReader(string sql, DataGrid dtgrd)
		{
			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand(sql))
				{
					using (IDataReader dataReader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						dtgrd.DataSource = dataReader;
						dtgrd.DataBind();
						dataReader.Close();
					}
				}
			}
			catch (Exception)
			{
				throw;
			}

		}

		//this function binds the grid with the datareader
		//takes as input the sql string and the datagridname
		public void BindGridReader(string sql, DataGrid dtgrd, SqlParameter[] param)
		{

			try
			{

				using (DbCommand cmd = DbFactory.GetDBCommand(sql))
				{
					foreach (DbParameter p in param)
					{
						if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
							p.Value = DBNull.Value;

						cmd.Parameters.Add(p);
					}
					using (IDataReader dataReader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						dtgrd.DataSource = dataReader;
						dtgrd.DataBind();
						dataReader.Close();
					}
				}
			}
			catch (Exception)
			{
				throw;
			}

		}


		//this function binds the repeater with the datareader
		//takes as input the sql string and the datagridname
		public void BindRepeaterReader(string sql, Repeater rpt)
		{
			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand(sql))
				{
					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						rpt.DataSource = dr;
						rpt.DataBind();
					}
				}

			}
			catch (Exception)
			{
				throw;
			}
		}

		//this function binds the repeater with the datareader
		//takes as input the sql string and the datagridname
		public void BindRepeaterReader(string sql, Repeater rpt, SqlParameter[] param)
		{
			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand(sql))
				{
					foreach (DbParameter p in param)
					{
						if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
							p.Value = DBNull.Value;

						cmd.Parameters.Add(p);
					}
					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						rpt.DataSource = dr;
						rpt.DataBind();
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		//this function binds the grid with the dataset
		//takes as input the sql string and the datagridname
		public void BindGridSet(string sql, DataGrid dtgrd)
		{
			try
			{
				using (DataSet dataSet = MySqlDatabase.SelectAdapterQuery(sql, ConnectionType.ReadOnly))
				{

					dtgrd.DataSource = dataSet;
					dtgrd.DataBind();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		//this function binds the grid with the dataset
		//takes as input the sql string and the datagridname
		public void BindGridSet(string sql, DataGrid dtgrd, SqlParameter[] param)
		{
			try
			{
				using (DataSet dataSet = MySqlDatabase.SelectAdapterQuery(sql, param, ConnectionType.ReadOnly))
				{
					dtgrd.DataSource = dataSet;
					dtgrd.DataBind();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// This function does the same thing as the above one,
		// but it has an unque feature of providing paging by default.
		// you have to do nothing but provide pageSize.
		public void BindGridSet(string sql, DataGrid dtgrd, int PageSize)
		{
			try
			{
				using (DataSet dataSet = MySqlDatabase.SelectAdapterQuery(sql, ConnectionType.ReadOnly))
				{
					if (dataSet.Tables[0].Rows.Count > PageSize)
					{
						dtgrd.AllowPaging = true;
						dtgrd.PageSize = PageSize;
					}
					else
					{
						dtgrd.AllowPaging = false;
					}

					dtgrd.DataSource = dataSet;
					dtgrd.DataBind();
				}

			}
			catch (Exception)
			{
				throw;
			}
		}

		// This function does the same thing as the above one,
		// but it has an unque feature of providing paging by default.
		// you have to do nothing but provide pageSize.
		public void BindGridSet(string sql, DataGrid dtgrd, int PageSize, SqlParameter[] param)
		{
			try
			{
				using (DataSet dataSet = MySqlDatabase.SelectAdapterQuery(sql, param, ConnectionType.ReadOnly))
				{
					if (dataSet.Tables[0].Rows.Count > PageSize)
					{
						dtgrd.AllowPaging = true;
						dtgrd.PageSize = PageSize;
					}
					else
					{
						dtgrd.AllowPaging = false;
					}

					dtgrd.DataSource = dataSet;
					dtgrd.DataBind();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		//this function binds the datalist with the datareader
		//takes as input the sql string and the datalist id
		public void BindListReader(string sql, DataList dtlst)
		{

			throw new Exception("Method not used/commented");

			//SqlDataReader dataReader = null;
			//Database objSelect = new Database();
			//try
			//{
			//    dataReader = objSelect.SelectQry(sql);
			//    dtlst.DataSource = dataReader;
			//    dtlst.DataBind();
			//    dataReader.Close();
			//}
			//catch (Exception)
			//{
			//    throw;
			//}
			//finally
			//{
			//    if (dataReader != null)
			//        dataReader.Close();
			//    objSelect.CloseConnection();
			//}
		}

		//this function binds the datalist with the datareader
		//takes as input the sql string and the datalist id
		public void BindListReader(string sql, DataList dtlst, SqlParameter[] param)
		{

			throw new Exception("Method not used/commented");

			//SqlDataReader dataReader = null;
			//Database objSelect = new Database();
			//try
			//{
			//    dataReader = objSelect.SelectQry(sql, param);
			//    dtlst.DataSource = dataReader;
			//    dtlst.DataBind();
			//    dataReader.Close();
			//}
			//catch (Exception)
			//{
			//    throw;
			//}
			//finally
			//{
			//    if (dataReader != null)
			//        dataReader.Close();
			//    objSelect.CloseConnection();
			//}
		}

		//this function binds the dropdownlist with the datareader
		//takes as input the sql string, dropdownlist name, the text field and the value field
		public void FillDropDown(string sql, DropDownList drp, string text, string value)
		{
			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand(sql))
				{
					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{

						if (dr != null)
						{
							drp.DataSource = dr;
							drp.DataTextField = text;
							drp.DataValueField = value;
							drp.DataBind();
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		//this function binds the dropdownlist with the datareader
		//takes as input the sql string, dropdownlist name, the text field and the value field
		public void FillDropDown(string sql, DropDownList drp, string text, string value, DbParameter[] param)
		{
			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand(sql))
				{
					foreach (DbParameter p in param)
					{
						if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
							p.Value = DBNull.Value;

						cmd.Parameters.Add(p);
					}
					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null)
						{
							drp.DataSource = dr;
							drp.DataTextField = text;
							drp.DataValueField = value;
							drp.DataBind();
						}
					}
				}

			}
			catch (Exception)
			{
				throw;
			}
		}

		//this function binds the checkboxlist 
		//takes as input the sql string, dropdownlist name, the text field and the value field
		public void BindCheckBoxList(string sql, CheckBoxList chk, string text, string value)
		{
			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand(sql))
				{
					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null)
						{
							chk.DataSource = dr;
							chk.DataTextField = text;
							chk.DataValueField = value;
							chk.DataBind();
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}

		}

		//this function binds the checkboxlist 
		//takes as input the sql string, dropdownlist name, the text field and the value field
		public void BindCheckBoxList(string sql, CheckBoxList chk, string text, string value, DbParameter[] param)
		{
			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand(sql))
				{
					foreach (DbParameter p in param)
					{
						if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
							p.Value = DBNull.Value;

						cmd.Parameters.Add(p);
					}
					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null)
						{
							chk.DataSource = dr;
							chk.DataTextField = text;
							chk.DataValueField = value;
							chk.DataBind();
						}
					}
				}

			}
			catch (Exception)
			{
				throw;
			}
		}

		//this function binds the RadioButtonList with the datareader
		//takes as input the sql string and the datagridname
		public void BindRadioListReader(string sql, RadioButtonList clst)
		{
			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand(sql))
				{
					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null)
						{
							clst.DataSource = dr;
							clst.DataBind();
						}
					}
				}

			}
			catch (Exception)
			{
				throw;
			}
		}

		//this function binds the RadioButtonList with the datareader
		//takes as input the sql string and the datagridname
		public void BindRadioListReader(string sql, RadioButtonList clst, DbParameter[] param)
		{
			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand(sql))
				{
					foreach (DbParameter p in param)
					{
						if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
							p.Value = DBNull.Value;

						cmd.Parameters.Add(p);
					}
					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null)
						{
							clst.DataSource = dr;
							clst.DataBind();
						}
					}
				}

			}
			catch (Exception)
			{
				throw;
			}
		}

		///<summary>
		/// This Property will be used for Application Path.
		///</summary>
		public static string AppPath
		{
			get
			{
				return (string)ConfigurationManager.AppSettings["AppPath"];
			}
		} // AppPath

		///<summary>
		/// This Property will be used for Ad Path.
		///</summary>
		public static string AdPath
		{
			get
			{
				string adPath = "";

				if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("carwale.com") >= 0)
					adPath = "https://www.carwale.com/";
				else
					adPath = "/";

				return adPath;
			}
		} // AdPath


		///<summary>
		/// This Method will be used for resolving relative paths 
		/// in reference to the Absolute Application Path.
		/// <param name="RelativePath">Relative Path of file.</param>
		///</summary>
		public static string ResolvePath(string RelativePath)
		{
			return (string)ConfigurationManager.AppSettings["AppPath"] + RelativePath;

		} // ResolvePath

		///<summary>
		/// This Method will be used for resolving relative paths 
		/// in reference to the Absolute Application Path.
		/// <param name="RelativePath">Relative Path of file.</param>
		///</summary>
		public static string ResolvePhysicalPath(string RelativePath)
		{
			string fullPath = (string)ConfigurationManager.AppSettings["AppPath"] + RelativePath;
			// the next line is to make path compatible with Plesk 6.5.
			// Plesh uses paths after appending non_ssl.
			Page myPage = new Page();
			string makePleskCompatible = myPage.Server.MapPath(fullPath).Replace("\\default\\htdocs\\", "\\carwale.com\\httpdocs\\");
			return makePleskCompatible;

		} // ResolvePath

		// This Function will save image to the images.carwale.com.
		// If the application is running locally, it will save the 
		// file in the requested directory only.
		public static void SaveImage(HtmlInputFile fil, string RelativePath)
		{
			string fullPath = (string)ConfigurationManager.AppSettings["AppPath"] + RelativePath;
			fullPath = fullPath.Replace("images/", "");
			HttpContext.Current.Trace.Warn("Images Path : " + fullPath);
			// the next line is to make path compatible with Plesk 6.5.
			// Plesh uses paths after appending non_ssl.
			Page myPage = new Page();
			string makePleskCompatible = myPage.Server.MapPath(fullPath).Replace("\\default\\htdocs\\", "\\carwale.com\\subdomains\\images\\httpdocs\\");

			fil.PostedFile.SaveAs(makePleskCompatible);
		} // ResolvePath

		// This Function returns the path of the image for the images.carwale.com.
		// If the application is running locally, it will return the path of the  
		// file in the requested directory only.
		//NOTE:-- if some changes is done in the below function, then corresponding changes
		//is to be done in the function SaveImage, just above this function
		//for the path of the image.
		public static string ImagePathForSavingImages(string RelativePath)
		{
			string fullPath = (string)ConfigurationManager.AppSettings["AppPath"] + RelativePath;

			if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("carwale.com") >= 0)
				fullPath = fullPath.Replace("images/", "");

			HttpContext.Current.Trace.Warn("Images Path : " + fullPath);
			// the next line is to make path compatible with Plesk 6.5.
			// Plesh uses paths after appending non_ssl.
			Page myPage = new Page();
			string makePleskCompatible = myPage.Server.MapPath(fullPath).Replace("\\default\\htdocs\\", "\\carwale.com\\subdomains\\images\\httpdocs\\");
			HttpContext.Current.Trace.Warn("makePleskCompatible : " + makePleskCompatible);

			return makePleskCompatible;
		} // ResolvePath

		// Returns the Images Path.
		public static string ImagePath
		{
			get
			{
				string imgPath = "";

				if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("carwale.com") >= 0)
				{
					imgPath = "https://img.carwale.com/";

					// remove the following line as soon as 
					// images.carwale.com is activated.
					//imgPath = CommonOpn.AppPath + "img/";
				}
				else
				{
					imgPath = CommonOpn.AppPath + "images/";
				}

				//HttpContext.Current.Trace.Warn( "Image Path : " + imgPath );

				return imgPath;
			}
		}

		public static string ResolveImagePath(string imgPath)
		{

			Page myPage = new Page();
			//HttpContext.Current.Trace.Warn( "Original Image Path : " + imgPath);
			//string absPath = myPage.Server.MapPath( "/" );
			string absPath = "";

			if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("carwale.com") >= 0)
			{
				absPath = myPage.Server.MapPath(imgPath.Replace("https://img.carwale.com/", "/")).ToLower().Replace("\\carwale\\", "\\carwaleimg\\");

				// remove the following line as soon as 
				// images.carwale.com is activated.
				//imgPath = CommonOpn.AppPath + "img/";
			}
			else
			{
				absPath = myPage.Server.MapPath(imgPath);
			}

			//return makePleskCompatible; 

			//HttpContext.Current.Trace.Warn( "Index Of : " + imgPath.IndexOf( "images.carwale.com" ) );

			//absPath = absPath.Replace( "\\default\\htdocs\\","\\carwale.com\\subdomains\\images\\httpdocs\\" )
			//+ imgPath.Replace( "https://images.carwale.com", "" ).Replace( "/", "\\" );

			HttpContext.Current.Trace.Warn("Resolved Image Path : " + absPath);
			return absPath;

		}


		///<summary>
		/// This Method is used to verify the id of as passed in the url
		/// This matches the string with the regular expression, and also
		/// check its length not to be greater than 9
		/// <param name="input">The input string to be verified.</param>
		///</summary>
		public static bool CheckId(string input)
		{
			bool retVal = false;
			try
			{
				//check with the regular expression
				if (Regex.IsMatch(input, @"^[0-9]+$") == true)
				{
					//check its length
					if (input.Length <= 9)
					{
						retVal = true;
					}
					else
					{
						retVal = false;
					}
				}
				else
				{
					retVal = false;
				}
			}
			catch (Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				retVal = false;
			}

			return retVal;
		} // CheckId


		public static bool IsNumeric(string input)
		{
			bool retVal = false;
			try
			{
				//check with the regular expression
				if (Regex.IsMatch(input, @"^[0-9]+$") == true)
				{
					//check its length
					if (input.Length <= 9)
					{
						retVal = true;
					}
					else
					{
						retVal = false;
					}
				}
				else
				{
					retVal = false;
				}
			}
			catch (Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				retVal = false;
			}

			return retVal;
		} // IsNumeric

		///<summary>
		/// This Method is used to verify the id of as passed in the url
		/// This matches the string with the regular expression, and also
		/// check its length not to be greater than 15
		/// <param name="input">The input string to be verified.</param>
		///</summary>
		public static bool CheckLongId(string input)
		{
			bool retVal = false;
			try
			{
				//check with the regular expression
				if (Regex.IsMatch(input, @"^[0-9]+$") == true)
				{
					//check its length
					if (input.Length <= 15)
					{
						retVal = true;
					}
					else
					{
						retVal = false;
					}
				}
				else
				{
					retVal = false;
				}
			}
			catch (Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				retVal = false;
			}

			return retVal;
		} // CheckLongId



		/********************************************************************************************
        6()
        THIS FUNCTION sends the mail to the dealers with thte email id as passed in the text format.
        Note that web.config file is case sensitive, 
        The mail id from which the mail is to be sent is get from the key, "localMail".
        ********************************************************************************************/
		public void SendMail(string email, string subject, string body)
		{
			try
			{
				// make sure we use the local SMTP server
				SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);//"127.0.0.1";

				//get the from mail address
				string localMail = ConfigurationManager.AppSettings["localMail"].ToString();

				MailAddress from = new MailAddress(localMail, "CarWale.com");

				// Set destinations for the e-mail message.
				MailAddress to = new MailAddress(email);

				// create mail message object
				MailMessage msg = new MailMessage(from, to);

				// Add Reply-to in the message header.
				msg.Headers.Add("Reply-to", "contact@carwale.com");

				// set some properties
				msg.IsBodyHtml = true;
				msg.Priority = MailPriority.High;

				//prepare the subject
				msg.Subject = subject;

				//body = " Person Accessing the Page : " + CurrentUser.Email + "\n" + body;                            
				msg.Body = body;

				// Mail Server Configuration. Needed for Rediff Hosting.
				//msg.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 1;
				//msg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverpickupdirectory"] = "C:\\inetpub\\mailroot\\pickup";

				// Send the e-mail
				client.Send(msg);

				objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
			}
			catch (Exception err)
			{
				objTrace.Trace.Warn("CommonOpn:SendMail: " + err.Message);
				ErrorClass.LogError(err, "SendMail in CommonOpn");

			}

		}

		/********************************************************************************************
        SendMail()
        THIS FUNCTION sends the mail to the dealers with thte email id as passed, in the html format.
        Note that web.config file is case sensitive, 
        The mail id from which the mail is to be sent is get from the key, "localMail".
        ********************************************************************************************/
		public void SendMail(string email, string subject, string body, bool htmlType)
		{
			try
			{
				// make sure we use the local SMTP server
				SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);//"127.0.0.1";

				//get the from mail address
				string localMail = ConfigurationManager.AppSettings["localMail"].ToString();

				MailAddress from = new MailAddress(localMail, "CarWale.com");

				// Set destinations for the e-mail message.
				MailAddress to = new MailAddress(email);

				// create mail message object
				MailMessage msg = new MailMessage(from, to);

				// Add Reply-to in the message header.
				msg.Headers.Add("Reply-to", "contact@bikewale.com");

				// set some properties
				msg.IsBodyHtml = true;
				msg.Priority = MailPriority.High;

				//prepare the subject
				msg.Subject = subject;

				//body = " Person Accessing the Page : " + CurrentUser.Email + "\n" + body;                            
				msg.Body = body;

				// Mail Server Configuration. Needed for Rediff Hosting.
				//msg.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 1;
				//msg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverpickupdirectory"] = "C:\\inetpub\\mailroot\\pickup";

				// Send the e-mail
				client.Send(msg);

				objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
			}
			catch (Exception err)
			{
				objTrace.Trace.Warn("CommonOpn:SendMail: " + err.Message);
				ErrorClass.LogError(err, "SendMail in CommonOpn");

			}

		}

		/********************************************************************************************
        Does exactly what the conventional SendMail function does except it 
        needs replyTo parameter as well.
        ********************************************************************************************/
		public void SendMail(string email, string subject, string body, bool htmlType, string replyTo)
		{
			try
			{
				// make sure we use the local SMTP server
				SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);//"127.0.0.1";

				//get the from mail address
				string localMail = ConfigurationManager.AppSettings["localMail"].ToString();

				MailAddress from = new MailAddress(localMail, "CarWale.com");

				// Set destinations for the e-mail message.
				MailAddress to = new MailAddress(email);

				// create mail message object
				MailMessage msg = new MailMessage(from, to);

				// Add Reply-to in the message header.
				msg.Headers.Add("Reply-to", "contact@carwale.com");

				// set some properties
				msg.IsBodyHtml = true;
				msg.Priority = MailPriority.High;

				//prepare the subject
				msg.Subject = subject;

				//body = " Person Accessing the Page : " + CurrentUser.Email + "\n" + body;                            
				msg.Body = body;

				// Mail Server Configuration. Needed for Rediff Hosting.
				//msg.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 1;
				//msg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverpickupdirectory"] = "C:\\inetpub\\mailroot\\pickup";

				// Send the e-mail
				client.Send(msg);

				objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
			}
			catch (Exception err)
			{
				objTrace.Trace.Warn("CommonOpn:SendMail: " + err.Message);
				ErrorClass.LogError(err, "SendMail in CommonOpn");

			}

		}


		//Send mail to multiple clients
		public void SendMail(string email, string subject, string body, int mutipleMail)
		{
			try
			{
				if (email != "")
				{
					// make sure we use the local SMTP server
					//SmtpClient client = new SmtpClient("124.153.73.180");//"127.0.0.1";
					SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);

					//get the from mail address
					string localMail = ConfigurationManager.AppSettings["localMail"].ToString();

					MailAddress from = new MailAddress(localMail, "CarWale.com");

					// Set destinations for the e-mail message.
					MailAddress to = new MailAddress(email);

					// create mail message object
					MailMessage msg = new MailMessage(from, to);

					string[] emailList = email.Split(',');
					if (emailList.Length > 0)
					{
						for (int i = 0; i < emailList.Length; i++)
						{
							msg.To.Add(new MailAddress(emailList[i].ToString()));
						}
					}

					// Add Reply-to in the message header.
					msg.Headers.Add("Reply-to", "contact@carwale.com");

					// set some properties
					msg.IsBodyHtml = true;
					msg.Priority = MailPriority.High;

					//prepare the subject
					msg.Subject = subject;

					//body = " Person Accessing the Page : " + CurrentUser.Email + "\n" + body;                            
					msg.Body = body;

					objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);

					// Send the e-mail
					client.Send(msg);

				}
			}
			catch (Exception err)
			{
				ErrorClass.LogError(err, "SendMail in CommonOpn");

			}
		}

		//following is the function which returns a portion of a query which excludes the members whose status is in suspended mode
		public string GetExcludedMembers()
		{
			string sql = "";
			sql = " NOT IN ( SELECT Id FROM Dealers WHERE Status = 1 )";

			return sql;
		}


		/********************************************************************************************
        Does exactly what the conventional SendMail function does except it 
        needs replyTo parameter as well.
        ********************************************************************************************/
		public void SendMail(string email, string subject, string body, bool htmlType, string replyTo, string fromEmail, string addCC, string adBCC)
		{
			try
			{
				// make sure we use the local SMTP server
				SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);//"127.0.0.1";

				//get the from mail address
				string localMail = ConfigurationManager.AppSettings["localMail"].ToString();

				MailAddress from = new MailAddress(fromEmail, "");

				// Set destinations for the e-mail message.
				MailAddress to = new MailAddress(email);

				// create mail message object
				MailMessage msg = new MailMessage(from, to);

				// Add Reply-to in the message header.
				msg.Headers.Add("Reply-to", replyTo);

				// Check if cc is there or not
				if (addCC != "")
				{
					MailAddress cc = new MailAddress(addCC);
					msg.CC.Add(cc);
				}

				// Check if BCC is there or not
				if (adBCC != "")
				{
					MailAddress bcc = new MailAddress(adBCC);//BCC
					msg.Bcc.Add(bcc);
				}

				// set some properties
				msg.IsBodyHtml = true;
				msg.Priority = MailPriority.High;

				//prepare the subject
				msg.Subject = subject;

				//body = " Person Accessing the Page : " + CurrentUser.Email + "\n" + body;                            
				msg.Body = body;

				// Mail Server Configuration. Needed for Rediff Hosting.
				//msg.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 1;
				//msg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverpickupdirectory"] = "C:\\inetpub\\mailroot\\pickup";

				// Send the e-mail
				client.Send(msg);

				objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
			}
			catch (Exception err)
			{
				objTrace.Trace.Warn("CommonOpn:SendMail: " + err.Message);
				ErrorClass.LogError(err, "SendMail in CommonOpn");

			}

		}

		/*****************************************************************************************************
            /// <summary>
            /// This method can be used to Create Script for filling
            /// chained combos. It will return script, which should be
            /// registered on the page(for three combos linked together).
            /// </summary>
        *****************************************************************************************************/
		public string GenerateChainScript(string DropDownList1, string DropDownList2, string DropDownList3, string Query1, string Query2)
		{
			throw new Exception("Method not used/commented");


			//StringBuilder sb = new StringBuilder();

			//sb.Append("<script language=\"javascript\" src=\"../src/chains.js\"></script>");
			//sb.Append("<script language=\"javascript\">");
			//sb.Append("document.getElementById('" + DropDownList1 + "').onchange = " + DropDownList1 + "_OnChange; ");
			//sb.Append("document.getElementById('" + DropDownList2 + "').onchange = " + DropDownList2 + "_OnChange; ");
			//sb.Append("function " + DropDownList1 + "_OnChange( e ) {");
			//sb.Append("var DropDownList1 = document.getElementById('" + DropDownList1 + "');");
			//sb.Append("var arrayValues = new Array(); var i = 0;");
			//SqlDataReader dr;
			//Database db = new Database();
			//try
			//{
			//    dr = db.SelectQry(Query1);

			//    while (dr.Read())
			//    {
			//        sb.Append("arrayValues[i] = [ " + dr[0] + ",\"" + dr[1] + "\"," + dr[2] + " ]; i++;");
			//    }
			//    //sb.Append( "alert('" + DropDownList2 + "');" );
			//    sb.Append("fillChain( '" + DropDownList2 + "', DropDownList1, arrayValues, '" + DropDownList3 + "' ); }");

			//    dr.Close();
			//}
			//catch (SqlException ex)
			//{
			//    HttpContext.Current.Trace.Warn("Inside GenerateChainScript : " + ex.Message);
			//}
			//finally
			//{
			//    db.CloseConnection();
			//}

			//sb.Append("function " + DropDownList2 + "_OnChange( e ) {");
			//sb.Append("var DropDownList = document.getElementById('" + DropDownList2 + "');");
			//sb.Append("var arrayValues = new Array(); var i = 0;");

			//dr = db.SelectQry(Query2);

			//while (dr.Read())
			//{
			//    sb.Append("arrayValues[i] = [ " + dr[0] + ",\"" + dr[1] + "\"," + dr[2] + " ]; i++;");
			//}

			//dr.Close();
			//db.CloseConnection();

			//sb.Append("fillChain( '" + DropDownList3 + "', DropDownList, arrayValues, 'NA' ); } ");

			//sb.Append("</script>");

			//return sb.ToString();

		}
		/*****************************************************************************************************
            /// <summary>
            /// This method can be used to Create Script for filling
            /// chained combos with a text box value on selection of last combo. It will return script, which should be
            /// registered on the page(for three combos linked together).
            /// </summary>
        *****************************************************************************************************/
		public string GenerateChainScript(string DropDownList1, string DropDownList2, string DropDownList3, string TextBox1, string TextBox2, string Query1, string Query2, string Query3)
		{

			throw new Exception("Method not used/commented");
			//StringBuilder sb = new StringBuilder();

			//sb.Append("<script language=\"javascript\" src=\"../src/chains.js\"></script>");
			//sb.Append("<script language=\"javascript\">");
			//sb.Append("document.getElementById('" + DropDownList1 + "').onchange = " + DropDownList1 + "_OnChange; ");
			//sb.Append("document.getElementById('" + DropDownList2 + "').onchange = " + DropDownList2 + "_OnChange; ");
			//sb.Append("document.getElementById('" + DropDownList3 + "').onchange = " + DropDownList3 + "_OnChange; ");
			//sb.Append("function " + DropDownList1 + "_OnChange( e ) {");
			//sb.Append("var DropDownList1 = document.getElementById('" + DropDownList1 + "');");
			//sb.Append("var arrayValues = new Array(); var i = 0;");
			//SqlDataReader dr;
			//Database db = new Database();
			//try
			//{
			//    dr = db.SelectQry(Query1);

			//    while (dr.Read())
			//    {
			//        sb.Append("arrayValues[i] = [ " + dr[0] + ",\"" + dr[1] + "\"," + dr[2] + " ]; i++;");
			//    }

			//    sb.Append("fillChain( '" + DropDownList2 + "', DropDownList1, arrayValues, '" + DropDownList3 + "','" + TextBox1 + "','" + TextBox2 + "'); }");

			//    dr.Close();
			//}
			//catch (SqlException ex)
			//{
			//    HttpContext.Current.Trace.Warn("Inside GenerateChainScript : " + ex.Message);
			//}
			//finally
			//{
			//    db.CloseConnection();
			//}

			//sb.Append("function " + DropDownList2 + "_OnChange( e ) {");
			//sb.Append("var DropDownList = document.getElementById('" + DropDownList2 + "');");
			//sb.Append("var arrayValues = new Array(); var i = 0;");

			//dr = db.SelectQry(Query2);

			//while (dr.Read())
			//{
			//    sb.Append("arrayValues[i] = [ " + dr[0] + ",\"" + dr[1] + "\"," + dr[2] + " ]; i++;");
			//}

			//sb.Append("fillChain( '" + DropDownList3 + "', DropDownList, arrayValues, 'NA','" + TextBox1 + "','" + TextBox2 + "' ); } ");

			//dr.Close();
			//db.CloseConnection();

			//sb.Append("function " + DropDownList3 + "_OnChange( e ) {");
			//sb.Append("var DropDownList = document.getElementById('" + DropDownList3 + "');");
			//sb.Append("var arrayValues = new Array(); var i = 0;");

			//dr = db.SelectQry(Query3);

			//while (dr.Read())
			//{
			//    sb.Append("arrayValues[i] = [ " + dr[0] + ",\"" + dr[1] + "\",\"" + dr[2] + "\" ]; i++;");
			//}
			//sb.Append("fillChainText( '" + TextBox1 + "','" + TextBox2 + "', DropDownList, arrayValues ); } ");
			//dr.Close();
			//db.CloseConnection();


			//sb.Append("</script>");

			//return sb.ToString();

		}
		/*****************************************************************************************************
            /// <summary>
            /// This method can be used to Create Script for filling
            /// chained combos. It will return script, which should be
            /// registered on the page(for two combos linked together).
            /// </summary>
        *****************************************************************************************************/
		public string GenerateChainScript(string DropDownList1, string DropDownList2, string Query1)
		{

			throw new Exception("Method not used/commented");

			//StringBuilder sb = new StringBuilder();

			//sb.Append("<script language=\"javascript\" src=\"../src/chains.js\"></script>");
			//sb.Append("<script language=\"javascript\">");
			//sb.Append("document.getElementById('" + DropDownList1 + "').onchange = " + DropDownList1 + "_OnChange; ");
			//sb.Append("function " + DropDownList1 + "_OnChange( e ) {");
			//sb.Append("var DropDownList1 = document.getElementById('" + DropDownList1 + "');");
			//sb.Append("var arrayValues = new Array(); var i = 0;");
			//SqlDataReader dr;
			//Database db = new Database();
			//try
			//{
			//    dr = db.SelectQry(Query1);

			//    while (dr.Read())
			//    {
			//        sb.Append("arrayValues[i] = [ " + dr[0] + ",\"" + dr[1] + "\"," + dr[2] + " ]; i++;");
			//    }

			//    sb.Append("fillChainTwo( '" + DropDownList2 + "', DropDownList1, arrayValues); }");

			//    dr.Close();
			//}
			//catch (SqlException ex)
			//{
			//    HttpContext.Current.Trace.Warn("Inside GenerateChainScript : " + ex.Message);
			//}
			//finally
			//{
			//    db.CloseConnection();
			//}

			//sb.Append("</script>");

			//return sb.ToString();

		}
		/*****************************************************************************************************
            /// <summary>
            /// This method can be used to Create Script for filling
            /// chained combos. It will return script, which should be
            /// registered on the page(for two combos linked together).
            ///wid the previous functions we fill the other combo wid the values 
            ///tht matches the value in the first combo.and with this function we the 
            ///the other combos wid thevalues that does not match the value in fiorst combo
            /// </summary>
        *****************************************************************************************************/
		public string GenerateReverseChainScript(string DropDownList1, string DropDownList2, string Query1)
		{
			throw new Exception("Method not used/commented");

			//StringBuilder sb = new StringBuilder();

			//sb.Append("<script language=\"javascript\" src=\"../src/chains.js\"></script>");
			//sb.Append("<script language=\"javascript\">");
			//sb.Append("document.getElementById('" + DropDownList1 + "').onchange = " + DropDownList1 + "_OnChange; ");
			//sb.Append("function " + DropDownList1 + "_OnChange( e ) {");
			//sb.Append("var DropDownList1 = document.getElementById('" + DropDownList1 + "');");
			//sb.Append("var arrayValues = new Array(); var i = 0;");
			//SqlDataReader dr;
			//Database db = new Database();
			//try
			//{
			//    dr = db.SelectQry(Query1);

			//    while (dr.Read())
			//    {
			//        sb.Append("arrayValues[i] = [ " + dr[0] + ",\"" + dr[1] + "\"," + dr[2] + " ]; i++;");
			//    }

			//    sb.Append("fillReverseChain( '" + DropDownList2 + "', DropDownList1, arrayValues); }");

			//    dr.Close();
			//}
			//catch (SqlException ex)
			//{
			//    HttpContext.Current.Trace.Warn("Inside GenerateChainScript : " + ex.Message);
			//}
			//finally
			//{
			//    db.CloseConnection();
			//}

			//sb.Append("</script>");

			//return sb.ToString();

		}
		/*****************************************************************************************************
            /// <summary>
            /// This method can be used to Create Script for filling
            /// chained combos. It will return script, which should be
            /// registered on the page(for three combos linked together).
            /// </summary>
        *****************************************************************************************************/
		public string GenerateChainScript(string DropDownList1, string DropDownList2, string DropDownList3, string Query1, string Query2, bool fillTwoCombosWithOneParent)
		{


			throw new Exception("Method not used/commented");
			//StringBuilder sb = new StringBuilder();

			//sb.Append("<script language=\"javascript\" src=\"../src/chains.js\"></script>");
			//sb.Append("<script language=\"javascript\">");
			//sb.Append("document.getElementById('" + DropDownList1 + "').onchange = " + DropDownList1 + "_OnChange; ");
			//sb.Append("function " + DropDownList1 + "_OnChange( e ) {");
			//sb.Append("var DropDownList1 = document.getElementById('" + DropDownList1 + "');");
			//sb.Append("var arrayValues = new Array(); var i = 0;");
			//SqlDataReader dr;
			//Database db = new Database();
			//try
			//{
			//    dr = db.SelectQry(Query1);

			//    while (dr.Read())
			//    {
			//        sb.Append("arrayValues[i] = [ " + dr[0] + ",'" + dr[1] + "'," + dr[2] + " ]; i++;");
			//    }

			//    dr.Close();
			//}
			//catch (SqlException ex)
			//{
			//    HttpContext.Current.Trace.Warn("Inside GenerateChainScript : " + ex.Message);
			//}
			//finally
			//{
			//    db.CloseConnection();
			//}

			//sb.Append("var arrayValues1 = new Array(); i = 0;");

			//try
			//{

			//    dr = db.SelectQry(Query2);

			//    while (dr.Read())
			//    {
			//        sb.Append("arrayValues1[i] = [ " + dr[0] + ",'" + dr[1] + "'," + dr[2] + " ]; i++;");
			//    }

			//    dr.Close();
			//}
			//catch (SqlException ex)
			//{
			//    HttpContext.Current.Trace.Warn("Inside GenerateChainScript : " + ex.Message);
			//}
			//finally
			//{
			//    db.CloseConnection();
			//}

			//sb.Append("fillChainTwoSameParent( '" + DropDownList2 + "','" + DropDownList3 + "', DropDownList1, arrayValues , arrayValues1 ); }");

			//sb.Append("</script>");

			//return sb.ToString();

		}

		public string GenerateChainScript(string DropDownList1, string DropDownList2, string Query1, string selectString)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("<script language=\"javascript\" src=\"/src/chains.js\"></script>");
			sb.Append("<script language=\"javascript\">");

			sb.Append("document.getElementById('" + DropDownList1 + "').onchange = " + DropDownList1 + "_OnChange; var arrayValues = new Array(); ");
			//sb.Append("$(function(){$('#" + DropDownList1 + "').on('change',function(){});});

			//sb.Append("var DropDownList1 = document.getElementById('" + DropDownList1 + "');");
			sb.Append("\n var i = 0;");

			try
			{

				using (DbCommand cmd = DbFactory.GetDBCommand(Query1))
				{
					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null)
						{
							while (dr.Read())
							{
								sb.Append("\narrayValues[i++] = [ " + dr[0] + ",\"" + dr[1] + "\"," + dr[2] + " ];");
							}
						}
					}
				}

				sb.Append("\nfunction " + DropDownList1 + "_OnChange( e ) {");
				sb.Append("\nfillChainTwo( '" + DropDownList2 + "', " + DropDownList1 + ", arrayValues, '" + selectString + "' ); }");
				//sb.Append("document.getElementById('" + DropDownList1 + "').trigger('change');");
				sb.Append("fillChainTwo( '" + DropDownList2 + "', " + DropDownList1 + ", arrayValues, '" + selectString + "' );");

			}
			catch (SqlException ex)
			{
				HttpContext.Current.Trace.Warn("Inside GenerateChainScript : " + ex.Message);
			}

			sb.Append("</script>");

			return sb.ToString();

		}

		/*  Summary     : Get Model From Make
            Author      : Dilip V 13-Jun-2012
            Modifier    : */
		public static DataSet GetModelFromMake(string makeId)
		{
			DataSet ds = new DataSet();
			DbCommand cmd = DbFactory.GetDBCommand();
			MySqlDbUtilities db = new MySqlDbUtilities();

			if (makeId == "")
				return ds;

			string sql = "";

			sql = " select id as Value, name as Text from bikemodels where isdeleted = 0 and  bikemakeid in (" + db.GetInClauseValue(makeId, "MakeId", cmd) + ") order by text ";

			HttpContext.Current.Trace.Warn("sql =" + sql);

			try
			{
				cmd.CommandText = sql;
				ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly);
			}
			catch (Exception err)
			{
				CommonOpn.ExceptionError(err);
			}

			return ds;
		}

		/*  Summary     : Sending SqlException to Trace and Mail
            Author      : Dilip V 25-Jan-2012
            Modifier    :*/
		public static void SqlError(SqlException err)
		{
			HttpContext.Current.Trace.Warn("sql: " + err.Message + err.Source);
			ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

		}

		/*  Summary     : Sending Exception to Trace and Mail
            Author      : Dilip V 25-Jan-2012
            Modifier    :*/
		public static void ExceptionError(Exception err)
		{
			HttpContext.Current.Trace.Warn(err.Message + err.Source);
			ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

		}


		/// <summary>
		/// Written By : Ashish G. Kamble on 9/4/2012
		/// This function will calculate insurance charges for given city and new bike version-id.
		/// Modified By : Ashish G. Kamble on 14 June 2018
		/// Modified : Bike versions data retrived from cache and optimized code
		/// </summary>
		/// <param name="bikeVersionId"></param>
		/// <param name="cityId"></param>
		/// <param name="price"></param>
		/// <returns></returns>
		public static double GetInsurancePremium(string bikeVersionId, string cityId, double price)
		{			
			double premium = 0;
			BikewaleOpr.Entities.BikeData.BikeVersionEntity objVersion = null;

			if (!String.IsNullOrEmpty(bikeVersionId))
			{
				try
				{
					ushort versionId = Convert.ToUInt16(bikeVersionId);

					using (IUnityContainer container = new UnityContainer())
					{
						container.RegisterType<Bikewale.Interfaces.Cache.Core.ICacheManager, Bikewale.Cache.Core.MemcacheManager>();
						container.RegisterType<BikewaleOpr.Interface.BikeData.IBikeVersionsCacheRepository, BikewaleOpr.Cache.BikeData.BikeVersionsCacheRepository>();
						container.RegisterType<BikewaleOpr.Interface.BikeData.IBikeVersions, BikewaleOpr.DALs.Bikedata.BikeVersionsRepository>();

						BikewaleOpr.Interface.BikeData.IBikeVersionsCacheRepository versionsRepo = container.Resolve<BikewaleOpr.Interface.BikeData.IBikeVersionsCacheRepository>();

						objVersion = versionsRepo.GetVersionDetails(versionId);
					}
				}
				catch (Exception ex)
				{
					ErrorClass.LogError(ex, String.Format("BikeWaleOpr.Common.CommonOpn.GetInsurancePremium_version_{0}_city_{1}_price_{2}", bikeVersionId, cityId, price));
				}
			}

			try
			{
				if (objVersion != null)
				{
					if (price > 0 && objVersion.Displacement > 0)
					{
						double rate = 0; // rate to be applied on IDV.
						double depreciation = 5; // depreciation to be applied.
						string depKey = "", rateKey = "";
						double idvBike = 0;
						double liabilities = 0, liaPremium = 0; //liaPADriverOwner = 0, liaPaidDriver = 0;
						string zone = "B";

						ushort[] zoneA = new ushort[] { 1, 2, 10, 12, 105, 128, 176, 198, 1066 }; // zoneA array contains list of metro CityIds					

						depKey = "d_.5"; // even a new bike will have a depreciation of 5%.

						// normalize cc. Three categories. 1000, 1500 and above 1500.
						if (objVersion.Displacement <= 150) objVersion.Displacement = 1; // category 1
						else if (objVersion.Displacement > 150 && objVersion.Displacement <= 350) objVersion.Displacement = 2; // category 2
						else objVersion.Displacement = 3; // category 3

						// check if the city comes in zone A?
						for (int i = 0; i < zoneA.Length; i++)
						{
							if (zoneA[i] == Convert.ToUInt16(cityId)) zone = "A";
						}

						rateKey = zone + ":" + objVersion.Displacement;

						double discount = 0;

						rate = double.Parse(ConfigurationManager.AppSettings[rateKey]);

						rate = rate * (1 - discount / 100.0);   //deduct the discount

						// get depreciation % from web.config.						
						depreciation = 100 - double.Parse(ConfigurationManager.AppSettings[depKey]);
						
						// calculate IDV 					
						idvBike = price * depreciation / 100.0;
						
						// calculate od.
						premium = idvBike * rate / 100.0;
						
						// Third-party Liability.					
						liaPremium = double.Parse(ConfigurationManager.AppSettings["L:" + objVersion.Displacement]);
						
						liabilities = liaPremium;
						
						// Get the Premium.
						premium += liabilities;						

						// Add service tax.						
						premium += premium * double.Parse(ConfigurationManager.AppSettings["ServiceTax"]) / 100.0;						

						// round premium
						premium = Math.Round(premium, 2);
					}
					else if (objVersion.BikeFuelType == 5)  // Insurance Calculations for electric bikes FuelType - 5 (Electric bikes)
					{
						if (objVersion.TopSpeed > 25)
							premium = 1250;
						else if (objVersion.TopSpeed <= 25 && objVersion.TopSpeed > 0)
							premium = 1000;
					}
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, String.Format("BikeWaleOpr.Common.CommonOpn.GetInsurancePremium_version_{0}_city_{1}_price_{2}", bikeVersionId, cityId, price));

			}

			return premium;
		}   // End of GetInsurancePremium method



		/// <summary>
		/// Written By : Ashish G. Kamble on 4/9/2012
		/// This function will calculate registration charges for given city and bike version-id.
		/// Modified by : Ashutosh Sharma on 12-Sep-2017
		/// Description : Added 500 in roadTax for Rajasthan(Green tax)
		/// Modified By : Ashish G. Kamble on 14 June 2018
		/// Modified : Bike versions data retrived from cache and optimized code
		/// </summary>
		/// <param name="bikeVersionId"></param>
		/// <param name="cityId"></param>
		/// <param name="price"></param>
		/// <returns></returns>
		public static double GetRegistrationCharges(string bikeVersionId, string cityId, double price)
		{			
			int stateId = 0;
			double regCharges = 0, roadTax = 0;
			BikewaleOpr.Entities.BikeData.BikeVersionEntity objVersion = null;			

			if (!String.IsNullOrEmpty(bikeVersionId))
			{
				try
				{
					ushort versionId = Convert.ToUInt16(bikeVersionId);

					using (IUnityContainer container = new UnityContainer())
					{
						container.RegisterType<Bikewale.Interfaces.Cache.Core.ICacheManager, Bikewale.Cache.Core.MemcacheManager>();
						container.RegisterType<BikewaleOpr.Interface.BikeData.IBikeVersionsCacheRepository, BikewaleOpr.Cache.BikeData.BikeVersionsCacheRepository>();
						container.RegisterType<BikewaleOpr.Interface.BikeData.IBikeVersions, BikewaleOpr.DALs.Bikedata.BikeVersionsRepository>();

						BikewaleOpr.Interface.BikeData.IBikeVersionsCacheRepository versionsRepo = container.Resolve<BikewaleOpr.Interface.BikeData.IBikeVersionsCacheRepository>();

						objVersion = versionsRepo.GetVersionDetails(versionId);
					}
				}
				catch (Exception ex)
				{
					ErrorClass.LogError(ex, String.Format("BikeWaleOpr.Common.CommonOpn.GetRegistrationCharges_version_{0}_city_{1}_price_{2}", bikeVersionId, cityId, price));
				}
			}


			string sql = String.Format("select StateId from cities where isdeleted = 0 and Id = {0};", cityId);

			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand(sql))
				{
					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null && dr.Read())
						{
							stateId = Convert.ToInt32(dr["StateId"].ToString());
						}
					}
				}
			}
			catch (Exception err)
			{
				ErrorClass.LogError(err, String.Format("BikeWaleOpr.Common.CommonOpn.GetRegistrationCharges_version_{0}_city_{1}_price_{2}", bikeVersionId, cityId, price));
			}

			if (objVersion != null)
			{
				double tmpTax = 0;
				switch (stateId)
				{
					// Maharashtra.
					/*
						Maharashtra	Indian	0-99 cc	8%
						Maharashtra	Indian	100-299 cc	9%
						Maharashtra	Indian	300 + CC	10%
						Maharashtra	Imported	0-99 cc	16%
						Maharashtra	Imported	100-299 cc	18%

					*/
					case 1:
						if (objVersion.Displacement >= 0 && objVersion.Displacement <= 99)
						{
							if (objVersion.IsImported)
							{
								roadTax = price * 0.18;
							}
							else
							{
								roadTax = price * 0.10;
							}
						}
						else if (objVersion.Displacement > 100 && objVersion.Displacement <= 299)
						{
							if (objVersion.IsImported)
							{
								roadTax = price * 0.20;
							}
							else
							{
								roadTax = price * 0.11;
							}
						}
						else
						{
							if (objVersion.IsImported)
							{
								roadTax = price * 0.22;
							}
							else
							{
								roadTax = price * 0.12;
							}
						}


						break;
					// Tamilnadu	All		8%
					case 11:
						roadTax = price * 0.08;						
						break;
					// Andhra Pradesh. 
					// 12% if price upto 1000000
					// 14% for others
					case 6:
						roadTax = price * 0.09;						
						break;

					// calculation for telangana
					case 41:
						roadTax = price * 0.09;
						break;

					// Delhi. 
					/*
						Delhi		0-25000 Rs.	2%
						Delhi		25000-40000	4%
						Delhi		40000 - 60000	6%
						Delhi		60000 +	8%
					*/
					case 5:
						if (price <= 25000)
						{
							roadTax = price * 0.02;
						}
						else if (price > 25000 && price <= 40000)
						{
							roadTax = price * 0.04;
						}
						else if (price > 40000 && price <= 60000)
						{
							roadTax = price * 0.06;
						}
						else
						{
							roadTax = price * 0.08;
						}
						break;
					// GOA. 
					// 5% if price less than or equal to 600000 + 310
					// Above 6L = 6%+310
					case 17:
						if (price <= 200000)
						{
							roadTax = price * 0.08;
							//roadTax += 310;
						}
						else
						{
							roadTax = price * 0.12;
							//roadTax += 310;
						}
						break;

					// ASSAM. 
					//Assam	<65 kg		2600
					//Assam	65-90 kg		3600
					//Assam	90-135 kg		5000
					//Assam	135-165 kg		5500
					//Assam	>165 kg		6500
					case 16:
						if (objVersion.KerbWeight < 65)
						{
							roadTax = 2600;
						}
						else if (objVersion.KerbWeight >= 65 && objVersion.KerbWeight <= 90)
						{
							roadTax = 3600;
						}
						else if (objVersion.KerbWeight > 90 && objVersion.KerbWeight <= 135)
						{
							roadTax = 5000;
						}
						else if (objVersion.KerbWeight > 135 && objVersion.KerbWeight <= 165)
						{
							roadTax = 5500;
						}
						else
						{
							roadTax = 6500;
						}
						break;

					//Uttar Pradesh

					// Uttar Prdesh	All		10%
					case 15:
						roadTax = price * 0.1;
						break;
					// Madhya Pradesh. 7%!
					case 3:
						if (objVersion.BikeFuelType == 5) //FuelType - 5 (Electri Bikes)
							roadTax = price * 0.05;
						else
							roadTax = price * 0.07;
						break;
					// Orissa. 5%!
					case 20:
						roadTax = price * 0.05;
						break;
					// Gujarat. 5.2174%, 10.434% (imported)!
					case 9:						
						roadTax = price * 0.06;
						break;

					//Chhattisgarh 
					//0-500000 - 5% of Ex
					//500000+  - 7% of Ex
					case 8:
						if (price <= 500000)
						{
							roadTax = price * 0.05;
						}
						else
						{
							roadTax = price * 0.07;
						}
						break;
					// Karnataka. 
					// 14.3% if price up to 500000
					// 15.4% if price upto 1000000
					// 18.7% if price upto 2000000
					// 19.8% otherwise.
					case 2:
						if (objVersion.BikeFuelType == 5)
						{
							roadTax = price * 0.04;
						}
						else
						{
							if (price <= 50000)
							{
								roadTax = price * 0.11;
							}
							else
							{
								roadTax = price * 0.13;
							}
						}
						break;
					// Bihar. 
					case 14:
						roadTax = price * 0.07;
						break;
					// Kerala. 6%!
					case 4:
						roadTax = price * 0.06;
						break;
					// Rajasthan. 
					// 5% if price less than 6 lakh
					// 8% if price less than 10 lakh
					// 10% otherwise.
					case 10:

						if (objVersion.Displacement >= 50)
						{
							roadTax = price * 0.04;
						}
						else
						{
							roadTax = price * 0.06;
						}
						roadTax += 500; //Green Tax for Rajasthan
						break;
					// Uttaranchal. 2%!
					case 25:
						if (price <= 1000000)
						{
							roadTax = price * 0.04;
						}
						else
						{
							roadTax = price * 0.05;
						}

						//roadTax = price * 0.02;
						break;
					// Manipur. 
					// Rs.2925 till 1000kg
					// Rs.3600 till 1500kg
					// Rs.4500 till 2000kg
					// Rs. 4500 + 2925 more than 2000kg
					case 36:
						if (objVersion.KerbWeight <= 1000)
						{
							roadTax = 2925;
						}
						else if (objVersion.KerbWeight <= 1500)
						{

							roadTax = 3600;
						}
						else if (objVersion.KerbWeight <= 2000)
						{
							roadTax = 4500;
						}
						else
						{
							roadTax = 4500 + 2925;
						}
						break;
					// Punjab. 2%!
					case 18:
						if (objVersion.Displacement >= 50)
						{
							roadTax = price * 0.015;
						}
						else
						{
							roadTax = price * 0.03;
						}
						break;
					// Chandigarh. 
					//if price < 7 lakh Then 2% + 3000.
					//if price > 7 lakh and price < 20 lakh 3% + 5000
					//if price > 20 lakh Then 4% + 10000.
					case 21:
						if (price <= 100000)
							roadTax = (price * 0.0268);
						else if (price > 100000 && price <= 400000)
							roadTax = (price * 0.0357);
						else
							roadTax = (price * 0.0446);
						break;
					// Haryana. 
					//Haryana	< 75,000		4%
					//Haryana	75,000 - 2 Lakh		6%
					//Haryana	2 + Lakh		8%

					case 22:
						if (price < 75000)
							roadTax = (price * 0.04);
						else if (price >= 75000 && price <= 200000)
							roadTax = (price * 0.06);
						else
							roadTax = (price * 0.08);
						break;

					//west bengal

					//West Bengal		Upto 80 cc	6.5% of vehicle cost or Rs 1800 (whichever is higher)
					//West Bengal		Between 80 and 160 cc	9% of vehicle cost or Rs 3600 (whichever is higher)
					//West Bengal		More than 160 cc	10% of vehicle cost or Rs 5800 (whichever is higher)
					case 12:
						//double tmpTax = 0;

						if (objVersion.Displacement >= 0 && objVersion.Displacement <= 80)
						{
							tmpTax = price * 0.065;

							if (tmpTax <= 1800)
							{
								roadTax = 1800;
							}
							else
							{
								roadTax = tmpTax;
							}
						}
						else if (objVersion.Displacement > 80 && objVersion.Displacement <= 160)
						{
							tmpTax = price * 0.09;

							if (tmpTax <= 3600)
							{
								roadTax = 3600;
							}
							else
							{
								roadTax = tmpTax;
							}
						}
						else
						{
							tmpTax = price * 0.1;

							if (tmpTax <= 5800)
							{
								roadTax = 5800;
							}
							else
							{
								roadTax = tmpTax;
							}
						}
						break;

					// Jharkhand. 
					// 0-5 Seater 3% of Ex-showroom
					// 6-8 seater 4% of Ex-showroom
					//8 + seater 5% of Ex-showroom
					case 23:
						roadTax = price * 0.03;						
						break;

					// Arunachal Pradesh
					// Correct Logic:
					//If weight < 100 Kgs		Rs. 2000 + Rs. 90
					//If weight 100 - 135 Kgs	Rs.3000 + Rs. 90
					//If weight > 135 Kgs		Rs. 3500 + Rs. 90
					case 35:
						if (objVersion.KerbWeight <= 100)
						{
							roadTax = 2090;
						}
						else if (objVersion.KerbWeight > 100 && objVersion.KerbWeight <= 135)
						{
							roadTax = 3090;
						}
						else
						{
							roadTax = 3590;
						}
						break;

					// Daman & Diu
					// If price < 2 lakh	8%
					// If price > 2 lakh	12%
					case 38:
						if (objVersion.IsImported)
						{
							roadTax = price * 0.05;
						}
						else
						{
							roadTax = price * 0.025;
						}
						break;

					//Jammu & Kashmir
					//If Body-Style is Scooter	Rs. 2400
					//If Body-Style is not Scooter	Rs. 4000
					case 24:
						if (objVersion.BodyStyleId == 5)
						{
							roadTax = 2400;
						}
						else
						{
							roadTax = 4000;
						}
						break;

					//Meghalaya
					//If weight < 65 Kgs		Rs. 1050
					//If weight 65 - 90 Kgs		Rs. 1725
					//If weight 90 - 135 Kgs	Rs. 2400
					//If weight > 135 Kgs		Rs. 2850
					case 13:
						if (objVersion.KerbWeight <= 65)
						{
							roadTax = 1050;
						}
						else if (objVersion.KerbWeight > 65 && objVersion.KerbWeight <= 90)
						{
							roadTax = 1725;
						}
						else if (objVersion.KerbWeight > 90 && objVersion.KerbWeight <= 135)
						{
							roadTax = 2400;
						}
						else
						{
							roadTax = 2850;
						}
						break;

					//Tripura
					//If bike is without gear	Rs. 1000
					//If bike is with gear
					//If price < 1 lakh	RS. 2200
					//If price > 1 lakh	RS. 2650
					case 26:
						if (objVersion.BodyStyleId == 5)
						{
							roadTax = 1000;
						}
						else
						{
							if (price <= 100000)
							{
								roadTax = 2200;
							}
							else
							{
								roadTax = 2650;
							}
						}
						break;


					default:
						break;
				}

				// now include approx 1% of price or 4000 flat 
				// as dealer commission, service and handling charges etc.

				if (regCharges != 0)
				{
					regCharges = roadTax + regCharges;
				}
				else
				{
					if (price <= 500000)
					{
						regCharges = roadTax + 1500;
					}
					else if (price > 500000 && price <= 800000)
					{
						regCharges = roadTax + 5000;
					}
					else if (price > 800000 && price <= 1500000)
					{
						regCharges = roadTax + 8000;
					}
					else if (price > 1500000 && price <= 3000000)
					{
						regCharges = roadTax + 12000;
					}
					else if (price > 3000000 && price <= 8000000)
					{
						regCharges = roadTax + 25000;
					}
					else
					{
						regCharges = roadTax + 50000;
					}

					// RTO Calculations for electric bikes whose speed is less than 25
					if (objVersion.BikeFuelType == 5)
					{
						if (objVersion.TopSpeed <= 25 && objVersion.TopSpeed > 0)
							regCharges = 0;
					}
				}
			}

			return regCharges;
		}   // End of GetRegistrationCharges method

		public bool verifyPrivilege(int PageId)
		{
			bool found = false;

			if (HttpContext.Current.Request.Cookies["Customer"] != null && HttpContext.Current.Request.Cookies["Customer"].Value != "")
			{
				//HttpContext.Current.Trace.Warn("Reading Cookie : " + HttpContext.Current.Request.Cookies["Customer"].Value );
				found = true;
				string[] str = HttpContext.Current.Request.Cookies["Customer"].Value.Split(',');
				for (int i = 0; i < str.Length; i++)
				{
					if (str[i] == PageId.ToString())
					{
						found = true;
						break;
					}
					else
						found = false;
				}
			}
			else
				found = false;
			return found;
		}

		/// <summary>
		/// Created By : Sadhana Upadhyay on 13th Feb 2014
		/// Summary : To get Current time stamp
		/// </summary>
		/// <returns></returns>
		public static string GetTimeStamp()
		{
			return DateTime.Now.ToString("yyyy dd MM HH mm ss").Replace(" ", "");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string GetFuelType(string id)
		{
			string fuelVal = "-";
			switch (id)
			{
				case "1":
					fuelVal = "Petrol";
					break;
				case "2":
					fuelVal = "Diesel";
					break;
				case "3":
					fuelVal = "CNG";
					break;
				case "4":
					fuelVal = "LPG";
					break;
				case "5":
					fuelVal = "Electric";
					break;
			}
			return fuelVal;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="numberToFormat"></param>
		/// <returns></returns>
		public static string FormatNumeric(string numberToFormat)
		{
			string formatted = "";
			int breakPoint = 3;

			for (int i = numberToFormat.Length - 1; i >= 0; i--)
			{
				formatted = numberToFormat[i].ToString() + formatted;
				if ((numberToFormat.Length - i) == breakPoint && numberToFormat.Length > breakPoint)
				{
					HttpContext.Current.Trace.Warn(formatted);
					formatted = "," + formatted;
					breakPoint += 2;
				}
			}

			return formatted;
		}

	}//End Class 
}//End 0namespace