using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; } = string.Empty;
    }
}
