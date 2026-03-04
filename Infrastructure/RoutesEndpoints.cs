
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

        public RoutesEndpoints(DapperDbConnectionFactory _connection, IOracleSqlText _sql,IMariaDbSqlText _mariadbSql)
        {
            connection = _connection;
            oracleSql = _sql;
            mariaDbSqlText= _mariadbSql;
        }

        public async Task<List<Domain.Parcel>> InsParcelRoutesInMariaDbAsync()
        {
            List<Domain.Parcel> parcelRoutes;
            bool res = false;
            Stopwatch  st = new Stopwatch();
            st.Start();
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

                    var tm = st.ElapsedMilliseconds;

                    await new OracleRoutes(connection, oracleSql)
                                    .DeleteProcessed();
                }
            }
            catch (Exception ex)
            {
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
                throw new Exception(ex.Message);
            }
        }
    }
 
}
