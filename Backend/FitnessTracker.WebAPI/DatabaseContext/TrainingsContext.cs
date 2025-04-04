using FitnessTracker.WebAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;

namespace FitnessTracker.WebAPI.DatabaseContext
{
    public class TrainingsContext : DbContext
    {
        public TrainingsContext(DbContextOptions<TrainingsContext> options) : base(options)
        {
        }

        public DbSet<AerobicTraining> AerobicTrainings { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<StrengthTraining> StrenghtTrainings { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StrengthTraining>()
            .HasMany(t => t.Sets)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Set>()
            .HasOne(s => s.StrenghtTraining)       // Set has one StrengthTraining
            .WithMany(st => st.Sets)               // StrengthTraining has many Sets
            .HasForeignKey(s => s.StrenghtTrainingId)  // Use StrengthTrainingId as foreign key
            .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
