using ContactManagement.Core;

namespace ContactManagement.Data.Repository;

public interface IContactData
{
    Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm, CancellationToken cancellationToken);
    Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Contact Update(Contact updatedContact);
    Contact Add(Contact newContact);
    Task<Contact?> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<int> GetCountOfContactsAsync(CancellationToken cancellationToken);
    Task<int> CommitAsync(CancellationToken cancellationToken);
}