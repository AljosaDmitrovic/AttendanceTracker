using System;
using System.Collections.Generic;

namespace TrackAttendanceAPI.Models
{
    public partial class Users
    {
        public Users()
        {
            SignEntries = new HashSet<SignEntries>();
            StudentGroups = new HashSet<StudentGroups>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IndexNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? SignDate { get; set; }
        public bool? IsStudent { get; set; }
        public string LoginName { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public int? ModuleId { get; set; }

        public virtual Modules Module { get; set; }
        public virtual ICollection<SignEntries> SignEntries { get; set; }
        public virtual ICollection<StudentGroups> StudentGroups { get; set; }
    }
}
