using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public static class FormatMinSpecs
    {
        public static string GetMinSpecs(string displacement, string fuelEffecient, string maxpower)
        {
            string str=String.Empty;
            if (displacement != "0")
                str += "<span><span>"+ displacement +"</span><span class='text-light-grey'> CC</span>, </span>";

            if (fuelEffecient != "0")
                str += "<span><span>" + fuelEffecient + "</span><span class='text-light-grey'> Kmpl</span>, </span>";

            if (maxpower != "0")
                str += "<span><span>" + maxpower + "</span><span class='text-light-grey'> bhp @ </span></span>";

            if (str != "")
                return str;
            else
                return "Specs Unavailable";
        }

        public static string GetMinSpecs(string displacement, string fuelEffecient, string maxpower, string maxtorque)
        {
            string str = String.Empty;
            if (displacement != "0")
                str += "<span><span>" + displacement + "</span><span class='text-light-grey'> CC</span>, </span>";

            if (fuelEffecient != "0")
                str += "<span><span>" + fuelEffecient + "</span><span class='text-light-grey'> Kmpl</span>, </span>";

            if (maxpower != "0")
                str += "<span><span>" + maxpower + "</span><span class='text-light-grey'> bhp @ </span></span>";

            if (maxtorque != "0")
                str += "<span><span>" + maxtorque + "</span><span class='text-light-grey'> rpm</span></span>";

            if (str != "")
                return str;
            else
                return "Specs Unavailable";
        }
    }
}
