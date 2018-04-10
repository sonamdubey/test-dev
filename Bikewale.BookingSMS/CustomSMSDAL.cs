using System;
using System.Collections.Generic;
using System.Configuration;

namespace Bikewale.BookingSMS
{
    /// <summary>
    /// Author  : Sumit Kate
    /// Created : 16 July 2015
    /// Use to communicate with BikeWale Db using ADO.NET
    /// </summary>
    public class CustomSMSDAL
    {
        protected string connectionString = String.Empty;

        /// <summary>
        /// Constructor used to initialize the connectionString member variable
        /// by reading AppSettings key bwconnectionstring
        /// </summary>
        public CustomSMSDAL()
        {
            this.connectionString = ConfigurationManager.AppSettings["mySqlBWConnectionString"];
        }

        /// <summary>
        /// Saves the SMS by calling InsertSMSSent stored procedure
        /// </summary>
        /// <param name="number">Recepient</param>
        /// <param name="message">Message</param>
        /// <param name="smsType">Type of SMS</param>        
        /// <param name="retMsg"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public string InsertSMS(string number, string message, EnumSMSServiceType smsType, string retMsg, bool status)
        {

            throw new Exception(" InsertSMS(string number, string message, EnumSMSServiceType smsType, string retMsg, bool status) Method is NotImplemented ");
            
        }

        /// <summary>
        /// Gets the Offer SMS data by calling GetRecipientForOfferSMS.
        /// </summary>
        /// <returns>Enumerable collection of CustomSMSEntity</returns>
        public IEnumerable<CustomSMSEntity> FetchSMSData()
        {
            throw new Exception("FetchSMSData() Method is NotImplemented ");
        }
    }
}
