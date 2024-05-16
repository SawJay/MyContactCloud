using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyContactCloud.Client.Models;
using MyContactCloud.Client.Services.Interfaces;
using MyContactCloud.Data;
using MyContactCloud.Model;
using MyContactCloud.Services;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ContactsController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    private string _userId => _userManager.GetUserId(User)!; // [authorize] means userId cannot be null

    private readonly IContactDTOService _contactDTOService;

    public ContactsController(IContactDTOService contactDTOService, UserManager<ApplicationUser> userManager)
    {
        _contactDTOService = contactDTOService;

        _userManager = userManager;
    }



    // GET: "api/contacts" OR "api/contacts?categoryId=4" -> list of user contacts, optionally filtered by category
    [HttpGet]
    public async Task<ActionResult<List<ContactDTO>>> GetContacts([FromQuery] int? categoryId)
    {
        try
        {       
            if(categoryId == null) 
            {
                IEnumerable<ContactDTO> contacts = await _contactDTOService.GetContactsAsync(_userId);
                return Ok(contacts);
            }
            else
            {
                IEnumerable<ContactDTO> contactsByCategory = await _contactDTOService.GetContactsByCategoryIdAsync(categoryId.Value, _userId);
                return Ok(contactsByCategory);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }

    // GET: "api/contacts/5" -> a contact or 404
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContactDTO?>> GetContact([FromRoute] int id)
    {
        try
        {
            ContactDTO? contactDTO = (await _contactDTOService.GetContactByIdAsync(id, _userId))!;
            return Ok(contactDTO);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }

    // GET: "api/contacts/search?query=whatever" -> contacts matching the search query
    [HttpGet("search")]
    public async Task<ActionResult<ContactDTO>> SearchContacts([FromQuery] string searchTerm)
    {
        try
        {
            IEnumerable<ContactDTO> contacts = await _contactDTOService.SearchContactsAsync(searchTerm, _userId);
            return Ok(contacts);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }

    // POST: "api/contacts" -> creates and returns the created contact
    [HttpPost]
    public async Task<ActionResult<ContactDTO>> CreateContact([FromBody] ContactDTO contactDTO)
    {
        try
        {
            ContactDTO createdContactDTO = await _contactDTOService.CreateContactAsync(contactDTO, _userId);
            return Ok(createdContactDTO);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }

    // PUT: "api/contacts/5" -> updates the selected contact and returns Ok
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateContact([FromRoute] int id, [FromBody] ContactDTO contact)
    {
        try
        {
            if (id != contact.Id)
            {
                return BadRequest();

            }
            else
            {
                await _contactDTOService.UpdateContactAsync(contact, _userId);
                return Ok();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }

    // DELETE: "api/contacts/5" -> deletes the selected contact and returns NoContent
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteContact([FromRoute] int id)
    {
        try
        {
            await _contactDTOService.DeleteContactAsync(id, _userId);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }

    // POST: "api/contacts/5/email" -> sends an email to contact and returns Ok or BadRequest to indicate success or failure
    [HttpPost("{id:int}/email")]
    public async Task<ActionResult> EmailContact([FromRoute] int id, [FromBody] EmailData emailData)
    {
        try
        {
            await _contactDTOService.EmailContactAsync(id, emailData, _userId);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }
}


