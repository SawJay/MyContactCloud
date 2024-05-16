using MyContactCloud.Client.Models;
using MyContactCloud.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace MyContactCloud.Client.Services
{
    public class WASMCategoryDTOService : ICategoryDTOService
    {
        private readonly HttpClient _httpClient;

        public WASMCategoryDTOService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Categories", category);
            response.EnsureSuccessStatusCode();

            CategoryDTO? categoryDTO = await response.Content.ReadFromJsonAsync<CategoryDTO>();
            return categoryDTO!;
        }

        public async Task DeleteCategoryAsync(int categoryId, string userId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Categories/{categoryId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> EmailCategoryAsync(int categoryId, EmailData emailData, string userId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/Categories/{categoryId}/email", emailData);
            if (response.IsSuccessStatusCode == true)
            {
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string userId)
        {
            IEnumerable<CategoryDTO> request = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDTO>>($"api/Categories") ?? [];

            return request;
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId, string userId)
        {
            CategoryDTO? categoryDTO = await _httpClient.GetFromJsonAsync<CategoryDTO>($"api/Categories/{categoryId}");
            return categoryDTO;
        }

        public async Task UpdateCategoryAsync(CategoryDTO category, string userId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/Categories/{category.Id}", category);
            response.EnsureSuccessStatusCode();
        }
    }
}
