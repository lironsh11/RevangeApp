using Microsoft.EntityFrameworkCore;
using RevengeApp.Models;


namespace RevengeApp.Data
{
    public class RevengeContext : DbContext
    {
        public RevengeContext(DbContextOptions<RevengeContext> options) : base(options) { }

        // Constructor ברירת מחדל לdesign-time
        public RevengeContext() { }

        public DbSet<Revenge> Revenges { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=revenges.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Revenge>()
                .Property(r => r.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Revenge>()
                .Property(r => r.Category)
                .HasConversion<string>();

            // Seed some initial dramatic revenge data
            // Seed some initial dramatic revenge data
            modelBuilder.Entity<Revenge>().HasData(
                new Revenge
                {
                    Id = 1,
                    Title = "נקמת השפתון האדום",
                    Description = "הלקוחה המעצבנת שהשפילה אותי בפני כולם תקבל את מה שמגיע לה!",
                    Target = "הלקוחה המעצבנת מהדואר",
                    Date = new DateTime(2024, 12, 1), // תאריך קבוע במקום DateTime.Now
                    Status = RevengeStatus.Planned,
                    DramaLevel = 4,
                    Category = RevengeCategory.AnnoyingCustomer,
                    Notes = "צריכה להיות דרמטית במיוחד!"
                },
                new Revenge
                {
                    Id = 2,
                    Title = "נקמת האקס הבגדן",
                    Description = "הוא חשב שהוא יכול לברוח עם החברה הכי טובה שלי? טעות!",
                    Target = "יוסי האקס",
                    Date = new DateTime(2024, 11, 20), // תאריך קבוע
                    Status = RevengeStatus.Completed,
                    DramaLevel = 5,
                    Category = RevengeCategory.Ex,
                    CompletedDate = new DateTime(2024, 12, 3), // תאריך קבוע
                    Notes = "הנקמה הושלמה בהצלחה רבה! הוא יזכור את זה לנצח!"
                }
            
            );
        }
    }
}