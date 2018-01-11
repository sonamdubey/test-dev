using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Utility
{
    public class CollectionHelper
    {
        public static bool IsEmpty(IEnumerable collection)
        {
            if (collection == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool IsNotEmpty(string[] arr)
        {
            if (arr != null && arr.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jan 2018
        /// Description :   Returns index of key in given dictionary of type <string,uint>
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int IndexOf<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, TKey key)
        {
            int index = 0;
            try
            {
                int size = dictionary != null ? dictionary.Count() : 0;
                if (size > 0)
                {
                    foreach (var item in dictionary)
                    {
                        if (item.Key.Equals(key))
                        {
                            break;
                        }
                        index++;
                    }
                }
                else
                {
                    index = -1;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return index;
        }

        /// <summary>
        /// Craeted by  :   Sumit Kate on 05 Jan 2018
        /// Description :   Returns Value present at index in given dictionary of type <string,uint>
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static TValue ValueAtIndex<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, int index)
        {
            int currentIndex = 0;
            TValue value = default(TValue);
            try
            {
                int size = dictionary != null ? dictionary.Count() : 0;
                if (size > 0 && index < size)
                {
                    foreach (var item in dictionary)
                    {
                        if (index == currentIndex)
                        {
                            value = item.Value;
                            break;
                        }
                        currentIndex++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return value;
        }

        /// <summary>
        /// Craeted by  :   Sumit Kate on 05 Jan 2018
        /// Description :   Try to get key and value in given dictionary of type <string,uint>
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="index"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryValueAtIndex<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, int index, out TKey key, out TValue value)
        {
            int currentIndex = 0;
            bool isFound = false;
            key = default(TKey);
            value = default(TValue);
            try
            {
                int size = dictionary.Count();

                if (index < size)
                {
                    foreach (var item in dictionary)
                    {
                        if (index == currentIndex)
                        {
                            value = item.Value;
                            key = item.Key;
                            isFound = true;
                            break;
                        }
                        currentIndex++;
                    }
                }
            }
            catch (Exception)
            {
                if (!isFound)
                {
                    key = default(TKey);
                    value = default(TValue);
                }
            }
            return isFound;
        }
    }
}
