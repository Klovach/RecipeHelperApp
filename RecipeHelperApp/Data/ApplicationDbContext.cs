using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeHelperApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //  ADD NEW FIELDS HERE ---> 
        // THEN :
        // add connection string > update-database > 
        // THEN :
        // add-migration customFields
        // update-database

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.BirthDate)
                .HasColumnType("date");

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.Sex)
                .HasMaxLength(250);

            modelBuilder.Entity<ApplicationUser>()
              .Property(e => e.Height)
              .HasPrecision(5, 2); 

            modelBuilder.Entity<ApplicationUser>()
              .Property(e => e.Weight)
              .HasPrecision(7, 2);

            modelBuilder.Entity<ApplicationUser>()
              .Property(e => e.TargetWeight)
              .HasPrecision(7, 2);

            modelBuilder.Entity<ApplicationUser>()
              .Property(e => e.TargetWeightDate)
             .HasColumnType("date");

            modelBuilder.Entity<ApplicationUser>()
              .Property(e => e.ActivityLevel)
              .HasMaxLength(250);

            modelBuilder.Entity<ApplicationUser>()
            .Property(e => e.FitnessGoal)
            .HasMaxLength(250);

        }
    }
}