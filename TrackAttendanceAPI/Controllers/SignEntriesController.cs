using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackAttendanceAPI.Models;

namespace TrackAttendanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignEntriesController : ControllerBase
    {
        public class LogAtt
        {
            public int userId { get; set; }
            public int attandanceId { get; set; }
            public string password { get; set; }
        }

        private readonly TrackAttendanceContext _context;

        public SignEntriesController(TrackAttendanceContext context)
        {
            _context = context;
        }

        // GET: api/SignEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SignEntries>>> GetSignEntries()
        {
            return await _context.SignEntries.Where(se => se.User.IsStudent == true).ToListAsync();
        }

        // GET: api/SignEntries/Group/5
        [HttpGet("Group/{id}")]
        public async Task<ActionResult<IEnumerable<SignEntries>>> GetSignEntriesByGroup(int id)
        {
            return await _context.SignEntries.Where(se => (se.User.IsStudent == true) && (se.Appointment.Id == id)).ToListAsync();
        }

        // GET: api/SignEntries/Appointment/5
        [HttpGet("Appointment/{id}")]
        public async Task<ActionResult<IEnumerable<SignEntries>>> GetSignEntriesByAppointment(int id)
        {
            return await _context.SignEntries.Where(se => (se.User.IsStudent == true) && (se.Appointment.Group.LectureId == id)).ToListAsync();
        }

        // GET: api/SignEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SignEntries>> GetSignEntries(int id)
        {
            var signEntries = await _context.SignEntries.FindAsync(id);

            if (signEntries == null)
            {
                return NotFound();
            }

            return signEntries;
        }

        [HttpPost("Log")]
        public async Task<IActionResult> ModifySignEntryByUserIdAndAttandanceId(LogAtt log)
        {
            SignEntries signEntry = _context.SignEntries.Where(s => s.UserId == log.userId && s.AppointmentId == log.attandanceId).First();
            string field = DateTime.Now.ToString().Split(" ")[0] + ",true";
            if (log.password == null)
                log.password = "";

            DateTime todayNow = DateTime.Today;
            DateTime today = DateTime.Today;

            Appointments appointment = _context.Appointments.Where(a => a.Id == log.attandanceId).First();
            if (!appointment.Password.Equals(log.password))
                return NotFound("Netačna lozinka");

            TimeSpan minute = (TimeSpan)appointment.OpenTime;
            int currentMinute = DateTime.Now.Minute;
            if ((minute.TotalMinutes - currentMinute) <= 0 && appointment.Active == false)
                return NotFound("Zakasnili ste");

            Groups group = _context.Groups.Where(g => g.Id == appointment.GroupId).First();
            Lectures lecture = _context.Lectures.Where(l => l.Id == group.LectureId).First();
            Semesters semester = _context.Semesters.Where(s => s.Id == lecture.SemesterId).First();
            int week = GetCurrentWeek(semester);
            //for (int i = (int)today.Month; i >= (int)semester.StartMonth; i--)
            //{
            //    if (i == (int)semester.StartMonth)
            //        week += (int)(DateTime.DaysInMonth((int)today.Year, (int)today.Month) / 7) - ((int)(DateTime.DaysInMonth((int)today.Year, (int)today.Month) / 7) - (int)(todayNow.Day / 7));
            //    else
            //        week += (int)(DateTime.DaysInMonth((int)today.Year, (int)today.Month) / 7);
            //    today.AddMonths(-1);
            //}

            if ((signEntry.Week1 == null || signEntry.Week1 == "") && week == 1)
                signEntry.Week1 = field;
            else if ((signEntry.Week2 == null || signEntry.Week2 == "") && week == 2)
                signEntry.Week2 = field;
            else if ((signEntry.Week3 == null || signEntry.Week3 == "") && week == 3)
                signEntry.Week3 = field;
            else if ((signEntry.Week4 == null || signEntry.Week4 == "") && week == 4)
                signEntry.Week4 = field;
            else if ((signEntry.Week5 == null || signEntry.Week5 == "") && week == 5)
                signEntry.Week5 = field;
            else if ((signEntry.Week6 == null || signEntry.Week6 == "") && week == 6)
                signEntry.Week6 = field;
            else if ((signEntry.Week7 == null || signEntry.Week7 == "") && week == 7)
                signEntry.Week7 = field;
            else if ((signEntry.Week8 == null || signEntry.Week8 == "") && week == 8)
                signEntry.Week8 = field;
            else if ((signEntry.Week9 == null || signEntry.Week9 == "") && week == 9)
                signEntry.Week9 = field;
            else if ((signEntry.Week10 == null || signEntry.Week10 == "") && week == 10)
                signEntry.Week10 = field;
            else if ((signEntry.Week11 == null || signEntry.Week11 == "") && week == 11)
                signEntry.Week11 = field;
            else if ((signEntry.Week12 == null || signEntry.Week12 == "") && week == 12)
                signEntry.Week12 = field;
            else
                return NotFound("Prijava je već izvršena");

            _context.Entry(signEntry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("AdminLog/{id}/{weekID}/{user}")]
        public async Task<IActionResult> ModifySignEntryByUserIdAndAttandanceId(int id, int weekID, int user)
        {
            SignEntries signEntry = _context.SignEntries.Where(s => s.UserId == user && s.AppointmentId == id).First();
            switch (weekID)
            {
                case 1:
                    {
                        if (signEntry.Week1 == null || signEntry.Week1 == "" || signEntry.Week1.Split(",")[1].Equals("false"))
                            try
                            {
                                signEntry.Week1 = signEntry.Week1.Split(",")[0] + ",true";
                            }
                            catch
                            {
                                signEntry.Week1 = DateTime.Now.ToString().Split(" ")[0] + ",true";
                            }

                        else
                            try
                            {
                                signEntry.Week1 = signEntry.Week1.Split(",")[0] + ",false";
                            }
                            catch
                            {
                                signEntry.Week1 = DateTime.Now.ToString().Split(" ")[0] + ",false";
                            }
                        break;
                    }
                case 2:
                    {
                        if (signEntry.Week2 == null || signEntry.Week2 == "" || signEntry.Week2.Split(",")[1].Equals("false"))
                            try
                            {
                                signEntry.Week2 = signEntry.Week2.Split(",")[0] + ",true";
                            }
                            catch
                            {
                                signEntry.Week2 = DateTime.Now.ToString().Split(" ")[0] + ",true";
                            }

                        else
                            try
                            {
                                signEntry.Week2 = signEntry.Week2.Split(",")[0] + ",false";
                            }
                            catch
                            {
                                signEntry.Week2 = DateTime.Now.ToString().Split(" ")[0] + ",false";
                            }
                        break;
                    }
                case 3:
                    {
                        if (signEntry.Week3 == null || signEntry.Week3 == "" || signEntry.Week3.Split(",")[1].Equals("false"))
                            try
                            {
                                signEntry.Week3 = signEntry.Week3.Split(",")[0] + ",true";
                            }
                            catch
                            {
                                signEntry.Week3 = DateTime.Now.ToString().Split(" ")[0] + ",true";
                            }

                        else
                            try
                            {
                                signEntry.Week3 = signEntry.Week3.Split(",")[0] + ",false";
                            }
                            catch
                            {
                                signEntry.Week3 = DateTime.Now.ToString().Split(" ")[0] + ",false";
                            }
                        break;
                    }
                case 4:
                    {
                        if (signEntry.Week4 == null || signEntry.Week4 == "" || signEntry.Week4.Split(",")[1].Equals("false"))
                            try
                            {
                                signEntry.Week4 = signEntry.Week4.Split(",")[0] + ",true";
                            }
                            catch
                            {
                                signEntry.Week4 = DateTime.Now.ToString().Split(" ")[0] + ",true";
                            }

                        else
                            try
                            {
                                signEntry.Week4 = signEntry.Week4.Split(",")[0] + ",false";
                            }
                            catch
                            {
                                signEntry.Week4 = DateTime.Now.ToString().Split(" ")[0] + ",false";
                            }
                        break;
                    }
                case 5:
                    {
                        if (signEntry.Week5 == null || signEntry.Week5 == "" || signEntry.Week5.Split(",")[1].Equals("false"))
                            try
                            {
                                signEntry.Week5 = signEntry.Week5.Split(",")[0] + ",true";
                            }
                            catch
                            {
                                signEntry.Week5 = DateTime.Now.ToString().Split(" ")[0] + ",true";
                            }

                        else
                            try
                            {
                                signEntry.Week5 = signEntry.Week5.Split(",")[0] + ",false";
                            }
                            catch
                            {
                                signEntry.Week5 = DateTime.Now.ToString().Split(" ")[0] + ",false";
                            }
                        break;
                    }
                case 6:
                    {
                        if (signEntry.Week6 == null || signEntry.Week6 == "" || signEntry.Week6.Split(",")[1].Equals("false"))
                            try
                            {
                                signEntry.Week6 = signEntry.Week6.Split(",")[0] + ",true";
                            }
                            catch
                            {
                                signEntry.Week6 = DateTime.Now.ToString().Split(" ")[0] + ",true";
                            }

                        else
                            try
                            {
                                signEntry.Week6 = signEntry.Week6.Split(",")[0] + ",false";
                            }
                            catch
                            {
                                signEntry.Week6 = DateTime.Now.ToString().Split(" ")[0] + ",false";
                            }
                        break;
                    }
                case 7:
                    {
                        if (signEntry.Week7 == null || signEntry.Week7 == "" || signEntry.Week7.Split(",")[1].Equals("false"))
                            try
                            {
                                signEntry.Week7 = signEntry.Week7.Split(",")[0] + ",true";
                            }
                            catch
                            {
                                signEntry.Week7 = DateTime.Now.ToString().Split(" ")[0] + ",true";
                            }

                        else
                            try
                            {
                                signEntry.Week7 = signEntry.Week7.Split(",")[0] + ",false";
                            }
                            catch
                            {
                                signEntry.Week7 = DateTime.Now.ToString().Split(" ")[0] + ",false";
                            }
                        break;
                    }
                case 8:
                    {
                        if (signEntry.Week8 == null || signEntry.Week8 == "" || signEntry.Week8.Split(",")[1].Equals("false"))
                            try
                            {
                                signEntry.Week8 = signEntry.Week8.Split(",")[0] + ",true";
                            }
                            catch
                            {
                                signEntry.Week8 = DateTime.Now.ToString().Split(" ")[0] + ",true";
                            }

                        else
                            try
                            {
                                signEntry.Week8 = signEntry.Week8.Split(",")[0] + ",false";
                            }
                            catch
                            {
                                signEntry.Week8 = DateTime.Now.ToString().Split(" ")[0] + ",false";
                            }
                        break;
                    }
                case 9:
                    {
                        if (signEntry.Week9 == null || signEntry.Week9 == "" || signEntry.Week9.Split(",")[1].Equals("false"))
                            try
                            {
                                signEntry.Week9 = signEntry.Week9.Split(",")[0] + ",true";
                            }
                            catch
                            {
                                signEntry.Week9 = DateTime.Now.ToString().Split(" ")[0] + ",true";
                            }

                        else
                            try
                            {
                                signEntry.Week9 = signEntry.Week9.Split(",")[0] + ",false";
                            }
                            catch
                            {
                                signEntry.Week9 = DateTime.Now.ToString().Split(" ")[0] + ",false";
                            }
                        break;
                    }
                case 10:
                    {
                        if (signEntry.Week10 == null || signEntry.Week10 == "" || signEntry.Week10.Split(",")[1].Equals("false"))
                            try
                            {
                                signEntry.Week10 = signEntry.Week10.Split(",")[0] + ",true";
                            }
                            catch
                            {
                                signEntry.Week10 = DateTime.Now.ToString().Split(" ")[0] + ",true";
                            }

                        else
                            try
                            {
                                signEntry.Week10 = signEntry.Week10.Split(",")[0] + ",false";
                            }
                            catch
                            {
                                signEntry.Week10 = DateTime.Now.ToString().Split(" ")[0] + ",false";
                            }
                        break;
                    }
                case 11:
                    {
                        if (signEntry.Week11 == null || signEntry.Week11 == "" || signEntry.Week11.Split(",")[1].Equals("false"))
                            try
                            {
                                signEntry.Week11 = signEntry.Week11.Split(",")[0] + ",true";
                            }
                            catch
                            {
                                signEntry.Week11 = DateTime.Now.ToString().Split(" ")[0] + ",true";
                            }

                        else
                            try
                            {
                                signEntry.Week11 = signEntry.Week11.Split(",")[0] + ",false";
                            }
                            catch
                            {
                                signEntry.Week11 = DateTime.Now.ToString().Split(" ")[0] + ",false";
                            }
                        break;
                    }
                case 12:
                    {
                        if (signEntry.Week12 == null || signEntry.Week12 == "" || signEntry.Week12.Split(",")[1].Equals("false"))
                            try
                            {
                                signEntry.Week12 = signEntry.Week12.Split(",")[0] + ",true";
                            }
                            catch
                            {
                                signEntry.Week12 = DateTime.Now.ToString().Split(" ")[0] + ",true";
                            }

                        else
                            try
                            {
                                signEntry.Week12 = signEntry.Week12.Split(",")[0] + ",false";
                            }
                            catch
                            {
                                signEntry.Week12 = DateTime.Now.ToString().Split(" ")[0] + ",false";
                            }
                        break;
                    }
                case 13:
                    {
                        if (signEntry.Week13 == null || signEntry.Week13 == "" || signEntry.Week13.Split(",")[1].Equals("false"))
                            try
                            {
                                signEntry.Week13 = signEntry.Week13.Split(",")[0] + ",true";
                            }
                            catch
                            {
                                signEntry.Week13 = DateTime.Now.ToString().Split(" ")[0] + ",true";
                            }

                        else
                            try
                            {
                                signEntry.Week13 = signEntry.Week13.Split(",")[0] + ",false";
                            }
                            catch
                            {
                                signEntry.Week13 = DateTime.Now.ToString().Split(" ")[0] + ",false";
                            }
                        break;
                    }
                default:
                    return NotFound("Greška");
            }

            _context.Entry(signEntry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }
        private int GetCurrentWeek(Semesters semester)
        {
            int week = 0;
            DateTime today = DateTime.Today;
            var d = today;
            string startDate = semester.StartMonth + "/" + semester.StartDay + "/" + today.Year;
            string endDate;
            if (semester.StartMonth > semester.EndMonth)
            {
                int year = today.Year + 1;
                endDate = semester.EndMonth + "/" + semester.EndDay + "/" + year;
            }
            else
            {
                endDate = semester.EndMonth + "/" + semester.EndDay + "/" + today.Year;
            }
            DateTime inputStartDate = DateTime.Parse(startDate.Trim());
            DateTime inputEndDate = DateTime.Parse(endDate.Trim());
            CultureInfo cul = CultureInfo.CurrentCulture;
            int weekStartNum = cul.Calendar.GetWeekOfYear(inputStartDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int currentweekNum = cul.Calendar.GetWeekOfYear(d, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int weekEndNum = cul.Calendar.GetWeekOfYear(inputEndDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            
             DateTime maxDate = new DateTime(today.Year, 12, 31);
            int maxNumberOfWeeks = cul.Calendar.GetWeekOfYear(maxDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday); 


            if (currentweekNum <= weekEndNum && currentweekNum >= weekStartNum)
                week = currentweekNum - weekStartNum;
            if (weekStartNum > weekEndNum)
            {
                if (currentweekNum >= weekStartNum && currentweekNum <= maxNumberOfWeeks)
                    week = currentweekNum - weekStartNum;
                if (currentweekNum <= weekEndNum)
                    week = maxNumberOfWeeks - weekStartNum + currentweekNum;
            }
            return week + 1;
        }
        [HttpPost("Refresh")]
        public void updateSignEntryEmptyWeeks(Users user)
        {
            List<SignEntries> signEntries = _context.SignEntries.ToList();
            string field = DateTime.Now.ToString().Split(" ")[0] + ",false";

            DateTime todayNow = DateTime.Today;
            DateTime today = DateTime.Today;

            foreach (var signEntry in signEntries)
            {
                Appointments appointment = _context.Appointments.Where(a => a.Id == signEntry.AppointmentId).First();
                Groups group = _context.Groups.Where(g => g.Id == appointment.GroupId).First();
                Lectures lecture = _context.Lectures.Where(l => l.Id == group.LectureId).First();
                Semesters semester = _context.Semesters.Where(s => s.Id == lecture.SemesterId).First();
                //+++NKU
                int week = GetCurrentWeek(semester);
                //---NKU
                //for (int i = (int)today.Month; i >= (int)semester.StartMonth; i--)
                //{
                //    if (i == (int)semester.StartMonth)
                //        week += (int)(DateTime.DaysInMonth((int)today.Year, (int)today.Month) / 7) - ((int)(DateTime.DaysInMonth((int)today.Year, (int)today.Month) / 7) - (int)(todayNow.Day / 7));
                //    else
                //        week += (int)(DateTime.DaysInMonth((int)today.Year, (int)today.Month) / 7);
                //    today.AddMonths(-1);
                //}
                for (int i = 1; i < week; i++)
                {
                    switch (i)
                    {
                        case 1:
                            if (signEntry.Week1 == null || signEntry.Week1 == "")
                                signEntry.Week1 = field;
                            break;
                        case 2:
                            if (signEntry.Week2 == null || signEntry.Week2 == "")
                                signEntry.Week2 = field;
                            break;
                        case 3:
                            if (signEntry.Week3 == null || signEntry.Week3 == "")
                                signEntry.Week3 = field;
                            break;
                        case 4:
                            if (signEntry.Week4 == null || signEntry.Week4 == "")
                                signEntry.Week4 = field;
                            break;
                        case 5:
                            if (signEntry.Week5 == null || signEntry.Week5 == "")
                                signEntry.Week5 = field;
                            break;
                        case 6:
                            if (signEntry.Week6 == null || signEntry.Week6 == "")
                                signEntry.Week6 = field;
                            break;
                        case 7:
                            if (signEntry.Week7 == null || signEntry.Week7 == "")
                                signEntry.Week7 = field;
                            break;
                        case 8:
                            if (signEntry.Week8 == null || signEntry.Week8 == "")
                                signEntry.Week8 = field;
                            break;
                        case 9:
                            if (signEntry.Week9 == null || signEntry.Week9 == "")
                                signEntry.Week9 = field;
                            break;
                        case 10:
                            if (signEntry.Week10 == null || signEntry.Week10 == "")
                                signEntry.Week10 = field;
                            break;
                        case 11:
                            if (signEntry.Week11 == null || signEntry.Week11 == "")
                                signEntry.Week11 = field;
                            break;
                        case 12:
                            if (signEntry.Week12 == null || signEntry.Week12 == "")
                                signEntry.Week12 = field;
                            break;
                        case 13:
                            if (signEntry.Week13 == null || signEntry.Week13 == "")
                                signEntry.Week13 = field;
                            break;
                        default:
                            break;
                    }
                }
                _context.Entry(signEntry).State = EntityState.Modified;
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }


        // PUT: api/SignEntries/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSignEntries(int id, SignEntries signEntries)
        {
            if (id != signEntries.UserId)
            {
                return BadRequest();
            }

            _context.Entry(signEntries).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SignEntriesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SignEntries
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<SignEntries>> PostSignEntries(SignEntries signEntries)
        {
            _context.SignEntries.Add(signEntries);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SignEntriesExists(signEntries.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSignEntries", new { id = signEntries.UserId }, signEntries);
        }

        // DELETE: api/SignEntries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SignEntries>> DeleteSignEntries(int id)
        {
            var signEntries = await _context.SignEntries.FindAsync(id);
            if (signEntries == null)
            {
                return NotFound();
            }

            _context.SignEntries.Remove(signEntries);
            await _context.SaveChangesAsync();

            return signEntries;
        }

        private bool SignEntriesExists(int id)
        {
            return _context.SignEntries.Any(e => e.UserId == id);
        }
    }
}
