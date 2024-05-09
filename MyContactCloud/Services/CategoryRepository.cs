using Microsoft.EntityFrameworkCore;
using MyContactCloud.Data;
using MyContactCloud.Model;
using MyContactCloud.Services.Interfaces;

namespace MyContactCloud.Services
{
    public class CategoryRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : ICategoryRepository
    {
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return category;
        }

        public async Task DeleteCategoryAsync(int categoryId, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId && c.AppUserId == userId);

            if (category is not null)
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Category> categories = await context.Categories.Where(x => x.AppUserId == userId).ToListAsync();

            return categories;
        }

        public async Task<Category?> GetCategoryByIdAsync(int categoryId, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId && c.AppUserId == userId);

            return category;
        }

        public async Task UpdateCategoryAsync(Category category, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool shouldUpdate = category.AppUserId == userId && await context.Categories.AnyAsync(c => c.Id == category.Id && c.AppUserId == userId);

            if (shouldUpdate)
            {
                context.Categories.Update(category);
                await context.SaveChangesAsync();
            }
        }
    }
}
