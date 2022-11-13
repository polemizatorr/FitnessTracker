namespace FitnessTracker.WebAPI.Entities.Models
{
    public class Exercise : Entity
    {
        public Guid ExerciseId { get; set; }
        public Guid StrenghtTrainingId { get; set; }
        public Guid SetId { get; set; }
        public int ExaushtionLevel { get; set; }

    }
}
