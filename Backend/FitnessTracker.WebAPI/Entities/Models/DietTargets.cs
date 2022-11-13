namespace FitnessTracker.WebAPI.Entities.Models
{
    public class DietTargets : Entity
    {
        public Guid DietTargetsId { get; set; }
        public Guid UserId { get; set; }
        public int CalorieTarget { get; set; }
        public int ProteinTarget { get; set; }
        public int FatTarget { get; set; }
        public int CarbsTarget { get; set; }

        public DietTargets(Guid userId, int calorieTarget, int proteinTarget, int fatTarget, int carbsTarget) : base()
        {
            DietTargetsId = new Guid();
            UserId = userId;
            CalorieTarget = calorieTarget;
            ProteinTarget = proteinTarget;
            FatTarget = fatTarget;
            CarbsTarget = carbsTarget;
        }
    }
}
