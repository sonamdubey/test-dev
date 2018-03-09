using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringHtmlHelpers
    {
        /// <summary>
        /// Truncates a string containing HTML to a number of text characters, keeping whole words.
        /// The result contains HTML and any tags left open are closed.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string InsertBetweenHtml(this string html, int maxCharacters, string insertHtml)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            // find the spot to truncate
            // count the text characters and ignore tags
            var textCount = 0;
            var charCount = 0;
            var ignore = false;
            foreach (char c in html)
            {
                charCount++;
                if (c == '<')
                    ignore = true;
                else if (!ignore)
                    textCount++;

                if (c == '>')
                    ignore = false;

                // stop once we hit the limit
                if (textCount >= maxCharacters)
                    break;
            }

            // Truncate the html and keep whole words only
            var trunc = new StringBuilder(html.Substring(0, maxCharacters));

            // keep track of open tags and close any tags left open
            var tags = new Stack<string>();
            var matches = Regex.Matches(trunc.ToString(),
                @"<((?<tag>[^\s/>]+)|/(?<closeTag>[^\s>]+)).*?(?<selfClose>/)?\s*>",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var tag = match.Groups["tag"].Value;
                    var closeTag = match.Groups["closeTag"].Value;

                    // push to stack if open tag and ignore it if it is self-closing, i.e. <br />
                    if (!string.IsNullOrEmpty(tag) && string.IsNullOrEmpty(match.Groups["selfClose"].Value))
                        tags.Push(tag);

                    // pop from stack if close tag
                    else if (!string.IsNullOrEmpty(closeTag))
                    {
                        // pop the tag to close it.. find the matching opening tag
                        // ignore any unclosed tags
                        while (tags.Count > 0 && tags.Pop() != closeTag)
                        { }
                    }
                }
            }

            string restOfText = html.Substring(maxCharacters);
            int closingTagIndex = 0, actualEndingIndex = 0;

            // pop the rest off the stack to close remainder of tags
            while (tags.Count > 0)
            {
                var _tag = tags.Pop();
                if (string.Equals(_tag, "div", StringComparison.CurrentCultureIgnoreCase) || string.Equals(_tag, "p", StringComparison.CurrentCultureIgnoreCase))
                {
                    string closingTag = string.Format("</{0}>", _tag);
                    closingTagIndex = restOfText.IndexOf(closingTag);
                    if (closingTagIndex != -1)
                    {
                        actualEndingIndex = closingTagIndex + closingTag.Length;
                        trunc = new StringBuilder(html.Substring(0, maxCharacters + actualEndingIndex));
                    }
                }

            }

            trunc.AppendFormat("{0}{1}", insertHtml, restOfText.Substring(actualEndingIndex));



            return trunc.ToString();
        }


        /// <summary>
        /// Truncates a string containing HTML to a number of text characters, keeping whole words.
        /// The result contains HTML and any tags left open are closed.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TruncateHtml(this string html, int maxCharacters, string trailingText)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            // find the spot to truncate
            // count the text characters and ignore tags
            var textCount = 0;
            var charCount = 0;
            var ignore = false;
            foreach (char c in html)
            {
                charCount++;
                if (c == '<')
                    ignore = true;
                else if (!ignore)
                    textCount++;

                if (c == '>')
                    ignore = false;

                // stop once we hit the limit
                if (textCount >= maxCharacters)
                    break;
            }

            // Truncate the html and keep whole words only
            var trunc = new StringBuilder(html.TruncateWords(charCount));

            // keep track of open tags and close any tags left open
            var tags = new Stack<string>();
            var matches = Regex.Matches(trunc.ToString(),
                @"<((?<tag>[^\s/>]+)|/(?<closeTag>[^\s>]+)).*?(?<selfClose>/)?\s*>",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var tag = match.Groups["tag"].Value;
                    var closeTag = match.Groups["closeTag"].Value;

                    // push to stack if open tag and ignore it if it is self-closing, i.e. <br />
                    if (!string.IsNullOrEmpty(tag) && string.IsNullOrEmpty(match.Groups["selfClose"].Value))
                        tags.Push(tag);

                    // pop from stack if close tag
                    else if (!string.IsNullOrEmpty(closeTag))
                    {
                        // pop the tag to close it.. find the matching opening tag
                        // ignore any unclosed tags
                        while (tags.Count > 0 && tags.Pop() != closeTag)
                        { }
                    }
                }
            }

            if (html.Length > charCount)
                // add the trailing text
                trunc.Append(trailingText);

            // pop the rest off the stack to close remainder of tags
            while (tags.Count > 0)
            {
                trunc.Append("</");
                trunc.Append(tags.Pop());
                trunc.Append('>');
            }

            return trunc.ToString();
        }

        /// <summary>
        /// Truncates a string containing HTML to a number of text characters, keeping whole words.
        /// The result contains HTML and any tags left open are closed.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TruncateHtml(this string html, int maxCharacters)
        {
            return html.TruncateHtml(maxCharacters, null);
        }

        /// <summary>
        /// Truncates a string containing HTML to the first occurrence of a delimiter
        /// </summary>
        /// <param name="html">The HTML string to truncate</param>
        /// <param name="delimiter">The delimiter</param>
        /// <param name="comparison">The delimiter comparison type</param>
        /// <returns></returns>
        public static string TruncateHtmlByDelimiter(this string html, string delimiter, StringComparison comparison = StringComparison.Ordinal)
        {
            var index = html.IndexOf(delimiter, comparison);
            if (index <= 0) return html;

            var r = html.Substring(0, index);
            return r.TruncateHtml(r.StripHtml().Length);
        }

        /// <summary>
        /// Strips all HTML tags from a string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string StripHtml(this string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
        }

        /// <summary>
        /// Created By : Deepak Israni
        /// Description : To remove all HTML tags from text.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtmlWithSpaces(string content)
        {
            content = Regex.Replace(content, @"<[^>]+>|&nbsp;", "").Trim();
            content = Regex.Replace(content, @"\s{2,}", " ");

            return content;
        }

        /// <summary>
        /// Truncates text to a number of characters
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <param name="trailingText"></param>
        /// <returns></returns>
        public static string HtmlTruncate(this string text, int maxCharacters)
        {
            return text.HtmlTruncate(maxCharacters, null);
        }

        /// <summary>
        /// Truncates text to a number of characters and adds trailing text, i.e. elipses, to the end
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <param name="trailingText"></param>
        /// <returns></returns>
        public static string HtmlTruncate(this string text, int maxCharacters, string trailingText)
        {
            if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
                return text;
            else
                return text.Substring(0, maxCharacters) + trailingText;
        }


        /// <summary>
        /// Truncates text and discars any partial words left at the end
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <param name="trailingText"></param>
        /// <returns></returns>
        public static string TruncateWords(this string text, int maxCharacters)
        {
            return text.TruncateWords(maxCharacters, null);
        }

        /// <summary>
        /// Truncates text and discars any partial words left at the end
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <param name="trailingText"></param>
        /// <returns></returns>
        public static string TruncateWords(this string text, int maxCharacters, string trailingText)
        {
            if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
                return text;

            // trunctate the text, then remove the partial word at the end
            return Regex.Replace(text.HtmlTruncate(maxCharacters),
                @"\s+[^\s]+$", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled) + trailingText;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 2nd Oct 2017
        /// Description : To strip html and simultaneously find the length of stripped html of source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Tuple<string, int> StripHtmlTagsWithLength(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return Tuple.Create(string.Empty, 0);
            }

            var arr = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            try
            {

                for (int i = 0; i < source.Length; i++)
                {
                    char let = source[i];
                    if (let == '<')
                    {
                        inside = true;
                        continue;
                    }
                    else if (let == '>')
                    {
                        inside = false;
                        continue;
                    }

                    if (!inside)
                    {
                        arr[arrayIndex] = let;
                        arrayIndex++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Tuple.Create(new string(arr, 0, arrayIndex), arrayIndex);
        }
        /// <summary>
        /// Created By : Sushil Kumar on 2nd Oct 2017
        /// Description : To insert html in between the source html 
        ///               If location is found i.e. index where html to be injected then 
        ///               find nearest ending or closing tag and insert html after that
        /// </summary>
        /// <param name="source"></param>
        /// <param name="inputHtml"></param>
        /// <param name="truncateAt"></param>
        /// <returns></returns>
        public static string InsertHTMLBetweenHTML(string source, string inputHtml, int truncateAt)
        {
            if (string.IsNullOrEmpty(source))
            {
                return inputHtml;
            }

            if (string.IsNullOrEmpty(inputHtml))
            {
                return source;
            }

            var arr = new char[source.Length + inputHtml.Length];
            int arrayIndex = 0;
            bool inside = false;

            try
            {

                for (int i = 0, j = 0; i < source.Length; i++)
                {
                    char let = source[i];
                    arr[j] = let;
                    if (let == '<')
                    {
                        inside = true;
                    }
                    else if (let == '>')
                    {
                        inside = false;
                    }

                    if (!inside)
                    {
                        arrayIndex++;
                        if (arrayIndex == truncateAt)
                        {
                            while (i < source.Length)
                            {
                                if (source[i] == '<')
                                {
                                    i--; j--;
                                    break;
                                }
                                else if (source[i] == '>')
                                {
                                    i++; j++;
                                    break;
                                }
                                arr[j++] = source[i++];
                            }

                            for (int k = 0; k < inputHtml.Length; k++, j++)
                            {
                                arr[j] = inputHtml[k];
                            }
                        }
                        else j++;

                    }
                    else j++;
                }

            }
            catch (Exception)
            {
                throw;
            }

            return new string(arr);
        }
        /// <summary>
        /// Created By : Ashutosh Sharma on 01 Mar 2018
        /// Description : To split source html into two html contents according to 'truncateAt' length of content inside html tags.
        /// </summary>
        /// <param name="source">Source html string.</param>
        /// <param name="truncateAt">Length of content inside html tags which will be topContent and remaining will be bottomContent.</param>
        /// <param name="topContent">Strign containing first part of split of length upto near to truncateAt index.</param>
        /// <param name="bottomContent">Strign containing remaining part of split.</param>
        public static void InsertHTMLBetweenHTMLPwa(string source, int truncateAt, out string topContent, out string bottomContent)
        {
            var arr = new char[source.Length];
            int truncateIndex = 0;
            int arrayIndex = 0, endIndex = 0;
            bool inside = false;

            try
            {
                for (int i = 0, j = 0; i < source.Length; i++)
                {
                    char let = source[i];
                    arr[j] = let;
                    endIndex = j;
                    if (let == '<')
                    {
                        inside = true;
                    }
                    else if (let == '>')
                    {
                        inside = false;
                    }

                    if (!inside)
                    {
                        arrayIndex++;
                        if (arrayIndex == truncateAt)
                        {
                            while (i < source.Length)
                            {
                                if (source[i] == '<')
                                {
                                    i--; j--;
                                    break;
                                }
                                else if (source[i] == '>')
                                {
                                    i++; j++;
                                    break;
                                }
                                arr[j++] = source[i++];
                            }
                            truncateIndex = i;
                        }
                        else j++;

                    }
                    else j++;
                }

            }
            catch (Exception)
            {
                throw;
            }
            topContent = new string(arr, 0, truncateIndex);
            bottomContent = new string(arr, truncateIndex, endIndex - truncateIndex + 1);
        }

        /// <summary>
        /// Created by : Sanskar Gupta on 19 Dec 2017
        /// Summary : Strips all the Malicious strings from the text
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string removeMaliciousCode(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script\s*>");
            while (rRemScript.IsMatch(text))
            {
                text = rRemScript.Replace(text, "");
            }
            return text;
        }
    }
}
