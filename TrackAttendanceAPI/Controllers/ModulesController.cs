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
    public class ModulesController : ControllerBase
    {
        private readonly TrackAttendanceContext _context;

        public ModulesController(TrackAttendanceContext context)
        {
            _context = context;
        }

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Modules>>> GetModules()
        {
            return await _context.Modules.ToListAsync();
        }

        // GET: api/Modules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Modules>> GetModules(int id)
        {
            var modules = await _context.Modules.FindAsync(id);

            if (modules == null)
            {
                return NotFound();
            }

            return modules;
        }

        // PUT: api/Modules/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModules(int id, Modules modules)
        {
            if (id != modules.Id)
            {
                return BadRequest();
            }

            _context.Entry(modules).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModulesExists(id))
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

        // POST: api/Modules
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Modules>> PostModules(Modules modules)
        {
            _context.Modules.Add(modules);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModules", new { id = modules.Id }, modules);
        }

        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Modules>> DeleteModules(int id)
        {
            var modules = await _context.Modules.FindAsync(id);
            if (modules == null)
            {
                return NotFound();
            }

            _context.Modules.Remove(modules);
            await _context.SaveChangesAsync();

            return modules;
        }

        private bool ModulesExists(int id)
        {
            return _context.Modules.Any(e => e.Id == id);
        }
    }
}
