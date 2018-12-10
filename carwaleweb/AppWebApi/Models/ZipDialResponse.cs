using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebApi.Models
{
    public class ZipDialResponse
    {
        public string message { get; set; }
        public string display_num { get; set; }
        public string client_transaction_id { get; set; }
        public string transaction_token { get; set; }
        public string status { get; set; }
        public string dial_num { get; set; }
        public string img { get; set; }
    }
}