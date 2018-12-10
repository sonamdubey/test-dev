using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.UsedDealers
{
    public class UsedCarDealerDetails
    {

        private string _dealerId = "0";
        private string _loginId = string.Empty, _firstName = string.Empty, _lastName = string.Empty, 
                _stateId = string.Empty, _cityId = string.Empty, _areaId = string.Empty, _state = string.Empty, _city = string.Empty,
                _area = string.Empty, _pinCode = string.Empty, _address1 = string.Empty, _address2 = string.Empty, _email = string.Empty,
                _mobile = string.Empty, _password = string.Empty, _organization = string.Empty, _contactHours = string.Empty,
                _contactPerson = string.Empty, _contactEmail = string.Empty, _fax = string.Empty, _website = string.Empty, _status = string.Empty, _bpmobileno = string.Empty,
                _bpcontactPerson = string.Empty, _logoUrl = string.Empty, _hostUrl = string.Empty, _financerLogo = string.Empty, _financerHostUrl = string.Empty, _latitude = string.Empty, _longitude = string.Empty;

        private string _joiningDate = string.Empty, _expiryDate = string.Empty, _masknumber = string.Empty;


        public string DealerId
        {
            get { return _dealerId == null ? string.Empty : _dealerId; }
            set { _dealerId = value; }
        }

        public string LoginId
        {
            get { return _loginId == null ? string.Empty : _loginId; }
            set { _loginId = value; }
        }

        public string FirstName
        {
            get { return _firstName == null ? string.Empty : _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName == null ? string.Empty : _lastName; }
            set { _lastName = value; }
        }

        public string Name
        {
            get { return _firstName + " " + _lastName;}
        }

        public string Email
        {
            get { return _email == null ? string.Empty : _email; }
            set { _email = value; }
        }

        public string Address1
        {
            get { return _address1 == null ? string.Empty : _address1; }
            set { _address1 = value; }
        }

        public string Address2
        {
            get { return _address2 == null ? string.Empty : _address2; }
            set { _address2 = value; }
        }

        public string StateId
        {
            get { return _stateId == null ? string.Empty : _stateId; }
            set { _stateId = value; }
        }

        public string CityId
        {
            get { return _cityId == null ? string.Empty : _cityId; }
            set { _cityId = value; }
        }

        public string AreaId
        {
            get { return _areaId == null ? string.Empty : _areaId; }
            set { _areaId = value; }
        }

        public string State
        {
            get { return _state == null ? string.Empty : _state; }
            set { _state = value; }
        }

        public string City
        {
            get { return _city == null ? string.Empty : _city; }
            set { _city = value; }
        }

        public string Area
        {
            get { return _area == null ? string.Empty : _area; }
            set { _area = value; }
        }

        public string PinCode
        {
            get { return _pinCode == null ? string.Empty : _pinCode; }
            set { _pinCode = value; }
        }       

        public string Mobile
        {
            get { return _mobile == null ? string.Empty : _mobile; }
            set { _mobile = value; }
        }

        public string BPMobileNo
        {
            get { return _bpmobileno == null ? string.Empty : _bpmobileno; }
            set { _bpmobileno = value; }
        }

        //-------added from Dealer.DealerDetails

        public string BPContactPerson
        {
            get { return _bpcontactPerson == null ? string.Empty : _bpcontactPerson; }
            set { _bpcontactPerson = value; }
        }
        //-----------------------------------------
        public string Password
        {
            get { return _password == null ? string.Empty : _password; }
            set { _password = value; }
        }

        public string Organization
        {
            get { return _organization == null ? string.Empty : _organization; }
            set { _organization = value; }
        }

        public string ContactHours
        {
            get { return _contactHours == null ? string.Empty : _contactHours; }
            set { _contactHours = value; }
        }

        public string ContactPerson
        {
            get { return _contactPerson == null ? string.Empty : _contactPerson; }
            set { _contactPerson = value; }
        }

        public string ContactEmail
        {
            get { return _contactEmail == null ? string.Empty : _contactEmail; }
            set { _contactEmail = value; }
        }

        public string Fax
        {
            get { return _fax == null ? string.Empty : _fax; }
            set { _fax = value; }
        }

        public string Website
        {
            get { return _website == null ? string.Empty : _website; }
            set { _website = value; }
        }

        public string JoiningDate
        {
            get { return _joiningDate == null ? string.Empty : _joiningDate; }
            set { _joiningDate = value; }
        }

        public string ExpiryDate
        {
            get { return _expiryDate == null ? string.Empty : _expiryDate; }
            set { _expiryDate = value; }
        }

        public string Status
        {
            get { return _status == null ? string.Empty : _status; }
            set { _status = value; }
        }

        //----------added from Dealer.DealerDetails
        public string LogoUrl
        {
            get { return _logoUrl == null ? string.Empty : _logoUrl; }
            set { _logoUrl = value; }
        }

        public string HostUrl
        {
            get { return _hostUrl == null ? string.Empty : _hostUrl; }
            set { _hostUrl = value; }
        }

        public string FinancerLogo
        {
            get { return _financerLogo == null ? string.Empty : _financerLogo; }
            set { _financerLogo = value; }
        }

        public string FinancerHostUrl
        {
            get { return _financerHostUrl == null ? string.Empty : _financerHostUrl; }
            set { _financerHostUrl = value; }
        }

        public string Latitude
        {
            get { return _latitude == null ? string.Empty : _latitude; }
            set { _latitude = value; }
        }

        public string Longitude
        {
            get { return _longitude == null ? string.Empty : _longitude; }
            set { _longitude = value; }
        }

        public string MaskNumber
        {
            get { return _masknumber == null ? string.Empty : _masknumber; }
            set { _masknumber = value; }
        }
    }
}
