using FitnessTracker.WebAPI.Entities.Models;

namespace FitnessTracker.WebAPI.Entities.DTO
{
    public class StrengthTrainingResponse
    {
        public Guid StrenghtTrainingId { get; set; }
        public string? TrainingName { get; set; }
        public DateTime TrainingDate { get; set; }
        public IList<Set>? Sets { get; set; }
    }
}
