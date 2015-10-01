using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bikewale.Entities.CMS.Articles;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 Sept 2015
    /// Summary : Class have functions to sanitize the html elements from the given content.
    /// </summary>
    public static class SanitizeHtmlContent
    {
        private static bool setMargin = true;

        /// <summary>
        /// Dictionary for the input html elements and associated tags for each element.
        /// </summary>
        private static readonly Dictionary<string, string> patternList = new Dictionary<string, string>()
        { 
            {"<br>", "<p>"},
            {"<br />", "<p>"},
            {"<div.*?>(.*?)</div>", "<p>$1<p>"},
            {"<div.*?>(.*?)|</div>", "<p>$1<p>"},
            {"<ul.*?>(.*?)</ul>", "<p>$1<p>"},
            {"<ul.*?>(.*?)|</ul>", "<p>$1<p>"},
            {"</li>", "<li>"},
            {"<p.*?>(.*?)", "<p>$1"},
            {"<h2.*?>(.*?)</h2>", "<p>[BWHeading]$1[BWHeading]<p>"},
            {"<h3.*?>(.*?)</h3>", "<p>[BWHeading]$1[BWHeading]<p>"},
            {"<h1.*?>(.*?)</h1>", "<p>[BWHeading]$1[BWHeading]<p>"},
            {"<P>", "<p>"},
            {"</P>", ""},
            {"</p>", ""},
            {"<!--.*?if.*?>(.*?)|<.*?endif.*?>", ""},
            {"<a.*?>(.*?)</a>", "$1 "},
            {"<a.*?>(.*?)|</a>", "$1 "},
            {@"<img\s+[^>]*?src\s*=\s*[""']([^""']+)[""']\s*.*?/*>", "[BWImageSrc]bwimgsep:$1[BWImageSrc]"},
            {"<em.*?>(.*?)</em>", "$1"},
            {"<span.*?>(.*?)</span>", "$1"},
            {"<span.*?>(.*?)|</span>", "$1"},
            {@"<object.*?>(.*?)</object>", ""},
            {@"<object.*?>(.*?)|</object>", ""},
            {@"<embed.*?>(.*?)|</embed>", ""},
            {@"<script.*?>(.*?)|</script>", ""},
            {@"<center.*?>(.*?)|</center>", ""},            
            {@"<table.*?>(\n|.)*</table>", ""},
            {@"<!--(\n|.)*-->", ""},
            {"&nbsp;|\t|\r|\n", ""},
            {"&rsquo;", "'"},
            {"&lsquo;", "'"},
            {"&ndash;", "-"},
            {"&lt;", "<"},
            {"&gt;", ">"},
            {"&amp;", "&"},
            {"&pound;", "£"},
            {"&euro;", "€"},
            {"&aacute;", "á"},
            {"&uuml;", "ü"},
            {"&ouml;", "ö"},
            {"&eacute;", "é"},
            {"&Prime;", "″"},
            {"&lrm;", " "},
            {"&yacute;", "ý"},
            {"&ldquo;", "“"},
            {"&rdquo;", "”"},
            {"&bull;", "-"},//bullet is replaced by -
            {@"<iframe\s+[^>]*?src\s*=\s*[""']([^""']+)[""']\s*.*?/*></iframe>", "[BWVideoSrc]bwvideosep:$1[BWVideoSrc]"}
        };

        /// <summary>
        /// Function to get the formatted content.
        /// </summary>
        /// <param name="inputContent"></param>
        /// <returns></returns>
        public static HtmlContent GetFormattedContent(string inputContent)
        {
            string sanitizedContent = string.Empty;
            HtmlContent objHtmlContent = new HtmlContent();

            try
            {
                sanitizedContent = SanitizeContent(inputContent);

                string[] stringSeperator = new string[] { "<p>" };
                string[] contents = sanitizedContent.Split(stringSeperator, StringSplitOptions.None);
                for (int i = 0; i < contents.Length; i++)
                {
                    if (contents[i].Trim() != "" && HeadingDetected(contents[i].Trim()))
                    {
                        contents[i] = RemoveStrongTag(contents[i]);
                        AddHeadingToList(objHtmlContent, contents[i].Trim(), true);
                    }
                    else if (contents[i].IndexOf("<li>") != -1)
                    {
                        string[] stringSeperatorLi = new string[] { "<li>" };
                        string[] arrContents = contents[i].Split(stringSeperatorLi, StringSplitOptions.None);

                        List<string> LiTextList = new List<string>();
                        for (int k = 0; k < arrContents.Length; k++)
                        {
                            if (arrContents[k].Trim() != "")
                                LiTextList.Add(arrContents[k].Trim());
                        }
                        AddLiTextToList(objHtmlContent, LiTextList, true);
                    }
                    else if (contents[i].IndexOf("[BWImageSrc]") != -1)
                    {
                        contents[i] = RemoveStrongTag(contents[i]);
                        string[] stringSeperatorImg = new string[] { "[BWImageSrc]" };
                        string[] arrContents = contents[i].Split(stringSeperatorImg, StringSplitOptions.None);
                        setMargin = true;
                        for (int j = 0; j < arrContents.Length; j++)
                        {
                            if (arrContents[j].IndexOf("bwimgsep:") == -1)                     //or if(j % 2 == 0)---->AddParaToList else AddImgToList
                            {
                                AddParaToList(objHtmlContent, arrContents[j].Trim(), setMargin);
                            }
                            else
                            {
                                AddImgToList(objHtmlContent, arrContents[j].Trim(), setMargin);
                            }
                        }
                    }
                    else if (contents[i].IndexOf("[BWHeading]") != -1)
                    {
                        string[] stringSeperatorH2 = new string[] { "[BWHeading]" };
                        string[] arrContents = contents[i].Split(stringSeperatorH2, StringSplitOptions.None);
                        setMargin = true;
                        for (int k = 0; k < arrContents.Length; k++)
                        {
                            AddH2ToList(objHtmlContent, arrContents[k].Trim(), setMargin);
                        }
                    }
                    else if (contents[i].IndexOf("[BWVideoSrc]") != -1)
                    {
                        contents[i] = RemoveStrongTag(contents[i]);
                        string[] stringSeperatorImg = new string[] { "[BWVideoSrc]" };
                        string[] arrContents = contents[i].Split(stringSeperatorImg, StringSplitOptions.None);
                        setMargin = true;
                        for (int j = 0; j < arrContents.Length; j++)
                        {
                            if (arrContents[j].IndexOf("bwvideosep:") == -1)                     //or if(j % 2 == 0)---->AddParaToList else AddImgToList
                            {
                                AddParaToList(objHtmlContent, arrContents[j].Trim(), setMargin);
                            }
                            else
                            {
                                AddVideoToList(objHtmlContent, arrContents[j].Trim(), setMargin);
                            }
                        }
                    }
                    else
                    {
                        contents[i] = RemoveStrongTag(contents[i]);
                        AddParaToList(objHtmlContent, contents[i].Trim(), true);
                    }
                }                
            }
            catch (Exception ex)
            {
                //ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.SanitizeHtmlContent.GetFormattedContent");
                //objErr.SendMail();
                return objHtmlContent;
            }

            return objHtmlContent;
        }
        

        /// <summary>
        /// Function to remove the html content from the input html
        /// </summary>
        /// <param name="inputHtml">Content need to be sanitized.</param>
        /// <returns></returns>
        private static string SanitizeContent(string inputHtml)
        {
            string content = inputHtml;

            foreach (KeyValuePair<string, string> pattern in patternList)
                content = Regex.Replace(content, pattern.Key, pattern.Value);

            return content;
        
        }


        /// <summary>
        /// Add paragraph to HtmlItems List of Text type
        /// </summary>
        /// <param name="objHtmlContent"></param>
        /// <param name="paraText"></param>
        /// <param name="setPMargin"></param>
        private static void AddParaToList(HtmlContent objHtmlContent, string paraText, bool setPMargin)
        {
            if (paraText != "")
            {
                HtmlItem objHTMLItem = new HtmlItem();
                objHTMLItem.Content = paraText;
                objHTMLItem.Type = "Text";
                objHTMLItem.SetMargin = setPMargin;
                objHtmlContent.HtmlItems.Add(objHTMLItem);
                //setMargin = false;
            }
        }


        /// <summary>
        /// Add image urls to HtmlItems List of Image type
        /// </summary>
        /// <param name="objHtmlContent"></param>
        /// <param name="imgUrl"></param>
        /// <param name="setIMargin"></param>
        private static void AddImgToList(HtmlContent objHtmlContent, string imgUrl, bool setIMargin)
        {
            string[] stringSeperatorImg = new string[] { "bwimgsep:" };
            string[] arrImgUrl = imgUrl.Split(stringSeperatorImg, StringSplitOptions.None);
            for (int i = 0; i < arrImgUrl.Length; i++)
            {
                if (arrImgUrl[i] != "")
                {
                    HtmlItem objHTMLItem = new HtmlItem();
                    objHTMLItem.Content = arrImgUrl[i];
                    objHTMLItem.Type = "Image";
                    objHTMLItem.SetMargin = setIMargin;
                    objHtmlContent.HtmlItems.Add(objHTMLItem);
                }
            }
            //setMargin = false;
        }

        
        /// <summary>
        /// Add image urls to HtmlItems List of Image type
        /// </summary>
        /// <param name="objHtmlContent"></param>
        /// <param name="imgUrl"></param>
        /// <param name="setIMargin"></param>
        private static void AddVideoToList(HtmlContent objHtmlContent, string imgUrl, bool setIMargin)
        {
            string[] stringSeperatorImg = new string[] { "bwvideosep:" };
            string[] arrImgUrl = imgUrl.Split(stringSeperatorImg, StringSplitOptions.None);
            for (int i = 0; i < arrImgUrl.Length; i++)
            {
                if (arrImgUrl[i] != "")
                {
                    HtmlItem objHTMLItem = new HtmlItem();
                    objHTMLItem.Content = arrImgUrl[i];
                    objHTMLItem.Type = "Video";
                    objHTMLItem.SetMargin = setIMargin;
                    objHtmlContent.HtmlItems.Add(objHTMLItem);
                }
            }
            //setMargin = false;
        }


        /// <summary>
        /// Add paragraph to HtmlItems List of bold type
        /// </summary>
        /// <param name="objHtmlContent"></param>
        /// <param name="headingText"></param>
        /// <param name="setHMargin"></param>
        private static void AddHeadingToList(HtmlContent objHtmlContent, string headingText, bool setHMargin)
        {
            HtmlItem objHTMLItem = new HtmlItem();
            objHTMLItem.Content = headingText;
            objHTMLItem.Type = "Bold";
            objHTMLItem.SetMargin = setHMargin;
            objHtmlContent.HtmlItems.Add(objHTMLItem);
        }


        /// <summary>
        /// Add paragraph to HtmlItems List of Heading type
        /// </summary>
        /// <param name="objHtmlContent"></param>
        /// <param name="h2Text"></param>
        /// <param name="setH2Margin"></param>
        private static void AddH2ToList(HtmlContent objHtmlContent, string h2Text, bool setH2Margin)
        {
            if (h2Text != "")
            {
                HtmlItem objHTMLItem = new HtmlItem();
                objHTMLItem.Content = h2Text;
                objHTMLItem.Type = "Heading";
                objHTMLItem.SetMargin = setH2Margin;
                objHtmlContent.HtmlItems.Add(objHTMLItem);
            }
        }


        /// <summary>
        /// Add paragraph to HtmlList list of List type
        /// </summary>
        /// <param name="objHtmlContent"></param>
        /// <param name="objListText"></param>
        /// <param name="setLMargin"></param>
        private static void AddLiTextToList(HtmlContent objHtmlContent, List<string> objListText, bool setLMargin)
        {
            HtmlItem objHTMLItem = new HtmlItem();
            objHTMLItem.ContentList = objListText;
            objHTMLItem.Type = "List";
            objHTMLItem.SetMargin = setLMargin;
            objHtmlContent.HtmlItems.Add(objHTMLItem);
        }


        /// <summary>
        /// Remove strong tags
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string RemoveStrongTag(string str)
        {
            string pattern = @"<strong.*?>(.*?)|</strong>";
            string patternbold = @"<b.*?>(.*?)|</b>";
            str = Regex.Replace(str, pattern, "$1");
            str = Regex.Replace(str, patternbold, "$1");
            return str;
        }


        /// <summary>
        /// Detect heading
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        private static bool HeadingDetected(string desc)
        {
            desc = desc.Replace("</strong>", "<strong>");
            desc = desc.Replace("<b>", "<strong>");
            desc = desc.Replace("</b>", "<strong>");
            string[] stringSeperatorStrong = new string[] { "<strong>" };
            string[] arrHeading = desc.Split(stringSeperatorStrong, StringSplitOptions.None);

            if (arrHeading.Length == 3 && arrHeading[0].Trim() == "" && arrHeading[2].Trim() == "" && arrHeading[1].Trim() != "")
            {
                return true;
            }
            else
                return false;
        }


    }   // class
}   // namespace
