
namespace QueueParcelRouteApi.Infrastructure
{
    public class MariaDbRoutes:BaseDbConnection
    {
        private readonly IMariaDbSqlText mariaDbSqlText;

        public MariaDbRoutes(DapperDbConnectionFactory connection, IMariaDbSqlText sql) : base(connection, "MariaDbConnection")
        {
            mariaDbSqlText = sql;
        }

        public async Task<bool> TransferRoutes(List<Domain.Parcel> parcelRoutes)
        {
            bool result;

            try
            {
                var routes = parcelRoutes.SelectMany(x => x.routes).AsEnumerable();

                result = await InsDelQueryAsync(mariaDbSqlText.InsertParcelsInMariaDb, mariaDbSqlText.InsertRoutesInMariaDb , parcelRoutes, routes);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }
    }
}
