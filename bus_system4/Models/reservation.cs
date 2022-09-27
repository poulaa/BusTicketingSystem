using bus_system4.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace bus_system4.Models
{
    public class reservation
    {

        public int Id { get; set; }
        public DateTime reservation_date { get; set; }
        public string departure { get; set; }
        public string arrival { get; set; }
        public int price { get; set; }
        public string UserId{ get; set; }
        [ForeignKey("UserId")]
        public bus_user bus_user { get; set; }


    }
}
