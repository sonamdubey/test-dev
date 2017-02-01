using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Bikewale.DAL.CoreDAL
{
    /// <summary>
    /// Written By : Ashish G. kamble on 23 Dec 2016
    /// Summary : Class core database related methods
    /// </summary>
    public static class DatabaseHelper
    {
        /// <summary>
        /// Function to get the connection to the readonly database
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetReadonlyConnection()
        {
            IDbConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["BikewaleReadonly"].ConnectionString);

            return conn;
        }

        /// <summary>
        /// Function to get the connection to the master database
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetMasterConnection()
        {
            IDbConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["BikewaleMaster"].ConnectionString);

            return conn;
        }
    }
}
