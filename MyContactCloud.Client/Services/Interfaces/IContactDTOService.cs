using MyContactCloud.Client.Models;

namespace MyContactCloud.Client.Services.Interfaces
{
    public interface IContactDTOService
    {
        Task<ContactDTO> CreateContactAsync(ContactDTO contactDTO, string userId);
        Task<IEnumerable<ContactDTO>> GetContactsAsync(string userId);
        Task UpdateContactAsync(ContactDTO contactDTO, string userId);
        Task<ContactDTO?> GetContactByIdAsync(int contactId, string userId);
    }
}
