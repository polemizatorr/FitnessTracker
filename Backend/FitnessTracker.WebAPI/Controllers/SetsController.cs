using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Entities.Models;
using FitnessTracker.WebAPI.Entities.DTO;

namespace FitnessTracker.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetsController : ControllerBase
    {
        private readonly TrainingsContext _context;

        public SetsController(TrainingsContext context)
        {
            _context = context;
        }

        // GET: api/Sets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Set>>> GetSets()
        {
          if (_context.Sets == null)
          {
              return NotFound();
          }
            return await _context.Sets.ToListAsync();
        }

        // GET: api/Sets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Set>> GetSet(Guid id)
        {
          if (_context.Sets == null)
          {
              return NotFound();
          }
            var @set = await _context.Sets.FindAsync(id);

            if (@set == null)
            {
                return NotFound();
            }

            return @set;
        }

        // PUT: api/Sets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSet(Guid id, Set @set)
        {
            if (id != @set.SetId)
            {
                return BadRequest();
            }

            _context.Entry(@set).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SetExists(id))
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

        // POST: api/Sets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Set>> PostSet(SetDto @set)
        {
          if (_context.Sets == null)
          {
              return Problem("Entity set 'TrainingsContext.Sets'  is null.");
          }

            var newSet = new Set(set.RepetitionsNumber, set.ExerciseName!, set.ExhaustionLevel);

            _context.Sets.Add(newSet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSet", new { id = newSet.SetId }, newSet);
        }

        // DELETE: api/Sets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSet(Guid id)
        {
            if (_context.Sets == null)
            {
                return NotFound();
            }
            var @set = await _context.Sets.FindAsync(id);
            if (@set == null)
            {
                return NotFound();
            }

            _context.Sets.Remove(@set);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SetExists(Guid id)
        {
            return (_context.Sets?.Any(e => e.SetId == id)).GetValueOrDefault();
        }
    }
}
