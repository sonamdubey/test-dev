/*
	this class do the automatic registration for the new comers, decided on the basis of the email. 
*/

using System;
using System.Data;
using System.Web;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Carwale.BL.Customers;
using Carwale.Entity;
using Carwale.Interfaces;
using Carwale.Interfaces.CustomerVerification;
using Microsoft.Practices.Unity;
using Carwale.Service;
using Carwale.Entity.Enum;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.DAL.Customers;
using Carwale.Interfaces.Blocking;

namespace MobileWeb.Common 
{
	public class AutomateRegistration
	{
		//used for writing the debug messages
		private HttpContext objTrace = HttpContext.Current;
        private static readonly IBlockMobileRepository _blockMobileRepo;

        public AutomateRegistration()
		{
		}

        static AutomateRegistration()
        {
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _blockMobileRepo = container.Resolve<IBlockMobileRepository>();
            }
        }
		
		/*this function process the reqiuest of automatic login.
		first check whether this emailid already exists. if it does not then register the user and also logged him in.
		in case the getCustomerId is set to true and the customer is already registerd then the customerid is to be returned
		but the customer not to be registered, and the details are updated if it is not the same
		*/
		public AutomateRegistrationResult ProcessRequest(string name, string eMail, string landlineNo, string mobileNo, 
														string cityId, string stateId, string cityName)
		{
			AutomateRegistrationResult arr = new AutomateRegistrationResult();
			string reEmail = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$";
			
			if ( Regex.IsMatch( eMail.ToLower(), reEmail ) )
			{
				if(name == "")
				{
					//get the name from the email
					name = eMail.Split('@')[0];
				}
				
				//also verify the mobile number and the landline number
				if(landlineNo != "" && landlineNo != "-")
				{
					landlineNo = CommonOpn.ParseLandlineNumber(landlineNo);
					
					if(landlineNo == "")
						arr.FakeLandlineNo = true;
				}
				
				if(mobileNo != "")
				{
					mobileNo = CommonOpn.ParseMobileNumber(mobileNo);
					
					if(mobileNo == "") arr.FakeMobileNo = true;
					else if( IsMobileBlocked(mobileNo) ) arr.FakeMobileNo = true; // check for customer mobile set the property to true if its blocked
				}
				
				bool existingCustomer = false, fakeCustomer = false, isApproved = false;
				
				// register
				if( arr.FakeMobileNo == false )
				{
					arr.CustomerId = RegisterUser(name, eMail, landlineNo, mobileNo, cityId, stateId, cityName, out existingCustomer, out fakeCustomer, out isApproved);
				}
				
				arr.ExistingCustomer = existingCustomer;
				arr.FakeCustomer = fakeCustomer;
				arr.IsApproved = isApproved;
			}
			else
			{
				arr.CustomerId = "-1";
				arr.FakeCustomer = true;
				arr.FakeEmail = true;
			}
			
			return arr;
		}
		
		string RegisterUser(string name, string eMail, string landlineNo, string mobileNo, string cityId, string stateId, 
								string cityName, out bool existingCustomer, out bool fakeCustomer, out bool isApproved)
		{
			string customerId = "-1";
			existingCustomer = false;
			fakeCustomer = false;
			isApproved = false;
			
            CustomerOnRegister customer = new CustomerOnRegister();
            string val = string.Empty;
            try
            {
                ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
                customer = customerRepo.CreateCustomer(new Customer()
                {
                    Name = name,
                    Email = eMail,
                    Mobile = mobileNo,
                    CityId = !string.IsNullOrEmpty(cityId) ? Convert.ToInt32(cityId) : -1,
                    City = cityName,
                    StateId = !string.IsNullOrEmpty(stateId) ? Convert.ToInt32(stateId) : -1,
                });
                customerId = customer.CustomerId;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "AutomateRegistration.RegisterUser");
                objErr.SendMail();
            } // catch Exception
			finally
			{
				if ( customer.StatusOnRegister == "N" ) 
				{
					// send acknowledgement email to customer.
                    CommonOpn co = new CommonOpn();
                    
                    //For optional email functionality of contact seller.
                    if (eMail.Contains("@unknown.com"))
                    {
                        eMail = string.Empty;
                    }
					CurrentUser.StartSession( name, customerId, eMail, false );
					
					//SaveData To SMS password.
					SaveDataForSMS( customerId );
					
					//also update the SourceId
					UpdateSourceId(customerId);
				}
                else if (customer.StatusOnRegister == "O")	// Already registered.
				{
					//if the email is already registered and somebody is loggdi, check whether it is the same customer
					//if not ehn log of the existing session
					if(CurrentUser.Id != "-1" && CurrentUser.Id != customerId)
						CurrentUser.EndSession();
					
					//the person is already registered. update the details now.
					fakeCustomer = UpdateCustomerDetails(customerId, name, eMail, landlineNo, mobileNo, cityId, stateId);
					existingCustomer = true;
				}
			}
			
