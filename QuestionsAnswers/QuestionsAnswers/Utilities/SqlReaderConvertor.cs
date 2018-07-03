using System;

namespace QuestionsAnswers.Utilities
{
    /// <summary>
    /// CReated by : Snehal Dange on 7th June 2018
    /// Desc : class created with all the customized filters
    /// </summary>
    class SqlReaderConvertor
    {
        public static bool? ToNullableBool(object reader)
        {
            bool? retVal = false;
            return ((DBNull.Value != reader) ? Convert.ToBoolean(reader) : retVal);
        }

        public static UInt16? ToNullableUInt16(object reader)
        {
            UInt16? retVal = 0;
            return ((DBNull.Value != reader) ? Convert.ToUInt16(reader) : retVal);
        }

        public static UInt32? ToNullableUInt32(object reader)
        {
            UInt32? retVal = 0;
            return ((DBNull.Value != reader) ? Convert.ToUInt32(reader) : retVal);
        }

        public static UInt64? ToNullableUInt64(object reader)
        {
            UInt64? retVal = 0;
            return ((DBNull.Value != reader) ? Convert.ToUInt64(reader) : retVal);
        }

        public static float? ToNullableFloat(object reader)
        {
            float? retVal = 0.0f;
            return ((DBNull.Value != reader) ? Convert.ToSingle(reader) : retVal);
        }


        public static Int64 ToInt64(object reader)
        {
            Int64 retVal = default(Int64);
            return ((DBNull.Value != reader) ? Convert.ToInt64(reader) : retVal);
        }

        public static int ToInt32(object reader)
        {
            int retVal = default(Int32);
            return ((DBNull.Value != reader) ? Convert.ToInt32(reader) : retVal);
        }

        public static float ToFloat(object reader)
        {
            float retVal = default(float);
            return ((DBNull.Value != reader) ? Convert.ToSingle(reader) : retVal);
        }

        public static UInt64 ToUInt64(object reader)
        {
            UInt64 retVal = default(UInt64);
            return ((DBNull.Value != reader) ? Convert.ToUInt64(reader) : retVal);
        }

        public static UInt32 ToUInt32(object reader)
        {
            UInt32 retVal = default(UInt32);
            return ((DBNull.Value != reader) ? Convert.ToUInt32(reader) : retVal);
        }

        public static UInt16 ToUInt16(object reader)
        {
            UInt16 retVal = default(UInt16);
            return ((DBNull.Value != reader) ? Convert.ToUInt16(reader) : retVal);
        }

        public static Int16 ToInt16(object reader)
        {
            Int16 retVal = default(Int16);
            return ((DBNull.Value != reader) ? Convert.ToInt16(reader) : retVal);
        }

        public static DateTime ToDateTime(object reader)
        {
            DateTime retVal = default(DateTime);
            return ((DBNull.Value != reader) ? Convert.ToDateTime(reader) : retVal);
        }

        public static Boolean ToBoolean(object reader)
        {
            Boolean retVal = default(Boolean);
            return ((DBNull.Value != reader) ? Convert.ToBoolean(reader) : retVal);
        }

        /// <summary>
        ///  Created By : Sushil Kumar on 26th March 2016
        ///  Description : Check for Dbnull and parse value to uint 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static uint ParseToUInt32(object reader)
        {
            uint retVal = default(uint);
            if (DBNull.Value != reader)
            {
                uint.TryParse(reader.ToString(), out retVal);
            }
            return retVal;

        }


        /// <summary>
        ///  Created By : Sushil Kumar on 26th March 2016
        ///  Description : Check for Dbnull and parse value to double
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static double ParseToDouble(object reader)
        {
            double retVal = default(double);
            if (DBNull.Value != reader)
            {
                double.TryParse(reader.ToString(), out retVal);
            }
            return retVal;

        }



    }
}
