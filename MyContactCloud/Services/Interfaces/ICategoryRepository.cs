using Microsoft.Data.SqlClient.DataClassification;
using MyContactCloud.Model;

namespace MyContactCloud.Services.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetCategoriesAsync(string userId);
        Task<Category?> GetCategoryByIdAsync(int categoryId, string userId);
        Task DeleteCategoryAsync(int categoryId, string userId);
        Task UpdateCategoryAsync (Category category, string userId);
    }
}
