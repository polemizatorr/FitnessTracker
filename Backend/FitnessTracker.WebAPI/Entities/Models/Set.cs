namespace FitnessTracker.WebAPI.Entities.Models
{
    public class Set
    {
        public Guid SetId { get; set; }
        public Guid StrenghtTrainingId { get; set; }
        public StrenghtTraining? StrenghtTraining { get; set; }
        public int RepetitionsNumber { get; set; }
        public string ExerciseName { get; set; }
        public int ExhaustionLevel { get; set; }

        public Set() { }
        public Set(int repetitionsNumber, string exerciseName, int exhaustionLevel)
        {
            SetId = new Guid();
            RepetitionsNumber = repetitionsNumber;
            ExerciseName = exerciseName;
            ExhaustionLevel = exhaustionLevel;
        }
    }
}
