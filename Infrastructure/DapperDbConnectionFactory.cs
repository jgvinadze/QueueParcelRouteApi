using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Oracle.ManagedDataAccess.Client;
using QueueParcelRouteApi.Application;
using System.Data;


namespace QueueParcelRouteApi.Infrastructure
{
    public class DapperDbConnectionFactory: IDapperDbConnectionFactory
    {
        private readonly string OracleConnectionString;
        private readonly string MariaDbConnectionString;

        public DapperDbConnectionFactory(IConfiguration configuration)
        {
            OracleConnectionString = configuration.GetConnectionString("OracleConnection");
            MariaDbConnectionString = configuration.GetConnectionString("MariaDbConnection");
        }

        public IDbConnection CreateOracleConnection() => new OracleConnection(OracleConnectionString);
        public IDbConnection CreateMariaDbConnection() => new MySqlConnection(MariaDbConnectionString);
    }
}
