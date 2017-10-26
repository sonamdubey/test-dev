using Consumer;
using NReco.PdfGenerator;
namespace Bikewale.RabbitMq.CapitalFirstLeadConsumer
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13-Sep-2017
    /// Summary: Converts HTML to PDF
    /// 
    /// </summary>
    public static class CreatePdf
    {
        public static byte[] ConvertToBytes(string htmlContent)
        {
            byte[] pdfBytes = null;
            try
            {
                HtmlToPdfConverter document = new HtmlToPdfConverter();
                pdfBytes = document.GeneratePdf(htmlContent);
                return pdfBytes;
            }
            catch
            {
                Logs.WriteErrorLog("Error occured while processing Lead: ConvertToBytes()");
            }

            return pdfBytes;
        }
    }
}
