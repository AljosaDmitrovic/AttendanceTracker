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
    public class LecturesController : ControllerBase
    {
        private readonly TrackAttendanceContext _context;

        public LecturesController(TrackAttendanceContext context)
        {
            _context = context;
        }

        // GET: api/Lectures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lectures>>> GetLectures()
        {
            return await _context.Lectures.ToListAsync();
        }

        // GET: api/Lectures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lectures>> GetLectures(int id)
        {
            var lectures = await _context.Lectures.FindAsync(id);

            if (lectures == null)
            {
                return NotFound();
            }

            return lectures;
        }

        // PUT: api/Lectures/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLectures(int id, Lectures lectures)
        {
            if (id != lectures.Id)
            {
                return BadRequest();
            }

            _context.Entry(lectures).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LecturesExists(id))
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

        // POST: api/Lectures
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Lectures>> PostLectures(Lectures lectures)
        {
            _context.Lectures.Add(lectures);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLectures", new { id = lectures.Id }, lectures);
        }

        // DELETE: api/Lectures/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Lectures>> DeleteLectures(int id)
        {
            var lectures = await _context.Lectures.FindAsync(id);
            if (lectures == null)
            {
                return NotFound();
            }

            _context.Lectures.Remove(lectures);
            await _context.SaveChangesAsync();

            return lectures;
        }

        private bool LecturesExists(int id)
        {
            return _context.Lectures.Any(e => e.Id == id);
        }
    }
}
