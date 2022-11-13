namespace FitnessTracker.WebAPI.Entities.Models
{
    public class DietTargetsResults
    {
        public Guid DietTargetResultsId { get; set; }
        public Guid UserId { get; set; }
        public int CalorieResult { get; set; }
        public int ProteinResult { get; set; }
        public int FatResult { get; set; }
        public int CarbsResult { get; set; }
        public DateOnly Date { get; set; }

        public DietTargetsResults(Guid userId, int calorieResult, int proteinResult, int fatResult, int carbsResult, DateOnly date) : base()
        {
            DietTargetResultsId = new Guid();
            UserId = userId;
            CalorieResult = calorieResult;
            ProteinResult = proteinResult;
            FatResult = fatResult;
            CarbsResult = carbsResult;
            Date = date;
        }
    }
}
