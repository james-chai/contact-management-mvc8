using ContactManagement.Core;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Data;

public class ContactDbContext : DbContext
{
    public ContactDbContext(DbContextOptions<ContactDbContext> options)
        : base(options) { }

    public DbSet<Contact> Contacts { get; set; }
}