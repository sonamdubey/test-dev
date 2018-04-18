using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Bikewale.BAL.ApiGateway.Entities.BikeData;

namespace Bikewale.BAL.ApiGateway.Utils
{
    public class FormatMinSpecsItem
    {

        /// <summary>
        /// Function to remove 0 value for numeric minSpec
        /// </summary>
        /// <param name="specItemList"></param>
        /// <param name="specId"></param>
        /// <returns></returns>
        public static SpecsItem SanitizeNumericMinSpec(IEnumerable<SpecsItem> specItemList, EnumSpecsFeaturesItems specId)
        {
            SpecsItem minSpec = null;
            if (specItemList != null)
            {
                minSpec = specItemList.FirstOrDefault(spec => spec.Id == (int)specId);
                if (minSpec != null)
                {
                    minSpec.Value = minSpec.Value != "0" ? minSpec.Value : String.Empty;
                }
            }
            else
            {
                minSpec = new SpecsItem();
            }
            return minSpec;
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
                switch ((EnumSpecsFeaturesItems)specItem.Id)
                {
                    case EnumSpecsFeaturesItems.WheelType:
                        return string.Format("{0} Wheels", specItem.Value);
                    case EnumSpecsFeaturesItems.StartType:
                        return specItem.Value;
                    case EnumSpecsFeaturesItems.AntilockBrakingSystem:
                        return specItem.Value.Equals("1") ? "ABS" : String.Empty;
                    case EnumSpecsFeaturesItems.RearBrakeType:
                        return string.Format("{0} Brake", specItem.Value);
                    default:
                        return String.Empty;
                }
            }
            return String.Empty;
        }


        public static string GetCommaSepratedGeneralSpecs(IEnumerable<SpecsItem> specItemList)
        {
            if (specItemList != null)
            {
                string brakeTypeName = GetSpecGeneralName(specItemList.FirstOrDefault(item => item.Id.Equals((int)EnumSpecsFeaturesItems.RearBrakeType)));
                string alloyWheelName = GetSpecGeneralName(specItemList.FirstOrDefault(item => item.Id.Equals((int)EnumSpecsFeaturesItems.WheelType)));
                return String.Format("{0}{1}{2}", brakeTypeName, String.IsNullOrEmpty(brakeTypeName) ? "" : ", ", alloyWheelName);
            }
            return string.Empty;
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
                    string generalSpecName = FormatMinSpecsItem.GetSpecGeneralName(specItem);
                    if (!String.IsNullOrEmpty(generalSpecName))
                    {
                        minSpecsStr.Append(String.Format("<li>{0}</li>", generalSpecName));
                    }
                }
            }
            return minSpecsStr.ToString();
        }

    }
}
