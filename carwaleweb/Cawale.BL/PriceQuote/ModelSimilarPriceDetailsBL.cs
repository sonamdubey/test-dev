using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.PriceQuote;
using Carwale.Utility;
using System;
using System.Configuration;

namespace Carwale.BL.PriceQuote
{
    public class ModelSimilarPriceDetailsBL : IModelSimilarPriceDetailsBL
    {
        private readonly IModelSimilarPriceDetailsRepo _modelSimilarDetailsRepo;
        private static string _priceRefreshLimit = ConfigurationManager.AppSettings["PriceRefreshLimit"];

        public ModelSimilarPriceDetailsBL(IModelSimilarPriceDetailsRepo modelSimilarDetailsRepo)
        {
            _modelSimilarDetailsRepo = modelSimilarDetailsRepo;
        }

        public void CreateOrUpdate(ModelSimilarPriceDetail modelSimilarPriceDetail)
        {
            bool isDetailPresent = false;

            var similarPriceDetail = _modelSimilarDetailsRepo.Get(modelSimilarPriceDetail.ModelId);
            if (similarPriceDetail != null)
            {
                isDetailPresent = similarPriceDetail.ModelId > 0;
            }

            if (isDetailPresent)
            {

                modelSimilarPriceDetail.RefreshCount = modelSimilarPriceDetail.CanResetRefreshCount ? 0 
                                                        : similarPriceDetail.RefreshCount + 1;
                modelSimilarPriceDetail.IsPricesRefreshed = similarPriceDetail.IsPricesRefreshed;
                modelSimilarPriceDetail.AvailableOn = GetAvailableOnDate(modelSimilarPriceDetail, similarPriceDetail.AvailableOn);

                _modelSimilarDetailsRepo.Update(modelSimilarPriceDetail);
            }
            else
            {
                modelSimilarPriceDetail.RefreshCount = modelSimilarPriceDetail.CanResetRefreshCount ? 0 : modelSimilarPriceDetail.RefreshCount + 1;
                _modelSimilarDetailsRepo.Create(modelSimilarPriceDetail);
            }
        }

        private string GetAvailableOnDate(ModelSimilarPriceDetail modelSimilarPriceDetail, string oldAvailableOnDate)
        {
            if (!modelSimilarPriceDetail.IsPricesRefreshed)
            {
                if (modelSimilarPriceDetail.RefreshCount == CustomParser.parseIntObject(_priceRefreshLimit))
                {
                    return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (modelSimilarPriceDetail.RefreshCount > CustomParser.parseIntObject(_priceRefreshLimit) && !String.IsNullOrEmpty(oldAvailableOnDate))
                {
                    return Convert.ToDateTime(oldAvailableOnDate).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            else if (!String.IsNullOrEmpty(oldAvailableOnDate))
            {
                return Convert.ToDateTime(oldAvailableOnDate).ToString("yyyy-MM-dd HH:mm:ss");
            }         
            return null;
        }
    }
}
