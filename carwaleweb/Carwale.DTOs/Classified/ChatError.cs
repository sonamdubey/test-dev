using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified
{
    public class ChatError
    {
        public string ErrorMessage { get; set; }
        public string AppId { get; set; }
        public string UserId { get; set; }
        public string AccessToken { get; set; }

    }
}
