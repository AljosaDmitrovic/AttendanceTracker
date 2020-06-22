using System;
using System.Collections.Generic;

namespace TrackAttendanceAPI.Models
{
    public partial class Lectures
    {
        public Lectures()
        {
            Groups = new HashSet<Groups>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? StudyYear { get; set; }
        public int? SemesterId { get; set; }

        public virtual Semesters Semester { get; set; }
        public virtual ICollection<Groups> Groups { get; set; }
    }
}
