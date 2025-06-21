using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ContactManagement.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ContactDbContext>
{
    public ContactDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddUserSecrets<DesignTimeDbContextFactory>()
            .Build();

        var connectionString = configuration.GetConnectionString("ContactManagementDb");
        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("Connection string 'ConnectionStrings' not found.");

        var builder = new DbContextOptionsBuilder<ContactDbContext>();
        builder.UseSqlServer(connectionString);

        return new ContactDbContext(builder.Options);
    }
}