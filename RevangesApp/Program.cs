using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RevengeApp.Data;
using RevengeApp.Services;
using RevengeApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure SQLite database context
builder.Services.AddDbContext<RevengeContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=revenges.db"));

// Add Identity services
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    // Configure password requirements (make them easier for demo)
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    // Configure user requirements
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<RevengeContext>();

// Configure application cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.ReturnUrlParameter = "returnUrl";

    // Set cookie expiration
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;

    // Configure cookie settings
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// Register revenge service for dependency injection
builder.Services.AddScoped<IRevengeService, RevengeService>();

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RevengeContext>();
    try
    {
        if (app.Environment.IsProduction())
        {
            // For In-Memory database, always ensure created
            context.Database.EnsureCreated();
            Console.WriteLine("✅ In-Memory Database initialized for Azure");
        }
        else
        {
            // For SQLite, ensure created
            context.Database.EnsureCreated();
            Console.WriteLine("✅ SQLite Database initialized for local");
        }

        var userCount = context.Users.Count();
        var revengeCount = context.Revenges.Count();
        Console.WriteLine($"📊 Database stats: {userCount} users, {revengeCount} revenges");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database error: {ex.Message}");
    }
}
// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add Authentication & Authorization middleware (order is important!)
app.UseAuthentication();
app.UseAuthorization();

// Configure routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Add Identity Razor pages (login/register/etc)
app.MapRazorPages();

// Log startup info
Console.WriteLine("🔥💄 Kokava Shabit Revenge App is starting... 👑✨");
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
Console.WriteLine("Navigate to: https://localhost:7084 or http://localhost:5000");

app.Run();