using System;
using System.Collections.Generic;

namespace TrackAttendanceAPI.Models
{
    public partial class Semesters
    {
        public Semesters()
        {
            Lectures = new HashSet<Lectures>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? StartDay { get; set; }
        public int? StartMonth { get; set; }
        public int? EndDay { get; set; }
        public int? EndMonth { get; set; }

        public virtual ICollection<Lectures> Lectures { get; set; }
    }
}
