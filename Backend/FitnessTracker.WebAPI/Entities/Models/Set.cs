namespace FitnessTracker.WebAPI.Entities.Models
{
    public class Set
    {
        public Guid SetId { get; set; }
        public int RepetitionsNumber { get; set; }
        public string ExerciseName { get; set; }

        public Set(int repetitionsNumber, string exerciseName)
        {
            SetId = new Guid();
            RepetitionsNumber = repetitionsNumber;
            ExerciseName = exerciseName;
        }
    }
}
