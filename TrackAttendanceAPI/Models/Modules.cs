using System;
using System.Collections.Generic;

namespace TrackAttendanceAPI.Models
{
    public partial class Modules
    {
        public Modules()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Duration { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
