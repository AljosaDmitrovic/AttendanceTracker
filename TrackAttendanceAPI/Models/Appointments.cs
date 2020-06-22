using System;
using System.Collections.Generic;

namespace TrackAttendanceAPI.Models
{
    public partial class Appointments
    {
        public Appointments()
        {
            SignEntries = new HashSet<SignEntries>();
        }

        public int Id { get; set; }
        public int GroupId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public int Day { get; set; }
        public bool? Active { get; set; }
        public string? Password { get; set; }

        public virtual Groups Group { get; set; }
        public virtual ICollection<SignEntries> SignEntries { get; set; }
    }
}
