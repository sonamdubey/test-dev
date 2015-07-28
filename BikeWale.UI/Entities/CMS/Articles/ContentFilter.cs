using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entity.CMS.Articles
{
    /// <summary>
    /// Created By : Ashwini Todkar on 17 Sept 2014
    /// </summary>
    [Serializable]
    public class ContentFilter
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }        
    }
}
