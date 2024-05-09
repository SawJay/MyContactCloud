﻿using MyContactCloud.Client.Models;

namespace MyContactCloud.Client.Services.Interfaces
{
    public interface IContactDTOService
    {
        Task<ContactDTO> CreateContactAsync(ContactDTO contactDTO, string userId);
        Task<IEnumerable<ContactDTO>> GetContactsAsync(string userId);
    }
}
