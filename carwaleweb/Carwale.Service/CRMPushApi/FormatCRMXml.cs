using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Carwale.Notifications;
using System.Xml.Serialization;
using System.Xml;
using System.Configuration;
using Carwale.Entity.CRMAPI;
using Carwale.Entity.PriceQuote;

namespace Carwale.Service
{
    public class FormatCRMXml
    {
        /// <summary>
        /// Function to generate xml
        /// </summary>
        /// <param name="pq"></param>
        /// <returns></returns>
        public string GetXMLFormat(PriceQuoteEntity pq)
        {            
            var crmData = new CRMData();

            crmData.VerificationData = new VerificationData()
            {
                CityId = pq.CityId,
                VersionId = pq.CarVersionId
            };

            crmData.CustomerData = new CustomerData()
            {
                FirstName = RemoveSpecialCharacters(pq.Name),
                Email = RemoveSpecialCharacters(pq.Email),
                Mobile = RemoveSpecialCharacters(pq.Mobile),
                CWCustId = pq.CustomerId,
                CityId = pq.CityId,
                
                Source = pq.PlatformSourceId,
                LeadSourceCategoryId = pq.LeadSourceCategoryId,
                LeadSourceId = pq.LeadSourceId,
                LeadSourceName = pq.LeadSourceName
            };

            crmData.CarData = new Carwale.Entity.CRMAPI.CarData()
            {
                CarVersionId = pq.CarVersionId,
                CityId = pq.CityId,
                PQId = pq.InquiryId,
                ExpectedBuyingDate = DateTime.Now.AddDays(Convert.ToInt32(pq.BuyTimeDays))
            };

            return GetXMLString(crmData);
        }

        static string GetXMLString(object Doc)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                // Save to XML string
                XmlSerializer ser = new XmlSerializer(Doc.GetType());
                using (var writer = XmlWriter.Create(sb))
                {
                    ser.Serialize(writer, Doc);
                }               
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, "Carwale.PriceQuote.GetXMLFormat");
                objErr.LogException();
            }
            return sb.ToString();
        }

        /// <summary>
        /// Removes if any special characters present in input values
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static string RemoveSpecialCharacters(string input)
        {
            return input.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
        }
    }
}
