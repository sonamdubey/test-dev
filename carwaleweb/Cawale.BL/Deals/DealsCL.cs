using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using Carwale.Notifications;

namespace Carwale.BL.Deals
{
    public class Advanatge
    {
        public static bool IsCityMumbaiAround(int cityId)
        {
            return (cityId == 1 || cityId == 13 || cityId == 40) ? true : false;
        }

        public static int GetOfferModelId(int cityId)
        {
            int modelId = 0;
            try
            {
                string offerModelIds = System.Configuration.ConfigurationManager.AppSettings["OfferOfWeek"].ToString();
                string[] models = new string[] { };
                string[] modelCity = new string[] { };
                if (!String.IsNullOrWhiteSpace(offerModelIds))
                {
                    models = offerModelIds.Split('|');
                    foreach (string model in models)
                    {
                        var modelArray = model.Split('-');
                        if (Convert.ToInt32(modelArray[1]) == cityId && Convert.ToDateTime(modelArray[2]) > DateTime.Now)
                        {
                            modelId = Convert.ToInt32(modelArray[0]);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarDealsCL.GetOfferModelId()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return modelId;
        }
    }
}
