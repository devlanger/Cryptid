using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class RegisterDto
    {
        public string Email { get; set; }
        
        public string Username { get; set; }
        
        public string Nickname { get; set; }
        public string Password { get; set; }
    }
}