namespace FitnessTracker.WebAPI.Entities.Models
{
    public class AerobicTraining : Entity
    {
        public Guid AerobicTrainingId { get; set; }
        public Guid UserId { get; set; }
        public string? ActivityType { get; set; }
        public TimeSpan ActivityDuration { get; set; }
        public int CalorieBurnt { get; set; }

        public AerobicTraining() { }

        public AerobicTraining(Guid userId, string activityType, TimeSpan activityDuration, int calorieBurnt) : base()
        {
            AerobicTrainingId = new Guid();
            UserId = userId;
            ActivityType = activityType;
            ActivityDuration = activityDuration;
            CalorieBurnt = calorieBurnt;
        }
    }
}
