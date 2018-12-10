using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Author
{
    [Serializable]
    public class AuthorEntity : AuthorList
    {
        public string FullDescription { get; set; }
        public string EmailId { get; set; }
        public string FacebookProfile { get; set; }
        public string GooglePlusProfile { get; set; }
        public string LinkedInProfile { get; set; }
        public string TwitterProfile { get; set; }

    }
}
