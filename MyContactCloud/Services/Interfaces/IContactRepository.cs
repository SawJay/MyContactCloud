using MyContactCloud.Client.Models;
using MyContactCloud.Model;

namespace MyContactCloud.Services.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> CreateContactAsync(Contact contact);
        Task AddCategoriesToContactAsync(int contactId, string userId, IEnumerable<int> categoryIds);
        Task RemoveCategoriesFromContactAsync(int contactId, string userId);
        Task<IEnumerable<Contact>> GetContactsAsync(string userId);
        Task UpdateContactAsync(Contact contact);
        Task<Contact?> GetContactByIdAsync(int contactId, string userId);

        /// <summary>
        /// Retrieves all contacts that belong to given category
        /// </summary>
        /// <param name="categoryId">ID of category to search</param>
        /// <param name="userId">ID of the user</param>
        /// <returns>Collection of contacts belonging to the given category</returns>
        Task<IEnumerable<Contact>> GetContactsByCategoryIdAsync(int categoryId, string userId);
        Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm, string userId);
        Task DeleteContactAsync(int contactId, string userId);


    }
}
