using MyContactCloud.Client.Models;
using MyContactCloud.Data;
using System.ComponentModel.DataAnnotations;

namespace MyContactCloud.Model
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string? Name { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();

        [Required]
        public string? AppUserId { get; set; }
        public virtual ApplicationUser? AppUser { get; set; }
    }

    public static class CategoryExtensions
    {
        public static CategoryDTO ToDTO(this Category category)
        {
            CategoryDTO dto = new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
            };

            foreach (Contact contact in category.Contacts)
            {
                contact.Categories.Clear();

                ContactDTO contactDTO = contact.ToDTO();
                dto.Contacts.Add(contactDTO);
            }
            return dto;
        }
    }
}

