﻿using System;
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
    public class SemestersController : ControllerBase
    {
        private readonly TrackAttendanceContext _context;

        public SemestersController(TrackAttendanceContext context)
        {
            _context = context;
        }

        // GET: api/Semesters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Semesters>>> GetSemesters()
        {
            return await _context.Semesters.ToListAsync();
        }

        // GET: api/Semesters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Semesters>> GetSemesters(int id)
        {
            var semesters = await _context.Semesters.FindAsync(id);

            if (semesters == null)
            {
                return NotFound();
            }

            return semesters;
        }

        // PUT: api/Semesters/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSemesters(int id, Semesters semesters)
        {
            if (id != semesters.Id)
            {
                return BadRequest();
            }

            _context.Entry(semesters).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SemestersExists(id))
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

        // POST: api/Semesters
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Semesters>> PostSemesters(Semesters semesters)
        {
            _context.Semesters.Add(semesters);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSemesters", new { id = semesters.Id }, semesters);
        }

        // DELETE: api/Semesters/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Semesters>> DeleteSemesters(int id)
        {
            var semesters = await _context.Semesters.FindAsync(id);
            if (semesters == null)
            {
                return NotFound();
            }

            _context.Semesters.Remove(semesters);
            await _context.SaveChangesAsync();

            return semesters;
        }

        private bool SemestersExists(int id)
        {
            return _context.Semesters.Any(e => e.Id == id);
        }
    }
}
