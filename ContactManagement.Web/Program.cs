using ContactManagement.Data;
using ContactManagement.Data.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ======================
// 1. Configuration Setup
// ======================
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("ContactManagementDb")
    ?? throw new InvalidOperationException("Connection string 'ContactManagementDb' not found.");

// ======================
// 2. Service Registration
// ======================
builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Preserve case
    });

builder.Services.AddDbContext<ContactDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IContactData, SqlContactData>();

// ======================
// 3. App Building
// ======================
var app = builder.Build();

// ======================
// 4. Middleware Pipeline
// ======================
if (app.Environment.IsDevelopment())
{
    await ApplyMigrationsAsync(app);
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ======================
// 5. Startup
// ======================
await app.RunAsync();

// ======================
// Helper Methods
// ======================
static async Task ApplyMigrationsAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ContactDbContext>();
        await dbContext.Database.MigrateAsync();
        logger.LogInformation("Migrations applied successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying migrations");
        throw;
    }
}