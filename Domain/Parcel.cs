using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace QueueParcelRouteApi.Domain
{
    [Table("parcels")]
    public class Parcel
    {
        public int parcel_id {  get; set; }
        public int parcel_status_id { get; set; }
        public int parcel_type_id {  get; set; }
        public int transit_type_id { get; set; }
        public int parcel_route_type_id { get; set; }
        public string internal_code { get; set; }
        public string tracking_code { get; set; }
        public string ips_tracking_code { get; set; }
        public DateTime create_date { get; set; }
        public int excise_status_id { get; set; }
        public string status { get; set; }
        public List<Domain.Route> routes { get; set; }= new List<Route>();
    }
}
