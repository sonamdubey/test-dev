using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.DTO.Widgets;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Make
{
    public class MakePageEntityMapper
    {
        public static MakePage Convert(BikeMakePageEntity entity)
        {
            if (entity != null)
            {
                Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
                Mapper.CreateMap<MostPopularBikesBase, MostPopularBikes>();
                Mapper.CreateMap<BikeDescriptionEntity, BikeDescription>();
                Mapper.CreateMap<BikeMakePageEntity, MakePage>();
                MakePage objDto = Mapper.Map<BikeMakePageEntity, MakePage>(entity);
                if (objDto != null && objDto.PopularBikes != null)
                {
                    IEnumerator<MostPopularBikesBase> popularBikeEnumerator = entity.PopularBikes.GetEnumerator();
                    float specValue;
                    foreach (var dtoBike in objDto.PopularBikes)
                    {
                        if (popularBikeEnumerator.MoveNext())
                        {
                            dtoBike.Specs = new MinSpecs();
                            IEnumerable<SpecsItem> specItemList = popularBikeEnumerator.Current.MinSpecsList;
                            foreach (var spec in specItemList)
                            {
                                specValue = float.TryParse(spec.Value, out specValue) ? specValue : 0;
                                switch ((EnumSpecsFeaturesItem)spec.Id)
                                {
                                    case EnumSpecsFeaturesItem.Displacement:
                                        dtoBike.Specs.Displacement = specValue;
                                        break;
                                    case EnumSpecsFeaturesItem.FuelEfficiencyOverall:
                                        dtoBike.Specs.FuelEfficiencyOverall = specValue;
                                        break;
                                    case EnumSpecsFeaturesItem.MaxPowerBhp:
                                        dtoBike.Specs.MaxPower = specValue;
                                        break;
                                    case EnumSpecsFeaturesItem.MaximumTorqueNm:
                                        dtoBike.Specs.MaximumTorque = specValue;
                                        break;
                                    case EnumSpecsFeaturesItem.KerbWeight:
                                        dtoBike.Specs.KerbWeight = specValue;
                                        break;
                                }
                            }
                        }
                    }
                }
                return objDto;
            }
            return null;
        }
    }
}