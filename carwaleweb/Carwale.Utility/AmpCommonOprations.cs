using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Carwale.Utility
{
    public static class AmpCommonOprations
    {
        public static Regex YoutubeVideoRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);

        public static void UpdateToAmpTags(HtmlDocument doc, string htmlTag, string ampTags, Dictionary<string, string> addAttributes)
        {
            var tagList = doc.DocumentNode.Descendants(htmlTag);

            if (!tagList.Any()) return;

            if (!HtmlNode.ElementsFlags.ContainsKey(ampTags))
                HtmlNode.ElementsFlags.Add(ampTags, HtmlElementFlag.Closed);

            foreach (var tag in tagList)
            {
                var original = tag.OuterHtml;
                if (tag.Name.Equals("iframe"))
                {
                    tag.Attributes["src"].Value = tag.Attributes["src"].Value.Replace("http:", "https:");
                    tag.AddAttribute("sandbox", "allow-same-origin allow-scripts allow-popups allow-forms");
                }

                if (tag.Name == "img" && tag.Attributes["src"] != null && tag.Attributes["data-original"] != null && !string.IsNullOrWhiteSpace(tag.Attributes["data-original"].Value))
                    tag.Attributes["src"].Value = tag.Attributes["data-original"].Value;
                tag.Name = ampTags;

                foreach (var attribute in addAttributes)
                {
                    tag.AddAttribute(attribute.Key, attribute.Value);
                }
              
                if (ampTags.Equals("amp-youtube"))
                    ReplaceObjectWithAmpYoutube(tag);
            }
        }

        public static HtmlDocument GetHtmlDocument(string htmlContent)
        {
            var doc = new HtmlDocument
            {
                OptionOutputAsXml = true,
                OptionDefaultStreamEncoding = Encoding.UTF8
            };
            doc.LoadHtml(htmlContent);
            return doc;
        }

        private static void AddAttribute(this HtmlNode htmlNode, string attributeName, string attributeValue)
        {
            htmlNode.RemoveAttribute(attributeName);

            htmlNode.Attributes.Add(attributeName, attributeValue);
        }

        private static void RemoveAttribute(this HtmlNode htmlNode, string attributeName)
        {
            if (htmlNode.Attributes.Contains(attributeName))
                htmlNode.Attributes.Remove(attributeName);
        }

        private static void ReplaceObjectWithAmpYoutube(HtmlNode tag)
        {
            var youtubeUrl = tag.ChildNodes["param"].Attributes["value"].Value;

            if (!(YoutubeVideoRegex.Match(youtubeUrl).Groups.Count >= 1))
                return;

            var youtubeId = YoutubeVideoRegex.Match(youtubeUrl).Groups[1].ToString();

            tag.AddAttribute("data-videoid", youtubeId);
            tag.RemoveAllChildren();
        }

        public static DateTime AmpStartDate = new DateTime(month: 1, day: 1, year: 2016);
    }
}
