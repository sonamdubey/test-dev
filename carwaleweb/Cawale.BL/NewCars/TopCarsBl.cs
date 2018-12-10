using Carwale.Interfaces.NewCars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.DTOs.CarData;
using Newtonsoft.Json.Linq;
using Carwale.Utility;
using Carwale.Entity;
using Carwale.Entity.ElasticEntities;
using Carwale.Interfaces.Elastic;
using AutoMapper;
using Carwale.Entity.PriceQuote;
using Carwale.DTOs.PriceQuote;
using Carwale.BL.Experiments;
using Carwale.Entity.Enum;

namespace Carwale.BL.NewCars
{
	public class TopCarsBl : ITopCarsBl
	{   private readonly INewCarElasticSearch _elasticsearch;

        public TopCarsBl(INewCarElasticSearch elasticsearch)
        {
            _elasticsearch = elasticsearch;
        }
		public JObject CreatSchema(List<ModelDetailsDto> modelList, string bodyType)
		{
			JObject jObject = null;
			if (modelList.IsNotNullOrEmpty())
			{
				List<JObject> itemList = new List<JObject>();
				int i = 1;
				foreach (var model in modelList)
				{
					JObject items = new JObject(
						 new JProperty("@type", "ListItem"),
						 new JProperty("position", i),
                         new JProperty("url", ManageCarUrl.CreateModelUrl(model.MakeName, model.MaskingName, true)),
						new JProperty("name", $"{model.MakeName} {model.ModelName}"),
						new JProperty("image", ImageSizes.CreateImageUrl(model.HostUrl, ImageSizes._227X128, model.OriginalImgPath))
						);
					itemList.Add(items);
					i++; // position of car
				}

				jObject = new JObject(
				new JProperty("@context", "http://schema.org"),
				new JProperty("@type", "ItemList"),
				new JProperty("url", ManageCarUrl.CreateTopCarsByBodyTypeUrl($"{bodyType}s".ToLower(), true)),
				new JProperty("name", $"Best {bodyType}s in India - {DateTime.Now.ToString("MMMM yyyy")}"),
				new JProperty("description", $"CarWale brings the list of best {bodyType}s in India for {DateTime.Now.ToString("MMMM yyyy")}. Explore the top {modelList.Count} {bodyType}s to buy the best car of your choice."),
				new JProperty("itemListElement", itemList)
				);
			}
			return jObject;
		}

		public List<BreadcrumbEntity> GetBreadCrumb(string bodyType, string price = "")
		{
			return new List<BreadcrumbEntity>
			{
				new BreadcrumbEntity{ Title = string.IsNullOrEmpty(price) ? $"Best {bodyType}s in India" : $"Best {bodyType}s under {price}",
				Text = string.IsNullOrEmpty(price) ? $"Best {bodyType}s in India" : $"Best {bodyType}s under {price}",
				Link = ""}
			};
		}

		public string GetSeoText(List<ModelDetailsDto> modelList, string bodyType, CarBodyStyle bodyStyle, string price ="")
		{
            string seoText = null;
            if(modelList != null){
			    StringBuilder topRanks = new StringBuilder(string.Empty);
			    int count = Math.Min(3, modelList.Count);
			    for (int i = 0; i < count; i++)
			    {
				    topRanks.Append($"{modelList[i].MakeName} {modelList[i].ModelName} ({modelList[i].CarPriceOverview.Price})");
				    if ( count >= 2 && i == count - 2)
				    {
					    topRanks.Append(" and ");
				    }
				    else if (i != count - 1)
				    {
					    topRanks.Append(", ");
				    }
			    }               
			    if (string.IsNullOrEmpty(price))
			    {
				    seoText = $"The top {bodyType} cars in India include {topRanks}. To see the latest price in your city, offers, variants, specifications, pictures, mileage and reviews of the best {bodyType} cars, please select your desired car models from the list below.";
			    }
			    else
			    {
				    seoText = $"The top {bodyType} cars under {price} in India include {topRanks}. To see the latest price in your city, offers, variants, specifications, pictures, mileage and reviews of the best {bodyType} cars, please select your desired car models from the list below.";
			    }
            }
			return seoText;
		}

        public TopCarsByBodyTypeDto GetTopModels(NewCarSearchInputs inputs, bool isNcf)
        {
            TopCarsByBodyTypeDto topCarsDto = new TopCarsByBodyTypeDto();
            topCarsDto.ModelList = new List<ModelDetailsDto>();
            var data = _elasticsearch.GetModels(inputs, isNcf);
            if (data.ModelVersionDict.Any())
            {
                foreach (var result in data.ModelVersionDict)
                {
                    var first = result.Value.First();
                    var topCarsByBodyType = new ModelDetailsDto()
                    {
                        MakeId = first.Value.MakeId,
                        MakeName = first.Value.MakeName,
                        ModelId = first.Value.ModelId,
                        ModelName = first.Value.ModelName,
                        MaskingName = first.Value.ModelMaskingName,
                        HostUrl = first.Value.HostUrl,
                        OriginalImgPath = first.Value.ImagePath,
                        Mileage = first.Value.MileageSummary,
                        CarPriceOverview = Mapper.Map<PriceOverview, PriceOverviewDTOV2>(first.Value.PriceOverview != null ? first.Value.PriceOverview : new PriceOverview())
                    };
                    if(!inputs.IsMobile)
                      {
                        topCarsByBodyType.NewsCount = Convert.ToInt32(first.Value.NewsCount);
                        topCarsByBodyType.ExpertReviewCount = Convert.ToInt32(first.Value.ExpertReviewsCount);
                        topCarsByBodyType.PhotoCount = Convert.ToInt32(first.Value.ImagesCount);
                        topCarsByBodyType.VideoCount = Convert.ToInt32(first.Value.VideoCount);             
                        topCarsByBodyType.Colours = first.Value.ColorsData;
                      }
                    topCarsDto.ModelList.Add(topCarsByBodyType);
                }
                topCarsDto.TotalModels = data.TotalModels;
            }
            return topCarsDto;
        }
	}
}
