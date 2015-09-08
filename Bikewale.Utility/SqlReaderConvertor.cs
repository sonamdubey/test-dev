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
            bool? retVal = null;
            if (!Convert.IsDBNull(reader))
            {
                return Convert.ToBoolean(reader);
            }
            else
            {
                return retVal;
            }
        }

        public static UInt16? ToNullableUInt16(object reader)
        {
            UInt16? retVal = null;
            if (!Convert.IsDBNull(reader))
            {
                return Convert.ToUInt16(reader);
            }
            else
            {
                return retVal;
            }
        }

        public static UInt32? ToNullableUInt32(object reader)
        {
            UInt32? retVal = null;
            if (!Convert.IsDBNull(reader))
            {
                return Convert.ToUInt32(reader);
            }
            else
            {
                return retVal;
            }
        }

        public static UInt64? ToNullableUInt64(object reader)
        {
            UInt64? retVal = null;
            if (!Convert.IsDBNull(reader))
            {
                return Convert.ToUInt64(reader);
            }
            else
            {
                return retVal;
            }
        }

        public static float? ToNullableFloat(object reader)
        {
            float? retVal = null;
            if (!Convert.IsDBNull(reader))
            {
                return Convert.ToSingle(reader);
            }
            else
            {
                return retVal;
            }
        }


        public static Int64 ToNullableInt64(object reader)
        {
            Int64 retVal = 0;
            if (!Convert.IsDBNull(reader))
            {
                return Convert.ToInt64(reader);
            }
            else
            {
                return retVal;
            }
        }
    }
}
