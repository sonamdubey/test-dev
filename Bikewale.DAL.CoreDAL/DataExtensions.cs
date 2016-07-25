using System;
using System.Data;

namespace AutoBiz.Utilities
{
    public static class DataExtensions
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">The type of the data stored in the record</typeparam>
        /// <param name="record">The record.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static T GetColumnValue<T>(this IDataRecord record, string columnName)
        {
            return GetColumnValue<T>(record, columnName, default(T));
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">The type of the data stored in the record</typeparam>
        /// <param name="record">The record.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">The value to return if the column contains a <value>DBNull.Value</value> value.</param>
        /// <returns></returns>
        public static T GetColumnValue<T>(this IDataRecord record, string columnName, T defaultValue)
        {
            object value = record[columnName];
            if (value == null || value == DBNull.Value)
            {
                return defaultValue;
            }
            else
            {
                return (T)value;
            }
        }
    } 
}