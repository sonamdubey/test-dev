using AutoMapper;
using Bikewale.DTO.Compare;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Utility;
using System.Linq;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Bike Compare Entity Mapper
    /// </summary>
    public class BikeCompareEntityMapper
    {
        /// <summary>
        /// Converts Entity to DTO
        /// Modified By : Rajan Chauhan on 04 Apr 2018
        /// Description : Binding of compareSpecifications and compareFeatures with VersionSpecsFeatures
        /// </summary>
        /// <param name="compareEntity"></param>
        /// <returns></returns>
        internal static BikeCompareDTO Convert(BikeCompareEntity compareEntity)
        {
            if (compareEntity != null)
            {
                Mapper.CreateMap<BikeEntityBase, BikeDTOBase>();
                Mapper.CreateMap<BikeColor, BikeColorDTO>();
                Mapper.CreateMap<BikeCompareEntity, BikeCompareDTO>();
                Mapper.CreateMap<BikeEntityBase, BikeDTOBase>();
                Mapper.CreateMap<BikeFeature, BikeFeatureDTO>();
                Mapper.CreateMap<Bikewale.Entities.Compare.BikeModelColor, BikeModelColorDTO>();
                Mapper.CreateMap<BikeSpecification, BikeSpecificationDTO>();
                Mapper.CreateMap<CompareBikeColor, CompareBikeColorDTO>();
                Mapper.CreateMap<CompareBikeColorCategory, CompareBikeColorCategoryDTO>();
                Mapper.CreateMap<CompareBikeData, CompareBikeDataDTO>();
                DTO.Compare.BikeCompareDTO objDto = Mapper.Map<BikeCompareEntity, BikeCompareDTO>(compareEntity);
                if (compareEntity.VersionSpecsFeatures != null)
                {
                    var specList = compareEntity.VersionSpecsFeatures.Specs;
                    if (specList != null && specList.Any())
                    {
                        CompareMainCategoryDTO specsDto = new CompareMainCategoryDTO()
                        {
                            Text = BWConstants.Specifications,
                            Value = BWConstants.Specifications,
                            Spec = new List<CompareSubMainCategoryDTO>()
                        };
                        CompareSubMainCategoryDTO subCategoryDto;
                        // Used for setting Value field in CompareSubMainCategoryDTO required for Icon placement in APP
                        int value = 2; 
                        foreach (var specCategory in specList)
                        {
                            subCategoryDto = new CompareSubMainCategoryDTO()
                            {
                                Text = specCategory.DisplayText,
                                Value = value.ToString(),
                                SpecCategory = Convert(specCategory.SpecsItemList)
                            };
                            specsDto.Spec.Add(subCategoryDto);
                            value++;
                        }
                        objDto.CompareSpecifications = specsDto;
                    }

                    var featuresList = compareEntity.VersionSpecsFeatures.Features;
                    if (featuresList != null && featuresList.Any())
                    {
                        CompareMainCategoryDTO featuresDto = new CompareMainCategoryDTO()
                        {
                            Text = BWConstants.Features,
                            Value = BWConstants.Features,
                            Spec = new List<CompareSubMainCategoryDTO>()
                        };
                        CompareSubMainCategoryDTO subCategoryDto;
                        subCategoryDto = new CompareSubMainCategoryDTO()
                        {
                            Text = BWConstants.Features,
                            Value = BWConstants.Features,
                            SpecCategory = Convert(featuresList)
                        };
                        featuresDto.Spec.Add(subCategoryDto);
                        objDto.CompareFeatures = featuresDto;
                    }

                }
                return objDto;
            }
            return null;
        }

        private static IList<CompareSubCategoryDTO> Convert(IEnumerable<SpecsFeaturesItem> specFeaturesItemList)
        {
            if (specFeaturesItemList != null)
            {
                IList<CompareSubCategoryDTO> subCategoryDto = new List<CompareSubCategoryDTO>();
                IList<CompareBikeDataDTO> bikeDataDtoList;
                string specDisplayText;
                foreach (var specFeatureItem in specFeaturesItemList)
                {
                    bikeDataDtoList = new List<CompareBikeDataDTO>(); 
                    foreach(var itemValue in specFeatureItem.ItemValues){
                        specDisplayText = FormatMinSpecs.ShowAvailable(itemValue, specFeatureItem.DataType,specFeatureItem.Id);
                        bikeDataDtoList.Add(new CompareBikeDataDTO(){
                            Text =  specDisplayText,
                            Value = specDisplayText
                        });
                    }
                    specDisplayText = string.Format("{0}{1}", specFeatureItem.DisplayText, string.IsNullOrEmpty(specFeatureItem.UnitTypeText) ?
                        string.Empty : string.Format(" ({0})", specFeatureItem.UnitTypeText));
                    subCategoryDto.Add(new CompareSubCategoryDTO()
                    {
                        Text = specDisplayText,
                        Value = specDisplayText,
                        CompareSpec = bikeDataDtoList
                    });

                }
                return subCategoryDto;
            }
            return null;
        }
    }
}