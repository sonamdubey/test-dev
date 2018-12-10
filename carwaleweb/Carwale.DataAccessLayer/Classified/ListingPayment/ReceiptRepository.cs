using Carwale.Entity.Classified.ListingPayment;
using Carwale.Interfaces.Classified.ListingPayment;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DAL.Classified.ListingPayment
{
    public class ReceiptRepository: RepositoryBase, IReceiptRepository
    {
        public bool Insert(Receipt receipt)
        {
            int rowsInserted = 0;
            try
            {
                if (receipt != null)
                {
                    using (IDbConnection con = ClassifiedMySqlMasterConnection)
                    {
                        string sql = @"insert into paid_lististing_receipt(inquiryId,receiptPath,pgtransactionid) 
                        values(@InquiryId, @Path, @PgTransactionId);";
                        rowsInserted = con.Execute(sql, receipt);
                    } 
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
            return rowsInserted > 0;
        }

        public List<Receipt> GetForInquiry(int inquiryId)
        {
            List<Receipt> receipts = null;
            try
            {
                if (inquiryId > 0)
                {
                    using (IDbConnection con = ClassifiedMySqlMasterConnection)
                    {
                        string sql = @"select inquiryId, receiptPath as path, entrydate, pgtransactionid
                                    from paid_lististing_receipt where inquiryId = @InquiryId";
                        receipts = con.Query<Receipt>(sql, new { InquiryId = inquiryId }).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
            return receipts;
        }
    }
}
