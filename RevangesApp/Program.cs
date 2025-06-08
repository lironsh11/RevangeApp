using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RevengeApp.Data;
using RevengeApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Minimal services for Azure
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Use In-Memory database
builder.Services.AddDbContext<RevengeContext>(options =>
    options.UseInMemoryDatabase("RevengeDB"));

// Add Identity with minimal configuration
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<RevengeContext>();

// Add services
builder.Services.AddScoped<IRevengeService, RevengeService>();

var app = builder.Build();

// Minimal pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Simple database initialization
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<RevengeContext>();
    context.Database.EnsureCreated();
}
catch
{
    // Ignore database errors for now
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();