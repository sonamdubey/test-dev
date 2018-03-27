using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Service.AutoMappers.BikeData
{
    public class SpecsFeaturesMapper
    {
        public static VersionMinSpecs ConvertToVersionMinSpecs(BikeVersionMinSpecs objVersionMinSpec)
        {
            VersionMinSpecs objVersion = null;
            try
            {
                if (objVersionMinSpec != null)
                {
                    objVersion = new VersionMinSpecs();
                    objVersion.VersionId = objVersionMinSpec.VersionId;
                    objVersion.VersionName = objVersionMinSpec.VersionName;
                    objVersion.ModelName = objVersionMinSpec.ModelName;
                    objVersion.Price = objVersionMinSpec.Price;
                    SpecsItem specsItem;
                    specsItem = objVersionMinSpec.MinSpecsList.Where(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.BrakeType)).FirstOrDefault();
                    if (specsItem != null)
                    {
                        objVersion.BrakeType = specsItem.Value;
                    }

                    specsItem = objVersionMinSpec.MinSpecsList.Where(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.AlloyWheels)).FirstOrDefault();
                    if (specsItem != null)
                    {
                        objVersion.AlloyWheels = specsItem.Value.Equals("YES");
                    }

                    specsItem = objVersionMinSpec.MinSpecsList.Where(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.ElectricStart)).FirstOrDefault();
                    if (specsItem != null)
                    {
                        objVersion.ElectricStart = specsItem.Value.Equals("YES");
                    }

                    specsItem = objVersionMinSpec.MinSpecsList.Where(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.AntilockBrakingSystem)).FirstOrDefault();
                    if (specsItem != null)
                    {
                        objVersion.AntilockBrakingSystem = specsItem.Value.Equals("YES");
                    }           
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Service.AutoMappers.BikeData.ConvertToVersionMinSpecs( BikeVersionMinSpecs {0})", objVersionMinSpec));
            }
            return objVersion;
        }
    }
}