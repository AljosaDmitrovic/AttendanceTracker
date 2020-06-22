using System;
using System.Collections.Generic;

namespace TrackAttendanceAPI.Models
{
    public partial class StudentGroups
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }

        public virtual Groups Group { get; set; }
        public virtual Users User { get; set; }
    }
}
