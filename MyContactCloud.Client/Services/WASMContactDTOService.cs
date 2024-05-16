using MyContactCloud.Client.Models;
using MyContactCloud.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace MyContactCloud.Client.Services
{
    public class WASMContactDTOService : IContactDTOService
    {
        private readonly HttpClient _httpClient;

        public WASMContactDTOService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ContactDTO> CreateContactAsync(ContactDTO contact, string userId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Contacts", contact);
            response.EnsureSuccessStatusCode();

            ContactDTO? contactDTO = await response.Content.ReadFromJsonAsync<ContactDTO>();
            return contactDTO!;
        }

        public async Task DeleteContactAsync(int contactId, string userId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Contacts/{contactId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> EmailContactAsync(int contactId, EmailData emailData, string userId)
        {

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/Contacts/{contactId}/email", emailData);
            if (response.IsSuccessStatusCode == true)
            {
                return true;
            }

            return false;

        }

        public async Task<ContactDTO?> GetContactByIdAsync(int contactId, string userId)
        {
            ContactDTO? contactDTO = await _httpClient.GetFromJsonAsync<ContactDTO>($"api/Contacts/{contactId}");
            return contactDTO;
        }

        public async Task<IEnumerable<ContactDTO>> GetContactsAsync(string userId)
        {
            IEnumerable<ContactDTO> request = await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>($"api/Contacts") ?? [];

            return request;
        }

        public async Task<IEnumerable<ContactDTO>> GetContactsByCategoryIdAsync(int categoryId, string userId)
        {
            IEnumerable<ContactDTO> contacts = (await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>($"api/Contacts?categoryId={categoryId}"))!;
            return contacts;
        }

        public async Task<IEnumerable<ContactDTO>> SearchContactsAsync(string searchTerm, string userId)
        {
            IEnumerable<ContactDTO> contacts = (await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>($"api/Contacts/search?SearchTerm={searchTerm}"))!;
            return contacts;
        }

        public async Task UpdateContactAsync(ContactDTO contact, string userId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/Contacts/{contact.Id}", contact);
            response.EnsureSuccessStatusCode();
        }
    }
}
