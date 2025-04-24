using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Interfaces;

namespace FitnessTracker.WebAPI.Services
{
    public class AerobicTrainingsService : IAerobicTrainingsService
    {
        private readonly TrainingsContext _context;
        private readonly IConfiguration _configuration;

        public AerobicTrainingsService(TrainingsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

    }
}
