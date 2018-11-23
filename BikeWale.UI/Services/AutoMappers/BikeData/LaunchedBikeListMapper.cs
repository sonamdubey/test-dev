using AutoMapper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.City;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Series;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.BikeData
{
    /// <summary>
    /// Modified by :   Sumit Kate on 13 Feb 2017
    /// Description :   Added new Entity to DTO convert methods
    /// Modified by :   Rajan Chauhan on 4 Apr 2018
    /// Description :   Binding of MinSpecList to DtoMinSpec
    /// </summary>
    public class LaunchedBikeListMapper
    {
        internal static IEnumerable<DTO.BikeData.LaunchedBike> Convert(IEnumerable<NewLaunchedBikeEntity> objRecent)
        {
            if (objRecent != null)
            {
            
                IEnumerable<DTO.BikeData.LaunchedBike> objDto = Mapper.Map<IEnumerable<NewLaunchedBikeEntity>, IEnumerable<LaunchedBike>>(objRecent);
                if (objDto != null)
                {
                    IEnumerator<NewLaunchedBikeEntity> similarBikeEnumerator = objRecent.GetEnumerator();
                    float specValue;
                    foreach (var dtoBike in objDto)
                    {
                        if (similarBikeEnumerator.MoveNext())
                        {
                            IEnumerable<SpecsItem> specItemList = similarBikeEnumerator.Current.MinSpecsList;
                            dtoBike.Specs = new MinSpecs();
                            foreach (var spec in specItemList)
                            {
                                specValue = float.TryParse(spec.Value, out specValue) ? specValue : 0;
                                switch ((EnumSpecsFeaturesItems)spec.Id)
                                {
                                    case EnumSpecsFeaturesItems.Displacement:
                                        dtoBike.Specs.Displacement = specValue;
                                        break;
                                    case EnumSpecsFeaturesItems.FuelEfficiencyOverall:
                                        dtoBike.Specs.FuelEfficiencyOverall = specValue.Equals(0) ? null : (float?)specValue;
                                        break;
                                    case EnumSpecsFeaturesItems.MaxPowerBhp:
                                        dtoBike.Specs.MaxPower = specValue;
                                        break;
                                    case EnumSpecsFeaturesItems.MaximumTorqueNm:
                                        dtoBike.Specs.MaximumTorque = specValue;
                                        break;
                                    case EnumSpecsFeaturesItems.KerbWeight:
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

        /// <summary>
        /// Created by  :   Sumit Kate on 13 Feb 2017
        /// Description :   Converts Input Filter DTO to entity
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        internal static Entities.BikeData.NewLaunched.InputFilter Convert(DTO.BikeData.NewLaunched.InputFilterDTO filter)
        {
            return Mapper.Map<DTO.BikeData.NewLaunched.InputFilterDTO, Entities.BikeData.NewLaunched.InputFilter>(filter);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 13 Feb 2017
        /// Description :   Converts New Launched Bike Entity to DTO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal static DTO.BikeData.NewLaunched.NewLaunchedBikeResultDTO Convert(Entities.BikeData.NewLaunched.NewLaunchedBikeResult entity)
        {
             return Mapper.Map<Entities.BikeData.NewLaunched.NewLaunchedBikeResult, DTO.BikeData.NewLaunched.NewLaunchedBikeResultDTO>(entity);
        }
    }
}