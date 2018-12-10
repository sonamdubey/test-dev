using Carwale.DAL.CoreDAL.MySql;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DAL
{
    public abstract class RepositoryBase
    {
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(DbConnections.Connection);
            }
        }
        internal IDbConnection ClassifiedMySqlReadConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.ClassifiedMySqlReadConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.ClassifiedMySqlReadConnection);
            }
        }
        internal IDbConnection ClassifiedMySqlMasterConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.ClassifiedMySqlMasterConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.ClassifiedMySqlMasterConnection);
            }
        }


        internal IDbConnection AdvantageMySqlReadConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.AdvantageMySqlReadConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.AdvantageMySqlReadConnection);
            }
        }
        internal IDbConnection AdvantageMySqlMasterConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.AdvantageMySqlMasterConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.AdvantageMySqlMasterConnection);
            }
        }

        internal IDbConnection NewCarMySqlReadConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.NewCarMySqlReadConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.NewCarMySqlReadConnection);
            }
        }
        internal IDbConnection NewCarMySqlMasterConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.NewCarMySqlMasterConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.NewCarMySqlMasterConnection);
            }
        }

        internal IDbConnection CarDataMySqlReadConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.CarDataMySqlReadConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.CarDataMySqlReadConnection);
            }
        }
        internal IDbConnection CarDataMySqlMasterConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.CarDataMySqlMasterConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.CarDataMySqlMasterConnection);
            }
        }

        internal IDbConnection ForumsMySqlReadConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.ForumsMySqlReadConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.ForumsMySqlReadConnection);
            }
        }
        internal IDbConnection ForumsMySqlMasterConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.ForumsMySqlMasterConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.ForumsMySqlMasterConnection);
            }
        }

        internal IDbConnection EsMySqlReadConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.EsMySqlReadConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.EsMySqlReadConnection);
            }
        }
        internal IDbConnection EsMySqlMasterConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.EsMySqlMasterConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.EsMySqlMasterConnection);
            }
        }
        internal IDbConnection EditCmsMySqlReadConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.EditCmsMySqlReadConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.EditCmsMySqlReadConnection);
            }
        }       
        internal IDbConnection EditCmsMySqlMasterConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.EditCmsMySqlMasterConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.EditCmsMySqlMasterConnection);
            }
        }
        internal IDbConnection AccessoriesMySqlReadConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.AccessoriesMySqlReadConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.AccessoriesMySqlReadConnection);
            }
        } 
        internal IDbConnection AccessoriesMySqlMasterConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.AccessoriesMySqlMasterConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.AccessoriesMySqlMasterConnection);
            }
        }
        internal IDbConnection OffersMySqlMasterConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.OffersMySqlMasterConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.OffersMySqlMasterConnection);
            }
        }
        internal IDbConnection OffersMySqlReadConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.OffersMySqlReadConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.OffersMySqlReadConnection);
            }
        }
        internal IDbConnection PricesMySqlReadConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.PricesMySqlReadConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.PricesMySqlReadConnection);
            }
        }
        internal IDbConnection PricesMySqlMasterConnection
        {
            get
            {
                return string.IsNullOrEmpty(DbConnections.PricesMySqlMasterConnection) ? new MySqlConnection() : new MySqlConnection(DbConnections.PricesMySqlMasterConnection);
            }
        }
    }
}