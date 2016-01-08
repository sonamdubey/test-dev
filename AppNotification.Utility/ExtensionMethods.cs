using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AppNotification.Utility
{
    public static class ExtensionMethods
    {
        public static T ConvertByteArrayToObject<T>(this byte[] byteArray)
        {
            T t = default(T);

            try
            {
                // convert byte array to memory stream
                using (MemoryStream memstm = new MemoryStream(byteArray))
                {
                    // create new BinaryFormatter
                    BinaryFormatter bf = new BinaryFormatter();
                    // bf.Binder = new Version1ToVersion2DeserializationBinder();
                    bf.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                    // set memory stream position to starting point
                    memstm.Position = 0;
                    // Deserializes a stream into an object graph and return as a object.

                    t = (T)bf.Deserialize(memstm);
                }
            }
            catch (Exception ex)
            {
                //var objErr = new ExceptionHandler(ex, "");
                //objErr.LogException();  
                string a = "";
            }

            return t;
        }

        public static byte[] ConvertObjectToByteArray<T>(this T t)
        {
            byte[] byteArray = null;

            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;

                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, t);
                    byteArray = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                //var objErr = new ExceptionHandler(ex, "");
                //objErr.LogException();
            }

            return byteArray;
        }

        /// <summary>
        /// Created BY : Supriya on 10/6/2014
        /// Desc : Function to convert DateTime to days or hours or minutes or seconds or years according to timespan difference
        /// </summary>
        /// <returns></returns>
        public static string ConvertDateToDays(this DateTime _displayDate)
        {
            string retVal = "";
            TimeSpan tsDiff = DateTime.Now.Subtract(_displayDate);

            if (tsDiff.Days > 0)
            {
                retVal = tsDiff.Days.ToString();
                if (retVal == "1")
                    retVal += " day ago";
                else
                    retVal += " days ago";
            }
            else if (tsDiff.Hours > 0)
            {
                retVal = tsDiff.Hours.ToString();

                if (retVal == "1")
                    retVal += " hour ago";
                else
                    retVal += " hours ago";
            }
            else if (tsDiff.Minutes > 0)
            {
                retVal = tsDiff.Minutes.ToString();

                if (retVal == "1")
                    retVal += " minute ago";
                else
                    retVal += " minutes ago";
            }
            else if (tsDiff.Seconds > 0)
            {
                retVal = tsDiff.Seconds.ToString();

                if (retVal == "1")
                    retVal += " second ago";
                else
                    retVal += " seconds ago";
            }

            if (tsDiff.Days > 360)
            {
                retVal = Convert.ToString(tsDiff.Days / 360);

                if (retVal == "1")
                    retVal += " year ago";
                else
                    retVal += " years ago";
            }
            else if (tsDiff.Days > 30)
            {
                retVal = Convert.ToString(tsDiff.Days / 30);

                if (retVal == "1")
                    retVal += " month ago";
                else
                    retVal += " months ago";
            }

            return retVal;
        }

        public static List<T> ConvertStringToList<T>(string ids, char delimiter)
        {
            var idList = new List<T>();

            try
            {
                if (!String.IsNullOrEmpty(ids) && delimiter != null)
                {
                    var _arrId = ids.Split(delimiter);

                    if (typeof(T) == typeof(int))
                    {
                        foreach (var _id in _arrId)
                        {
                            int id;
                            if (int.TryParse(_id, out id))
                                if (idList.IndexOf((T)Convert.ChangeType(id, typeof(T))) == -1)
                                    idList.Add((T)Convert.ChangeType(id, typeof(T)));
                        }
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        foreach (var _id in _arrId)
                        {
                            if (idList.IndexOf((T)Convert.ChangeType(_id, typeof(T))) == -1)
                                idList.Add((T)Convert.ChangeType(_id, typeof(T)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //var objErr = new ExceptionHandler(ex, "");
                //objErr.LogException();                
            }

            return idList;
        }
    }
}
