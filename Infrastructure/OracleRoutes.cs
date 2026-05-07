using Dapper;
using QueueParcelRouteApi.Domain;
using System.Data;

namespace QueueParcelRouteApi.Infrastructure
{
    public class OracleRoutes:BaseDbConnection
    {
        private readonly IOracleSqlText oracleSqlText;

        public OracleRoutes(DapperDbConnectionFactory connection,IOracleSqlText sql):base(connection,"OracleConnection")
        {
            oracleSqlText = sql;
        }

        public async Task<List<Domain.Parcel>> GetUnProcessedRoutes(CancellationToken ct)
        {
            List<Domain.Parcel> parcels_routes;

            try
            {                
                parcels_routes = await JoinedQueryAsync(oracleSqlText.selectUnProcessedParcels, ct, null).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return parcels_routes;
        }

        public async Task<bool> DeleteProcessed(CancellationToken ct)
        {
            bool res = false;

            try
            {
                res= await InsDelQueryAsync(oracleSqlText.deleteProcessedParcels, oracleSqlText.deleteProcessedRoutes,ct, null, null).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return res;
        }

        public async Task<bool> UpdateStatusProcessed(List<Parcel> parcels,CancellationToken ct)
        {
           bool result;

            try
            {
                var routeIds = parcels?.SelectMany(x => x.routes).Select(a => a.route_id).AsEnumerable();
                var parcelIds = parcels?.Select(x => x.parcel_id).AsEnumerable();

                result = await InsDelQueryAsync(oracleSqlText.updateStatusProcessedParcels, oracleSqlText.updateStatusProcessedRoutes,ct, new { parcelIds }, new { routeIds }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

    }
}
