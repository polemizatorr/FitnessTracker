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
        public DbSet<StrenghtTraining> StrenghtTrainings { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            /*modelBuilder.Entity<User>().HasData(new User("Tukan", "Tukan@vp.pl", "passwd",  "Domin",  "Czerniak", DateTime.Now));
            modelBuilder.Entity<User>().HasData(new User("Adam123", "adam@vp.pl", "passwd", "Adam", "Wiśnia", DateTime.Now));*/
        }

    }
}
