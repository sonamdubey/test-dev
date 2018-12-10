using NReco.PdfGenerator;
using System;

namespace Carwale.Utility
{
    public static class PdfProcessor
    {
        public static byte[] GeneratePdfBytesFromUrl(Uri htmlLocation)
        {
            byte[] pdfByte = null;
            if (htmlLocation != null)
            {
                var htmlToPdf = new HtmlToPdfConverter();
                pdfByte = htmlToPdf.GeneratePdfFromFile(htmlLocation.OriginalString, null);
            }
            return pdfByte;
        }

        public static byte[] GeneratePdfBytesFromHtmlString(string htmlString)
        {
            byte[] pdfByte = null;
            if (!string.IsNullOrEmpty(htmlString))
            {
                var htmlToPdf = new HtmlToPdfConverter();
                pdfByte = htmlToPdf.GeneratePdf(htmlString, null);
            }
            return pdfByte;
        }
    }
}
