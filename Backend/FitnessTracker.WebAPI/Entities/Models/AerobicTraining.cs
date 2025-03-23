using FitnessTracker.WebAPI.Entities.DTO;

namespace FitnessTracker.WebAPI.Entities.Models
{
    public class AerobicTraining : Entity
    {
        public Guid AerobicTrainingId { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string? ActivityType { get; set; }
        public int ActivityDurationMinutes { get; set; }
        public int CalorieBurnt { get; set; }
        public DateTime? ActivityDate { get; set; }

        public AerobicTraining() { }

        public AerobicTraining(Guid userId, string activityType, int activityDurationMinutes, int calorieBurnt, DateTime? activityDate) : base()
        {
            AerobicTrainingId = new Guid();
            UserId = userId;
            ActivityType = activityType;
            ActivityDurationMinutes = activityDurationMinutes;
            CalorieBurnt = calorieBurnt;
            ActivityDate = activityDate;
        }

        public AerobicTrainingDto ToDto()
        {
            return new AerobicTrainingDto
            {
                ActivityDate = this.ActivityDate,
                ActivityType = this.ActivityType,
                ActivityDurationMinutes = this.ActivityDurationMinutes,
                CalorieBurnt = this.CalorieBurnt,
                AerobicTrainingId = this.AerobicTrainingId,
            };
        }
    }
}
