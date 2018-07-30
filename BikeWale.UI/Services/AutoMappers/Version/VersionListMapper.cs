using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Utility;
using System.Collections.Generic;
using System.Linq;
using System;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.BikeData;

namespace Bikewale.Service.AutoMappers.Version
{
    public class VersionListMapper
    {
        private static IList<string> _categoryNameList = new List<string> { "Summary", "EngTrans", "BrakesAndWheels", "DimChassis", "FuelEffieciency" };
        /// <summary>
        /// Modified by :   Sumit Kate on 12 Apr 2016
        /// Description :   Mapping for MakeBase and ModelBase
        /// Modified by :   Aditi srivastava on 17 Oct 2016
        /// Description :   Mapping for Version Colors
        /// </summary>
        /// <param name="objVersion"></param>
        /// <returns></returns>
        internal static DTO.Version.VersionDetails Convert(Entities.BikeData.BikeVersionEntity objVersion)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeVersionEntity, VersionDetails>();
            return Mapper.Map<BikeVersionEntity, VersionDetails>(objVersion);
        }

        internal static VersionSpecifications Convert(BikeSpecificationEntity objSpecs)
        {
            Mapper.CreateMap<BikeSpecificationEntity, VersionSpecifications>();
            return Mapper.Map<BikeSpecificationEntity, VersionSpecifications>(objSpecs);
        }

        /// <summary>
        /// Modified By : Rajan Chauhan
        /// Description : Used common Convertor to convert BikeVersionMinSpecs to VersionMinSpecs
        /// </summary>
        /// <param name="objMVSpecsList"></param>
        /// <returns></returns>
        internal static IEnumerable<VersionMinSpecs> Convert(IEnumerable<BikeVersionMinSpecs> objMVSpecsList)
        {
            if (objMVSpecsList != null)
            {
                ICollection<VersionMinSpecs> versionMinSpecsList = new List<VersionMinSpecs>();
                foreach (BikeVersionMinSpecs bikeVersion in objMVSpecsList)
                {
                    versionMinSpecsList.Add(SpecsFeaturesMapper.ConvertToVersionMinSpecs(bikeVersion));
                }
                return versionMinSpecsList;
            }
            return null;
        }

        internal static IEnumerable<VersionBase> Convert(List<BikeVersionsListEntity> objVersionList)
        {
            Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
            return Mapper.Map<List<BikeVersionsListEntity>, List<VersionBase>>(objVersionList);
        }

