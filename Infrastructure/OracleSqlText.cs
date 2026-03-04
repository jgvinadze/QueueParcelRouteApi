namespace QueueParcelRouteApi.Infrastructure
{
    public class OracleSqlText:IOracleSqlText
    {
        public string selectUnProcessedParces => "SELECT a.parcel_id,a.parcel_type_id,a.parcel_status_id,a.transit_type_id,a.excise_status_id,a.parcel_route_type_id,a.internal_code,a.tracking_code,a.ips_tracking_code,a.create_date, a.status " +
            " ,b.parcel_id,b.route_id,b.route_type_id,b.routing_date_time,b.is_active,b.route_comment,b.office_id,b.parcel_id,b.status FROM GEOPOST.PARCELS_QUEUE a INNER JOIN GEOPOST.ROUTES_QUEUE b ON a.parcel_id=b.parcel_id where rownum<10";
        public string deleteProcessedParcels => "Delete FROM geopost.parcels_queue where status='processed'";
        public string deleteProcessedRoutes => "Delete FROM GEOPOST.ROUTES_QUEUE WHERE status='processed'";
        public string updateStatusProcessedParcels => "UPDATE GEOPOST.PARCELS_QUEUE SET status='processed' WHERE parcel_id IN :parcelIds";
        public string updateStatusProcessedRoutes => "UPDATE GEOPOST.ROUTES_QUEUE SET status='processed' WHERE route_id IN :routeIds";

    }
}
