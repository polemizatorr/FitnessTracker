namespace FitnessTracker.WebAPI.Entities.Models
{
    public class Set
    {
        public Guid SetId { get; set; }
        public Guid StrenghtTrainingId { get; set; }
        public StrengthTraining? StrenghtTraining { get; set; }
        public int RepetitionsNumber { get; set; }
        public string ExerciseName { get; set; }
        public int ExhaustionLevel { get; set; }
        public int Weight { get; set; }

        public Set() { }
        public Set(Guid strenghtTrainingId, int repetitionsNumber, string exerciseName, int exhaustionLevel, int weight)
        {
            SetId = new Guid();
            StrenghtTrainingId = strenghtTrainingId;
            RepetitionsNumber = repetitionsNumber;
            ExerciseName = exerciseName;
            ExhaustionLevel = exhaustionLevel;
            Weight = weight;
        }

        public Set(int repetitionsNumber, string exerciseName, int exhaustionLevel, int weight)
        {
            SetId = new Guid();
            RepetitionsNumber = repetitionsNumber;
            ExerciseName = exerciseName;
            ExhaustionLevel = exhaustionLevel;
            Weight = weight;
        }
    }
}
