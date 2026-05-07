using Dapper;
using QueueParcelRouteApi.Domain;
using System.Data;
using System.Data.Common;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml;

namespace QueueParcelRouteApi.Infrastructure
{
    public class BaseDbConnection
    {
        private readonly string connectionStringName;
        private DapperDbConnectionFactory connectionFactory;

        public BaseDbConnection(DapperDbConnectionFactory _connFactory, string _connectionStringName)
        {
            this.connectionStringName = _connectionStringName;
            this.connectionFactory = _connFactory;
        }

        protected IDbConnection CreateConnection()
        {
            if (connectionStringName == "OracleConnection")
                return connectionFactory.CreateOracleConnection();
            else
                return connectionFactory.CreateMariaDbConnection();
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string sqlText, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<T>(sqlText, parameters).ConfigureAwait(false);
            }
        }

        protected async Task<List<Parcel>> JoinedQueryAsync(string sqlText,CancellationToken token, object parameters = null)
        {
            var parcelDictionary = new Dictionary<int, Parcel>();
            using (var connection = CreateConnection())
            {
                var command = new CommandDefinition(sqlText, parameters, cancellationToken: token);

                IEnumerable<Parcel> listParcels = await connection.QueryAsync<Parcel, Domain.Route, Parcel>(

                        command,

                        (parcels, routes) =>
                        {
                            if (!parcelDictionary.TryGetValue(parcels.parcel_id, out Parcel existingParcel))
                            {
                                existingParcel = parcels;
                                parcelDictionary.Add(existingParcel.parcel_id, existingParcel);
                            }

                            existingParcel.routes.Add(routes);

                            return existingParcel;
                        },

                        splitOn: "route_Id"

                    ).ConfigureAwait(false);

            }

            return parcelDictionary.Values.ToList<Parcel>();
        }       

        protected async Task<IEnumerable<T>> ExecStoredProcForSelectAsync<T>(string sqlText, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<T>(sqlText, parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            }
        }

        protected async Task<bool> InsDelQueryAsync(string sqlParcelText, string sqlRoutesText, CancellationToken ct, object parameters_1 = null,object parameters_2 = null)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                    var routeCommand = new CommandDefinition(sqlRoutesText, parameters_2, transaction: transaction, cancellationToken: ct);

                    var r = await connection.ExecuteAsync(routeCommand).ConfigureAwait(false);

                    var parcelCommand = new CommandDefinition(sqlParcelText, parameters_1, transaction: transaction, cancellationToken: ct);

                    var p = await connection.ExecuteAsync(parcelCommand).ConfigureAwait(false);

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if(transaction!=null)transaction.Dispose();

                    connection.Close();
                    connection.Dispose();
                }

                return true;
            }
        }        
    }
}
