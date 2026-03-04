using System.ComponentModel.DataAnnotations.Schema;

namespace QueueParcelRouteApi.Domain
{
    [Table("routes")]
    public class Route
    {
        public int route_id { get; set; }
        public int route_type_id { get; set; }
        public DateTime routing_date_time {  get; set; }
        public int parcel_id {  get; set; }
        public bool is_active { get; set; }
        public string route_comment { get; set; }
        public int office_id { get; set; }
        public string status { get; set; }
    }
}
