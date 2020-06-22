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
    public class StudentGroupsController : ControllerBase
    {
        private readonly TrackAttendanceContext _context;

        public StudentGroupsController(TrackAttendanceContext context)
        {
            _context = context;
        }

        // GET: api/StudentGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentGroups>>> GetStudentGroups()
        {
            return await _context.StudentGroups.ToListAsync();
        }

        // GET: api/StudentGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentGroups>> GetStudentGroups(int id)
        {
            var studentGroups = await _context.StudentGroups.FindAsync(id);

            if (studentGroups == null)
            {
                return NotFound();
            }

            return studentGroups;
        }

        // PUT: api/StudentGroups/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentGroups(int id, StudentGroups studentGroups)
        {
            if (id != studentGroups.UserId)
            {
                return BadRequest();
            }

            _context.Entry(studentGroups).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentGroupsExists(id))
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

        // POST: api/StudentGroups
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<StudentGroups>> PostStudentGroups(StudentGroups studentGroups)
        {
            _context.StudentGroups.Add(studentGroups);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentGroupsExists(studentGroups.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudentGroups", new { id = studentGroups.UserId }, studentGroups);
        }

        // DELETE: api/StudentGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StudentGroups>> DeleteStudentGroups(int id)
        {
            var studentGroups = await _context.StudentGroups.FindAsync(id);
            if (studentGroups == null)
            {
                return NotFound();
            }

            _context.StudentGroups.Remove(studentGroups);
            await _context.SaveChangesAsync();

            return studentGroups;
        }

        private bool StudentGroupsExists(int id)
        {
            return _context.StudentGroups.Any(e => e.UserId == id);
        }
    }
}
