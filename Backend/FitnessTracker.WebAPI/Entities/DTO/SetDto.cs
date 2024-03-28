namespace FitnessTracker.WebAPI.Entities.DTO
{
    public class SetDto
    {
        public int RepetitionsNumber { get; set; }
        public string? ExerciseName { get; set; }
        public int ExhaustionLevel { get; set; }

        public SetDto() { }

        public SetDto(int repetitionsNumber, string? exerciseName, int exhaustionLevel)
        {
            RepetitionsNumber = repetitionsNumber;
            ExerciseName = exerciseName;
            ExhaustionLevel = exhaustionLevel;
        }
    }
}
