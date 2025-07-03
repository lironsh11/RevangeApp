using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RevengeApp.Data;
using RevengeApp.Services;

var builder = WebApplication.CreateBuilder(args);

// ------------------------
// Service configuration
// ------------------------
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// In-memory database
builder.Services.AddDbContext<RevengeContext>(options =>
    options.UseInMemoryDatabase("RevengeDB"));

// Identity (minimal settings)
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<RevengeContext>();

// DI services
builder.Services.AddScoped<IRevengeService, RevengeService>();

// --------------------------------------------------
// ** Listen on every interface, port 5000 **
// --------------------------------------------------
builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();

// ------------------------
// HTTP request pipeline
// ------------------------
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Ensure in-memory DB exists
try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<RevengeContext>();
    dbContext.Database.EnsureCreated();
}
catch
{
    // Ignore DB init errors for now
}

// Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
