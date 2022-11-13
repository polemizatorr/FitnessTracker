namespace FitnessTracker.WebAPI.Entities.Models
{
    public class StrenghtTraining : Entity
    {
        public Guid StrenghtTrainingId { get; set; }
        public Guid UserId { get; set; }
        public Guid ExerciseId { get; set; }

        public StrenghtTraining(Guid strenghtTrainingId, Guid userId, Guid exerciseId)
        {
            StrenghtTrainingId = strenghtTrainingId;
            UserId = userId;
            ExerciseId = exerciseId;
        }
    }
}
