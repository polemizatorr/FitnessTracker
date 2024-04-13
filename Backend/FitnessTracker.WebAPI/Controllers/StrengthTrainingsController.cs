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
using System.Security.Claims;

namespace FitnessTracker.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StrengthTrainingsController : ControllerBase
    {
        private readonly TrainingsContext _context;
        private readonly IHttpContextAccessor _http;

        public StrengthTrainingsController(TrainingsContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _http = httpContextAccessor;
        }

        // GET: api/StrenghtTrainings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StrengthTraining>>> GetStrenghtTrainings()
        {
          if (_context.StrenghtTrainings == null)
          {
              return NotFound();
          }
            return await _context.StrenghtTrainings.ToListAsync();
        }

        // GET: api/StrenghtTrainings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StrengthTraining>> GetStrenghtTraining(Guid id)
        {
          if (_context.StrenghtTrainings == null)
          {
              return NotFound();
          }
            var strenghtTraining = await _context.StrenghtTrainings.FindAsync(id);

            if (strenghtTraining == null)
            {
                return NotFound();
            }

            return strenghtTraining;
        }

        // PUT: api/StrenghtTrainings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStrenghtTraining(Guid id, List<SetDto> strenghtTrainingData)
        {
            var editStrenghtTraining = _context.StrenghtTrainings.Include(st => st.Sets).SingleOrDefault(st => st.StrenghtTrainingId == id);

            if (editStrenghtTraining == null)
            {
                return NotFound();
            }

            try
            {
                editStrenghtTraining.Sets.Clear();

                foreach (var data in strenghtTrainingData)
                {
                    var set = new Set(editStrenghtTraining.StrenghtTrainingId, data.RepetitionsNumber, data.ExerciseName, data.ExhaustionLevel);
                    editStrenghtTraining.Sets.Add(set);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StrenghtTrainingExists(id))
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

        // POST: api/StrenghtTrainings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StrengthTraining>> PostStrenghtTraining(List<SetDto> strenghtTrainingData)
        {
          if (_context.StrenghtTrainings == null)
          {
              return Problem("Entity set 'TrainingsContext.StrenghtTrainings'  is null.");
          }

          if (strenghtTrainingData.Count == 0) 
          { 
              return BadRequest();
          }

            var username = _http.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = _context.Users.First(u => u.UserName == username).UserId;

            if (userId == Guid.Empty)
            {
                throw new Exception("No user found for given username");
            }

            var strenghtTraining = new StrengthTraining(userId);

            foreach (var data in strenghtTrainingData)
            {
                var set = new Set(strenghtTraining.StrenghtTrainingId, data.RepetitionsNumber, data.ExerciseName, data.ExhaustionLevel);
                strenghtTraining.Sets.Add(set);
            }


            _context.StrenghtTrainings.Add(strenghtTraining);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStrenghtTraining", new { id = strenghtTraining.StrenghtTrainingId }, strenghtTraining);
        }

        // DELETE: api/StrenghtTrainings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStrenghtTraining(Guid id)
        {
            if (_context.StrenghtTrainings == null)
            {
                return NotFound();
            }
            var strenghtTraining = await _context.StrenghtTrainings.FindAsync(id);
            if (strenghtTraining == null)
            {
                return NotFound();
            }

            _context.StrenghtTrainings.Remove(strenghtTraining);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StrenghtTrainingExists(Guid id)
        {
            return (_context.StrenghtTrainings?.Any(e => e.StrenghtTrainingId == id)).GetValueOrDefault();
        }
    }
}
