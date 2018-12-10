using AEPLCore.Logging;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Carwale.Utility
{
	public static class ExtensionMethods
	{
		private static Logger Logger = LoggerFactory.GetLogger();

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


			if (!String.IsNullOrEmpty(ids) && delimiter != default(char))
			{
				var _arrId = ids.Split(delimiter);

				if (typeof(T) == typeof(int))
				{
					foreach (var _id in _arrId)
					{
						int id;
						if (int.TryParse(_id, out id) && idList.IndexOf((T)Convert.ChangeType(id, typeof(T))) == -1)
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


			return idList;
		}

		public static T GetValueFromHttpHeader<T>(this HttpHeaders headers, string key)
		{
			if (headers.Contains(key))
				return (T)Convert.ChangeType(headers.GetValues(key).FirstOrDefault(), typeof(T));
			else
				return default(T);
		}

		public static string ToDelimatedString<T>(this List<T> inputList, char delimeter)
		{
			string strList = string.Empty;

			inputList.ForEach(s => strList += Convert.ToString(s) + delimeter);

			if (strList.EndsWith(delimeter.ToString()))
			{
				strList = strList.Substring(0, strList.Length - 1);
			}

			return strList;
		}

		public static long ToUnixTime(this DateTime date)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
		}

		/// <summary>
		/// Written by : Sachin Bharti on 22nd Feb 2016
		/// identify the mobile apps based on platformId 
		/// </summary>
		/// <param name="platformId"></param>
		/// <returns></returns>
		public static bool IsFromApps(this int platformId)
		{
			if (platformId == 74 || platformId == 83)//74-Andoid,83-IOS
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// checks if value is present in comma Seperated string
		/// </summary>
		/// <param name="commaSeperatedString"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool CommaSeperatedContains(string commaSeperatedString, string value)
		{
			if (RegExValidations.ValidateCommaSeperatedNumbers(commaSeperatedString))
			{
				return (Array.IndexOf(commaSeperatedString.Split(','), value) >= 0);
			}
			return false;

		}
		/// <summary>
		/// get key value pair dictionary from strings with format 
		/// eg "key1=value1;key2=value2;key3=value3;"
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static Dictionary<string, string> GetDict(string text)
		{
			if (!IsNotNullOrEmpty(text) && text.IndexOf("=") < 0)
			{
				throw new ArgumentException(text);
			}

			return text.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
					   .Select(part => part.Split('='))
					   .ToDictionary(split => split[0], split => split[1]);
		}

		public static string ConvertToAmpContent(this string htmlString)
		{
			var ampString = htmlString;
			try
			{
				var attributes = new Dictionary<string, string>();

				attributes.Add("height", "160");
				attributes.Add("width", "278");
				attributes.Add("layout", "responsive");
				var doc = AmpCommonOprations.GetHtmlDocument(ampString);
				AmpCommonOprations.UpdateToAmpTags(doc, "img", "amp-img", attributes);
				AmpCommonOprations.UpdateToAmpTags(doc, "video", "amp-video", attributes);

				attributes.Clear();
				attributes.Add("layout", "responsive");
				AmpCommonOprations.UpdateToAmpTags(doc, "iframe", "amp-iframe", attributes);
				AmpCommonOprations.UpdateToAmpTags(doc, "object", "amp-youtube", attributes);

				ampString = doc.DocumentNode.InnerHtml;

				ampString = RegexManipulator.RemoveAmpProhibitedTags(ampString);
			}
			catch (Exception)
			{
				throw;
			}

			return ampString;
		}

		public static bool IsInDescOrder(this IEnumerable<int> list)
		{
			if (list.Any())
			{
				int prevValue = int.MaxValue;
				foreach (var value in list)
				{
					if (prevValue < value)
					{
						return false;
					}
					prevValue = value;
				}
				return true;
			}
			return false;
		}

		public static Array ConvertStringToArray(this string ipStr)
		{
			Array opArray = new int[0];
			try
			{
				if (!string.IsNullOrWhiteSpace(ipStr))
				{
					opArray = ipStr.Split(',').Select(int.Parse).OrderBy(x => x).ToArray();
				}

			}
			catch
			{
				throw;
			}
			return opArray;
		}
		public static Uri GetUri(this string s)
		{
			return new UriBuilder(s).Uri;
		}
		public static bool IsAbsoluteUrl(this string url)
		{
			Uri result;
			return Uri.TryCreate(url, UriKind.Absolute, out result);
		}

		public static T ToType<T>(this object value)
		{
			T t;

			if (typeof(T).IsEnum)
			{
				t = (T)Enum.Parse(typeof(T), value.ToString());
			}
			else
			{
				t = (T)Convert.ChangeType(value, typeof(T));
			}

			return t;
		}

		public static List<T> ConvertStringToList<T>(this string ipStr)
		{
			List<T> opList = new List<T>();
			try
			{
				if (!string.IsNullOrWhiteSpace(ipStr))
				{
					opList = ipStr.Split(',').Select(x => (T)Convert.ChangeType(x, typeof(T))).ToList<T>();
				}

			}
			catch
			{
				throw;
			}
			return opList;
		}

		public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> listToCheck)
		{
			return listToCheck != null && listToCheck.Any();
		}

		public static IRuleBuilder<T, IEnumerable<TElement>> ShouldNotContainMoreThan<T, TElement>(this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder, int maxLen, string elementName = null)
		{
			return ruleBuilder.Must((rootObject, list, context) =>
			{
				context.MessageFormatter.AppendArgument("MaxLength", maxLen);
				return list.Count() <= maxLen;
			})
			.WithMessage(string.Format("{0} should not contain more then {1} {2}.", "{PropertyName}", "{MaxLength}", string.IsNullOrEmpty(elementName) ? "items" : elementName));
		}

		//This method is only for that collection where typeof(TElement) is string
		public static IRuleBuilder<T, IEnumerable<TElement>> NotContainEmptyOrNullElement<T, TElement>(this IRuleBuilder<T, IEnumerable<TElement>> ruleBuiler)
		{
			return ruleBuiler.Must(l => l.All(m => m != null && m.ToString() != string.Empty))
							.WithMessage("{PropertyName} should not contain null or empty element.");
		}

		public static bool IsValidNumberList<T>(this ICollection<T> list)
		{
			if (list == null || list.Count == 0)
			{
				throw new ArgumentException("Invalid collection");
			}
			foreach (var item in list)
			{
				if (!RegExValidations.IsPositiveNumber(item.ToString()))
				{
					return false;
				}
			}
			return true;
		}

        //this method takes htmlstring and length as argument and gives string  ending with word
        public static string GetHtmlSubString(this string htmlString, int length)
        {
            string stringWithoutTages = SanitizeHTML.RemoveAllHtmlTags(htmlString);
            if (stringWithoutTages.Length < length)
                return stringWithoutTages;
            int position = stringWithoutTages.LastIndexOf(" ", length - 1);
            if (position >= 0)
                return stringWithoutTages.Substring(0, position);
            return null;
        }

    }   // class
}   // namespace
