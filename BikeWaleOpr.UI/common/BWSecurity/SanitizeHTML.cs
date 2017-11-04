﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Added By : Ashish G. Kamble on 4 Apr 2013
/// Summary : class to remove invalid html tags and sanitize the content.
/// </summary>
public class SanitizeHTML
{	
    private static readonly Regex HtmlTagExpression = new Regex(@"(?'tag_start'</?)(?'tag'\w+)((\s+(?'attr'(?'attr_name'\w+)(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+)))?)+\s*|\s*)(?'tag_end'/?>)", 
                                                                            RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex WhiteSpaceBetweenHtmlTagsExpression = new Regex(@">(/w+)<", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex HtmlLineBreakExpression = new Regex(@"<br(/s+)/>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Dictionary<string, List<string>> ValidHtmlTags = new Dictionary<string, List<string>> {
        { "p", new List<string> { "class" } },
        { "br", new List<string>() }, 
        { "strong", new List<string>() }, 
        { "b", new List<string>() }, 
        { "em", new List<string>() }, 
        { "i", new List<string>() }, 
        { "u", new List<string>() }, 
        { "strike", new List<string>() }, 
        { "ol", new List<string>() }, 
        { "ul", new List<string>() }, 
        { "li", new List<string>() }, 
        { "a", new List<string> { "href" } }, 
        { "img", new List<string> { "src", "height", "width", "alt" } },
        { "q", new List<string> { "cite" } }, 
        { "cite", new List<string>() }, 
        { "abbr", new List<string>() }, 
        { "acronym", new List<string>() }, 
        { "del", new List<string>() }, 
        { "ins", new List<string>() },
        { "hr", new List<string>() }
    };

    /// <summary>
    /// Toes the safe HTML.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    public static string ToSafeHtml(string text)
    {
        return RemoveInvalidHtmlTags(text);
    }

    /// <summary>
    /// Removes the invalid HTML tags.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    public static string RemoveInvalidHtmlTags(string text)
    {
        return HtmlTagExpression.Replace(text, new MatchEvaluator((Match m) =>
        {
            if (!ValidHtmlTags.ContainsKey(m.Groups["tag"].Value))
                return String.Empty;

            StringBuilder generatedTag = new StringBuilder(m.Length);

            System.Text.RegularExpressions.Group tagStart = m.Groups["tag_start"];
            System.Text.RegularExpressions.Group tagEnd = m.Groups["tag_end"];
            System.Text.RegularExpressions.Group tag = m.Groups["tag"];
            System.Text.RegularExpressions.Group tagAttributes = m.Groups["attr"];

            generatedTag.Append(tagStart.Success ? tagStart.Value : "<");
            generatedTag.Append(tag.Value);

            foreach (Capture attr in tagAttributes.Captures)
            {
                int indexOfEquals = attr.Value.IndexOf('=');

                // don't proceed any futurer if there is no equal sign or just an equal sign
                if (indexOfEquals < 1)
                    continue;

                string attrName = attr.Value.Substring(0, indexOfEquals);

                // check to see if the attribute name is allowed and write attribute if it is
                if (ValidHtmlTags[tag.Value].Contains(attrName))
                {
                    generatedTag.Append(' ');
                    generatedTag.Append(attr.Value);
                }
            }

            // add nofollow to all hyperlinks
            if (tagStart.Success && tagStart.Value == "<" && tag.Value.Equals("a", StringComparison.OrdinalIgnoreCase))
                generatedTag.Append(" rel=\"nofollow\"");

            generatedTag.Append(tagEnd.Success ? tagEnd.Value : ">");

            return generatedTag.ToString();
        }));
    }
}