///Added By Vivek Gupta on 12-02-2016
///to move viewstate at the end of the html page on browsers
using System.IO;
using System.Web.UI;
namespace Bikewale.Utility
{
    public class PageBase : System.Web.UI.Page
    {
        /// This method overrides the Render() method for the page and moves the ViewState
        /// from its default location at the top of the page to the bottom of the page. This
        /// results in better search engine spidering.
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            // Obtain the HTML rendered by the instance.
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            base.Render(hw);
            string html = sw.ToString();

            // Close the writers we don't need anymore.
            hw.Close();
            sw.Close();

            // Find the viewstate. 
            int start = html.IndexOf(@"<input type=""hidden"" name=""__VIEWSTATE""");
            // If we find it, then move it.
            if (start > -1)
            {
                int end = html.IndexOf("/>", start) + 2;

                string strviewstate = html.Substring(start, end - start);
                html = html.Remove(start, end - start);

                // Find the end of the form and insert it there.
                int formend = html.IndexOf(@"</form>") - 1;
                html = html.Insert(formend, strviewstate);
            }

            // Send the results back into the writer provided.
            writer.Write(html);
        }
    }
}
