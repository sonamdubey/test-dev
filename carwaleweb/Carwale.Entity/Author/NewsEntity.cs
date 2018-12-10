using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Author
{
    [Serializable]
    public class NewsEntity
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public int BasicId { get; set; }
    }
}
