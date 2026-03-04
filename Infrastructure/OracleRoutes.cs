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

        public async Task<List<Domain.Parcel>> GetUnProcessedRoutes()
        {
            List<Domain.Parcel> parcels_routes;

            try
            {                
                parcels_routes = await JoinedQueryAsync(oracleSqlText.selectUnProcessedParces, null).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return parcels_routes;
        }

        public async Task<bool> DeleteProcessed()
        {
            bool res = false;

            try
            {
                res= await InsDelQueryAsync(oracleSqlText.deleteProcessedParcels, oracleSqlText.deleteProcessedRoutes, null, null).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return res;
        }

        public async Task<bool> UpdateStatusProcessed(List<Parcel> parcels)
        {
           bool result;

            try
            {
                var routeIds = parcels?.SelectMany(x => x.routes).Select(a => a.route_id).AsEnumerable();
                var parcelIds = parcels?.Select(x => x.parcel_id).AsEnumerable();

                result = await InsDelQueryAsync(oracleSqlText.updateStatusProcessedParcels, oracleSqlText.updateStatusProcessedRoutes, new { parcelIds }, new { routeIds }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

    }
}
