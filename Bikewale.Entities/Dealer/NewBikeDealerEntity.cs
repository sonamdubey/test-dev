using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities;
using Bikewale.Entities.Location;

namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created By : Ashwini Todkar on 4 June 2014
    /// </summary>
    public class NewBikeDealerEntity : CityEntityBase
    {
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
        public string PinCode { get; set; }
        private StateEntityBase objState = new StateEntityBase();
        public StateEntityBase State { get { return objState; } set { objState = value; } }
    }
}
