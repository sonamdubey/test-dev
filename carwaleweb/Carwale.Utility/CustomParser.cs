using System;
using System.Globalization;

namespace Carwale.Utility
{
    public static class CustomParser
    {
        public static string parseStringObject<T>(T value)
        {
            return Convert.ToString(value);
        }

        public static int parseIntObject<T>(T value)
        {
            int outValue;
            int.TryParse(Convert.ToString(value), out outValue);
            return outValue;
        }

        public static uint parseUIntObject<T>(T value)
        {
            uint outValue;
            uint.TryParse(Convert.ToString(value), out outValue);
            return outValue;
        }

        public static short parseShortObject<T>(T value)
        {
            short outValue;
            short.TryParse(Convert.ToString(value), out outValue);
            return outValue;
        }

        public static bool parseBoolObject<T>(T value)
        {
            if (Convert.ToString(value) == "1")
                return true;
            else if (Convert.ToString(value) == "0")
                return false;
            bool outValue;
            Boolean.TryParse(Convert.ToString(value), out outValue);
            return outValue;
        }

        public static double parseDoubleObject<T>(T value)
        {
            double outValue;
            double.TryParse(Convert.ToString(value), out outValue);
            return outValue;
        }
        public static DateTime parseDateObject<T>(T value)
        {
            DateTime outValue;
            DateTime.TryParse(Convert.ToString(value), out outValue);
            return outValue;
        }

        public static byte parseByteObject<T>(T value)
        {
            byte outValue;
            var styles = NumberStyles.Integer;
            byte.TryParse(Convert.ToString(value), styles, null as IFormatProvider, out outValue);
            return outValue;
        }
    }
}
