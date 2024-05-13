using MyContactCloud.Client.Models;
using MyContactCloud.Client.Services.Interfaces;
using MyContactCloud.Helpers;
using MyContactCloud.Model;
using MyContactCloud.Services.Interfaces;

namespace MyContactCloud.Services
{
    public class ContactDTOService(IContactRepository repository) : IContactDTOService
    {
        public async Task<ContactDTO> CreateContactAsync(ContactDTO contactDTO, string userId)
        {
            Contact newContact = new Contact()
            {
                FirstName = contactDTO.FirstName,
                LastName = contactDTO.LastName,
                Email = contactDTO.Email,
                BirthDate = contactDTO.BirthDate,
                Address1 = contactDTO.Address1,
                Address2 = contactDTO.Address2,
                City = contactDTO.City,
                State = contactDTO.State,
                ZipCode = contactDTO.ZipCode,
                PhoneNumber = contactDTO.PhoneNumber,
                Created = DateTimeOffset.Now,
                AppUserId = userId,                            
            };

            // to do: categories & images
            if (contactDTO.ImageUrl.StartsWith("data:"))
            {
                newContact.Image = UploadHelper.GetImageUpload(contactDTO.ImageUrl);
            }

            Contact createdContact = await repository.CreateContactAsync(newContact);

            IEnumerable<int> categoryIds = contactDTO.Categories.Select(c => c.Id);
            await repository.AddCategoriesToContactAsync(createdContact.Id, userId, categoryIds);

            return createdContact.ToDTO();

        }

        public async Task DeleteContactAsync(int contactId, string userId)
        {
            await repository.DeleteContactAsync(contactId, userId);
        }

        public async Task<ContactDTO?> GetContactByIdAsync(int contactId, string userId)
        {     
                Contact? contact = await repository.GetContactByIdAsync(contactId, userId);

                return contact?.ToDTO();    
        }

        public async Task<IEnumerable<ContactDTO>> GetContactsAsync(string userId)
        {
            IEnumerable<Contact> contacts = await repository.GetContactsAsync(userId);

            List<ContactDTO> contactDtos = new List<ContactDTO>();

            foreach (Contact contact in contacts)
            {
                ContactDTO contactDTO = contact.ToDTO();
                contactDtos.Add(contactDTO);
            }

            return contactDtos;
        }

        public async Task<IEnumerable<ContactDTO>> GetContactsByCategoryIdAsync(int categoryId, string userId)
        {
            IEnumerable<Contact> contacts = await repository.GetContactsByCategoryIdAsync(categoryId, userId);

            List<ContactDTO> dtos = [];

            foreach (Contact contact in contacts)
            {
                dtos.Add(contact.ToDTO());
            }

            return dtos;
        }

        public async Task<IEnumerable<ContactDTO>> SearchContactsAsync(string searchTerm, string userId)
        {
            IEnumerable<Contact> contacts = await repository.SearchContactsAsync(searchTerm, userId);

            return contacts.Select(contact => contact.ToDTO());
        }

        public async Task UpdateContactAsync(ContactDTO contactDTO, string userId)
        {
            Contact? contact = await repository.GetContactByIdAsync(contactDTO.Id, userId);

            if (contact is not null)
            {
                contact.FirstName = contactDTO.FirstName;
                contact.LastName = contactDTO.LastName;
                contact.BirthDate = contactDTO.BirthDate;
                contact.Address1 = contactDTO.Address1;
                contact.Address2 = contactDTO.Address2;
                contact.City = contactDTO.City;
                contact.State = contactDTO.State;
                contact.ZipCode = contactDTO.ZipCode;
                contact.Email = contactDTO.Email;
                contact.PhoneNumber = contactDTO.PhoneNumber;

                if (contactDTO.ImageUrl.StartsWith("data:"))
                {
                    contact.Image = UploadHelper.GetImageUpload(contactDTO.ImageUrl);
                }
                else
                {
                    contact.Image = null;
                }

                // do not let database update categories yet
                contact.Categories.Clear();

                await repository.UpdateContactAsync(contact);

                // remove old categories
                await repository.RemoveCategoriesFromContactAsync(contact.Id, userId);

                // add back whatever the user selected
                IEnumerable<int> selectedCategoryIds = contactDTO.Categories.Select(c => c.Id);
                await repository.AddCategoriesToContactAsync(contact.Id, userId, selectedCategoryIds);
            }
        }
    }
}
