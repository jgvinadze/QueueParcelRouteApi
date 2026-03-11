namespace QueueParcelRouteApi.Infrastructure
{
    public interface IOracleSqlText
    {
        string selectUnProcessedParcels { get; }
        string updateStatusProcessedParcels { get; }
        string updateStatusProcessedRoutes { get; }
        string deleteProcessedParcels { get; }
        string deleteProcessedRoutes { get; }
    }
}
