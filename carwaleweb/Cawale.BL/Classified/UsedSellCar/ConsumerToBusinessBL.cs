using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Interfaces.Classified.SellCar;
using RabbitMqPublishing;
using System.Collections.Specialized;
using System.Configuration;

namespace Carwale.BL.Classified.UsedSellCar
{
    public class ConsumerToBusinessBL : IConsumerToBusinessBL
    {
        public void PushToIndividualStockQueue(int inquiryId, int hotLeadPrice = 0, bool optForGuaranteedSales=false)
        {
            RabbitMqPublish rabbitMqPublish = new RabbitMqPublish(); 
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("inquiryId", inquiryId.ToString());
            if (hotLeadPrice > 0)
            {
                nvc.Add("hotLeadPrice", hotLeadPrice.ToString());
            }
            rabbitMqPublish.PublishToQueue(ConfigurationManager.AppSettings["UsedStockProcurementQueueNew"], nvc);
        }
        public void PushToIndividualStockQueue(int tempInquiryId, C2BActionType action, int inquiryId = -1, int hotLeadPrice = 0, bool optForGuaranteedSales = false)
        {
            RabbitMqPublish rabbitMqPublish = new RabbitMqPublish(); 
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("tempInquiryId", tempInquiryId.ToString());
            nvc.Add("inquiryId", inquiryId.ToString());
            nvc.Add("action", action.ToString("D"));
            nvc.Add("optForGuaranteedSales", optForGuaranteedSales.ToString());
            if (hotLeadPrice > 0)
            {
                nvc.Add("hotLeadPrice", hotLeadPrice.ToString());
            }
            rabbitMqPublish.PublishToQueue(ConfigurationManager.AppSettings["UsedStockProcurementQueueNew"], nvc);
        }
    }
}
