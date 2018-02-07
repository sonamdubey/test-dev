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
                {
                    index++;
                }

                wordCount++;

                while (index < content.Length && char.IsWhiteSpace(content[index]))
                {
                    index++;
                }
            }

            return wordCount;
        }

        /// <summary>
        /// Created By : Deepak Israni on 5 Feb 2018
        /// Description : To get the estimated reading time of an article
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static uint GetContentReadingTime(uint wordcount)
        {
            uint readingtime = 0;

            if (wordcount > 0)
            {
                readingtime = (uint) Math.Ceiling(((double) wordcount) / ((double) _wordsperminute));
            }
            
            return readingtime;
        }
    }
}
