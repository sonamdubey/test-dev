using Bikewale.DTO.Model.v3;
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
            try
            {
                if (objVersionMinSpec != null)
                {
                    VersionMinSpecs objVersion = new VersionMinSpecs();
                    objVersion.VersionId = objVersionMinSpec.VersionId;
                    objVersion.VersionName = objVersionMinSpec.VersionName;
                    objVersion.ModelName = objVersionMinSpec.ModelName;
                    objVersion.Price = objVersionMinSpec.Price;
                    if (objVersionMinSpec.MinSpecsList != null)
                    {
                        SpecsItem specsItem = objVersionMinSpec.MinSpecsList.Where(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.BrakeType)).FirstOrDefault();
                        if (specsItem != null)
                        {
                            objVersion.BrakeType = specsItem.Value;
                        }
                        objVersion.AlloyWheels = CheckBoolSpecItem(objVersionMinSpec.MinSpecsList.Where(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.AlloyWheels)).FirstOrDefault());
                        objVersion.ElectricStart = CheckBoolSpecItem(objVersionMinSpec.MinSpecsList.Where(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.ElectricStart)).FirstOrDefault());
                        objVersion.AntilockBrakingSystem = CheckBoolSpecItem(objVersionMinSpec.MinSpecsList.Where(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.AntilockBrakingSystem)).FirstOrDefault());
                    }
                    return objVersion;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Service.AutoMappers.BikeData.ConvertToVersionMinSpecs( BikeVersionMinSpecs {0})", objVersionMinSpec));
            }
            return null;
        }

        public static VersionDetail ConvertToVersionDetail(BikeVersionMinSpecs objVersionMinSpec)
        {
            try
            {
                if (objVersionMinSpec != null)
                {
                    VersionDetail objVersionDetail = new VersionDetail();
                    objVersionDetail.VersionId = objVersionMinSpec.VersionId;
                    objVersionDetail.VersionName = objVersionMinSpec.VersionName;
                    objVersionDetail.Price = objVersionMinSpec.Price;
                    if (objVersionMinSpec.MinSpecsList != null)
                    {
                        SpecsItem specsItem = objVersionMinSpec.MinSpecsList.Where(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.BrakeType)).FirstOrDefault();
                        if (specsItem != null)
                        {
                            objVersionDetail.BrakeType = specsItem.Value;
                        }
                        objVersionDetail.AlloyWheels = CheckBoolSpecItem(objVersionMinSpec.MinSpecsList.Where(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.AlloyWheels)).FirstOrDefault());
                        objVersionDetail.ElectricStart = CheckBoolSpecItem(objVersionMinSpec.MinSpecsList.Where(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.ElectricStart)).FirstOrDefault());
                        objVersionDetail.AntilockBrakingSystem = CheckBoolSpecItem(objVersionMinSpec.MinSpecsList.Where(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.AntilockBrakingSystem)).FirstOrDefault());
                    }
                    return objVersionDetail;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Service.AutoMappers.BikeData.ConvertToVersionDetail( BikeVersionMinSpecs {0})", objVersionMinSpec));
            }
            return null;
        }

        private static bool CheckBoolSpecItem(SpecsItem specItem)
        {
            return specItem != null && specItem.Value.Equals("Yes") ? true : false;
        }
    }
}