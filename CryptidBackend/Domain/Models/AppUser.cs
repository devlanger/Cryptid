using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class AppUser : IdentityUser
    {
        public string? Nickname { get; set; }
    }
}