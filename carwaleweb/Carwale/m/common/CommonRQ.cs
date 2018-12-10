using System;
using System.ComponentModel;
using System.Reflection;

/// <summary>
/// Summary description for CommonRQ
/// </summary>
/// 
namespace MobileWeb.Common
{
    
    public class CommonRQ
    {
        #region enum decs
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        #endregion

       
    }
}