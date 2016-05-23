/*THIS CLASS HOLDS ALL TH EFUNCTION FOR BINDING GRID, FILLING DROPDOWN LIST AND OTHER SORTS OF
COMMON OPERATIONS.
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
using System.Security.Principal;
using System.Web.Mail;
using Bikewale.Notifications.CoreDAL;
using System.Data.Common;

namespace Bikewale.Common
{
    // This class retrieves customer info.
    // Customer Id must be provided in the constructor.
    public class CustomerDetails
    {
        private string customerId = "0";
        private string _name = "", _stateId = "", _cityId = "", _areaId = "", _pinCodeId = "",
                _state = "", _city = "", _area = "", _pinCode = "", _address = "", _email = "",
                _phone1 = "", _phone2 = "", _mobile = "", _primaryPhone = "", _password = "",
                _industry = "", _designation = "", _organization = "", _contactHours = "",
                _contactMode = "", _currentVehicle = "", _internetUsePlace = "",
                _carwaleContact = "", _internetUseTime = "", _comment = "";

        private bool _receiveNewsletters = false, _isEmailVerified = false, _isVerified = false, _isFake = false;
        private string _registrationDateTime = "", _dob = "";

        private bool _exists = false;

        public CustomerDetails(string customerId)
        {
            CommonOpn op = new CommonOpn();

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("fetchcustomerdetails"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_address", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 250, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_state", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_city", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_areaid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_area", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pincodeid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pincode", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 10, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_phone", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_password", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_receivenewsletters", DbParamTypeMapper.GetInstance[SqlDbType.Bit], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_registrationdatetime", DbParamTypeMapper.GetInstance[SqlDbType.DateTime], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isverified", DbParamTypeMapper.GetInstance[SqlDbType.Bit], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isfake", DbParamTypeMapper.GetInstance[SqlDbType.Bit], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isexist", DbParamTypeMapper.GetInstance[SqlDbType.Bit], ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd);

                    this.Exists = Convert.ToBoolean(cmd.Parameters["par_isexist"].Value);

                    if (this.Exists == true)
                    {
                        this.Name = cmd.Parameters["par_name"].Value.ToString();
                        this.Email = cmd.Parameters["par_email"].Value.ToString();
                        this.Address = cmd.Parameters["par_address"].Value.ToString();
                        this.StateId = cmd.Parameters["par_stateid"].Value.ToString();
                        this.State = cmd.Parameters["par_state"].Value.ToString();
                        this.CityId = cmd.Parameters["par_cityid"].Value.ToString();
                        this.City = cmd.Parameters["par_city"].Value.ToString();
                        this.AreaId = cmd.Parameters["par_areaid"].Value.ToString();
                        this.Area = cmd.Parameters["par_area"].Value.ToString();
                        this.PinCodeId = cmd.Parameters["par_pincodeid"].Value.ToString();
                        this.PinCode = cmd.Parameters["par_pincode"].Value.ToString();
                        this.Phone1 = cmd.Parameters["par_phone"].Value.ToString();
                        this.Mobile = cmd.Parameters["par_mobile"].Value.ToString();
                        this.Password = cmd.Parameters["par_password"].Value.ToString();
                        this.ReceiveNewsletters = Convert.ToBoolean(cmd.Parameters["par_receivenewsletters"].Value);

                        if (cmd.Parameters["par_registrationdatetime"].Value != DBNull.Value)
                            this.RegistrationDateTime = cmd.Parameters["par_registrationdatetime"].Value.ToString();

                        this.IsVerified = Convert.ToBoolean(cmd.Parameters["par_isverified"].Value);

                        this.IsFake = Convert.ToBoolean(cmd.Parameters["par_isfake"].Value);
                    }
                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn("Common.CustomerDetails : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.CustomerDetails");
                objErr.SendMail();
            } // catch SqlException
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Common.CustomerDetails : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.CustomerDetails");
                objErr.SendMail();
            } // catch Exception

        } // SaveData


        public string CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public string StateId
        {
            get { return _stateId; }
            set { _stateId = value; }
        }

        public string CityId
        {
            get { return _cityId; }
            set { _cityId = value; }
        }

        public string AreaId
        {
            get { return _areaId; }
            set { _areaId = value; }
        }

        public string PinCodeId
        {
            get { return _pinCodeId; }
            set { _pinCodeId = value; }
        }

        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public string Area
        {
            get { return _area; }
            set { _area = value; }
        }

        public string PinCode
        {
            get { return _pinCode; }
            set { _pinCode = value; }
        }

        public string Phone1
        {
            get { return _phone1; }
            set { _phone1 = value; }
        }

        public string Phone2
        {
            get { return _phone2; }
            set { _phone2 = value; }
        }

        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

        public string PrimaryPhone
        {
            get
            {
                string primary = "";

                switch (_primaryPhone)
                {
                    case "1":
                        primary = this.Phone1;
                        break;
                    case "2":
                        primary = this.Phone2;
                        break;
                    case "3":
                        primary = this.Mobile;
                        break;
                    default:
                        primary = this.Phone1;
                        break;
                }
                return primary;
            }
            set { _primaryPhone = value; }
        }

        public string PrimaryPhoneId
        {
            get { return _primaryPhone; }
            set { _primaryPhone = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string Industry
        {
            get { return _industry; }
            set { _industry = value; }
        }

        public string Designation
        {
            get { return _designation; }
            set { _designation = value; }
        }

        public string Organization
        {
            get { return _organization; }
            set { _organization = value; }
        }

        public string ContactHours
        {
            get { return _contactHours; }
            set { _contactHours = value; }
        }

        public string ContactMode
        {
            get { return _contactMode; }
            set { _contactMode = value; }
        }

        public string CurrentVehicle
        {
            get { return _currentVehicle; }
            set { _currentVehicle = value; }
        }

        public string InternetUsePlace
        {
            get { return _internetUsePlace; }
            set { _internetUsePlace = value; }
        }

        public string CarwaleContact
        {
            get { return _carwaleContact; }
            set { _carwaleContact = value; }
        }

        public string InternetUseTime
        {
            get { return _internetUseTime; }
            set { _internetUseTime = value; }
        }

        public bool ReceiveNewsletters
        {
            get { return _receiveNewsletters; }
            set { _receiveNewsletters = value; }
        }

        public bool IsEmailVerified
        {
            get { return _isEmailVerified; }
            set { _isEmailVerified = value; }
        }

        public bool IsVerified
        {
            get { return _isVerified; }
            set { _isVerified = value; }
        }

        public bool IsFake
        {
            get { return _isFake; }
            set { _isFake = value; }
        }

        public string RegistrationDateTime
        {
            get { return _registrationDateTime; }
            set { _registrationDateTime = value; }
        }

        public string DOB
        {
            get { return _dob; }
            set { _dob = value; }
        }

        public bool Exists
        {
            get { return _exists; }
            set { _exists = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        /// <summary>
        ///     Password Salt of the customer
        /// </summary>
        public string Salt { set; get; }

        /// <summary>
        ///     Password hash of the customer
        /// </summary>
        public string Hash { set; get; }

        //this function returns whether the customer has provided its contact information or not
        //for if it has given its any of phone no, or stateid, then return true, else return false
        public bool CustomerContactInformation
        {
            get
            {
                if (this.Phone1 == "" && this.Phone2 == "" && this.Mobile == "")
                    return false;
                else
                    return true;
            }
        }

        public string FirstName
        {
            get
            {
                string[] nameArr = _name.Split(' ');

                string fName = "";

                if (nameArr.Length == 1)
                {
                    fName = _name;
                }
                else
                {
                    //the last array val should be the last name, and combine the remaining 
                    //as the first name
                    for (int i = 0; i < nameArr.Length - 1; i++)
                    {
                        fName += nameArr[i] + " ";
                    }
                    fName = fName.Trim();
                }

                return fName;
            }
        }

        public string LastName
        {
            get
            {
                string[] nameArr = _name.Split(' ');

                string lName = "";

                if (nameArr.Length == 1)
                {
                    lName = "";
                }
                else
                {
                    lName = nameArr[nameArr.Length - 1];
                }

                return lName;
            }
        }

        //this function returns true or false depending on the customer is fake or not
        public static bool IsValidEmailId(string emailId)
        {

            throw new Exception("Method not used/commented");
            
            //SqlDataReader dr = null;
            //Database db = new Database();
            //string sql = "";
            //bool status = true;
            //try
            //{
            //    sql = " Select IsFake From Customers With(NoLock) Where Email = @emailId";

            //    SqlParameter[] param = { new SqlParameter("@emailId", emailId.Trim()) };
            //    dr = db.SelectQry(sql, param);

            //    if (dr.Read())
            //    {
            //        status = !(Convert.ToBoolean(dr["IsFake"]));
            //    }
            //}
            //catch (Exception err)
            //{
            //    HttpContext.Current.Trace.Warn("Common.IsValidEmailId : " + err.Message);
            //    ErrorClass objErr = new ErrorClass(err, "Common.IsValidEmailId");
            //    objErr.SendMail();
            //} // catch Exception
            //finally
            //{
            //    if (dr != null)
            //        dr.Close();

            //    db.CloseConnection();
            //}
            //return status;
        }

        //this function returns customerid of the customer based on customer email
        public static string GetCustomerIdFromEmail(string emailId)
        {

            string sql = "";
            string id = "-1";
            try
            {
                sql = " select id from customers  where email = @emailid";

                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@emailid", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], emailId.Trim()));

                     using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr.Read())
                        {
                            id = dr["Id"].ToString();
                        }
                    } 
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Common.GetCustomerIdFromEmail : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.GetCustomerIdFromEmail");
                objErr.SendMail();
            } // catch Exception

            return id;
        }
    }
}//namespace