        internal static DTO.Version.v2.VersionDetails ConvertV2(BikeVersionEntity objVersion)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeVersionEntity, DTO.Version.v2.VersionDetails>();
            return Mapper.Map<BikeVersionEntity, DTO.Version.v2.VersionDetails>(objVersion);
        }

        /// <summary>
        /// Created by  : Aditi srivastava on 20 Oct 2016
        /// Description : Mapping for Version Colors
        /// </summary>
        /// <param name="objVersionColors"></param>
        /// <returns></returns>
        internal static IEnumerable<DTO.BikeData.BikeColorsbyVersionsDTO> Convert(IEnumerable<Bikewale.Entities.BikeData.BikeColorsbyVersion> objVersionColors)
        {
            Mapper.CreateMap<Bikewale.Entities.BikeData.BikeColorsbyVersion, Bikewale.DTO.BikeData.BikeColorsbyVersionsDTO>();
            //Mapper.CreateMap<IEnumerable<Bikewale.Entities.BikeData.BikeColorsbyVersion>, BikeColorsbyVersion>();
            return Mapper.Map<IEnumerable<Bikewale.Entities.BikeData.BikeColorsbyVersion>, IEnumerable<BikeColorsbyVersionsDTO>>(objVersionColors);

        }

        /// <summary>
        /// Created by  : Rajan Chauhan on 20 Mar 2018
        /// Description : Mapping SpecsFeaturesItem List to Bikewale.DTO.ModelSpecs
        /// </summary>
        /// <param name="specFeatureItemList"></param>
        /// <returns></returns>
        internal static IEnumerable<Bikewale.DTO.Model.Specs> Convert(IEnumerable<SpecsFeaturesItem> specFeatureItemList)
        {
            
            try
            {
                if (specFeatureItemList != null)
                {
                    IList<Bikewale.DTO.Model.Specs> specsList = new List<Bikewale.DTO.Model.Specs>();
                    foreach (SpecsFeaturesItem specFeatureItem in specFeatureItemList)
                    {
                        string itemValue = specFeatureItem.ItemValues.FirstOrDefault();
                        itemValue = FormatMinSpecs.ShowAvailable(itemValue, specFeatureItem.UnitTypeText, specFeatureItem.DataType, specFeatureItem.Id);
                        specsList.Add(new Bikewale.DTO.Model.Specs()
                        {
                            DisplayText = specFeatureItem.DisplayText,
                            DisplayValue = itemValue
                        });
                    }
                    return specsList;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Service.AutoMappers.Version.VersionListMapper.Convert( IEnumerable<SpecsFeaturesItem> {0})", specFeatureItemList));
            }
            return null;
        }

        /// <summary>
        /// Created by  : Rajan Chauhan on 20 Apr 2018
        /// Description : Mapping SpecsItem List to Bikewale.DTO.ModelSpecs
        /// </summary>
        /// <param name="specFeatureItemList"></param>
        /// <returns></returns>
        internal static IEnumerable<Bikewale.DTO.Model.Specs> Convert(IEnumerable<SpecsItem> specFeatureItemList)
        {

            try
            {
                if (specFeatureItemList != null)
                {
                    IList<Bikewale.DTO.Model.Specs> specsList = new List<Bikewale.DTO.Model.Specs>();
                    foreach (SpecsItem specFeatureItem in specFeatureItemList)
                    {
                        string itemValue = FormatMinSpecs.ShowAvailable(specFeatureItem.Value, specFeatureItem.UnitType, specFeatureItem.DataType, specFeatureItem.Id);
                        specsList.Add(new Bikewale.DTO.Model.Specs()
                        {
                            DisplayText = specFeatureItem.Name,
                            DisplayValue = itemValue
                        });
                    }
                    return specsList;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Service.AutoMappers.Version.VersionListMapper.Convert( IEnumerable<SpecsItem> {0})", specFeatureItemList));
            }
            return null;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 26 Dec 2017
        /// Description : Method to map specs and features of a version.
        /// Modified by : Rajan Chauhan on 20 Mar 2018
        /// Description : Convert SpecsFeaturesEntity to VersionSpecs
        /// </summary>
        /// <param name="modelSpecEntity">Entity with specs and features of a version.</param>
        /// <returns>DTO with specs and features of a version.</returns>
        internal static VersionSpecs Convert(SpecsFeaturesEntity modelSpecEntity, IEnumerable<SpecsItem> summarySpecItemList)
        {
            VersionSpecs verisonSpecs = new VersionSpecs();
            try
            {
                if (modelSpecEntity != null)
                {
                    if (modelSpecEntity.Features != null)
                    {
                        verisonSpecs.objFeatures = new Bikewale.DTO.Model.Features()
                        {
                            DisplayName = "Features",
                            FeaturesList = Convert(modelSpecEntity.Features)
                        };
                    }

                    if (modelSpecEntity.Specs != null)
                    {
                        IList<Bikewale.DTO.Model.SpecsCategory> specCategoryList = new List<Bikewale.DTO.Model.SpecsCategory>();
                        specCategoryList.Add(new Bikewale.DTO.Model.SpecsCategory(){
                            DisplayName = "Summary",
                            CategoryName = "Summary",
                            Specs = Convert(summarySpecItemList)
                        });
                        foreach (SpecsFeaturesCategory specCategory in modelSpecEntity.Specs)
                        {
                            specCategoryList.Add(new Bikewale.DTO.Model.SpecsCategory()
                            {
                                DisplayName = specCategory.DisplayText,
                                CategoryName = GetCategoryName(specCategoryList.Count),
                                Specs = Convert(specCategory.SpecsItemList)
                            });
                        }
                        verisonSpecs.objSpecs = new Bikewale.DTO.Model.Specifications()
                        {
                            DisplayName = "Specifications",
                            SpecsCategory = specCategoryList
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Service.AutoMappers.Version.VersionListMapper.Convert( SpecsFeaturesEntity {0})", modelSpecEntity));
            }
            
            return verisonSpecs;
		}

        /// <summary>
        /// Method for supporting CategoryName field in api/version/{versionId}/specs/
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        private static string GetCategoryName(int currentIndex)
        {
            if (currentIndex >= 0 && currentIndex < _categoryNameList.Count)
                return _categoryNameList[currentIndex];
            else
                return "";
        }
	}
}