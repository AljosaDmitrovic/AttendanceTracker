using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackAttendanceAPI.Models;

namespace TrackAttendanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly TrackAttendanceContext _context;

        public AppointmentsController(TrackAttendanceContext context)
        {
            _context = context;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointments>>> GetAppointments()
        {
            return await _context.Appointments.ToListAsync();
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointments>> GetAppointments(int id)
        {
            var appointments = await _context.Appointments.FindAsync(id);

            if (appointments == null)
            {
                return NotFound();
            }

            return appointments;
        }

        [HttpGet("AppointmentsByUser/{id}")]
        public List<Appointments> GetOpenAppointmentsByUserId(int id)
        {
            DateTime today = DateTime.Today;
            List<StudentGroups> studentGroups = _context.StudentGroups.Where(s => s.UserId == id).ToList();
            List<Appointments> appointments = new List<Appointments>();
            List<Appointments> returnAppointments = new List<Appointments>();
            List<Groups> groups = new List<Groups>();
            Groups group = new Groups();
            Appointments appointment = new Appointments();

            foreach (var studentGroup in studentGroups)
            {
                group = _context.Groups.Find(studentGroup.GroupId);
                if (group != null)
                    groups.Add(group);
            }
            foreach (var grp in groups)
            {
                appointment = _context.Appointments.Where(a => a.GroupId == grp.Id).FirstOrDefault();
                if (appointment != null)
                    appointments.Add(appointment);
            }
            foreach (var appoint in appointments)
            {
                bool insertAppointment = false;
                if (appoint.Day == (int)today.DayOfWeek)
                {
                    string[] vreme = appoint.StartTime.ToString().Split(":");
                    if (vreme[0].Contains(DateTime.Now.ToString().Split(" ")[1].Split(":")[0]))
                        insertAppointment = true;
                }
                if (appoint.Active == true)
                    insertAppointment = true;
                if (insertAppointment)
                    returnAppointments.Add(appoint);
            }
            return returnAppointments;
        }

        // PUT: api/Appointments/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointments(int id, Appointments appointments)
        {
            if (id != appointments.Id)
            {
                return BadRequest();
            }

            _context.Entry(appointments).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentsExists(id))
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

        // POST: api/Appointments
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Appointments>> PostAppointments(Appointments appointments)
        {
            _context.Appointments.Add(appointments);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointments", new { id = appointments.Id }, appointments);
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Appointments>> DeleteAppointments(int id)
        {
            var appointments = await _context.Appointments.FindAsync(id);
            if (appointments == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointments);
            await _context.SaveChangesAsync();

            return appointments;
        }

        private bool AppointmentsExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
