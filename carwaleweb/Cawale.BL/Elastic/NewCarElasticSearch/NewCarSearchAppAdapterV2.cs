using AutoMapper;
using Carwale.DTOs.PriceQuote;
using Carwale.DTOs.Search.Model;
using Carwale.Entity.ElasticEntities;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.NewCars;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Carwale.BL.Elastic.NewCarSearch
{
    public class NewCarSearchAppAdapterV2 : IServiceAdapterV2
    {
        private readonly INewCarElasticSearch _elasticsearch;
        private readonly IUnityContainer _container;
        public NewCarSearchAppAdapterV2(IUnityContainer container,INewCarElasticSearch elasticsearch)
        {
            _container = container;
            _elasticsearch = elasticsearch;
        }


        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetNewCarSearchPageDtoForApp(input), typeof(T));
        }

        private NewCarSearchDtoV2 GetNewCarSearchPageDtoForApp<U>(U input)
        {
            NewCarSearchDtoV2 dto;
            NameValueCollection queryString = (NameValueCollection)(object)(input);
            dto = GetModels(queryString);
            return dto;
        }

        private NewCarSearchDtoV2 GetModels(NameValueCollection queryString)
        {
            NewCarSearchInputs inputs = _elasticsearch.GetElasticInputs(queryString);
            ElasticCarData data = _elasticsearch.GetModelsV2(inputs);
            NewCarSearchDtoV2 dto = new NewCarSearchDtoV2() { CarModels = new List<NewCarSearchModelDtoV2>() };
            dto.TotalModels = data.TotalModels;
            dto.TotalVersions = data.TotalVersions;
            if (data.ModelVersionDict.Any())
            {
                if (!inputs.CountsOnly)
                {
                    foreach (var model in data.ModelVersionDict)
                    {
                        var first = model.Value.First().Value;
                        bool areAllVersionsMatching = first.VersionsCount == first.MatchingVersionsCount;
                        bool justOneVersion = first.MatchingVersionsCount == 1;
                        var modelDTO = new NewCarSearchModelDtoV2()
                        {
                            MakeId = first.MakeId,
                            ModelId = first.ModelId,
                            MakeName = first.MakeName,
                            ModelName = first.ModelName,
                            MaskingName = first.ModelMaskingName,
                            CarRating = first.ReviewRate.ToString(),
                            OriginalImgPath = first.ImagePath,
                            HostUrl = first.HostUrl.Last() != '/' ? first.HostUrl + "/" : first.HostUrl,
                            PriceOverView = Mapper.Map<PriceOverview, PriceOverviewDTO>(first.PriceOverview)
                        };
                        if (first.PriceOverview != null)
						{
							modelDTO.MatchingVersionText = string.Format(justOneVersion ? "{0} {1} at ₹ <b>{2}</b>" : "{0} {1} starting at ₹ <b>{2}</b>"
								, first.MatchingVersionsCount
								, areAllVersionsMatching ? (justOneVersion ? "version available" : "versions available") : (justOneVersion ? "matching version" : "matching versions")
								, Format.PriceLacCr(first.PriceOverview.Price.ToNullSafeString())
								);
						}
                        dto.CarModels.Add(modelDTO);
                    }
                    dto.OrpText = inputs.cityId > 0 ? string.Empty : CWConfiguration.orpText;
                }
            }
            return dto;
        }
    }
}