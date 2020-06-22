using System;
using System.Collections.Generic;

namespace TrackAttendanceAPI.Models
{
    public partial class Groups
    {
        public Groups()
        {
            Appointments = new HashSet<Appointments>();
            StudentGroups = new HashSet<StudentGroups>();
        }

        public int Id { get; set; }
        public int LectureId { get; set; }
        public int? RoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Lectures Lecture { get; set; }
        public virtual Rooms Room { get; set; }
        public virtual ICollection<Appointments> Appointments { get; set; }
        public virtual ICollection<StudentGroups> StudentGroups { get; set; }
    }
}
