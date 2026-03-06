
using Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace QueueParcelRouteApi.Infrastructure
{
    public class RoutesEndpoints
    {
        private readonly DapperDbConnectionFactory connection;
        private readonly IOracleSqlText oracleSql;
        private readonly IMariaDbSqlText mariaDbSqlText;
        private readonly ILogger logger;

        public RoutesEndpoints(DapperDbConnectionFactory _connection,ILogger<GlobalExceptionHandler> _logger, IOracleSqlText _sql,IMariaDbSqlText _mariadbSql)
        {
            connection = _connection;
            oracleSql = _sql;
            mariaDbSqlText= _mariadbSql;
            logger = _logger;
        }

        public async Task<List<Domain.Parcel>> InsParcelRoutesInMariaDbAsync()
        {
            List<Domain.Parcel> parcelRoutes;
            bool res = false;

            try
            {
                OracleRoutes oracleRoute = new OracleRoutes(connection, oracleSql);

                parcelRoutes = await oracleRoute
                    .GetUnProcessedRoutes();

                if (parcelRoutes != null)
                {
                    res = await new MariaDbRoutes(connection, mariaDbSqlText)
                                    .TransferRoutes(parcelRoutes);

                    res = await oracleRoute
                                    .UpdateStatusProcessed(parcelRoutes);

                    await new OracleRoutes(connection, oracleSql)
                                    .DeleteProcessed();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
          
            return parcelRoutes;
        }

        public async Task<bool> DeleteProcessedParcelsAndRoutesAsync()
        {
            try
            {
                return await new OracleRoutes(connection, oracleSql)
                                    .DeleteProcessed();  
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
 
}
