using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public class SqlReaderConvertor
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


        public static Int64 ToNullableInt64(object reader)
        {
            Int64 retVal = 0;
            return ((DBNull.Value != reader) ? Convert.ToInt64(reader) : retVal);
        }
    }
}
