using System;

namespace Carwale.Utility
{
    public static class StringHtmlHelpers
    {
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
                                if (source[i] == '>' && source[i - 1] == 'p' && source[i - 2] == '/' && source[i - 3] == '<')
                                {
                                    arr[j++] = source[i++];
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
    }
}
