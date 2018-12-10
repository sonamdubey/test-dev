using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.PaymentGateway
{
    public class TransactionDetails
    {
        public string CustomerName { get; set; }
        public string CustCity { get; set; }
        public string CustState { get; set; }
        public string CustEmail { get; set; }
        public string CustMobile { get; set; }
        public int ConsumerType { get; set; }
        public int PackageId { get; set; }
        public string UserAgent { get; set; }

        /// <summary>
        /// Customer Id for customer's unique identification
        /// </summary>
        public ulong CustomerID { get; set; }

        /// <summary>
        ///  Transaction Amount 
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Transaction Id for unique identification of the transaction 
        /// </summary>
        public long PGRecordId { get; set; }


        public string ClientIP { get; set; }

        /// <summary>
        ///Inquiry Id of the customer
        /// </summary>
        public ulong PGId { get; set; }

        /// <summary>
        /// Payment Gateway id that is 1 for ICICI, 2 for CCAvenue
        /// </summary>
        public int SourceId { get; set; }

        public string UniqueTransactionId { get; set; } = ConfigurationManager.AppSettings["OfferUniqueTransaction"];

        /// <summary>
        /// ApplicationId for PG. 1: Carwale, 2: Bikewale, 3:Absure, 4: CarTrade
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Url to receive response from PG
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Url to request data to PG
        /// </summary>
        public string RequestToPGUrl { get; set; }

        /// <summary>
        /// PlatformId for PG. 1: Desktop, 43: Mobile, 74:Android, IOS:83
        /// </summary>
        public int PlatformId { get; set; }

        /// <summary>
        /// AccountIdentifier for PG. Default set to Carwale. Can be overriden to CarTrade etc.
        /// </summary>
        public PGAccountIdentifier PGAccountIdentifier  { get; set; } = PGAccountIdentifier.CARWALE;
    }
}



