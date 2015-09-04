using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Bikewale.Entities.Location;

namespace Bikewale.Entities.Customer
{
    public class CustomerEntity : CustomerEntityBase
    {
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public bool IsVerified { get; set; }
        public string ClientIP { get; set; }
        public UInt16 SourceId { get; set; }
        public bool IsExist { get; set; }

        private CityEntityBase cityBase = new CityEntityBase();
        public CityEntityBase cityDetails { get { return cityBase; } set { cityBase = value; } }

        private StateEntityBase stateBase = new StateEntityBase();
        public StateEntityBase stateDetails { get { return stateBase; } set { stateBase = value; } }

        private AreaEntityBase areaBase = new AreaEntityBase();
        public AreaEntityBase AreaDetails
        {
            get { return areaBase; }
            set { areaBase = value; }
        }

        public string AuthenticationTicket { get; set; }
    }
}
