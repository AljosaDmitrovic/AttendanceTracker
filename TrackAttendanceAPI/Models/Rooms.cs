using System;
using System.Collections.Generic;

namespace TrackAttendanceAPI.Models
{
    public partial class Rooms
    {
        public Rooms()
        {
            Groups = new HashSet<Groups>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? AvailablePlaces { get; set; }

        public virtual ICollection<Groups> Groups { get; set; }
    }
}
