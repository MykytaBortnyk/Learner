using System.Collections.Generic;
using RestAPI.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace RestAPI.Models
{
    /// <summary>
    /// Модель юзера
    /// </summary>
    public class AppUser : IdentityUser
    {
        public List<Word> Words { get; set; }

        public List<Collection> Collections { get; set; }
    }
}