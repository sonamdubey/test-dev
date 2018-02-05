using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public static class ArticleHelper
    {

        const uint _wordsperminute = 150;

        /// <summary>
        /// Created By : Deepak Israni on 5th Feb 2018
        /// Description: Get the word count of any string.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static uint GetWordCount(string content)
        {
            uint wordCount = 0;
            int index = 0;

            while (index < content.Length)
            {
                while (index < content.Length && !char.IsWhiteSpace(content[index]))
                    index++;

                wordCount++;

                while (index < content.Length && char.IsWhiteSpace(content[index]))
                    index++;
            }

            return wordCount;
        }

        /// <summary>
        /// Created By : Deepak Israni
        /// Description : To remove all HTML tags from text.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            content = Regex.Replace(content, @"<[^>]+>|&nbsp;", "").Trim();
            content = Regex.Replace(content, @"\s{2,}", " ");

            return content;
        }

        public static uint GetContentReadingTime(string content)
        {
            uint readingtime = 0;
            if (content != null)
            {
                content = RemoveHtml(content);
                uint wordcount = GetWordCount(content);
                if (wordcount > 0)
                {
                    readingtime = (uint) Math.Ceiling(((double) wordcount) / ((double) _wordsperminute));
                }
            }
            
            return readingtime;
            
        }
    }
}
