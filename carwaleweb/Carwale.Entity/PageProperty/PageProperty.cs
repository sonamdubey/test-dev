using Carwale.Entity.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.PageProperty
{
    [Serializable]
    public class PageProperty
    {
        public int Id { get; set; }
        public Templates Template { get; set; }
    }
}
