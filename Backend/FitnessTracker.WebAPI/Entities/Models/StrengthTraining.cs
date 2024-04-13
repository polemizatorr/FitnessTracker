using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.WebAPI.Entities.Models
{
    public class StrengthTraining : Entity
    {
        [Key]
        public Guid StrenghtTrainingId { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public IList<Set> Sets { get; set; }

        public StrengthTraining(Guid userId)
        {
            StrenghtTrainingId = new Guid();
            UserId = userId;
            Sets = new List<Set>();
        }

        public StrengthTraining(Guid userId, IList<Set> sets)
        {
            StrenghtTrainingId = new Guid();
            UserId = userId;
            Sets = sets;
        }
    }
}
