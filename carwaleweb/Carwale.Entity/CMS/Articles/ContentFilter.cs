using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.Articles
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 13 Aug 2014
    /// </summary>
    [Serializable]
    public class ContentFilter
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }        
    }
}
