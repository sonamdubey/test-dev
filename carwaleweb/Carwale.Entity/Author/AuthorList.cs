using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Author
{
    [Serializable]
    public class AuthorList
    {
        public string AuthorName { get; set; }
        public string ProfileImage { get; set; }
        public string Designation { get; set; }
        public string ShortDescription { get; set; }
        public string MaskingName { get; set; }
        public string HostUrl { get; set; }
        public string ImageName { get; set; }
    }
}
