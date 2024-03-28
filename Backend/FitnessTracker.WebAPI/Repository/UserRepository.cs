using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Entities.Models;

namespace FitnessTracker.WebAPI.Repository
{
    public class UserRepository
    {
        private readonly TrainingsContext _context;
        public UserRepository(TrainingsContext context)
        {
            _context = context;
        }

        public User GetUserByUsername(string username)
        {
            var user = _context.Users.Single(u => u.UserName == username);

            if (user is null)
            {
                throw new Exception("User with username {} not found");
            }

            return user;
        }
    }
}
