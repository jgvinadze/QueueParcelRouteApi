namespace QueueParcelRouteApi.Infrastructure
{
    public interface IMariaDbSqlText
    {
        string InsertParcelsInMariaDb { get; }
        string InsertRoutesInMariaDb { get; }
        string SelectRouteIdsForDelete { get; }
        string SelectParcelIdsForDelete { get; }
    }
}
