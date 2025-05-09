﻿using FitnessTracker.WebAPI.Entities.DTO;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.WebAPI.Entities.Models
{
    public class StrengthTraining : Entity
    {
        [Key]
        public Guid StrenghtTrainingId { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string TrainingName { get; set; }
        public DateTime TrainingDate { get; set; }
        public List<Set> Sets { get; set; }

        public StrengthTraining(Guid userId, string trainingName, DateTime trainingDate)
        {
            StrenghtTrainingId = new Guid();
            UserId = userId;
            Sets = new List<Set>();
            TrainingName = trainingName;
            TrainingDate = trainingDate;
        }

        public StrengthTraining(Guid userId, List<Set> sets, string trainingName, DateTime trainingDate)
        {
            StrenghtTrainingId = new Guid();
            UserId = userId;
            Sets = sets;
            TrainingName = trainingName;
            TrainingDate = trainingDate;
        }

        public StrengthTrainingDTO ToDto()
        {
            return new StrengthTrainingDTO
            {
                Sets = Sets.Select(s => s.ToDto()).ToList(),
                TrainingName = this.TrainingName,
                TrainingDate = this.TrainingDate,
            };
        }
    }
}
