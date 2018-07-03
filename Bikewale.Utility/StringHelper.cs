using System.Text;
using System.Text.RegularExpressions;

namespace Bikewale.Utility.StringExtention
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 14th October 2015
    /// Summary : To truncate string 
    /// </summary>
    public static class StringHelper
    {
        public static string Truncate(this string str, int length)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return str.Length <= length ? str : str.Substring(0, length);
        }
        /// <summary>
        /// Created By :- Subodh Jain 03 July 2017
        /// Summary :- Use the current thread's culture info for conversion
        /// </summary>
        public static string ToProperCase(string str)
        {
            var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        public static string ToSentenceCase(string str)
        {
            // matches the first sentence of a string, as well as subsequent sentences
            var r = new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture);
            // MatchEvaluator delegate defines replacement of setence starts to uppercase
            return r.Replace(str.ToLower(), s => s.Value.ToUpper());
        }
        /// <summary>
        /// Created By : Lucky Rathore 
        /// Created on : 23 feb 2016
        /// Summary : To capitlize string
        /// </summary>
        /// <param name="str">string to be capitlize</param>
        /// <returns>capitlize string</returns>
        public static string Capitlization(string str)
        {
            var regCapitalize = Regex.Replace(str, @"\b(\w)", m => m.Value.ToUpper());
            str = Regex.Replace(regCapitalize, @"(\s(of|in|by|and)|\'[st])\b", m => m.Value.ToLower(), RegexOptions.IgnoreCase);
            return str;

        }
        // Added by Sangram Nandkhile on 24 May 2016
        // Desc: To be used instead of  " " in entire code eg. StringHelper.Space
        public static string Space = " ";

        public static string ToTitleCase(string text, char afterCharacter, char? replacedCharacter = null)
        {
            bool capitalize = true;
            string formattedString = "";

            foreach (char character in text)
            {

                if (character != afterCharacter)
                {
                    if (capitalize && char.IsLetter(character))
                        formattedString += char.ToUpper(character);
                    else
                        formattedString += character;
                }

                capitalize = false;

                if (character == afterCharacter)
                {
                    formattedString += replacedCharacter.HasValue ? replacedCharacter.Value : character;
                    capitalize = true;
                }
            }

            return formattedString;
        }

        /// <summary>
        /// Created By : Deepak Israni on 13 June 2018
        /// Description: Function to mask input string with Xs.
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <returns></returns>
        public static string MaskUserName(string inputName, int index, int maskingLength)
        {
            var aStringBuilder = new StringBuilder(inputName);
            aStringBuilder.Remove(index, maskingLength);

            int length = index + maskingLength;

            for (int i = index; i < length; i++)
            {
                aStringBuilder.Insert(i, "X");
            }

            inputName = aStringBuilder.ToString();

            return inputName;
        }
    }
}
