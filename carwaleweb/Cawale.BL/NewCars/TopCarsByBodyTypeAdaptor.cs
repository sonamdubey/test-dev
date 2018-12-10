using AutoMapper;
using Carwale.BL.Elastic.NewCarSearch;
using Carwale.DTOs.CarData;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.CarData;
using Carwale.Entity.ElasticEntities;
using Carwale.Entity.Enum;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.NewCars
{
    public class TopCarsByBodyTypeAdaptor : IServiceAdapterV2
    {
        private readonly ITopCarsBl _topCarBl;

        public TopCarsByBodyTypeAdaptor(ITopCarsBl topCarBl)
        {
            _topCarBl = topCarBl;
        }
        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetTopCarsByBodyType(input), typeof(T));
        }

        private TopCarsByBodyTypeDto GetTopCarsByBodyType<U>(U input)
        {
            TopCarsByBodyTypeDto dto = new TopCarsByBodyTypeDto();
            
            try
            {
                Type inputTopCarsByBodyTypeParams = input.GetType();
                NewCarSearchInputs inputs;
                if (typeof(TopCarsByBodyTypeParams) == inputTopCarsByBodyTypeParams)
                {
                    TopCarsByBodyTypeParams inputParam = (TopCarsByBodyTypeParams)Convert.ChangeType(input, typeof(U));
                    inputs = new NewCarSearchInputs()
                    {
                        bodytype = inputParam.BodyType.ToString().Split(','),
                        pageSize = inputParam.Count,
                        pageNo = inputParam.PageNo,
                        cityId = inputParam.CityId,
                        sortField1 = new Tuple<Field, SortOrder>(new Field("modelPopularity"), SortOrder.Descending),
                        ShowOrp = true,
                        IsMobile = inputParam.IsMobile
                    };
                    dto = _topCarBl.GetTopModels(inputs, false);
                    CarBodyStyle bodyStyle = (CarBodyStyle)inputParam.BodyType;
                    string friendlybodyType = (short)CarBodyStyle.SUV == inputParam.BodyType ? (bodyStyle).ToFriendlyString() : (bodyStyle).ToFriendlyString().ToLower();
                    if (!inputParam.IsMobile)
                    {
                        dto.SEOText = _topCarBl.GetSeoText(dto.ModelList, friendlybodyType, bodyStyle, string.Empty);
                    }
                   dto.BreadcrumbEntitylist = _topCarBl.GetBreadCrumb(friendlybodyType);
                }
                else
                {
                    inputs = (NewCarSearchInputs)Convert.ChangeType(input, typeof(U));
                    dto = _topCarBl.GetTopModels(inputs, true);
                }
                
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "TopCarsByBodyTypeAdaptor.GetTopCarsByBodyType<U>(U input)\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return dto;
        }
    }
}
