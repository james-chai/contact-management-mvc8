using ContactManagement.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ContactManagement.Data.Repository;

public class SqlContactData(ContactDbContext db, ILogger<SqlContactData> logger) : IContactData
{
    public Contact Add(Contact newContact)
    {
        ArgumentNullException.ThrowIfNull(newContact);

        logger.LogInformation("Adding new contact: {ContactId}", newContact.Id);
        db.Contacts.Add(newContact);

        return newContact;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to save changes to database");
            throw;
        }
    }

    public async Task<Contact?> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Attempting to delete contact: {ContactId}", id);

        var contact = await GetByIdAsync(id, cancellationToken);
        if (contact == null) return null;

        db.Contacts.Remove(contact);
        await CommitAsync(cancellationToken);

        return contact;
    }

    public async Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await db.Contacts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<int> GetCountOfContactsAsync(CancellationToken cancellationToken = default)
        => await db.Contacts.CountAsync(cancellationToken);

    public async Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var query = db.Contacts.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var normalizedTerm = searchTerm.Trim().ToLower();
            query = query.Where(c =>
                c.FirstName.ToLower().Contains(normalizedTerm) ||
                c.LastName.ToLower().Contains(normalizedTerm) ||
                (c.CompanyName != null && c.CompanyName.ToLower().Contains(normalizedTerm)) ||
                (c.Mobile != null && c.Mobile.Contains(normalizedTerm)) ||
                c.Email.ToLower().Contains(normalizedTerm));
        }

        await Task.Delay(1000); // simulate loading spinner delay

        return await query
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .ToListAsync(cancellationToken);
    }

    public Contact Update(Contact updatedContact)
    {
        ArgumentNullException.ThrowIfNull(updatedContact);

        logger.LogInformation("Updating contact: {ContactId}", updatedContact.Id);
        var entity = db.Contacts.Update(updatedContact);
        entity.State = EntityState.Modified;

        return updatedContact;
    }
}