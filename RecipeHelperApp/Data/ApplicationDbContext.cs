using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using RecipeHelperApp.Models;

namespace RecipeHelperApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<NutritionForm> NutritionForms { get; set; }
        public DbSet<Week> Weeks { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Recipe> Recipes { get; set; }


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

            // These are our properties. 

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.NutritionForm)
                .WithOne(n => n.User)
                .HasForeignKey<NutritionForm>(n => n.UserId)
                .IsRequired(); // Make UserId required

            modelBuilder.Entity<ApplicationUser>()
              .HasMany(w => w.Weeks)
              .WithOne(d => d.User)
              .HasForeignKey(d => d.UserId)
              .IsRequired(); // Make UserId required.

            modelBuilder.Entity<Week>()
              .HasMany(w => w.Days)
              .WithOne(d => d.Week)
              .HasForeignKey(d => d.WeekId)
              .IsRequired(); // Make WeekId required

            modelBuilder.Entity<Day>()
                .HasMany(d => d.Recipes)
                .WithOne(r => r.Day)
                .HasForeignKey(r => r.DayId)
                .IsRequired(); // Make DayId required
        }

    
    }
}