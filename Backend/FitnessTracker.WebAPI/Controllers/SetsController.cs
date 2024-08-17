using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitnessTracker.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetsController : ControllerBase
    {
        private readonly TrainingsContext _context;
        private readonly IHttpContextAccessor _http;

        public SetsController(TrainingsContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _http = httpContextAccessor;
        }

        // GET: api/<SetsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Set>>> GetSetsForTrining(Guid trainingId)
        {
            if (_context.Sets == null)
            {
                return NotFound();
            }

            return await _context.Sets.Where(s => s.StrenghtTrainingId == trainingId).ToListAsync();
        }

        // GET api/<SetsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SetsController>
        [HttpPost("{strengthTrainingId}")]
        [EnableCors("AllowAll")]
        public async Task<ActionResult<Set>> CreateSet(Guid strengthTrainingId, SetDto set)
        {
            if (_context.Sets == null)
            {
                return Problem("Entity set 'TrainingsContext.Sets'  is null.");
            }

            var username = _http.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = _context.Users.First(u => u.UserName == username).UserId;

            if (userId == Guid.Empty)
            {
                throw new Exception("No user found for given username");
            }

            var newSet = new Set(strengthTrainingId, set.RepetitionsNumber, set.ExerciseName!, set.ExhaustionLevel, set.Weight);
            _context.Sets.Add(newSet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("CreateSet", new { id = newSet.SetId }, set);
        }

        // PUT api/<SetsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SetsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSet(Guid id)
        {
            if (_context.Sets == null)
            {
                return NotFound();
            }
            var set = await _context.Sets.FindAsync(id);
            if (set == null)
            {
                return NotFound();
            }

            _context.Sets.Remove(set);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SetExists(Guid id)
        {
            return (_context.Sets?.Any(e => e.SetId == id)).GetValueOrDefault();
        }
    }
}
