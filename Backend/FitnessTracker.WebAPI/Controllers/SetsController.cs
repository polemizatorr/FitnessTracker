using FitnessTracker.WebAPI.ApiResponse;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Entities.Models;
using FitnessTracker.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetsController : ControllerBase
    {
        private readonly ISetsService _service;

        public SetsController(ISetsService service)
        {
            _service = service;
        }

        [HttpGet]
        public ApiResponse<IEnumerable<Set>> GetSetsForTrining(Guid trainingId)
        {
            var response = _service.GetSetsForTraining(trainingId);

            return response;
        }

        [HttpPost("{strengthTrainingId}")]
        public async Task<ApiResponse<Set>> CreateSet(Guid strengthTrainingId, SetDto set)
        {
            var response = await _service.CreateSet(strengthTrainingId, set);

            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<Set>> DeleteSet(Guid id)
        {
            var response = await _service.DeleteSet(id);

            return response;
        }
    }
}
