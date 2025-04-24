using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Interfaces;

namespace FitnessTracker.WebAPI.Services
{
    public class StrengthTrainingsService : IStrengthTrainingsService
    {
        private readonly TrainingsContext _context;
        private readonly IConfiguration _configuration;

        public StrengthTrainingsService(TrainingsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
    }
}
