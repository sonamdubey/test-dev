﻿using System;
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
                str += "<span><span>" + maxpower + "</span><span class='text-light-grey'> bhp</span></span>";

            if (str != "")
                return str;
            else
                return "Specs Unavailable";
        }

        /// <summary>
        /// Written By : Ashish G. Kamble On 10 Sept 2015
        /// Summary : Function to format the availability
        /// </summary>
        /// <param name="value">Value to be checked whether available or not.</param>
        /// <returns>If value is null function will return --</returns>
        public static string ShowAvailable(string value)
        {
            string showValue = string.Empty;

            if (String.IsNullOrEmpty(value))
            {
                showValue = "--";
            }
            else
            {
                bool isBoolValue = false;
                
                if (Boolean.TryParse(value, out isBoolValue))
                {                    
                    showValue = isBoolValue ? "Yes" : "No";
                }
                else
                { 
                    showValue = value;
                }
            }
            return showValue;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble On 10 Sept 2015
        /// Summary : Function to format the availability
        /// </summary>
        /// <param name="value">Value to be checked whether available or not.</param>
        /// <returns>If value is less than 0 function will return --</returns>
        public static string ShowAvailable(int value)
        {
            string showValue = string.Empty;

            if (value > 0)
            {
                showValue = Format.FormatNumeric(Convert.ToString(value));
            }
            else 
            {
                showValue = "--";
            }

            return showValue;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble On 10 Sept 2015
        /// Summary : Function to format the availability
        /// </summary>
        /// <param name="value">Value to be checked whether available or not.</param>
        /// <returns>If value is less than 0 function will return --</returns>
        public static string ShowAvailable(float value)
        {
            string showValue = string.Empty;

            if (value > 0)
            {
                //string a = String.Format(",", value);
                //showValue = String.Format(",",(Convert.ToString(value)));

                if ((value % 1 == 0))
                {
                    showValue = value.ToString("N0", new System.Globalization.CultureInfo("en-US"));
                }
                else
                {
                    showValue = value.ToString("N", new System.Globalization.CultureInfo("en-US"));
                }                
            }
            else
            {
                showValue = "--";
            }

            return showValue;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble On 10 Sept 2015
        /// Summary : Function to format the availability
        /// </summary>
        /// <param name="value">Value to be checked whether available or not.</param>
        /// <returns>Returns Yes or No depends on the value</returns>
        public static string ShowAvailable(bool value)
        {
            string showValue = string.Empty;

            showValue = value ? "Yes" : "No";

            return showValue;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 12 Sept 2015
        /// Summary : Function to format the version specs
        /// </summary>
        /// <param name="alloyWheel"></param>
        /// <param name="elecStart"></param>
        /// <param name="abs"></param>
        /// <param name="breakType"></param>
        /// <returns></returns>
        public static string GetMinVersionSpecs(bool alloyWheel, bool elecStart, bool abs, string breakType)
        {
            string format = "";
            if (alloyWheel)
            {
                format = String.Concat(format.Trim(), " Alloy Wheels,");
            }
            else
            {
                format = String.Concat(format.Trim(), " Spoke Wheels,");
            }

            if (elecStart)
            {
                format = String.Concat(format.Trim(), " Electric Start,");
            }
            else
            {
                format = String.Concat(format.Trim(), " Kick Start,");
            }

            if (abs)
            {
                format = String.Concat(format.Trim(), " ABS,");
            }

            if (!String.IsNullOrEmpty(breakType))
            {
                format = String.Concat(format.Trim(), breakType, " Brake,");
            }

            if (String.IsNullOrEmpty(format.Trim()))
            {
                return "No specifications.";
            }
            return format.Trim().Substring(0, format.Length - 1);    
        }
    }
}
