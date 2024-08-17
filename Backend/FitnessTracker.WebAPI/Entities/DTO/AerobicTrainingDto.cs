namespace FitnessTracker.WebAPI.Entities.DTO
{
    public class AerobicTrainingDto
    {
        public string? ActivityType { get; set; }
        public int ActivityDurationMinutes { get; set; }
        public int CalorieBurnt { get; set; }
        public DateTime? ActivityDate { get; set; }

        public Guid? AerobicTrainingId { get; set; }

        public AerobicTrainingDto() { }

        public AerobicTrainingDto(string? activityType, int activityDurationMinutes, int calorieBurnt)
        {
            ActivityType = activityType;
            ActivityDurationMinutes = activityDurationMinutes;
            CalorieBurnt = calorieBurnt;
        }
    }
}
