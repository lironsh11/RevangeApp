using Microsoft.EntityFrameworkCore;
using RevengeApp.Data;
using RevengeApp.Services;
using RevengeApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure SQLite database context
builder.Services.AddDbContext<RevengeContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=revenges.db"));

// Register revenge service for dependency injection
builder.Services.AddScoped<IRevengeService, RevengeService>();

var app = builder.Build();

// Initialize database with sample data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RevengeContext>();
    try
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // Add sample data if table is empty
        if (!context.Revenges.Any())
        {
            context.Revenges.AddRange(
                new Revenge
                {
                    Title = "נקמת השפתון האדום",
                    Description = "הלקוחה המעצבנת שהשפילה אותי בפני כולם תקבל את מה שמגיע לה!",
                    Target = "הלקוחה המעצבנת מהדואר",
                    Date = new DateTime(2024, 12, 1),
                    Status = RevengeStatus.Planned,
                    DramaLevel = 4,
                    Category = RevengeCategory.AnnoyingCustomer,
                    Notes = "צריכה להיות דרמטית במיוחד!"
                },
                new Revenge
                {
                    Title = "נקמת האקס הבגדן",
                    Description = "הוא חשב שהוא יכול לברוח עם החברה הכי טובה שלי? טעות!",
                    Target = "יוסי האקס",
                    Date = new DateTime(2024, 11, 20),
                    Status = RevengeStatus.Completed,
                    DramaLevel = 5,
                    Category = RevengeCategory.Ex,
                    CompletedDate = new DateTime(2024, 12, 3),
                    Notes = "הנקמה הושלמה בהצלחה רבה! הוא יזכור את זה לנצח!"
                }
            );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database error: {ex.Message}");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Configure default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();