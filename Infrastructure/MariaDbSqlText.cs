
namespace QueueParcelRouteApi.Infrastructure
{
    public class MariaDbSqlText:IMariaDbSqlText
    {
        public string InsertParcelsInMariaDb => "INSERT INTO parcels(parcel_id,parcel_status_id,internal_code,tracking_code,ips_tracking_code,parcel_type_id,transit_type_id,excise_status_id,parcel_route_type_id,create_date,is_active) " +
            "VALUES(@parcel_id,@parcel_status_id,@internal_code,@tracking_code,@ips_tracking_code,@parcel_type_id,@transit_type_id,@excise_status_id,@parcel_route_type_id,@create_date,@is_active) ON DUPLICATE KEY UPDATE " +
            "parcel_status_id=@parcel_status_id,internal_code=@internal_code,tracking_code=@tracking_code,ips_tracking_code=@ips_tracking_code,parcel_type_id=@parcel_type_id," +
            "transit_type_id=@transit_type_id,excise_status_id=@excise_status_id,parcel_route_type_id=@parcel_route_type_id,create_date=@create_date,is_active=@is_active;";

        public string InsertRoutesInMariaDb=> "INSERT INTO routes(route_id,route_type_id,routing_date_time,parcel_id,is_active,route_comment,office_id) " +
            "VALUES(@route_id,@route_type_id,@routing_date_time,@parcel_id,@is_active,@route_comment,@office_id) ON DUPLICATE KEY UPDATE "+
            "route_type_id=@route_type_id,routing_date_time=@routing_date_time,parcel_id=@parcel_id,is_active=@is_active,route_comment=@route_comment,office_id=@office_id; SELECT LAST_INSERT_ID();";

        public string SelectRouteIdsForDelete => "select route_id from routes where route_id IN @routeids";

        public string SelectParcelIdsForDelete => "select parcel_id from parcels where parcel_id IN @parcelids";
    }
}
