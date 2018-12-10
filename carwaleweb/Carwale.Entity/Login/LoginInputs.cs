using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    [Serializable]
    public class LoginInputs
    {
        public string AccessToken { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string SocialPlatform { get; set; }
        public string FbId { get; set; }
        public string OAuth { get; set; }
        public int CustId { get; set; }
    }
}
