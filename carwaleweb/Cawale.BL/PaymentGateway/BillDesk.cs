using Carwale.Entity.Enum;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
using Carwale.Utility;
using System;
using System.Configuration;
using System.Web;

namespace Carwale.BL.PaymentGateway
{
    public class BillDesk : IPaymentGateway
    {
        string requestUrl = string.Empty;
        string Merchant_Id = string.Empty;
        string CustomerId = string.Empty;
        string TxnAmount = string.Empty;
        string CurrencyType = string.Empty;
        string TypeField1 = string.Empty;
        string SecurityId = string.Empty;
        string TypeField2 = string.Empty;
        string RU = string.Empty;
        string Checksum = string.Empty;
        string WorkingKey = string.Empty;
        string MessageDescription = string.Empty;
        string RequstToBillDesk = string.Empty;

        public BillDesk()
        {
            requestUrl = "https://pgi.billdesk.com/pgidsk/PGIMerchantPayment";
            CurrencyType = "INR";
            TypeField1 = "R";
            TypeField2 = "F";
            WorkingKey = ConfigurationManager.AppSettings["BillDeskWorkingKey"].ToString();
        }

        public void Request(TransactionDetails _details)
        {
            RU = _details.ReturnUrl;
            if (_details.ApplicationId == 1 || _details.ApplicationId == 4)   //Request from Carwale/CarTrade user
            {
                Merchant_Id = "CARWALE";
                SecurityId = "carwale";
            }
            else if (_details.ApplicationId == 2)     //Request from Bikewale User
            {
                Merchant_Id = "BIKEWALE";
                SecurityId = "bikewale";
            }
            else     //Request from Absure User
            {
                Merchant_Id = "ABSURE";
                SecurityId = "absure";
            }

            string MessageDescription = Merchant_Id + "|" + _details.UniqueTransactionId + "|NA|" + _details.Amount + "|NA|NA|NA|"
                                      + CurrencyType + "|NA|" + TypeField1 + "|" + SecurityId + "|NA|NA|" + TypeField2 + "|NA|NA|NA|NA|NA|NA|"
                                      + _details.PGAccountIdentifier + "|" + RU;

            string hash = CarwaleSecurity.GetHMACSHA256(MessageDescription, ConfigurationManager.AppSettings["BillDeskWorkingKey"]).ToUpper();
            string msg = MessageDescription + "|" + hash;
            string encryptMsg = CarwaleSecurity.Encrypt(msg);
            HttpContext.Current.Response.Redirect(_details.RequestToPGUrl + "?msg=" + encryptMsg);
        }

        public GatewayResponse GetResponse()
        {
            var pgResponse = new GatewayResponse();
            string _paymentResp = HttpContext.Current.Request.Form["msg"];
            string[] arrResponse = _paymentResp.Split('|'); //PG
            string authStatus = arrResponse[14];
            HttpContext.Current.Response.Write("authStatus : " + authStatus);
            string txnAmount = arrResponse[4];
            string checksum = arrResponse[25];
            string _customerId = arrResponse[1];
            int lastInx = _paymentResp.LastIndexOf('|');
            string _errorDescription = arrResponse[24];
            string txnReferenceNo = arrResponse[2];
            string txnDate = arrResponse[13]; //dd-mm-yyyy
            pgResponse.Name = arrResponse[16].Trim();
            pgResponse.Email = arrResponse[17].Trim();
            pgResponse.Mobile = arrResponse[18].Trim();

            string hash = CarwaleSecurity.GetHMACSHA256(_paymentResp.Substring(0, lastInx), ConfigurationManager.AppSettings["BillDeskWorkingKey"]);

            if (hash.ToUpper() != checksum.ToUpper())
            {
                pgResponse.PGRespCode = "0005";
                pgResponse.PGMessage = "Transaction is not successful";
            }
            else
            {
                if (authStatus == "0300")
                {
                    pgResponse.PGRespCode = authStatus;
                    pgResponse.PGMessage = "Transaction Successfull";
                }
                else if (authStatus == "NA")
                {
                    pgResponse.PGRespCode = "0006";
                    pgResponse.PGMessage = "Invalid input in request message-" + _errorDescription;
                }
                else if (authStatus == "0399")
                {
                    pgResponse.PGRespCode = authStatus;
                    pgResponse.PGMessage = "Invalid Authentication at bank -" + _errorDescription;
                }
                else if (authStatus == "0002")
                {
                    pgResponse.PGRespCode = authStatus;
                    pgResponse.PGMessage = "BillDesk is waiting for a response from bank-" + _errorDescription;
                }
                else if (authStatus == "0001")
                {
                    pgResponse.PGRespCode = authStatus;
                    pgResponse.PGMessage = "Error at bill Desk -" + _errorDescription;
                }
            }
            pgResponse.PGTransId = System.Text.RegularExpressions.Regex.Replace(_customerId, @"[^\d]", "");
            pgResponse.PGEPGTransId = "BillDesk";
            pgResponse.PGAuthIdCode = "BillDesk";
            int respcd = Convert.ToInt16(BillDeskTransactionStatusCode.Successfull);
            pgResponse.IsTransactionCompleted = Convert.ToInt16(pgResponse.PGRespCode) == respcd ? true : false;
            return pgResponse;
        }

    }
}
