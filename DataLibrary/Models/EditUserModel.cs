using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class EditUserModel
    {
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public List<string> Claims { get; set; } = new();
        public IList<string> Roles { get; set; }

    }
}
