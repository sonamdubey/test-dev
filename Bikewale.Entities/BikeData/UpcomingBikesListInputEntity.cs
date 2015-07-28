using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 12 May 2014
    /// Summary : Class have list of input parameters required to get the upcoming bikes list.
    /// Start Index and End Index are required. MakeId and ModelId are optional.
    /// </summary>
    public class UpcomingBikesListInputEntity
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }        
    }
}
