using Carwale.Entity.CarData;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.XmlFeed;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.XmlFeeds
{
    public class CarModelsSitemapFeed : IXmlFeed
    {
        private readonly ICarModelCacheRepository _modelRepo;
        private readonly ICarPriceQuoteAdapter _priceRepo;
        private readonly ICarModels _carmodels;

        public CarModelsSitemapFeed(ICarModelCacheRepository modelRepo, ICarPriceQuoteAdapter priceRepo, ICarModels carmodels)
        {            
            _modelRepo = modelRepo;
            _priceRepo = priceRepo;
            _carmodels = carmodels;
        }

        /// <summary>
        /// To Generate xml feeds for Sociomantic product
        /// Written By : Jitendra Singh on 15 july
        /// </summary>
        /// <returns>Entity for model feed</returns>
        public List<SociomanticProduct> GenerateSociomanticXmlFeed()
        {
            var sociomanticProductList = new List<SociomanticProduct>();
            try
            {
                List<CarMakeModelEntityBase> modelsInfo = _modelRepo.GetAllModels("new");
                List<int> modelIds = modelsInfo.Select(c => c.ModelId).ToList();
                IDictionary<int, PriceOverview>  modelPriceList = _priceRepo.GetModelsCarPriceOverview(modelIds, -1);
                foreach (var model in modelsInfo)
                {
                    PriceOverview modelprice = null;
                    modelPriceList.TryGetValue(model.ModelId, out modelprice);

                    sociomanticProductList.Add(new SociomanticProduct()
                    {
                        Id = model.ModelId,
                        Brand = model.ModelName,
                        Category = model.MakeName,
                        ImageUrl = Utility.CWConfiguration._imgHostUrl + Utility.ImageSizes._0X0 + model.ImageUrl,
                        RegularPrice = modelprice != null ? modelprice.Price : 0,
                        SalePrice = modelprice != null ? modelprice.Price : 0,
                        Title = model.MakeName + " "+ model.ModelName,
                        Url = Utility.ManageCarUrl.CreateModelUrl(model.MakeName,model.MaskingName,true),
                        Description = _carmodels.GetDescription(model.MakeName,model.ModelName, (Utility.Format.GetFormattedPriceV2(modelprice != null ? modelprice.Price.ToString() : "0")))                                              
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return sociomanticProductList;
        }

        /// <summary>
        /// To Generate Xml Feeds For Models
        /// Written By : Ashish Verma on 18/2/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<url> GenerateXmlFeed()
        {
            var modelUrlList = new List<url>();
            try
            {
                List<CarMakeModelEntityBase> modelList = (List<CarMakeModelEntityBase>)_modelRepo.GetAllModels("New");

                foreach (var model in modelList)
                {
                    modelUrlList.Add(new url() {
                        loc = Utility.ManageCarUrl.CreateModelUrl(model.MakeName,model.MaskingName,true)                        
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return modelUrlList;
        }       
    }
}
