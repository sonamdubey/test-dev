using System.Collections.Generic;

namespace Bikewale.DTO.DealerLocator.v2
{
    public class DealersInIndia
    {
        public string stateName;
        public int countState;
        public IEnumerable<DealersInState> cities { get; set; }
    }
}
