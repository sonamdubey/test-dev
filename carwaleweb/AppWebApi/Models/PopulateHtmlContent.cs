using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Web;
using System.Text.RegularExpressions;
using AppWebApi.Models;
using HtmlAgilityPack;

namespace App.PopulateDataContracts
{
    public class PopulateHtmlContent
    {
        private bool setMargin = true;
        private bool isTipsAdvicePage=false;
        public bool IsTipsAdvicePage
        {
            get { return isTipsAdvicePage; }
            set { isTipsAdvicePage = value; }
        }
        /*
         Author:Rakesh Yadav 
         Date Created: 1 Oct 2013
         Desc : Convert Html Content into paragraphs and specify paragraph content type
         */

        public PopulateHtmlContent(string content, HtmlContent objHtmlContent, bool isTipsAdvicePage = false)
        {
            IsTipsAdvicePage = isTipsAdvicePage;
            content = ReplaceContent(content);

            string[] stringSeperator = new string[] { "<p>"};
            string[] contents = content.Split(stringSeperator, StringSplitOptions.None);
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
                else if (contents[i].IndexOf("[CWImageSrc]") != -1)
                {
                    contents[i] = RemoveStrongTag(contents[i]);
                    string[] stringSeperatorImg = new string[] { "[CWImageSrc]" };
                    string[] arrContents = contents[i].Split(stringSeperatorImg, StringSplitOptions.None);
                    setMargin = true;
                    for (int j = 0; j < arrContents.Length; j++)
                    {
                        if (arrContents[j].IndexOf("cwimgsep:") == -1)                     //or if(j % 2 == 0)---->AddParaToList else AddImgToList
                        {
                            AddParaToList(objHtmlContent, arrContents[j].Trim(), setMargin);
                        }
                        else
                        {
                            string src = GetImageSource(arrContents[j]);
                            AddImgToList(objHtmlContent, src.Trim(), setMargin);
                        }
                    }
                }
                else if (contents[i].IndexOf("[CWHeading]") != -1)
                {
                    string[] stringSeperatorH2 = new string[] { "[CWHeading]" };
                    string[] arrContents = contents[i].Split(stringSeperatorH2, StringSplitOptions.None);
                    setMargin = true;
                    for (int k = 0; k < arrContents.Length; k++)
                    {
                        AddH2ToList(objHtmlContent, arrContents[k].Trim(), setMargin);
                    }
                }
                else if (contents[i].IndexOf("[CWVideoSrc]") != -1)
                {
                    contents[i] = RemoveStrongTag(contents[i]);
                    string[] stringSeperatorImg = new string[] { "[CWVideoSrc]" };
                    string[] arrContents = contents[i].Split(stringSeperatorImg, StringSplitOptions.None);
                    setMargin = true;
                    for (int j = 0; j < arrContents.Length; j++)
                    {
                        if (arrContents[j].IndexOf("cwvideosep:") == -1)                     //or if(j % 2 == 0)---->AddParaToList else AddImgToList
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

        /*
         Author:Rakesh Yadav 
         Date Created: 1 Oct 2013
         Desc: converts the content in in paragraphs and remove unwanted contents
         */

        private string ReplaceContent(string content)
        {
            Dictionary<string, string> patternList = new Dictionary<string, string>();
            patternList.Add("<br>", "<p>");
            patternList.Add("<br />", "<p>");
            patternList.Add("<div.*?>(.*?)</div>", "<p>$1<p>");
            patternList.Add("<div.*?>(.*?)|</div>", "<p>$1<p>");
            patternList.Add("<ul.*?>(.*?)</ul>", "<p>$1<p>");
            patternList.Add("<ul.*?>(.*?)|</ul>", "<p>$1<p>");
            patternList.Add("</li>", "<li>");
            patternList.Add("<p.*?>(.*?)", "<p>$1");
            patternList.Add("<h2.*?>(.*?)</h2>", "<p>[CWHeading]$1[CWHeading]<p>");
            patternList.Add("<h3.*?>(.*?)</h3>", "<p>[CWHeading]$1[CWHeading]<p>");
            patternList.Add("<h1.*?>(.*?)</h1>", "<p>[CWHeading]$1[CWHeading]<p>");
            patternList.Add("<P>", "<p>");
            patternList.Add("</P>", "");
            patternList.Add("</p>", "");
            patternList.Add("<!--.*?if.*?>(.*?)|<.*?endif.*?>", "");//ask
            patternList.Add("<a.*?>(.*?)</a>", "$1 ");
            patternList.Add("<a.*?>(.*?)|</a>", "$1 ");
            patternList.Add(@"(<img.*?>)", "[CWImageSrc]cwimgsep:$1[CWImageSrc]");
            patternList.Add("<em.*?>(.*?)</em>", "$1");
            patternList.Add("<span.*?>(.*?)</span>", "$1");
            patternList.Add("<span.*?>(.*?)|</span>", "$1");
            patternList.Add(@"<object.*?>(.*?)</object>", "");
            patternList.Add(@"<object.*?>(.*?)|</object>", "");
            patternList.Add(@"<embed.*?>(.*?)|</embed>", "");
            patternList.Add(@"<script.*?>(.*?)|</script>", "");
            patternList.Add(@"<center.*?>(.*?)|</center>", "");
            if (!IsTipsAdvicePage)
            {
                patternList.Add(@"<table.*?>(\n|.)*</table>", "");
            }
            patternList.Add(@"<!--(\n|.)*-->", "");
            patternList.Add("&nbsp;|\t|\r|\n", "");
            patternList.Add("&rsquo;", "'");
            patternList.Add("&lsquo;", "'");
            patternList.Add("&ndash;", "-");
            patternList.Add("&lt;", "<");
            patternList.Add("&gt;", ">");
            patternList.Add("&amp;", "&");
            patternList.Add("&pound;", "£");
            patternList.Add("&euro;", "€");
            patternList.Add("&aacute;", "á");
            patternList.Add("&uuml;", "ü");
            patternList.Add("&ouml;", "ö");
            patternList.Add("&eacute;", "é");
            patternList.Add("&Prime;", "″");
            patternList.Add("&lrm;", " ");
            patternList.Add("&yacute;", "ý");
            patternList.Add("&ldquo;", "“");
            patternList.Add("&rdquo;", "”");
            patternList.Add("&bull;", "-");//bullet is replaced by -
            patternList.Add(@"<iframe\s+[^>]*?src\s*=\s*[""']([^""']+)[""']\s*.*?/*></iframe>", "[CWVideoSrc]cwvideosep:$1[CWVideoSrc]");//ask
            patternList.Add("&#x20B9;", "₹");

            foreach (KeyValuePair<string, string> pattern in patternList)
                content = Regex.Replace(content, pattern.Key, pattern.Value);

            return content;
        }


        /*
         Author:Rakesh Yadav 
         Date Created: 1 Oct 2013
         Desc : Add paragraph to HtmlItems List of Text type
         */
        private void AddParaToList(HtmlContent objHtmlContent, string paraText, bool setPMargin)
        {
            if (paraText != "")
            {
                HTMLItem objHTMLItem = new HTMLItem();
                objHTMLItem.Content = paraText;
                objHTMLItem.Type = "Text";
                objHTMLItem.SetMargin = setPMargin;
                objHtmlContent.HtmlItems.Add(objHTMLItem);
                setMargin = false;
            }
        }

        /*
         Author:Rakesh Yadav 
         Date Created: 1 Oct 2013
         Desc : Add image urls to HtmlItems List of Image type
         */
        private void AddImgToList(HtmlContent objHtmlContent, string imgUrl, bool setIMargin)
        {
            string[] stringSeperatorImg = new string[] { "cwimgsep:" };
            string[] arrImgUrl = imgUrl.Split(stringSeperatorImg, StringSplitOptions.None);
            for (int i = 0; i < arrImgUrl.Length; i++)
            {
                if (arrImgUrl[i] != "")
                {
                    HTMLItem objHTMLItem = new HTMLItem();
                    objHTMLItem.Content = arrImgUrl[i];
                    objHTMLItem.Type = "Image";
                    objHTMLItem.SetMargin = setIMargin;
                    objHtmlContent.HtmlItems.Add(objHTMLItem);
                }
            }
            setMargin = false;
        }

        /*
         Author:Rakesh Yadav 
         Date Created: 1 Oct 2013
         Desc : Add image urls to HtmlItems List of Image type
         */
        private void AddVideoToList(HtmlContent objHtmlContent, string imgUrl, bool setIMargin)
        {
            string[] stringSeperatorImg = new string[] { "cwvideosep:" };
            string[] arrImgUrl = imgUrl.Split(stringSeperatorImg, StringSplitOptions.None);
            for (int i = 0; i < arrImgUrl.Length; i++)
            {
                if (arrImgUrl[i] != "")
                {
                    HTMLItem objHTMLItem = new HTMLItem();
                    objHTMLItem.Content = arrImgUrl[i];
                    objHTMLItem.Type = "Video";
                    objHTMLItem.SetMargin = setIMargin;
                    objHtmlContent.HtmlItems.Add(objHTMLItem);
                }
            }
            setMargin = false;
        }

        /*
         Author:Rakesh Yadav 
         Date Created: 1 Oct 2013
         Desc :Add paragraph to HtmlItems List of bold type
         */
        private void AddHeadingToList(HtmlContent objHtmlContent, string headingText, bool setHMargin)
        {
            HTMLItem objHTMLItem = new HTMLItem();
            objHTMLItem.Content = headingText;
            objHTMLItem.Type = "Bold";
            objHTMLItem.SetMargin = setHMargin;
            objHtmlContent.HtmlItems.Add(objHTMLItem);
        }

        /*
         Author:Rakesh Yadav 
         Date Created: 1 Oct 2013
         Desc : Add paragraph to HtmlItems List of Heading type
         */
        private void AddH2ToList(HtmlContent objHtmlContent, string h2Text, bool setH2Margin)
        {
            if (h2Text != "")
            {
                HTMLItem objHTMLItem = new HTMLItem();
                objHTMLItem.Content = h2Text;
                objHTMLItem.Type = "Heading";
                objHTMLItem.SetMargin = setH2Margin;
                objHtmlContent.HtmlItems.Add(objHTMLItem);
            }
        }

        /*
         Author:Rakesh Yadav 
         Date Created: 1 Oct 2013
         Desc : Add paragraph to HtmlList list of List type
         */
        private void AddLiTextToList(HtmlContent objHtmlContent, List<string> objListText, bool setLMargin)
        {
            HTMLItem objHTMLItem = new HTMLItem();
            objHTMLItem.ContentList = objListText;
            objHTMLItem.Type = "List";
            objHTMLItem.SetMargin = setLMargin;
            objHtmlContent.HtmlItems.Add(objHTMLItem);
        }

        /*
         Author:Rakesh Yadav 
         Date Created: 1 Oct 2013
         Desc : Remove strong tags
         */
        private string RemoveStrongTag(string str)
        {
            string pattern = @"<strong.*?>(.*?)|</strong>";
            string patternbold = @"<b.*?>(.*?)|</b>";
            str = Regex.Replace(str, pattern, "$1");
            str = Regex.Replace(str, patternbold, "$1");
            return str;
        }

        /*
         Author:Rakesh Yadav 
         Date Created: 1 Oct 2013
         Desc :detect heading
         */
        private bool HeadingDetected(string desc)
        {
            desc = desc.Replace("</strong>", "<strong>");
            desc = desc.Replace("<b>", "<strong>");
            desc = desc.Replace("</b>", "<strong>");
            string[] stringSeperatorStrong = new string[] { "<strong>" };
            string[] arrHeading = desc.Split(stringSeperatorStrong, StringSplitOptions.None);
            
            if (arrHeading.Length==3 && arrHeading[0].Trim() == "" && arrHeading[2].Trim() == "" && arrHeading[1].Trim() != "")
            {
                return true;
            }
            else
                return false;
        }

        private string GetImageSource(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNodeCollection images = new HtmlNodeCollection(doc.DocumentNode.ParentNode);
            images = doc.DocumentNode.SelectNodes("//img");
            string src = string.Empty;

            if (images.Count > 0)
            {
                if (images[0].Attributes["data-original"] != null)
                    src = images[0].Attributes[@"data-original"].Value;
                else if (images[0].Attributes["src"] != null)
                    src = images[0].Attributes[@"src"].Value;
            }

            return src;
        }
    }
}
