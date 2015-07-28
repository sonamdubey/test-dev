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
            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();
            CommonOpn op = new CommonOpn();

            string conStr = db.GetConString();

            con = new SqlConnection(conStr);

            try
            {
                cmd = new SqlCommand("FetchCustomerDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt);
                prm.Value = customerId;

                prm = cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@Address", SqlDbType.VarChar, 250);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@StateId", SqlDbType.BigInt);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@State", SqlDbType.VarChar, 50);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@CityId", SqlDbType.BigInt);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@City", SqlDbType.VarChar, 50);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@AreaId", SqlDbType.BigInt);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@Area", SqlDbType.VarChar, 50);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@PinCodeId", SqlDbType.BigInt);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@PinCode", SqlDbType.VarChar, 10);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@Phone", SqlDbType.VarChar, 50);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 50);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@ReceiveNewsletters", SqlDbType.Bit);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@RegistrationDateTime", SqlDbType.DateTime);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@IsVerified", SqlDbType.Bit);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@IsFake", SqlDbType.Bit);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@IsExist", SqlDbType.Bit);
                prm.Direction = ParameterDirection.Output;

                con.Open();
                //run the command
                cmd.ExecuteNonQuery();

                //HttpContext.Current.Trace.Warn("Common.CustomerDetails : Returned Value : " + ret.ToString());

                this.Exists = Convert.ToBoolean(cmd.Parameters["@IsExist"].Value);

                if (this.Exists == true)
                {
                    this.Name = cmd.Parameters["@Name"].Value.ToString();
                    this.Email = cmd.Parameters["@Email"].Value.ToString();
                    this.Address = cmd.Parameters["@Address"].Value.ToString();
                    this.StateId = cmd.Parameters["@StateId"].Value.ToString();
                    this.State = cmd.Parameters["@State"].Value.ToString();
                    this.CityId = cmd.Parameters["@CityId"].Value.ToString();
                    this.City = cmd.Parameters["@City"].Value.ToString();
                    this.AreaId = cmd.Parameters["@AreaId"].Value.ToString();
                    this.Area = cmd.Parameters["@Area"].Value.ToString();
                    this.PinCodeId = cmd.Parameters["@PinCodeId"].Value.ToString();
                    this.PinCode = cmd.Parameters["@PinCode"].Value.ToString();
                    this.Phone1 = cmd.Parameters["@Phone"].Value.ToString();
                    this.Mobile = cmd.Parameters["@Mobile"].Value.ToString();
                    this.Password = cmd.Parameters["@Password"].Value.ToString();
                    this.ReceiveNewsletters = Convert.ToBoolean(cmd.Parameters["@ReceiveNewsletters"].Value);

                    if (cmd.Parameters["@RegistrationDateTime"].Value != DBNull.Value)
                        this.RegistrationDateTime = cmd.Parameters["@RegistrationDateTime"].Value.ToString();

                    this.IsVerified = Convert.ToBoolean(cmd.Parameters["@IsVerified"].Value);

                    this.IsFake = Convert.ToBoolean(cmd.Parameters["@IsFake"].Value);
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
            finally
            {
                //close the connection	
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

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
            SqlDataReader dr = null;
            Database db = new Database();
            string sql = "";
            bool status = true;
            try
            {
                sql = " Select IsFake From Customers With(NoLock) Where Email = @emailId";

                SqlParameter[] param = { new SqlParameter("@emailId", emailId.Trim()) };
                dr = db.SelectQry(sql, param);

                if (dr.Read())
                {
                    status = !(Convert.ToBoolean(dr["IsFake"]));
                }                
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Common.IsValidEmailId : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.IsValidEmailId");
                objErr.SendMail();
            } // catch Exception
            finally
            {
                if(dr != null)
                    dr.Close();

                db.CloseConnection();
            }
            return status;
        }

        //this function returns customerid of the customer based on customer email
        public static string GetCustomerIdFromEmail(string emailId)
        {
            SqlDataReader dr = null;
            Database db = new Database();
            string sql = "";
            string id = "-1";
            try
            {
                sql = " Select Id From Customers With(NoLock) Where Email = @emailId";

                SqlParameter[] param = { new SqlParameter("@emailId", emailId.Trim()) };
                dr = db.SelectQry(sql, param);

                if (dr.Read())
                {
                    id = dr["Id"].ToString();
                }                
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Common.GetCustomerIdFromEmail : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.GetCustomerIdFromEmail");
                objErr.SendMail();
            } // catch Exception
            finally
            {
                if(dr != null)
                    dr.Close();

                db.CloseConnection();
            }
            return id;
        }
    }
}//namespace
