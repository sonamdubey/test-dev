using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 6 jan 2017
    /// Summary : Class have methods for the accelerated mobile pages (AMP) conversions. Methods convert normal tags to amp tags. Sanitize all the tags
    /// </summary>
    public static class AmpOperations
    {
        public static Regex YoutubeVideoRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);

        /// <summary>
        /// Convert normal html content to amp html
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public static string ConvertToAmpContent(this string htmlString)
        {
            var ampString = htmlString;

            try
            {
                var attributes = new Dictionary<string, string>();

                attributes.Add("height", "160");
                attributes.Add("width", "278");
                attributes.Add("layout", "responsive");
                ampString = UpdateToAmpTags(ampString, "img", "amp-img", attributes);
                ampString = UpdateToAmpTags(ampString, "video", "amp-video", attributes);

                attributes.Clear();
                attributes.Add("layout", "responsive");
                ampString = UpdateToAmpTags(ampString, "iframe", "amp-iframe", attributes);
                ampString = UpdateToAmpTags(ampString, "object", "amp-youtube", attributes);

                ampString = RemoveAmpProhibitedTags(ampString);
            }
            catch (Exception err)
            {
                throw err;
            }

            return ampString;
        }

        /// <summary>
        /// Funtion to remove style tag from the given string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveStyleAttribute(string input)
        {
            return Regex.Replace(input, @"style=""[^\""]*""", "");
        }

        /// <summary>
        /// Function to remove the colgroup tag from the given string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveColgroupTags(string input)
        {
            return Regex.Replace(input, @"<colgroup(.*?)</colgroup>", "");
        }

        /// <summary>
        /// Function to remove the embed tags from the given string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveEmbed(string input)
        {
            return Regex.Replace(input, @"<embed(.*?)(\/embed>|\/>)", "");
        }

        /// <summary>
        /// Function to remove the form tag from given string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveFormTag(string input)
        {
            return Regex.Replace(input, @"<form((.|\n)*?)<\/form>", "");
        }

        /// <summary>
        /// Functio remove the tags which are not supported in the amp
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveAmpProhibitedTags(string input)
        {
            return Regex.Replace(input, @"(style=""[^\""]*"")|(<form((.|\n)*?)<\/form>)|(<colgroup(.*?)<\/colgroup>)|(<embed(.*?)(\/embed>|\/>))|(align=""(.*)""|align='(.*)')|(frameborder=""[0-9]*"")|(border=""[0-9]*"")", "");
        }

        /// <summary>
        /// Function to replace the html tags with amp tags
        /// </summary>
        /// <param name="response"></param>
        /// <param name="htmlTag"></param>
        /// <param name="ampTags"></param>
        /// <param name="addAttributes"></param>
        /// <returns></returns>
        public static string UpdateToAmpTags(string response, string htmlTag, string ampTags, Dictionary<string, string> addAttributes)
        {
            try
            {
                var doc = GetHtmlDocument(response);
                var tagList = doc.DocumentNode.Descendants(htmlTag);

                if (!tagList.Any()) return response;

                if (!HtmlNode.ElementsFlags.ContainsKey(ampTags))
                    HtmlNode.ElementsFlags.Add(ampTags, HtmlElementFlag.Closed);

                foreach (var tag in tagList)
                {
                    var original = tag.OuterHtml;
                    var replacement = tag.Clone();
                    if (tag.Name.Equals("iframe"))
                    {
                        replacement.Attributes["src"].Value = tag.Attributes["src"].Value.Replace("http:", "https:");
                        replacement.AddAttribute("sandbox", "allow-same-origin allow-scripts allow-popups allow-forms");
                    }
                    replacement.Name = ampTags;

                    foreach (var attribute in addAttributes)
                    {
                        replacement.AddAttribute(attribute.Key, attribute.Value);
                    }
                    response = ampTags.Equals("amp-youtube") ? ReplaceObjectWithAmpYoutube(response, tag, replacement) : response.Replace(original, replacement.OuterHtml);
                }
            }
            catch (Exception err)
            {
                throw err;
            }

            return response;
        }

        /// <summary>
        /// Function to add the youtube tag to the html
        /// </summary>
        /// <param name="response"></param>
        /// <param name="htmlTag"></param>
        /// <returns></returns>
        public static string ReplaceObjectTagWithAmpYoutube(string response, string htmlTag)
        {
            try
            {
                var ampTag = "amp-youtube";
                var doc = GetHtmlDocument(response);
                var tagList = doc.DocumentNode.Descendants(htmlTag);

                if (!tagList.Any()) return response;

                if (!HtmlNode.ElementsFlags.ContainsKey(ampTag))
                    HtmlNode.ElementsFlags.Add(ampTag, HtmlElementFlag.Closed);

                foreach (var tag in tagList)
                {
                    var original = tag.OuterHtml.ToString();
                    var replacement = tag.Clone();
                    replacement.Name = ampTag;
                }
            }
            catch (Exception err)
            {
                throw err;
            }

            return response;
        }

        /// <summary>
        /// Function to read the html document from given string
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        private static HtmlDocument GetHtmlDocument(string htmlContent)
        {
            var doc = new HtmlDocument
            {
                OptionOutputAsXml = true,
                OptionDefaultStreamEncoding = Encoding.UTF8
            };

            try
            {                
                doc.LoadHtml(htmlContent);
            }
            catch (Exception err)
            {
                throw err;
            }

            return doc;
        }

        /// <summary>
        /// Function to replace the existing attribute with the new attribute
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        private static void AddAttribute(this HtmlNode htmlNode, string attributeName, string attributeValue)
        {
            try
            {
                htmlNode.RemoveAttribute(attributeName);

                htmlNode.Attributes.Add(attributeName, attributeValue);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        /// <summary>
        /// Function to remove given attribute from the html
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <param name="attributeName"></param>
        private static void RemoveAttribute(this HtmlNode htmlNode, string attributeName)
        {
            try
            {
                if (htmlNode.Attributes.Contains(attributeName))
                    htmlNode.Attributes.Remove(attributeName);
            }
            catch (Exception err)
            {
                throw err;
            }
        }


        /// <summary>
        /// FUnction to replace object tag with the youtube tag
        /// </summary>
        /// <param name="response"></param>
        /// <param name="tag"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        private static string ReplaceObjectWithAmpYoutube(string response, HtmlNode tag, HtmlNode replacement)
        {
            try
            {
                var youtubeUrl = tag.ChildNodes["param"].Attributes["value"].Value;

                if (!(YoutubeVideoRegex.Match(youtubeUrl).Groups.Count >= 1))
                    return response;

                var youtubeId = YoutubeVideoRegex.Match(youtubeUrl).Groups[1].ToString();

                replacement.AddAttribute("data-videoid", youtubeId);
                replacement.RemoveAllChildren();
                var ampYoutubeRegex = @"<object ((.|\n)*?)" + youtubeId + @"((.|\n)*?)<\/object>";

                response = Regex.Replace(response, ampYoutubeRegex, replacement.OuterHtml);
            }
            catch (Exception err)
            {
                throw err;
            }

            return response;
        }

        public static DateTime AmpStartDate = new DateTime(month: 1, day: 1, year: 2016);
    }
}
