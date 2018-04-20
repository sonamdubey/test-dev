using AutoMapper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Utility;
using System;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.BikeData
{
    public class SimilarBikeListMapper
    {
        /// <summary>
        /// Modified By : Rajan Chauhan on 3 Apr 2018
        /// Description : Binding of specs in dto with specsItemList
        /// </summary>
        /// <param name="objSimilarBikes"></param>
        /// <returns></returns>
        internal static IEnumerable<SimilarBike> Convert(IEnumerable<Entities.BikeData.SimilarBikeEntity> objSimilarBikes)
        {
            if (objSimilarBikes != null)
            {
                Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
                Mapper.CreateMap<SimilarBikeEntity, SimilarBike>();
                IEnumerable<SimilarBike> dtoSimilarBikeList = Mapper.Map<IEnumerable<SimilarBikeEntity>, IEnumerable<SimilarBike>>(objSimilarBikes);
                if (dtoSimilarBikeList != null)
                {
                    IEnumerator<SimilarBikeEntity> similarBikeEnumerator = objSimilarBikes.GetEnumerator();
                    foreach (var dtoBike in dtoSimilarBikeList)
                    {
                        if (similarBikeEnumerator.MoveNext())
                        {
                            IEnumerable<SpecsItem> specItemList = similarBikeEnumerator.Current.MinSpecsList;
                            float specValue;
                            foreach (var spec in specItemList)
                            {
                                specValue = float.TryParse(spec.Value, out specValue) ? specValue : 0;
                                switch ((EnumSpecsFeaturesItems)spec.Id)
                                {
                                    case EnumSpecsFeaturesItems.Displacement:
                                        dtoBike.Displacement = specValue;
                                        break;
                                    case EnumSpecsFeaturesItems.FuelEfficiencyOverall:
                                        dtoBike.FuelEfficiencyOverall = specValue.Equals(0) ? null : (float?)specValue;
                                        break;
                                    case EnumSpecsFeaturesItems.MaxPowerBhp:
                                        dtoBike.MaxPower = specValue;
                                        break;
                                    case EnumSpecsFeaturesItems.MaximumTorqueNm:
                                        dtoBike.MaximumTorque = specValue;
                                        break;
                                    case EnumSpecsFeaturesItems.KerbWeight:
                                        dtoBike.KerbWeight = specValue;
                                        break;
                                }
                            }
                        }
                    }
                }
                return dtoSimilarBikeList;
            }
            return null;
        }
    }
}