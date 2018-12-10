using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Notifications
{
    public class HashAssistanceType
    {
        private Hashtable GetHashData()
        {
            Hashtable assistancecat = new Hashtable();
            assistancecat.Add(101,"actual price and related");
            assistancecat.Add(100,"actual price and related");
            assistancecat.Add(99,"actual price and related");
            assistancecat.Add(98,"actual price and related");
            assistancecat.Add(1,"actual price and related");
            assistancecat.Add(2,"actual price and related");
            assistancecat.Add(102,"car buying");
            assistancecat.Add(103,"car buying");
            assistancecat.Add(104,"EMI information");
            assistancecat.Add(83,"actual price and related");
            assistancecat.Add(74,"actual price and related");
            assistancecat.Add(50,"offer and discount");
            assistancecat.Add(52,"offer and discount");
            assistancecat.Add(53,"offer and discount");
            assistancecat.Add(54,"offer and discount");
            assistancecat.Add(60,"actual price and related");
            assistancecat.Add(61,"actual price and related");
            assistancecat.Add(62,"offer and discount");
            assistancecat.Add(63,"offer and discount");
            assistancecat.Add(51,"offer and discount");
            assistancecat.Add(55,"offer and discount");
            assistancecat.Add(56,"offer and discount");
            assistancecat.Add(105, "");
            assistancecat.Add(115, "");
            assistancecat.Add(106, "Test Drive");
            assistancecat.Add(108,"Test Drive");
            assistancecat.Add(107, "");
            assistancecat.Add(111, "EMI information");
            assistancecat.Add(112, "EMI information");
            assistancecat.Add(109, "EMI information");
            assistancecat.Add(110, "EMI information");
            assistancecat.Add(113, "actual price and related");
            assistancecat.Add(114, "actual price and related");
            assistancecat.Add(125, "actual price and related");
            assistancecat.Add(128, "actual price and related");
            assistancecat.Add(129, "actual price and related");
            return assistancecat;
        }

        public string GetAssistanceType(int leadClickSource) {
            Hashtable hashtable = GetHashData();
            string value = ""; 
            value = (string)hashtable[leadClickSource];
            value = (value != "" ? value : "");
            return value;
        }
    }
}
