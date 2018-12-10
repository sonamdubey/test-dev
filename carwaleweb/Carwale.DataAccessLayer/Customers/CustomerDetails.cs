using Carwale.DAL.CoreDAL.MySql;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DAL.Customers
{
    public class CustomerDetails
    {
        private string customerId = "0";
        private string _name = "", _stateId = "", _cityId = "", _areaId = "", _pinCodeId = "",
                _state = "", _city = "", _area = "", _pinCode = "", _address = "", _email = "",
                _phone1 = "", _phone2 = "", _mobile = "", _primaryPhone = "",
                _industry = "", _designation = "", _organization = "", _contactHours = "",
                _contactMode = "", _currentVehicle = "", _internetUsePlace = "",
                _carwaleContact = "", _internetUseTime = "", _comment = "";

        private bool _receiveNewsletters = false, _isEmailVerified = false, _isVerified = false, _isFake = false;
        private string _registrationDateTime = "", _dob = "";

        private bool _exists = false;

        public CustomerDetails(string customerId)
        {
            long custId = 0;
            long.TryParse(customerId, out custId);
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("FetchCustomerDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_customerid", DbType.Int32, custId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_name", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_email", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_address", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_stateid", DbType.Decimal, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_state", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_cityid", DbType.Decimal, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_city", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_areaid", DbType.Decimal, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_area", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_pincodeid", DbType.Decimal, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_pincode", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_phone1", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_phone2", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_mobile", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_primaryphone", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_industry", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_designation", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_organization", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_dob", DbType.DateTime, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_contacthours", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_contactmode", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_currentvehicle", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_internetuseplace", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_carwalecontact", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_internetusetime", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_receivenewsletters", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_registrationdatetime", DbType.DateTime, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_isemailverified", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_isverified", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_isfake", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_isexist", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_comment", DbType.String, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.CarDataMySqlReadConnection);

                    this.Exists = Convert.ToBoolean(cmd.Parameters["v_isexist"].Value);

                    if (this.Exists == true)
                    {
                        this.Name = cmd.Parameters["v_name"].Value.ToString();
                        this.Email = cmd.Parameters["v_email"].Value.ToString();
                        this.Address = cmd.Parameters["v_address"].Value.ToString();
                        this.StateId = cmd.Parameters["v_stateid"].Value.ToString();
                        this.State = cmd.Parameters["v_state"].Value.ToString();
                        this.CityId = cmd.Parameters["v_cityid"].Value.ToString();
                        this.City = cmd.Parameters["v_city"].Value.ToString();
                        this.AreaId = cmd.Parameters["v_areaid"].Value.ToString();
                        this.Area = cmd.Parameters["v_area"].Value.ToString();
                        this.PinCodeId = cmd.Parameters["v_pincodeid"].Value.ToString();
                        this.PinCode = cmd.Parameters["v_pincode"].Value.ToString();
                        this.Phone1 = cmd.Parameters["v_phone1"].Value.ToString();
                        this.Phone2 = cmd.Parameters["v_phone2"].Value.ToString();
                        this.Mobile = cmd.Parameters["v_mobile"].Value.ToString();
                        this.PrimaryPhone = cmd.Parameters["v_primaryphone"].Value.ToString();
                        this.Industry = cmd.Parameters["v_industry"].Value.ToString();
                        this.Designation = cmd.Parameters["v_designation"].Value.ToString();
                        this.Organization = cmd.Parameters["v_organization"].Value.ToString();

                        if (cmd.Parameters["v_dob"].Value.ToString() != "")
                            this.DOB = cmd.Parameters["v_dob"].Value.ToString();

                        this.ContactHours = cmd.Parameters["v_contacthours"].Value.ToString();
                        this.ContactMode = cmd.Parameters["v_contactmode"].Value.ToString();
                        this.CurrentVehicle = cmd.Parameters["v_currentvehicle"].Value.ToString();
                        this.InternetUsePlace = cmd.Parameters["v_internetuseplace"].Value.ToString();
                        this.CarwaleContact = cmd.Parameters["v_carwalecontact"].Value.ToString();
                        this.InternetUseTime = cmd.Parameters["v_internetusetime"].Value.ToString();
                        this.ReceiveNewsletters = Convert.ToBoolean(cmd.Parameters["v_receivenewsletters"].Value);

                        if (cmd.Parameters["v_registrationdatetime"].Value != DBNull.Value)
                            this.RegistrationDateTime = cmd.Parameters["v_registrationdatetime"].Value.ToString();

                        this.IsEmailVerified = Convert.ToBoolean(cmd.Parameters["v_isemailverified"].Value);
                        this.IsVerified = Convert.ToBoolean(cmd.Parameters["v_isverified"].Value);

                        this.IsFake = Convert.ToBoolean(cmd.Parameters["v_isfake"].Value);

                        this.Comment = cmd.Parameters["v_comment"].Value.ToString();
                    }
                }

            }

            catch (Exception err)
            {
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
    }
}
