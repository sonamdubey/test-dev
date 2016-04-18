using BikeWaleOpr.Common;
using System;
using System.IO;
using System.Net;
using System.Web;

namespace BikewaleOpr.common
{
    /// <summary>
    /// Class to maintain all the API calls for Knowlarity
    /// </summary>
    public class KnowlarityAPI
    {
        /// <summary>
        /// Created by: Sangram Nandkhile on 04 Apr 2016
        /// Description: Push data to Knowlarity
        /// </summary>
        /// <param name="isPresent">If masking number exists then update</param>
        /// <param name="saveId"></param>
        /// <param name="dlrMobileNo">Dealer Mobile Number</param>
        /// <param name="updateMaskingNumber">Old number which needs to be updated</param>
        /// <param name="newMaskingNumber">New masking number</param>
        public void pushDataToKnowlarity(bool isPresent, string saveId, string dlrMobileNo, string updateMaskingNumber, string newMaskingNumber)
        {
            string sql;
            int rowcount;
            //Push Data to Knowlarity
            if (saveId == "-1")
            {
                //if masking number exists then update
                if (isPresent == true)
                {
                    sql = "%20UPDATE%20carwale_agent_mappings_after_16Dec2013%20SET%20mapped_numbers='" + dlrMobileNo + "'%20WHERE%20knumber='" + newMaskingNumber + "'";
                    rowcount = CallDealerMobilMaskingAPI(sql);
                }
                else
                {
                    sql = "%20UPDATE%20carwale_agent_mappings_after_16Dec2013%20SET%20mapped_numbers='" + dlrMobileNo + "'%20WHERE%20knumber='" + newMaskingNumber + "'";
                    rowcount = CallDealerMobilMaskingAPI(sql);
                    if (rowcount == 0)
                    {
                        sql = "%20INSERT%20INTO%20carwale_agent_mappings_after_16Dec2013(Client_Id,mapped_numbers,knumber)%20VALUES%20('100013','" + dlrMobileNo + "','" + newMaskingNumber + "')%20";
                        CallDealerMobilMaskingAPI(sql);
                    }
                }
            }
            else//in case of edit
            {
                sql = "%20UPDATE%20carwale_agent_mappings_after_16Dec2013%20SET%20mapped_numbers='" + dlrMobileNo + "'%20WHERE%20knumber='" + updateMaskingNumber + "'";
                rowcount = CallDealerMobilMaskingAPI(sql);
            }
        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 04 Apr 2016
        /// Description: API call to send SQL query created dynamically
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public int CallDealerMobilMaskingAPI(string sqlQuery)
        {
            string result = "";
            int rowcount = 0;
            string username, password, url;
            try
            {
                username = "carwale";
                password = "@ep!3xchang3009";
                url = "http://kapps.knowlarity.com/api/kreport?username=" + username + "&password=" + password + "&sql=" + sqlQuery;
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                var byteData = new byte[] { 0x20 };
                request.ContentLength = byteData.Length;

                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    rowcount = Convert.ToInt16(result.Split(',')[1].Split(':')[1]);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"] + "-CallDealerMobilMaskingAPI");
                objErr.SendMail();
            }
            return rowcount;
        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 04 Apr 2016
        /// Description: Clear Masking Number
        /// </summary>
        /// <param name="maskingNumber">Masking Number</param>
        public void clearMaskingNumber(string maskingNumber)
        {
            string urlSql = "%20UPDATE%20carwale_agent_mappings_after_16Dec2013%20SET%20mapped_numbers=''%20WHERE%20knumber='" + maskingNumber.Trim() + "'";
            this.CallDealerMobilMaskingAPI(urlSql);
        }
    }
}