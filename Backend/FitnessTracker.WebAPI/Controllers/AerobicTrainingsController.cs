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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FitnessTracker.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AerobicTrainingsController : ControllerBase
    {
        private readonly TrainingsContext _context;
        private readonly IHttpContextAccessor _http;

        public AerobicTrainingsController(TrainingsContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _http = httpContextAccessor; 
        }

        // GET: api/AerobicTrainings
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AerobicTraining>>> GetAerobicTrainings()
        {
          if (_context.AerobicTrainings == null)
          {
              return NotFound();
          }
                    
            return await _context.AerobicTrainings.ToListAsync();
        }

        // GET: api/AerobicTrainings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AerobicTraining>> GetAerobicTraining(Guid id)
        {
          if (_context.AerobicTrainings == null)
          {
              return NotFound();
          }
            var aerobicTraining = await _context.AerobicTrainings.FindAsync(id);

            if (aerobicTraining == null)
            {
                return NotFound();
            }

            return aerobicTraining;
        }

        // PUT: api/AerobicTrainings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAerobicTraining(Guid id, AerobicTrainingDto aerobicTraining)
        {
            var editTraining = _context.AerobicTrainings.Find(id);

            if (editTraining == null)
            {
                return BadRequest();
            }

            // _context.Entry(aerobicTraining).State = EntityState.Modified;

            try
            {
                editTraining.ActivityDurationMinutes = aerobicTraining.ActivityDurationMinutes;
                editTraining.CalorieBurnt = aerobicTraining.CalorieBurnt;
                editTraining.ActivityType = aerobicTraining.ActivityType;
                editTraining.ModificationTime = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AerobicTrainingExists(id))
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

        // POST: api/AerobicTrainings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AerobicTraining>> PostAerobicTraining(AerobicTrainingDto aerobicTraining)
        {
          if (_context.AerobicTrainings == null)
          {
              return Problem("Entity set 'TrainingsContext.AerobicTrainings'  is null.");
          }


            var username = _http.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = _context.Users.First(u => u.UserName == username).UserId;

            if (userId == Guid.Empty) 
            {
                throw new Exception("No user found for given username");
            }

            var newAerobicTraining = new AerobicTraining(userId, aerobicTraining.ActivityType!, aerobicTraining.ActivityDurationMinutes, aerobicTraining.CalorieBurnt);

            _context.AerobicTrainings.Add(newAerobicTraining);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAerobicTraining", new { id = newAerobicTraining.AerobicTrainingId }, aerobicTraining);
        }

        // DELETE: api/AerobicTrainings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAerobicTraining(Guid id)
        {
            if (_context.AerobicTrainings == null)
            {
                return NotFound();
            }
            var aerobicTraining = await _context.AerobicTrainings.FindAsync(id);
            if (aerobicTraining == null)
            {
                return NotFound();
            }

            _context.AerobicTrainings.Remove(aerobicTraining);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AerobicTrainingExists(Guid id)
        {
            return (_context.AerobicTrainings?.Any(e => e.AerobicTrainingId == id)).GetValueOrDefault();
        }
    }
}
