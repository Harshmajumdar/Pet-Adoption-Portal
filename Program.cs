using PetAdoptionPortal.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. MVC (Controllers + Views)
builder.Services.AddControllersWithViews();


 // 2. Database: Entity Framework Core + MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "DefaultConnection is missing. Check appsettings.json.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 36))));

// 3. State management: Session (remembers staff login + the visitor's shortlist)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Create the database + tables automatically on first run and add sample data
using (var scope = app.Services.CreateScope())
{
    try

    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
        DbSeeder.Seed(db);
    }
    catch (Exception ex)
    {
        Console.WriteLine("DATABASE ERROR: " + ex.Message);
        Console.WriteLine("Check that MySQL is running and the password in appsettings.json is correct.");
        throw;
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();   // must come before MapControllerRoute

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
