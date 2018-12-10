using Carwale.Entity.Stock;
using Carwale.Entity.Stock.Finance;
using Carwale.Interfaces.Stock;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Newtonsoft.Json;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace Carwale.BL.Stock
{
    public class StockFinanceBL
    {
        private static readonly string _usedStockFinanceQueue = ConfigurationManager.AppSettings["UsedStockFinanceQueue"];
        private static readonly string _financeLinkText = ConfigurationManager.AppSettings["FinanceLinkText"];

        public static void PushToFinanceQueue(List<StockFinance> financeList)
        {
            RabbitMqPublish publisher = new RabbitMqPublish();
            foreach (StockFinance finance in financeList)
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("StockFinance", JsonConvert.SerializeObject(finance));
                publisher.PublishToQueue(_usedStockFinanceQueue, nvc);
            }
        }

        public static StockFinanceData GetFinanceData(FinanceUrlParameter param)
        {
            StockFinanceData stockFinance = new StockFinanceData();
            stockFinance.FinanceUrl = GetFinanceUrl(param);
            stockFinance.FinanceUrlText = _financeLinkText;
            return stockFinance;
        }

        private static string GetFinanceUrl(FinanceUrlParameter param)
        {
            if (param != null)
            {
                return String.Format("{0}{1}/step1?makeid={2}&modelid={3}&mfgyear={4}&cityid={5}&price={6}&owner={7}&mfgmonth={8}", param.HostUrl,
                param.ProfileId, param.MakeId, param.ModelId, param.MakeYear, param.CityId, param.PriceNumeric, param.OwnerNumeric, param.MakeMonth);               
            }
            return null;
        }
    }
}
