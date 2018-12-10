using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Geolocation
{
    [Serializable]
    public class States
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
		public string StateMaskingName { get; set; }
	}
}
