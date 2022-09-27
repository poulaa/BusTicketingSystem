using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace bus_system4.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the bus_user class
    public class bus_user : IdentityUser
    {
        public string first_name { get; set; }
        public string last_name
        {
            get; set;
        }
    }
}
