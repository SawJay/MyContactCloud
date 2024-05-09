using Microsoft.EntityFrameworkCore;
using MyContactCloud.Client.Models;
using MyContactCloud.Data;
using MyContactCloud.Model;
using MyContactCloud.Services.Interfaces;

namespace MyContactCloud.Services
{
    public class ContactRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IContactRepository
    {
        public async Task AddCategoriesToContactAsync(int contactId, string userId, IEnumerable<int> categoryIds)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Contact? contact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId && c.AppUserId == userId);

            if (contact is not null) 
            { 
                foreach (int categoryId in categoryIds)
                {
                    Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId && c.AppUserId == userId);
                    
                    if (category is not null)
                    {
                        contact.Categories.Add(category);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task<Contact> CreateContactAsync(Contact contact)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            context.Contacts.Add(contact);
            await context.SaveChangesAsync();

            return contact;
        }

        public async Task<IEnumerable<Contact>> GetContactsAsync(string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Contact> contacts = await context.Contacts.Where(x => x.AppUserId == userId).ToListAsync();

            return contacts;
        }

    }
}