			return customerId;
		}
		
		private void UpdateSourceId(string id)
		{
            ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
            customerRepo.UpdateSourceId(EnumTableType.AppCustomers, id);
		}
		
		void SaveDataForSMS( string tempCustomerId )
		{
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("SaveDataForSms_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, tempCustomerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_EntryDateTime", DbType.DateTime, DateTime.Now));
                    MySqlDatabase.InsertQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "AutomateRegistration.RegisterUser");
                objErr.SendMail();
            }
		}	
		bool UpdateCustomerDetails(string customerId, string name, string eMail, string landlineNo, string mobileNo, string cityId, string stateId)
		{
			bool fake = false;
			
			CommonOpn op = new CommonOpn();
						
			string comment = "";
			//check for the customer details
            CustomerDetails cd = new CustomerDetails(customerId);
            ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
			fake = cd.IsFake;
			
			if(fake == false)
			{
				try
				{
					
					comment = cd.Comment;
					
					//for the phone numbers 
					if(landlineNo == "")
						landlineNo = cd.Phone1;
					else
					{
						//check if the new number equals to the existing number. if it does not then add the new number to the 
						//existing one and add the existing one to the comment
						string old = CommonOpn.ParseLandlineNumber(cd.Phone1);
						string newO = CommonOpn.ParseLandlineNumber(landlineNo);
						if(newO != "" && newO != old)
						{
							landlineNo = newO;
							comment += " LandlineNo" + old; 
						}
						else
							landlineNo = old;
					}
					
					if(mobileNo == "")
						mobileNo = cd.Mobile;
					else
					{
						//check if the new number equals to the existing number. if it does not then add the new number to the 
						//existing one and add the existing one to the comment
						string old = CommonOpn.ParseMobileNumber(cd.Mobile);
						string newO = CommonOpn.ParseMobileNumber(mobileNo);
						if(newO != "" && newO != old)
						{
							mobileNo = newO;
							comment += " MobileNo" + old; 
						}
						else
							mobileNo = old;
					}
				
					if(stateId == "")
					{
						stateId = cd.StateId;
					}
					
					stateId = stateId == "" ? "-1" : stateId;
					
					if(cityId == "")
					{
						cityId = cd.CityId;
					}
					
					cityId = cityId == "" ? "-1" : cityId;
						
					string areaId = cd.AreaId;
					
					areaId = areaId == "" ? "-1" : areaId;

                    customerRepo.UpdateCustomerDetails(new Customer()
                    {
                        CustomerId = customerId,
                        Name = name,
                        Email = eMail,
                        Address = cd.Address,
                        StateId = Convert.ToInt32(stateId),
                        CityId = Convert.ToInt32(cityId),
                        Phone = landlineNo,
                        Mobile = mobileNo,
                        ReceiveNewsletters = cd.ReceiveNewsletters,
                    });
				}
				catch(Exception err)
				{
					ErrorClass objErr = new ErrorClass(err,"AutomateRegistration.UpdateCustomerDetails");
					objErr.SendMail();
				}
			}
			
			return fake;	
		}

		bool IsMobileBlocked( string number )
		{
            return _blockMobileRepo.IsNumberBlocked(number);
		}
	}
	
	public class AutomateRegistrationResult
	{
		private string _customerId = "-1";
		private bool _existingCustomer = false;
		private bool _fakeCustomer = false;
		private bool _fakeEmail = false;
		private bool _fakeLandlineNo = false;
		private bool _fakeMobileNo = false;
		private bool _isApproved = false;
		
		public AutomateRegistrationResult()
		{
		}
		
		public string CustomerId
		{
			get{return _customerId;}
			set{_customerId = value;}
		}
		
		public bool ExistingCustomer
		{
			get{return _existingCustomer;}
			set{_existingCustomer = value;}
		}
		
		public bool FakeCustomer
		{
			get{return _fakeCustomer;}
			set{_fakeCustomer = value;}
		}
		
		public bool FakeEmail
		{
			get{return _fakeEmail;}
			set{_fakeEmail = value;}
		}
		
		public bool FakeLandlineNo
		{
			get{return _fakeLandlineNo;}
			set{_fakeLandlineNo = value;}
		}
		
		public bool FakeMobileNo
		{
			get{return _fakeMobileNo;}
			set{_fakeMobileNo = value;}
		}
		
		public bool IsApproved
		{
			get{return _isApproved;}
			set{_isApproved = value;}
		}
	}
	
}//namespace
