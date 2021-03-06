﻿using System;
using System.Collections.Generic;

namespace TrackAttendanceAPI.Models
{
    public partial class SignEntries
    {
        public int UserId { get; set; }
        public int AppointmentId { get; set; }
        public int Year { get; set; }
        public string Week1 { get; set; }
        public string Week2 { get; set; }
        public string Week3 { get; set; }
        public string Week4 { get; set; }
        public string Week5 { get; set; }
        public string Week6 { get; set; }
        public string Week7 { get; set; }
        public string Week8 { get; set; }
        public string Week9 { get; set; }
        public string Week10 { get; set; }
        public string Week11 { get; set; }
        public string Week12 { get; set; }
        public string Week13 { get; set; }
        public string Week14 { get; set; }
        public string Week15 { get; set; }

        public virtual Appointments Appointment { get; set; }
        public virtual Users User { get; set; }
    }
}
