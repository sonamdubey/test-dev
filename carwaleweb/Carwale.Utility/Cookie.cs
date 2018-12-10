using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Utility
{
    public class Cookie
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public DateTime Expires { get; set; }
        public string Domain { get; set; }
        public bool Secure { get; set; }
        public List<KeyValuePair<string, string>> Values { get; set; }

        public Cookie(string name)
        {
            this.Name = name;
            Path = "/";
            Expires = DateTime.MinValue;
            if (ConfigurationManager.AppSettings["Domain"] != "localhost")
                Domain = "." + ConfigurationManager.AppSettings["Domain"];
            Secure = false;
            Values = new List<KeyValuePair<string, string>>();
        }
    }
}
