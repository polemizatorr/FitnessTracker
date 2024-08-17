using FitnessTracker.WebAPI.Entities.Models;

namespace FitnessTracker.WebAPI.Entities.DTO
{
    public class StrengthTrainingDTO
    {
        public string? TrainingName { get; set; }

        public DateTime TrainingDate { get; set; } = new DateTime();
        public IList<SetDto> Sets { get; set; } = new List<SetDto>();
    }
}
