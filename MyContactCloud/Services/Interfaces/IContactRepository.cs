using MyContactCloud.Client.Models;
using MyContactCloud.Model;

namespace MyContactCloud.Services.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> CreateContactAsync(Contact contact);
        Task AddCategoriesToContactAsync(int contactId, string userId, IEnumerable<int> categoryIds);
        Task<IEnumerable<Contact>> GetContactsAsync(string userId);
    }
}
