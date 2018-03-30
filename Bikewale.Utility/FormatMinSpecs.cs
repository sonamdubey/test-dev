using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace Bikewale.Utility
{
	public static class FormatMinSpecs
	{

        private static readonly string _notAvaliableText = "--";

		public static string GetMinSpecsAsLiElement(string displacement, string fuelEffecient, string maxpower, string weight)
		{
			string str = String.Empty;
			try
			{
				if (!string.IsNullOrEmpty(displacement) && displacement != "0")
					str = string.Format("{0} <li>{1} cc</li>", str, displacement);

				if (!string.IsNullOrEmpty(fuelEffecient) && fuelEffecient != "0")
					str = string.Format("{0} <li>{1} kmpl</li>", str, fuelEffecient);


				if (!string.IsNullOrEmpty(maxpower) && maxpower != "0")
					str = string.Format("{0} <li>{1} bhp</li>", str, maxpower);

				if (!string.IsNullOrEmpty(weight) && weight != "0")
					str = string.Format("{0} <li>{1} kgs</li>", str, weight);

				if (!string.IsNullOrEmpty(str))
				{
					return str.Substring(1);
				}
				else
				{
					return "Specs Unavailable";
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Summary: Overload created to cater weight in Min specs
		/// </summary>
		/// <param name="displacement">CC</param>
		/// <param name="fuelEffecient">kmpl</param>
		/// <param name="maxpower">bhp</param>
		/// <param name="weight">kgs</param>
		/// <returns></returns>
		public static string GetMinSpecs(string displacement, string fuelEffecient, string maxpower, string weight)
		{
			string str = String.Empty;
			try
			{
				if (!string.IsNullOrEmpty(displacement) && displacement != "0")
					str = string.Format("{0}, <span>{1} cc</span>", str, displacement);

				if (!string.IsNullOrEmpty(fuelEffecient) && fuelEffecient != "0")
					str = string.Format("{0}, <span>{1} kmpl</span>", str, fuelEffecient);


				if (!string.IsNullOrEmpty(maxpower) && maxpower != "0")
					str = string.Format("{0}, <span>{1} bhp</span>", str, maxpower);

				if (!string.IsNullOrEmpty(weight) && weight != "0")
					str = string.Format("{0}, <span>{1} kgs</span>", str, weight);

				if (!string.IsNullOrEmpty(str))
				{
					return str.Substring(1);
				}
				else
				{
					return "Specs Unavailable";
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        /// <summary>
        /// Created by : Ashutosh Sharma on 26 Mar 2018
        /// Description : Method to format specs items in span elements.
        /// </summary>
        /// <param name="specsItemList">List of Specs Items.</param>
        /// <returns>String containing comma separated specs items in span elements.</returns>
        public static string GetMinSpecsSpanElements(IEnumerable<SpecsItem> specsItemList)
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                if (specsItemList != null)
                {
                    foreach (var specItem in specsItemList)
                    {
                        if (!string.IsNullOrEmpty(specItem.Value))
                        {
                            builder.AppendFormat("<span>{0} {1}</span>, ", specItem.Value, specItem.UnitType);
                        }
                    }
                    if (builder.Length > 1)
                    {
                        builder.Remove(builder.Length - 2, 2);
                        return builder.ToString();
                    }
                    else
                    {
                        return "Specs Unavailable";
                    }

                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// Written By  : Ashish G. Kamble On 10 Sept 2015
        /// Summary     : Function to format the availability
        /// Modified By : Rajan Chauhan on 27 Mar 2018
        /// Description : Changed logic for specFeature MS
        /// </summary>
        /// <param name="value">Value to be checked whether available or not.</param>
        /// <returns>If value is null function will return --</returns>
        public static string ShowAvailable(string value)
		{
			string showValue = string.Empty;

			if (String.IsNullOrEmpty(value))
			{
				showValue = _notAvaliableText;
			}
			else
			{
				showValue = value;
			}
			return showValue;
		}

        /// <summary>
        /// Created By  : Rajan Chauhan on 29 Mar 2018
        /// Description : Method to show value aware of its datatype
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static string ShowAvailable(string value, EnumSpecDataType dataType)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (dataType.Equals(EnumSpecDataType.Boolean))
                {
                    return value.Equals("1") ? "Yes" : "No";
                }
                else
                {
                    return value;
                }
            }
            return _notAvaliableText;
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
		/// Created by : Aditi Srivastava on 17 May 2017
		/// Summary    : Format bool values based on tru, false or null
		/// </summary>
		public static string ShowAvailable(bool? value)
		{
			string showValue = string.Empty;

			showValue = value.HasValue ? (value.Value.Equals(true) ? "Yes" : "No") : "--";

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


        /// <summary>
        /// Created By  : Rajan Chauhan on 23 Mar 2018
        /// Description : Method for getting general name of specs
        /// </summary>
        /// <param name="specItem"></param>
        /// <returns></returns>
        public static string GetSpecGeneralName(SpecsItem specItem)
        {
            if (specItem != null)
            {
                if (String.IsNullOrEmpty(specItem.Value))
                {
                    return String.Empty;
                }
                switch (specItem.Id)
                {
                    case (int)EnumSpecsFeaturesItem.AlloyWheels:
                        return string.Format("{0} Wheels", specItem.Value.Equals("1" )? "Alloy" : "Spoke");
                    case (int)EnumSpecsFeaturesItem.ElectricStart:
                        return string.Format("{0} Start", specItem.Value.Equals("1") ? "Electric" : "Kick");
                    case (int)EnumSpecsFeaturesItem.AntilockBrakingSystem:
                        return specItem.Value.Equals("1") ? "ABS" : String.Empty;
                    case (int)EnumSpecsFeaturesItem.BrakeType:
                        return string.Format("{0} Brake", specItem.Value);
                    default:
                        return String.Empty;
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// Created By  : Rajan Chauhan on 28 Mar 2018
        /// Description : Method to return Unordered list 
        /// </summary>
        /// <param name="specItemList"></param>
        /// <returns></returns>
        public static string GetMinSpecsAsLiElement(IEnumerable<SpecsItem> specItemList)
        {
            StringBuilder minSpecsStr = new StringBuilder();
            if (specItemList != null)
            {
                foreach (var specItem in specItemList)
                {
                    string generalSpecName = FormatMinSpecs.GetSpecGeneralName(specItem);
                    if (!String.IsNullOrEmpty(generalSpecName))
                    {
                        minSpecsStr.Append(String.Format("<li>{0}</li>", generalSpecName));
                    }
                }
            }
            return minSpecsStr.ToString();
        }

        public static string GetCommaSepratedGeneralSpecs(IEnumerable<SpecsItem> specItemList)
        {
            if (specItemList != null)
            {
                string brakeTypeName = GetSpecGeneralName(specItemList.FirstOrDefault(item => item.Id.Equals((int)EnumSpecsFeaturesItem.BrakeType)));
                string alloyWheelName = GetSpecGeneralName(specItemList.FirstOrDefault(item => item.Id.Equals((int)EnumSpecsFeaturesItem.AlloyWheels)));
                return String.Format("{0}{1}{2}", brakeTypeName, String.IsNullOrEmpty(brakeTypeName) ? "" : ", ", alloyWheelName);
            }
            return string.Empty;
        }
		//Overloading of ShowAvailable

		/// <summary>
		/// Written By : Lucky Rathore On 23 Sept 2015
		/// Summary : Function to format the availability and append respective unit.
        /// Modified By : Rajan Chauhan on 21 Mar 2018
        /// Description : Removed Boolean TryParse for Specs Microservice integration
		/// </summary>
		/// <param name="value">Value to be checked whether available or not.</param>
		/// <param name="unit">unit of respective value e.g. cc, kg.</param>
		/// <returns>If value is null function will return --</returns>
        public static string ShowAvailable(string value, string unit)
        {

            if (!String.IsNullOrEmpty(value))
            {
                return String.Format("{0}{1}{2}", value, String.IsNullOrEmpty(unit) ? String.Empty : " ", unit);
            }
            return _notAvaliableText;
        }

        /// <summary>
        /// Created By  : Rajan Chauhan on 29 Mar 2018
        /// Description : Method to show value unit pair aware of its datatype
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static string ShowAvailable(string value, string unit, EnumSpecDataType dataType)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (dataType.Equals(EnumSpecDataType.Boolean))
                {
                    return value.Equals("1") ? "Yes" : "No";
                }
                else
                {
                    return String.Format("{0} {1}", value, unit);
                }
            }
            return _notAvaliableText;
        }

		/// <summary>
		/// Written By : Lucky Rathore On 23 Sept 2015
		/// Summary : Function to format the availability and append respective unit.
		/// </summary>
		/// <param name="value">Value to be checked whether available or not.</param>
		/// <param name="unit">unit of respective value e.g. cc, kg.</param>
		/// <returns>If value is null function will return --</returns>
		public static string ShowAvailable(int value, string unit)
		{
			string showValue = string.Empty;

			if (value > 0)
			{
				showValue = Format.FormatNumeric(Convert.ToString(value)) + " " + unit;
			}
			else
			{
				showValue = "--";
			}

			return showValue;
		}

		/// <summary>
		/// Written By : Lucky Rathore On 23 Sept 2015
		/// Summary : Function to format the availability and append respective unit.
		/// </summary>
		/// <param name="value">Value to be checked whether available or not.</param>
		/// <param name="unit">unit of respective value e.g. cc, kg.</param>
		/// <returns>If value is null function will return --</returns>
		public static string ShowAvailable(float value, string unit)
		{
			string showValue = string.Empty;

			if (value > 0)
			{
				if ((value % 1 == 0))
				{
					showValue = value.ToString("N0", new System.Globalization.CultureInfo("en-US")) + " " + unit;
				}
				else
				{
					showValue = value.ToString("N", new System.Globalization.CultureInfo("en-US")) + " " + unit;
				}
			}
			else
			{
				showValue = "--";
			}

			return showValue;
		}

		/// <summary>
		/// Written By : Lucky Rathore On 23 Sept 2015
		/// Summary : Function to format the availability and append respective unit.
		/// </summary>
		/// <param name="value1">Value1 to be checked whether available or not.</param>
		/// <param name="unit1">unit1 of respective value e.g. cc, kg.</param>
		///<param name="value2">Value2 to be checked whether available or not.</param>
		/// <param name="unit2">unit2 of respective value e.g. cc, kg.</param>
		/// <returns>If both value1 and value2 is null function will return -- else return respective unit</returns>
		public static string ShowAvailable(int value1, string unit1, float value2, string unit2)
		{
			string showValue = string.Empty;

			string showValue1 = ShowAvailable(value1, unit1);
			string showValue2 = ShowAvailable(value2, unit2);
			if (showValue1.Equals("--") && showValue2.Equals("--"))
			{
				showValue = "--";
			}
			else
			{
				if (showValue1.Equals("--"))
				{
					showValue = showValue2;
				}
				else if (showValue2.Equals("--"))
				{
					showValue = showValue1;
				}
				else
				{
					showValue = showValue1 + " @ " + showValue2;
				}
			}
			return showValue;
		}

		/// <summary>
		/// Written By : Lucky Rathore On 23 Sept 2015
		/// Summary : Function to format the availability and append respective unit.
		/// </summary>
		/// <param name="value1">Value1 to be checked whether available or not.</param>
		/// <param name="unit1">unit1 of respective value e.g. cc, kg.</param>
		///<param name="value2">Value2 to be checked whether available or not.</param>
		/// <param name="unit2">unit2 of respective value e.g. cc, kg.</param>
		/// <returns>If both value1 and value2 is null function will return -- else return respective unit</returns>
		public static string ShowAvailable(float value1, string unit1, float value2, string unit2)
		{
			string showValue = string.Empty;

			string showValue1 = ShowAvailable(value1, unit1);
			string showValue2 = ShowAvailable(value2, unit2);

			if (showValue1.Equals("--") && showValue2.Equals("--"))
			{
				showValue = "--";
			}
			else
			{
				if (showValue1.Equals("--"))
				{
					showValue = showValue2;
				}
				else if (showValue2.Equals("--"))
				{
					showValue = showValue1;
				}
				else
				{
					showValue = showValue1 + " @ " + showValue2;
				}
			}
			return showValue;
		}

		/// <summary>
		/// Created by : Vivek Singh Tomar on 29th Sep 2017
		/// Summary : Get min specs as text without html tags
		/// Modified by : Ashutosh Sharam on 31 Oct 2017
		/// Description : Replaced 'return str.Substring(1)' with 'return str'.
		/// </summary>
		/// <param name="displacement"></param>
		/// <param name="fuelEffecient"></param>
		/// <param name="maxpower"></param>
		/// <param name="weight"></param>
		/// <returns></returns>
		public static string GetMinSpecsAsText(string displacement, string fuelEffecient, string maxpower, string weight)
		{
			string str = String.Empty;
			try
			{
				if (!string.IsNullOrEmpty(displacement) && Convert.ToDecimal(displacement) > 0)
					str = string.Format("{0} cc", displacement);

				if (!string.IsNullOrEmpty(fuelEffecient) && Convert.ToDecimal(fuelEffecient) > 0)
					str = string.Format("{0}, {1} kmpl", str, fuelEffecient);


				if (!string.IsNullOrEmpty(maxpower) && Convert.ToDecimal(maxpower) > 0)
					str = string.Format("{0}, {1} bhp", str, maxpower);

				if (!string.IsNullOrEmpty(weight) && Convert.ToDecimal(weight) > 0)
					str = string.Format("{0}, {1} kgs", str, weight);

				if (!string.IsNullOrEmpty(str))
				{
					return str;
				}
				else
				{
					return "Specs Unavailable";
				}

			}
			catch
			{
				return "Specs Unavailable";
			}
		}

        /// <summary>
        /// Created by : Pratibha Verma on 28 Mar 2018
        /// Description : Method to format specs items.
        /// </summary>
        /// <param name="specsItemList">List of Specs Items.</param>
        /// <returns>String containing comma separated specs items in span elements.</returns>
        public static string GetMinSpecsAsText(IEnumerable<SpecsItem> specsItemList)
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                if (specsItemList != null)
                {
                    foreach (var specItem in specsItemList)
                    {
                        if (!string.IsNullOrEmpty(specItem.Value))
                        {
                            builder.AppendFormat("{0} {1}, ", specItem.Value, specItem.UnitType);
                        }
                    }
                }
                if (builder.Length > 0)
                {
                    builder.Remove(builder.Length - 2, 2);
                    return builder.ToString();
                }
                else
                {
                    return "Specs Unavailable";
                }
            }
            catch(Exception)
            {
                return string.Empty;
            }
        }

    }
}
