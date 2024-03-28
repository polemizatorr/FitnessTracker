namespace FitnessTracker.WebAPI.Entities.Models
{
    public class StrenghtTraining : Entity
    {
        public Guid StrenghtTrainingId { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public IList<Set> Sets { get; set; }

        public StrenghtTraining(Guid strenghtTrainingId, Guid userId)
        {
            StrenghtTrainingId = strenghtTrainingId;
            UserId = userId;
            Sets = new List<Set>();
        }
    }
}
